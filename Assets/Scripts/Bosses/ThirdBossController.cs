using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossController : FloaterController {
    
    public GameObject angel;
    public GameObject gameManager;
    public GameObject levelDataManager;

    GameManager gmScript;

    GameObject leftWing;
    GameObject rightWing;

    float health;
    float damageTakenPerProjectile;
    bool inMove;

    float minXDropPosTopLocal;
    float maxXDropPosTopLocal;

    float radiusOfAngel;
    Vector2 wingPosition;
    Vector2 wingScale;
    float wingAngle;

    float angelAccel = 10;
    float angelCorrectingForce = 10;
    float angelSpeedUntilStop = 1.75f;
    float distanceTraveledBeforeStopAngels = 1.5f;

    float bossCorrectingForce = 8;
    //float speedUntilStop = 1.1f;
    float diveBombAccel = 10;

    Quaternion standardOrientation = new Quaternion(0, 0, 0, 0);

	// Use this for initialization
	protected override void Start () {
        base.Start();

        SetHealth(30);
        damageTakenPerProjectile = 1;
        leftWing = transform.GetChild(0).gameObject;
        rightWing = transform.GetChild(1).gameObject;

        gmScript = gameManager.GetComponent<GameManager>();

        radiusOfAngel = transform.localScale.x;
        wingPosition = rightWing.transform.localPosition;
        wingScale = rightWing.transform.localScale;
        maxXDropPosTopLocal = wingPosition.x + (wingScale.x / 2) * Mathf.Cos(wingAngle);
        minXDropPosTopLocal = -1 * maxXDropPosTopLocal;

        //SetPlayer(GameObject.FindGameObjectWithTag("Player"));

        SetDistanceBeforePerch(2);
        SetDirection("TopToBottom");
        SetAcceleration(new Vector2(0, diveBombAccel));
        SetVelocityBeforePerch(1f);
        SetAccelerationBeforePerch(1f);
        SetCorrectingForce(bossCorrectingForce);
	}

    private void Update()
    {
        if (inMove || gmScript.IsGameEnded() || !hasPerched) {
            return;
        }
            
        inMove = true;
        DoMove();
    }

    private void FixedUpdate()
    {
        if (!hasPerched) {
            base.FixedUpdate();
        }

    }

    private void DoMove() {
        int numMoves = 5;
        int move = Random.Range(0, numMoves);
        StartCoroutine(DiveBomb());
        return;

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
        for (float pos = gmScript.getMaxHorCircleDropPosY(); pos >= 0; pos -= 1) {
            Instantiate(angel, new Vector2(gmScript.getHorDropPos(), pos), standardOrientation);
            Instantiate(angel, new Vector2(-1 * gmScript.getHorDropPos(), pos), standardOrientation);
            Instantiate(angel, new Vector2(gmScript.getHorDropPos(), -1 * pos), standardOrientation);
            Instantiate(angel, new Vector2(-1 * gmScript.getHorDropPos(), pos), standardOrientation);
        }

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

        leftWing.SetActive(false);
        rightWing.SetActive(false);

        while (transform.position.y > -10) {
            base.FixedUpdate();
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);
        leftWing.SetActive(true);
        rightWing.SetActive(true);

        transform.position = originalPos;
        hasPerched = false;

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
        gmScript.EndThirdBoss();
        Destroy(gameObject);
    }


    public void SetHealth(float newHealth) {
        health = newHealth;
    }

    void SetGameManager (GameObject newGameManager) {
        gameManager = newGameManager;
        gmScript = gameManager.GetComponent<GameManager>();
    }


}
