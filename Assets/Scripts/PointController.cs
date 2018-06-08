using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour {

    float minX = -5f;
    float maxX = 5f;
    float minY = -3.7f;
    float maxY = 3.7f;

	// Use this for initialization
	void Start () {
        Reposition();
	}
	
    public void Reposition() {
        transform.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public void SetIsWorthPoint(bool worth) {
        if (worth) {
            gameObject.tag = "Point";
        } else {
            gameObject.tag = "ShieldPoint";
        }
    }

    public bool GetIsWorthPoint() {
        return gameObject.tag.Equals("Point");
    }
}
