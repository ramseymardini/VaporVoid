﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float accelerationX;
    protected float accelerationY;

    protected Rigidbody2D rb;
    protected CircleCollider2D cc;

    protected void Start() {
        SetRigidBodyAndCollider();
    }

    // Update is called once per frame
    protected void FixedUpdate() {
        UpdateVelocity();
        CheckIfDestroy();
    }

    protected void UpdateVelocity() {
        rb.AddForce(new Vector2(accelerationX * rb.mass, accelerationY * rb.mass));
        //rb.velocity += new Vector2(accelerationX, accelerationY);
    }

    public void SetAcceleration(Vector2 accel) {
        accelerationX = accel.x;
        accelerationY = accel.y;

        //Debug.Log(accelerationX + " " + accelerationY);
    }

    public void SetVelocity(Vector2 velocity) {
        rb.velocity = velocity;
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall") {
            cc.isTrigger = true;
        }
    }

    protected void CheckIfDestroy() {
        if (Mathf.Abs(transform.position.x) > 15 || Mathf.Abs(transform.position.y) > 10) {
            Destroy(gameObject);
        }
    }

    protected void SetRigidBodyAndCollider() {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
    }
}
