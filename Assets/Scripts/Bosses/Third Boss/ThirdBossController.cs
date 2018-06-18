using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossController : FloaterController {

    /*Things to do:
     * Make it so projectiles don't damage the boss originally
     * Make the projectiles go correctly across the wing */
    
    public GameObject angel;
    public GameObject angelNoPerch;
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

    float radiusOfBody;
    Vector2 wingPosition;
    Vector2 wingScale;
    float wingAngle;

    /*(float angelCorrectingForce = 100;
    float angelSpeedUntilStop = 1.75f;
    float distanceBeforePerchAngels = 4f;*/

    float bossSpeedBeforePerch = 1f;
    float bossCorrectingForce = 40;
    //float speedUntilStop = 1.1f;

    Quaternion standardOrientation = new Quaternion(0, 0, 0, 0);

	// Use this for initialization
	protected override void Start () { 
        base.Start();
        SetHealth(300);
        damageTakenPerProjectile = 1;
        leftWing = transform.GetChild(0).gameObject;
        rightWing = transform.GetChild(1).gameObject;

        gmScript = gameManager.GetComponent<GameManager>();

        radiusOfBody = transform.localScale.x;
        wingPosition = rightWing.transform.localPosition;
        wingScale = rightWing.transform.localScale;
        maxXDropPosTopLocal = wingPosition.x + (wingScale.x / 2) * Mathf.Cos(wingAngle);
        minXDropPosTopLocal = -1 * maxXDropPosTopLocal;

        SetPlayer(GameObject.FindGameObjectWithTag("Player"));

        SetDistanceBeforePerch(2.5f);
        SetAccelDive(6f);
        //SetDirection("TopToBottom");
        SetSpeedBeforePerch(bossSpeedBeforePerch);
        //SetAccelerationBeforePerch(new Vector2(0, 1));
        SetAccelerationBeforePerch(5);
        SetCorrectingForce(bossCorrectingForce);
        //Debug.Log("Start");
	}

    private void Update() {
        //Debug.Log("Update!");
        if (inMove || gmScript.IsGameEnded() || !hasPerched) {
            //Debug.Log(hasPerched);
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
        else {
            GoBackAndForth();
        }

    }

    private void DoMove() {
        int numMoves = 5;
        int move = Random.Range(0, numMoves);
        StartCoroutine(RainDownAtOnce());
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
            //Destroy(collision.gameObject);
        }
    }

    IEnumerator HorizontalAssault() {
        //Debug.Log("Horizontal Assault!");

        /*GameObject angelTopRight = Instantiate(angel, new Vector2(gmScript.getHorDropPos(), gmScript.getMaxHorCircleDropPosY() - 2), standardOrientation);
        FloaterController angelTopRightController = angelTopRight.GetComponent<FloaterController>();*/
        for (float pos = gmScript.getMaxHorCircleDropPosY() - 1; pos >= 0; pos -= 2)
        {
            //Debug.Log(pos);
            GameObject angelTopRight = Instantiate(angel, new Vector2(gmScript.getHorDropPos(), pos), standardOrientation);
            GameObject angelTopLeft = Instantiate(angel, new Vector2(-1 * gmScript.getHorDropPos(), pos), standardOrientation);
            GameObject angelBotRight = Instantiate(angel, new Vector2(gmScript.getHorDropPos(), -1 * pos), standardOrientation);
            GameObject angelBotLeft = Instantiate(angel, new Vector2(-1 * gmScript.getHorDropPos(), -1 * pos), standardOrientation);
            //yield return new WaitForFixedUpdate();
        }
        //Debug.Log("Done!");
        yield return new WaitForSeconds(1.0f);
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
        float posYAngel;
        //Debug.Log(wingPosition.x + wingScale.x / 2);
        for (float i = transform.position.x + 0.5f; i <= transform.position.x + wingPosition.x + wingScale.x  *  transform.localScale.x / 2; i += 0.8f) {
            posYAngel = i * Mathf.Sin(wingAngle);
            Instantiate(angelNoPerch, transform.position + new Vector3(i, posYAngel, 0), standardOrientation);
            Instantiate(angelNoPerch, transform.position + new Vector3(-i, posYAngel, 0), standardOrientation);
            yield return new WaitForSeconds(0.7f);
        }

        yield return new WaitForSeconds(5f);
        inMove = false;
    }

    IEnumerator DiveBomb() {

        leftWing.SetActive(false);
        rightWing.SetActive(false);

        while (transform.position.y > -10) {
            base.FixedUpdate();
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(-4, rb.velocity.y));
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);
        leftWing.SetActive(true);
        rightWing.SetActive(true);

        transform.position = originalPos;
        ResetConditionals();

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

    void GoBackAndForth() {
        return;
    }


}
