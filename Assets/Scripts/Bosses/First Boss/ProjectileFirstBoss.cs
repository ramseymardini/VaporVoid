using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFirstBoss : Projectile
{

    protected GameObject parent;

    protected virtual void Start() {
        base.Start();
        rb.AddTorque(50f);
    }

    public void SetParent(GameObject newParent) {
        parent = newParent;
    }

    public GameObject GetParent() {
        return parent;
    }
}
