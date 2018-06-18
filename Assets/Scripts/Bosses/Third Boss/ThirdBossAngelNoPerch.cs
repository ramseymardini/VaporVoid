using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossAngelNoPerch : ThirdBossAngel {
    
	// Use this for initialization
	void Start () {
        base.Start();
        SetTimeToStayPerched(0f);
        SetDistanceBeforePerch(0f);
	}
}
