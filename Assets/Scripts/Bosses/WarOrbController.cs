using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarOrbController : OrbitingFirerer {

	// Use this for initialization
	void Start () {
        base.Start();

        SetAccelerationOfProjectiles(7f);
        SetHealth(40);
        SetTimePerProjectile(1.9f);
	}
}
