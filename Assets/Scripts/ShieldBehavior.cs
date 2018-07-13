using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour {

    GameObject parent;

    SoundManager soundManagerScript;
    
    float originalScaleX;
    float originalScaleY;
    float maxScaleX = 1.55f;
    float maxScaleY = 1.55f;

    float timeToDeletion = 0.35f;

    CircleCollider2D cc;

	// Use this for initialization
	void Start () {
        //GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        soundManagerScript = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        originalScaleX = transform.localScale.x;
        originalScaleY = transform.localScale.y;
        StartCoroutine(ExpandToFullScale());
        cc = GetComponent<CircleCollider2D>();
        Color decreasedOpacity = new Color(255, 255, 255);
        decreasedOpacity.a = 0.5f;
        GetComponent<SpriteRenderer>().color = decreasedOpacity;
    }

    IEnumerator ExpandToFullScale() {
        soundManagerScript.PlayShieldGainNoise();
        float timeToExpand = 0.7f;
        float currentScaleIncrementerAmount = 0.02f;
        float timeToWait = timeToExpand * currentScaleIncrementerAmount;

        float currentScale = 0; //If currentScale = 0 , cirlce at original scale. If currentScale = 1 then the circle will be at maxScale

        while (currentScale < 1) {
            transform.localScale = new Vector2(Mathf.Lerp(originalScaleX, maxScaleX, currentScale), Mathf.Lerp(originalScaleY, maxScaleY, currentScale));
            currentScale += currentScaleIncrementerAmount;
            yield return new WaitForSeconds(timeToWait);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("ProjectileEnemy")) {
            StartCoroutine(DoCollisionWithProjectile(collision));
        }

        if (collision.gameObject.tag.Equals("ExplosionEnemy") || collision.gameObject.tag.Equals("BossEnemy")) {
            StartCoroutine(DoCollisionWithExplosionOrBoss(collision));
        }

    }

    private void OnTriggerStay2D(Collider2D collision) {
        OnTriggerEnter2D(collision);
    }

    public void SetParent(GameObject newParent) {
        parent = newParent;
    }

    IEnumerator DoCollisionWithProjectile(Collider2D collision) {
        cc.isTrigger = false;
        collision.isTrigger = false;
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        Rigidbody2D colliderRB = collision.gameObject.GetComponent<Rigidbody2D>();
        float colliderVelocityX = colliderRB.velocity.x;
        float colliderVelocityY = colliderRB.velocity.y;

        //Move back the object a bit so that it can collide with the shield.
        colliderRB.position = new Vector2(colliderRB.position.x - colliderVelocityX * (Time.deltaTime * 2f), colliderRB.position.y - colliderVelocityY * (Time.deltaTime * 2f));
        collision.GetComponent<Projectile>().Deactivate();

        yield return new WaitForSeconds(timeToDeletion);

        cc.isTrigger = true;
        //collision.isTrigger = true;
        parent.GetComponent<PlayerController>().RemoveShield();
        Destroy(gameObject);
    }

    IEnumerator DoCollisionWithExplosionOrBoss(Collider2D collision) {
        cc.isTrigger = false;
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        //collision.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(timeToDeletion);
        cc.isTrigger = true;
        parent.GetComponent<PlayerController>().RemoveShield();
        Destroy(gameObject);
    }
}
