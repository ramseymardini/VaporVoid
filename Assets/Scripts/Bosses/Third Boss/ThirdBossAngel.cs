using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossAngel : FloaterController {

	// Use this for initialization
	void Start () {
        base.Start();

        SetDistanceBeforePerch(2);
        SetAccelDive(0f);
        //SetDirection("TopToBottom");
        //SetVelocityBeforePerch(new Vector2(0, -1));
        //SetAccelerationBeforePerch(new Vector2(0, 1));
        SetAccelerationBeforePerch(5);
        SetCorrectingForce(50f);
        SetCanReverse(true);
	}
}
