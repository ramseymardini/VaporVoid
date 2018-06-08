using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossController : FloaterController {

    public GameObject thirdBossAngel;
    public GameObject gameManager;

    GameManager gameManagerScript; 

    float health;
    float damageTakenPerProjectile;
    bool inMove;

	// Use this for initialization
	void Start () {
        SetHealth(30);
        damageTakenPerProjectile = 1;
	}

    private void Update()
    {
        if (inMove || gameManagerScript.IsGameEnded()) {
            return;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ProjectileEnemy")) {
            StartCoroutine(TakeDamage(damageTakenPerProjectile));
        }
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
