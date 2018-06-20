using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarOrbController : OrbitingFirerer {

	// Use this for initialization
    protected override void Start() {
        base.Start();

        SetAccelerationOfProjectiles(7f);
        SetHealth(4);
        SetTimePerProjectile(1.9f);
	}
}
