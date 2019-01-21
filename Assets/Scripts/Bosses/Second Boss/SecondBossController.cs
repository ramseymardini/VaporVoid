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
    private GameObject gameplayManager;
    private bool canTakeDamage;
    private float timeToDelete = 1.5f;


    private float originalScaleX;
    private float originalScaleY;
    private float maxScaleX = 1f;
    private float maxScaleY = 1f;

    public float entranceTime;
    public float accelSpeed;
    public int health;
    public int bBackMulipl;

    private Sprite normalSprite;
    public Sprite DamagedSprite;
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
        originalScaleX = transform.localScale.x;
        originalScaleY = transform.localScale.y;
        normalSprite = spriteRenderer.sprite;
        timeCreated = Time.time;
        state = State.ENTRANCE;
        takingDamage = false;
        canTakeDamage = false;
    }

    public void SetGameManager(GameObject newGameManager)
    {
        gameplayManager = newGameManager;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.ENTRANCE:
                if (didEntrance) break;
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
                Die();
                break;
        }
    }

    private void DoAttack() {
        if (health < 0) return;
        
        MoveTowardPlayer();
    }

    private bool InEntranceAnimation() {
        return ((Time.time - timeCreated) < entranceTime);
    }

    private IEnumerator EntranceAnimation() {
        // Entrance animation
        didEntrance = true;
        float currentScaleIncrementerAmount = 0.005f;
        float timeToWait = entranceTime * currentScaleIncrementerAmount;

        float currentScale = 0; //If currentScale = 0 , circle at original scale. If currentScale = 1 then the circle will be at maxScale

        while (currentScale < 1)
        {
            currentScale += currentScaleIncrementerAmount;
            transform.localScale = new Vector2(Mathf.Lerp(originalScaleX, maxScaleX, currentScale), Mathf.Lerp(originalScaleY, maxScaleY, currentScale));

            if (Mathf.Abs(currentScale - 0.4f) < currentScaleIncrementerAmount / 2) {
                    canTakeDamage = true;
                    gameObject.tag = "BossEnemy";
            } 

            if (Mathf.Abs(currentScale - 0.3f) < 0.002f || Mathf.Abs(currentScale - 0.6f) < 0.002f || Mathf.Abs(currentScale - 1f) < 0.002f)
            {
                //Debug.Log("Blink: " + currentScale);
                
                /*spriteRenderer.color = new Color(255, 0, 0);
                yield return new WaitForSeconds(0.5f);
                spriteRenderer.color = new Color(255, 255, 255);
                yield return new WaitForSeconds(0.5f);
                */
                StartCoroutine(flashRed());

            }

            yield return new WaitForSeconds(timeToWait);

        }

        state = State.ATTACK;
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
        if (!canTakeDamage) return;

        if (collision.gameObject.tag.Equals("Player")) {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController.HasShield())
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

        /* if (collision.gameObject.tag.Equals("ExplosionEnemy"))
        {
            if (health <= 0)
            {
                state = State.DEATH;
            }
            else
            {
                state = State.DAMAGE;
            }
        } */

	}

    //private void TakeDamage() {
    private IEnumerator TakeDamage() {
        if (!takingDamage) {
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
        spriteRenderer.sprite = DamagedSprite;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = normalSprite;
    }

    private void Die() {
        StartCoroutine(DieHelper());
    }

    IEnumerator DieHelper() {
        float currentScaleDecrementerAmount = 0.005f;
        float timeToWait = timeToDelete  * currentScaleDecrementerAmount;

        float currentScale = 1; //If currentScale = 0 , circle at original scale. If currentScale = 1 then the circle will be at maxScale

        while (currentScale > 0)
        {
            transform.localScale = new Vector2(Mathf.Lerp(originalScaleX, maxScaleX, currentScale), Mathf.Lerp(originalScaleY, maxScaleY, currentScale));
            currentScale -= currentScaleDecrementerAmount;
            yield return new WaitForSeconds(timeToWait);
        }

        gameplayManager.GetComponent<GameplayManager>().EndSecondBoss();
        Destroy(this.gameObject);
    }
}
