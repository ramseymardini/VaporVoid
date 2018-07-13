using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilenceProjectileFirstBoss : ProjectileFirstBoss {

    float rotationSpeed = 10f;

    private void Awake()
    {
    }

    // Update is called once per frame
    void FixedUpdate () {
        Vector2 dir = rb.velocity;
        //transform.rotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
        transform.rotation,
        Quaternion.LookRotation(dir),
        Time.deltaTime * rotationSpeed);
	}
}
