using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public GameObject shield;
    public Sprite noPointsShield;
    public Sprite onePointShield;
    public Sprite twoPointsShield;
    public Sprite threePointsShield;
    public Sprite fourPointsShield;

    readonly float MAX_VELOCITY = 1.0f;
    readonly float COOLDOWN_POINT = 0.05f;

    //Quaternion standardOrientation = new Quaternion(0, 0, 0, 0);

    Rigidbody2D rb;
    public GameObject gm;
    GameObject scoreboard;

    bool gameEnded;
    int playerSpeed;

    //float playerCorrectionFactorX = 0.3f;
    //float playerCorrectionFactorY = 1.3f;

    int shieldCounter;
    bool hasShield;

    bool canPickUpPoint;


    // Use this for initialization
    void Start() {
        SetOriginalPosition();
        rb = GetComponent<Rigidbody2D>();
        //gm = GameObject.FindGameObjectWithTag("GameController");
        playerSpeed = PlayerPrefs.GetInt("Mouse Sensitivity");
        scoreboard = GameObject.FindGameObjectWithTag("Score");
        gameEnded = false;
        shieldCounter = 0;
        hasShield = false;
        rb.freezeRotation = true;
        canPickUpPoint = true;
    }

    void Update() {
        CheckShield();
    }

    void FixedUpdate() {
        if (gameEnded) {
            return;
        }

        SetVelocity();
	}

    private void OnTriggerEnter2D (Collider2D collision) {
        if (gameEnded) {
            return;
        }

        if (collision.gameObject.tag.Equals("ProjectileEnemy") || collision.gameObject.tag.Equals("ExplosionEnemy") || collision.gameObject.tag.Equals("BossEnemy")) {
            //Handheld.Vibrate();
            if (hasShield) {
                return;
            }
            gm.GetComponent<GameManager>().EndGame();
            SetGameEnded();
        }

        if (collision.gameObject.tag.Equals("Point") && canPickUpPoint) {
            if (shieldCounter < 4)
            {
                shieldCounter++;
            }
            canPickUpPoint = false;
            StartCoroutine(WaitCooldownPoint());
            UpdateSprite();
            scoreboard.GetComponent<Scoreboard>().IncrementScore();
            collision.gameObject.GetComponent<PointController>().Reposition();
        }

        if (collision.gameObject.tag.Equals("ShieldPoint")) {
            if (shieldCounter < 4)
            {
                shieldCounter++;
            }
            UpdateSprite();
            collision.gameObject.GetComponent<PointController>().Reposition();
        }

    }

    private void OnTriggerStay2D(Collider2D collision) {
        OnTriggerEnter2D(collision);
        /*if (gameEnded)
        {
            return;
        }

        if (collider.gameObject.tag.Equals("Enemy"))
        {
            gm.SendMessage("EndGame");
        }

        if (collider.gameObject.tag.Equals("Point"))
        {
            shieldCounter++;
            scoreboard.SendMessage("IncrementScore");
            collider.gameObject.SendMessage("Reposition");
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    private void SetOriginalPosition() {
        /*Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Min(6, mousePos.x);
        mousePos.x = Mathf.Max(-6, mousePos.x);
        mousePos.y = Mathf.Min(10, mousePos.y);
        mousePos.y = Mathf.Max(-10, mousePos.y);
        transform.position = mousePos;*/

        transform.position = new Vector2(0, 0);
    }

    private void SetVelocity() {
        /*float touchX;
        float touchY;
        if (Input.touchCount > 0)
        {
            touchX = Input.GetTouch(0).deltaPosition.x * Input.GetTouch(0).deltaTime;
            touchY = Input.GetTouch(0).deltaPosition.y * Input.GetTouch(0).deltaTime;
        }
        else
        {
            touchX = 0;
            touchY = 0;
        }*/

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        //rb.velocity = new Vector2(touchX * playerSpeed, touchY * playerSpeed);
        rb.velocity = new Vector2(Mathf.Min(mouseX, MAX_VELOCITY) * playerSpeed, Mathf.Min(mouseY, MAX_VELOCITY) * playerSpeed);

    }

    IEnumerator WaitCooldownPoint() {
        yield return new WaitForSeconds(COOLDOWN_POINT);
        canPickUpPoint = true;
    }

    void UpdateSprite() {
        switch (shieldCounter) {
            case 0:
                GetComponent<SpriteRenderer>().sprite = noPointsShield;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = onePointShield;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = twoPointsShield;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = threePointsShield;
                break;
            case 4:
                GetComponent<SpriteRenderer>().sprite = fourPointsShield;
                break;
        }
    }

    void CheckShield() {
        if (shieldCounter >= 4 && !hasShield) {
            CreateShield();
        }
    }

    void CreateShield() {
        GameObject newShield = Instantiate(shield, transform, false);
        newShield.GetComponent<ShieldBehavior>().SetParent(gameObject);
        hasShield = true;
        shieldCounter = 0;
        UpdateSprite();
    }

    public void RemoveShield() {
        hasShield = false;
    }

    void SetGameEnded() {
        gameEnded = true;
        gameObject.SetActive(false);
    }


}
