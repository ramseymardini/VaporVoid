using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilenceOrbController : OrbitingFirerer {

	// Use this for initialization
    protected override void Start () {
        base.Start();

        SetAccelerationOfProjectiles(8f);
        SetHealth(3);
        SetTimePerProjectile(1.7f);
	}
}
