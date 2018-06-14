using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAndExplodeController : Projectile {

    SoundManager soundManager;
    
    float sizeToReachInExplosion = 1.6f;

    bool isVertical;
    bool isTopToBottom;
    bool isLeftToRight;

    bool hasBounced;
    bool hasExploded;
    Vector2 velocityBeforeBounce;


    float defaultAccel = 10f;

    float bouncePercentage = 0.9f;

	// Use this for initialization
    protected override void Start () {
        base.Start();
        SetDirection("TopToBottom");
        SetAcceleration(new Vector2(2, -defaultAccel));
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	}

    protected override void FixedUpdate() {
        if (!hasExploded) {
            base.FixedUpdate();
            UpdateVelocityBeforeBounce();
            if (hasBounced) {
                CheckIfExplode();
            }
        }
    }

    //protected void OnCollision

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (deactivated) {
            base.OnCollisionEnter2D(collision);
        }

        if (!hasBounced && collision.gameObject.tag.Equals("Wall")) {
            StartCoroutine(Bounce());
        }

    }

    protected void OnTriggerExit2D(Collider2D collision) {
        coll.isTrigger = false;
    }

    IEnumerator Bounce()
    {
        hasBounced = true;

        if (isVertical) {
            rb.velocity = new Vector2(velocityBeforeBounce.x, -velocityBeforeBounce.y * bouncePercentage);
        } else if (!isVertical) {
            rb.velocity = new Vector2(-velocityBeforeBounce.x, velocityBeforeBounce.y * bouncePercentage);
        }

        coll.isTrigger = true;
        yield return new WaitForFixedUpdate();
    }

    IEnumerator Explode() {
        soundManager.PlayExplosionNoise();
        hasExploded = true;
        transform.localScale = new Vector2(sizeToReachInExplosion, sizeToReachInExplosion);
        gameObject.tag = "ExplosionEnemy";
        SetAcceleration(new Vector2(0, 0));
        rb.bodyType = RigidbodyType2D.Static;

        Color currColor = GetComponent<SpriteRenderer>().color;

        while (currColor.a > 0)
        {
            currColor.a -= 0.02f;
            GetComponent<SpriteRenderer>().color = currColor;

            if (currColor.a < 0.8) {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            yield return new WaitForSeconds(0.0000075f);
        }

        Destroy(gameObject);
    }

    void UpdateVelocityBeforeBounce() {
        if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(velocityBeforeBounce.y) || Mathf.Abs(rb.velocity.x) > Mathf.Abs(velocityBeforeBounce.x)) {
            velocityBeforeBounce = rb.velocity;
        }

        //Debug.Log(velocityBeforeBounce);
    }

    void CheckIfExplode() {
        if (!hasBounced) {
            return;
        }

        if (isTopToBottom && rb.velocity.y <= 0) {
            StartCoroutine(Explode());
        } else if (isVertical && !isTopToBottom && rb.velocity.y >= 0) {
            StartCoroutine(Explode());
        } else if (isLeftToRight && rb.velocity.x >= 0) {
            StartCoroutine(Explode());
        } else if (!isVertical && !isLeftToRight && rb.velocity.x <= 0) {
            StartCoroutine(Explode());
        }
    }

    public void SetDirection(string direction) {
        if (direction.Equals("TopToBottom")) {
            isVertical = true;
            isTopToBottom = true;
        } else if (direction.Equals("BottomToTop")) {
            isVertical = true;
            isTopToBottom = false;
        }
        else if (direction.Equals("LeftToRight")) {
            isVertical = false;
            isLeftToRight = true;
        }
        else {
            isVertical = false;
            isLeftToRight = false;
        }
    }
}
