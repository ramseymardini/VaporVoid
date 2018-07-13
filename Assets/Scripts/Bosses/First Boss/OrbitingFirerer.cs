using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingFirerer : MonoBehaviour
{
    public Sprite[] damageTakenSprites;
    public GameObject projectile;

    protected GameObject controller;
    protected GameObject player;
    protected Vector2 currentPlayerPosition;

    protected Collider2D coll;
    protected Rigidbody2D rb;

    protected float defaultAccelProjectile;
    protected float currAccelProjectile;

    protected float health;
    protected Vector2 centerOfRotation;
    protected float radiusOfRotation;
    protected float speedOfRotation;

    protected float maxScaleX = 1.3f;
    protected float maxScaleY = 1.3f;
    protected float timeToExpand = 6.5f;

    protected int damageTakenSpriteIndicator;

    protected float originalScaleX;
    protected float originalScaleY;

    protected bool isLastOrb;
    float timeCreated;

    protected bool finishedIncreasing;

    protected float damageTakenPerProjectile;

    protected float timePerProjectile;

    // Use this for initialization
    protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        radiusOfRotation = FirstBossController.GetRadius();
        speedOfRotation = 3.5f;

        originalScaleX = transform.localScale.x;
        originalScaleY = transform.localScale.y;

        health = 5f;
        centerOfRotation = new Vector2(0, 0);
        defaultAccelProjectile = 3f;
        currAccelProjectile = defaultAccelProjectile;

        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        timeCreated = Time.time;

        StartCoroutine(IncreaseSizeAndStartRotation());

        damageTakenPerProjectile = 1f;
        timePerProjectile = 1f;

        StartCoroutine(StartAttacking());
    }

    // Update is called once per frame
    protected void FixedUpdate() {
        if (finishedIncreasing) {
            MoveInCircle();
        }
    }
   
    IEnumerator IncreaseSizeAndStartRotation() {
        float currentScaleIncrementerAmount = 0.01f;
        float timeToWait = timeToExpand * currentScaleIncrementerAmount;

        float currentScale = 0; //If currentScale = 0 , circle at original scale. If currentScale = 1 then the circle will be at maxScale

        while (currentScale < 1) {
            transform.localScale = new Vector2(Mathf.Lerp(originalScaleX, maxScaleX, currentScale), Mathf.Lerp(originalScaleY, maxScaleY, currentScale));
            currentScale += currentScaleIncrementerAmount;
            yield return new WaitForSeconds(timeToWait);
        }

        GoCounterClockwise();

        finishedIncreasing = true;
    }

    protected void MoveInCircle() {
        float angleToCenter = FindAngleToCenter();
        float forceVec = rb.mass * speedOfRotation * speedOfRotation / radiusOfRotation;
        rb.AddForce(new Vector2(forceVec * Mathf.Cos(angleToCenter), forceVec * Mathf.Sin(angleToCenter)));
    }

    protected float FindAngleToCenter() {
        return Mathf.Atan2((centerOfRotation.y - transform.position.y), (centerOfRotation.x - transform.position.x));
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (isLastOrb)
        {
            return;
        }

         if (collision.gameObject.tag.Equals("ProjectileEnemy") && collision.gameObject.GetComponent<ProjectileFirstBoss>().GetParent() != gameObject) 
        {
            Destroy(collision.gameObject);
            StartCoroutine(TakeDamage(damageTakenPerProjectile));
        }
    }

    IEnumerator StartAttacking() {
        while (!finishedIncreasing) {
            yield return new WaitForEndOfFrame();
        }
        while (true) {
            if (player.GetComponent<PlayerController>().IsGameEnded()) {
                break;
            }

            if (!isLastOrb) {
                AttackPlayer();
            } else {
                FinalOrbAttack();
            }
            yield return new WaitForSeconds(timePerProjectile);
        }
    }

    protected void AttackPlayer() {
        currentPlayerPosition = player.transform.position;
        float angleBetweenPlayerAndCircle = Mathf.Atan2((currentPlayerPosition.y - transform.position.y), (currentPlayerPosition.x - transform.position.x));
        GameObject firedProjectile = Instantiate(projectile, transform.position, new Quaternion(0, 0, 0, 0));
        ProjectileFirstBoss scriptOfProjectile = firedProjectile.GetComponent<ProjectileFirstBoss>();
        scriptOfProjectile.SetParent(gameObject);
        scriptOfProjectile.SetAcceleration(new Vector2(currAccelProjectile * Mathf.Cos(angleBetweenPlayerAndCircle), currAccelProjectile * Mathf.Sin(angleBetweenPlayerAndCircle)));
    }

    public void GoCounterClockwise() {
        float angleToCenter = FindAngleToCenter();
        float anglePerpToCenter = angleToCenter - Mathf.PI / 2;

        rb.velocity = new Vector2(speedOfRotation * Mathf.Cos(anglePerpToCenter), speedOfRotation * Mathf.Sin(anglePerpToCenter));
    }

    public void GoClockwise(){
        float angleToCenter = FindAngleToCenter();
        float anglePerpToCenter = angleToCenter - Mathf.PI / 2;

        rb.velocity = new Vector2(speedOfRotation * -1 * Mathf.Cos(anglePerpToCenter), speedOfRotation * Mathf.Sin(anglePerpToCenter));
    }

    protected IEnumerator TakeDamage(float damage) {
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        health -= damage;
        CheckIfDead();
        damageTakenSpriteIndicator += 1;
        GetComponent<SpriteRenderer>().sprite = damageTakenSprites[damageTakenSpriteIndicator];

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    protected void CheckIfDead() {
        if (health <= 0 || (isLastOrb && transform.localScale.x <= 0.6))
        {
            Die();
        }
    }

    protected void Die() {
        controller.GetComponent<FirstBossController>().SetOrbDead(gameObject);
        Destroy(gameObject);
    }

    public void SetCenterOfRotation(Vector2 newCenter) {
        centerOfRotation = newCenter;
    }

    public void SetRadiusOfRotation(float newRadius) {
        radiusOfRotation = newRadius;
    }

    public void SetAccelerationOfProjectiles(float newAcceleration) {
        currAccelProjectile = newAcceleration;
    }

    public void SetHealth(float newHealth) {
        health = newHealth;
    }

    public void SetTimePerProjectile(float time) {
        timePerProjectile = time;
    }

    public void SetController(GameObject newController) {
        controller = newController;
    }

    public void IncreaseDifficulty() {
        timePerProjectile -= 0.35f;
        currAccelProjectile += 1.0f;
    }

    public void SetAsFinalOrb() {
        timePerProjectile = 0.35f;
        currAccelProjectile = 14f;
        isLastOrb = true;
    }

    public void FinalOrbAttack() {
        AttackPlayer();
        transform.localScale -= new Vector3(0.05f, 0.05f, 0);
        CheckIfDead();
    }

}
