using UnityEngine;
using System.Collections;

public class SecondBossController : MonoBehaviour
{
    readonly static float radius = 4.5f;

    private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float timeCreated;
    private bool didEntrance;
    private bool takingDamage;

    public float entranceTime;
    public float accelSpeed;
    public int health;
    public int bBackMulipl;
    private State state;

    private enum State {
        ENTRANCE,
        ATTACK,
        DAMAGE,
        DEATH
    };

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeCreated = Time.time;
        state = State.ENTRANCE;
        takingDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.ENTRANCE:
                StartCoroutine(EntranceAnimation());
                break;
            case State.ATTACK:
                DoAttack();
                break;
            case State.DAMAGE:
                StartCoroutine(TakeDamage());
                //TakeDamage();
                break;
            case State.DEATH:
                DoDeath();
                break;
        }
    }

    private void DoAttack() {
        MoveTowardPlayer();
    }

    private bool InEntranceAnimation() {
        return ((Time.time - timeCreated) < entranceTime);
    }

    private IEnumerator EntranceAnimation() {
        // Entrance animation
        if (!didEntrance) {
            Debug.Log("Doing Entrance");
            didEntrance = true;
            for (int i = 0; i < 3; i++) {
                yield return flashRed();
            }
            state = State.ATTACK;
        }
    }

    private void MoveTowardPlayer() {
        Vector2 currentPlayerPosition = player.transform.position;
        float angle = findAngleBetween(currentPlayerPosition, transform.position);
        float accelerationX = accelSpeed * Mathf.Cos(angle);
        float accelerationY = accelSpeed * Mathf.Sin(angle);
        rb.AddForce(new Vector2(accelerationX * rb.mass, accelerationY * rb.mass));
    }

    private IEnumerator bounceAwayFromPlayer() {
        Vector2 currentPlayerPosition = player.transform.position;
        float angle = -findAngleBetween(currentPlayerPosition, transform.position);
        float accelerationX = accelSpeed * Mathf.Cos(angle);
        float accelerationY = accelSpeed * Mathf.Sin(angle);
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(accelerationX * bBackMulipl * rb.mass, accelerationY * bBackMulipl * rb.mass));
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(0, 0);
    }

    private float findAngleBetween(Vector2 target, Vector2 source) {
        return Mathf.Atan2((target.y - source.y), (target.x - source.x));
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag.Equals("Player")) {
            PlayerMovement playerMovController = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovController.HasShield())
            {
                if (health <= 0)
                {
                    state = State.DEATH;
                }
                else
                {
                    state = State.DAMAGE;
                }
            }
        }
	}

    //private void TakeDamage() {
    private IEnumerator TakeDamage() {
        if (!takingDamage) {
            Debug.Log("Boss collision damage");
            takingDamage = true;
            health--;
            StartCoroutine(bounceAwayFromPlayer());
            StartCoroutine(flashRed());
            yield return new WaitForSeconds(1);
            takingDamage = false;
            state = State.ATTACK;
        }
    }

	private IEnumerator flashRed() {
        spriteRenderer.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = new Color(255, 255, 255);
        yield return new WaitForSeconds(0.5f);
    }

    private void DoDeath() {
        Debug.Log("Death");
        Destroy(this.gameObject);
    }
}
