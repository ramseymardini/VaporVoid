using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAndExplodeController : Projectile {
    
    float sizeToReachInExplosion = 1.2f;

    bool hasBounced;
    bool hasExploded;
    float velocityBeforeExplosionX;
    float veloictyBeforeExplosionY;


    float defaultAccel = 10f;

	// Use this for initialization
    protected override void Start () {
        base.Start();

        //SetAcceleration(new Vector2(0, defaultAccel));
	}

    protected override void FixedUpdate()
    {
        if (!hasExploded) {
            base.FixedUpdate();
            if (hasBounced) {
                
            }
        }
    }

    //protected void OnCollision

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (deactivated) {
            base.OnCollisionEnter2D(collision);
        }

        if (!hasBounced && collision.gameObject.tag.Equals("Wall")) {
            Bounce();
        }

    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        coll.isTrigger = false;
    }

    IEnumerator Bounce() {
        hasBounced = true;

        Vector2 velocityBeforeBounce = rb.velocity;
        Debug.Log(velocityBeforeBounce);

        Debug.Log("BOUNCE!");
        //while()
        yield return new WaitForFixedUpdate();
    }

    void Explode() {
        hasExploded = true;
        transform.localScale = new Vector2(sizeToReachInExplosion, sizeToReachInExplosion);
        gameObject.tag = "ExplosionEnemy";
    }
}
