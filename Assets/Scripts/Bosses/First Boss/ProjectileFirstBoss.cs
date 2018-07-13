using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFirstBoss : Projectile
{

    protected GameObject parent;

    public void SetParent(GameObject newParent) {
        parent = newParent;
    }

    public GameObject GetParent() {
        return parent;
    }
}
