using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOrbController : OrbitingFirerer {

	// Use this for initialization
    protected override void Start() {
        base.Start();

        SetAccelerationOfProjectiles(10.5f);
        SetHealth(20);
        SetTimePerProjectile(1f);
		
	}
}
