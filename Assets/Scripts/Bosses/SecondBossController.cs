using UnityEngine;
using System.Collections;

public class SecondBossController : MonoBehaviour
{
    readonly static float radius = 4.5f;

    private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float timeCreated;
    private bool startedAnimation;

    public float entranceTime;
    public float accelSpeed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeCreated = Time.time;
        startedAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startedAnimation) {
            StartCoroutine(EntranceAnimation());
            startedAnimation = true;
        }

        if (!InEntranceAnimation()) {
            // Begin the attack
            MoveTowardPlayer();
        }
    }

    private bool InEntranceAnimation() {
        return ((Time.time - timeCreated) < entranceTime);
    }

    private IEnumerator EntranceAnimation() {
        // Entrance animation
        while (InEntranceAnimation()) {
            //spriteRenderer.color = new Color(255, 0, 0);
            spriteRenderer.color = new Color(255, 0, 0);
            yield return new WaitForSeconds(0.5f);
            //spriteRenderer.color = new Color(255, 255, 255);
            spriteRenderer.color = new Color(255, 255, 255);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void MoveTowardPlayer() {
        Vector2 currentPlayerPosition = player.transform.position;
        float angle = findAngleBetween(currentPlayerPosition, transform.position);
        float accelerationX = accelSpeed * Mathf.Cos(angle);
        float accelerationY = accelSpeed * Mathf.Sin(angle);
        rb.AddForce(new Vector2(accelerationX * rb.mass, accelerationY * rb.mass));
    }

    private float findAngleBetween(Vector2 target, Vector2 source) {
        return Mathf.Atan2((target.y - source.y), (target.x - source.x));
    }
}
