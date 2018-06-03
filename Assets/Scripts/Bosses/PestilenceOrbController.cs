using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilenceOrbController : OrbitingFirerer {

	// Use this for initialization
	void Start () {
        base.Start();

        SetAccelerationOfProjectiles(8f);
        SetHealth(30);
        SetTimePerProjectile(1.7f);
	}
}
