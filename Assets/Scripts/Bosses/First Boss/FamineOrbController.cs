using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamineOrbController : OrbitingFirerer {

	// Use this for initialization
    protected override void Start() {
        base.Start();

        SetAccelerationOfProjectiles(9f);
        SetHealth(3);
        SetTimePerProjectile(1.5f);
		
	}
}
