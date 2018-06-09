using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossController : FloaterController {
    
    public GameObject angel;
    public GameObject gameManager;

    GameManager gameManagerScript;

    GameObject leftWing;
    GameObject rightWing;

    float health;
    float damageTakenPerProjectile;
    bool inMove;

    Vector2 defaultPosition;

    float minXDropPosTopLocal;
    float maxXDropPosTopLocal;


    float radiusOfAngel;
    Vector2 wingPosition;
    Vector2 wingScale;
    float wingAngle;

    float angelAccel;
    float angelCorrectingForce;
    float angelSpeedUntilStop;
    float distanceTraveledBeforeStopAngels;

    float speedUntilStop;
    float diveBombAccel;

	// Use this for initialization
	void Start () {
        SetHealth(30);
        damageTakenPerProjectile = 1;
        leftWing = transform.GetChild(0).gameObject;
        rightWing = transform.GetChild(1).gameObject;

        radiusOfAngel = transform.localScale.x;
        wingPosition = rightWing.transform.localPosition;
        wingScale = rightWing.transform.localScale;
        maxXDropPosTopLocal = wingPosition.x + (wingScale.x / 2) * Mathf.Cos(wingAngle);
        minXDropPosTopLocal = -1 * maxXDropPosTopLocal;

        angelAccel = 10;
        angelCorrectingForce = 10;
        angelSpeedUntilStop = 1.75f;
        distanceTraveledBeforeStopAngels = 1.5f;

        speedUntilStop = 1.1f;
        diveBombAccel = 10;
	}

    private void Update()
    {
        if (inMove || gameManagerScript.IsGameEnded()) {
            return;
        }

        inMove = true;
        DoMove();
    }

    private void DoMove() {
        int numMoves = 5;
        int move = Random.Range(0, numMoves);

        switch (move) {
            case 0:
                StartCoroutine(HorizontalAssault());
                break;
            case 1:
                StartCoroutine(VerticalAssault());
                break;
            case 2:
                StartCoroutine(RainDownRandom());
                break;
            case 3:
                StartCoroutine(RainDownAtOnce());
                break;
            case 4:
                StartCoroutine(DiveBomb());
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ProjectileEnemy")) {
            StartCoroutine(TakeDamage(damageTakenPerProjectile));
        }
    }

    IEnumerator HorizontalAssault() {
        yield return new WaitForEndOfFrame();
        inMove = false;
    }

    IEnumerator VerticalAssault() {
        yield return new WaitForEndOfFrame();
        inMove = false;
    }

    IEnumerator RainDownRandom() {
        yield return new WaitForEndOfFrame();
        inMove = false;
    }

    IEnumerator RainDownAtOnce() {
        yield return new WaitForEndOfFrame();
        inMove = false;
    }

    IEnumerator DiveBomb() {
        yield return new WaitForEndOfFrame();
        inMove = false;
    }

    private IEnumerator TakeDamage(float damage)
    {
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        health -= damage;
        CheckIfDead();
        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    public void SetHealth(float newHealth) {
        health = newHealth;
    }

    void SetGameManager (GameObject newGameManager) {
        gameManager = newGameManager;
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }


}
