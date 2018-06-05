using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamineOrbController : OrbitingFirerer {

	// Use this for initialization
	void Start () {
        base.Start();

        SetAccelerationOfProjectiles(9f);
        SetHealth(30);
        SetTimePerProjectile(1.5f);
		
	}
}
