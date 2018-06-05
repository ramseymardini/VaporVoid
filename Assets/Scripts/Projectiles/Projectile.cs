using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected bool deactivated;
    protected float accelerationX;
    protected float accelerationY;

    protected Rigidbody2D rb;
    protected Collider2D coll;

    protected virtual void Start() {
        SetRigidBodyAndCollider();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate() {
        UpdateVelocity();
        CheckIfDestroy();
    }

    protected void UpdateVelocity() {
        rb.AddForce(new Vector2(accelerationX * rb.mass, accelerationY * rb.mass));
        //rb.velocity += new Vector2(accelerationX, accelerationY);
    }

    public virtual void SetAcceleration(Vector2 accel) {
        accelerationX = accel.x;
        accelerationY = accel.y;

        //Debug.Log(accelerationX + " " + accelerationY);
    }

    public void SetVelocity(Vector2 velocity) {
        rb.velocity = velocity;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall") {
            coll.isTrigger = true;
        }
    }

    protected void CheckIfDestroy() {
        if (Mathf.Abs(transform.position.x) > 15 || Mathf.Abs(transform.position.y) > 10) {
            Destroy(gameObject);
        }
    }

    protected void SetRigidBodyAndCollider() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
    }

    public void Deactivate() {
        deactivated = true;
        SetAcceleration(new Vector2(0, 10));
        gameObject.tag = "Untagged";
    }
}
