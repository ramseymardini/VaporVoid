using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour {

    public GameObject player;

    float minX = -5f;
    float maxX = 5f;
    float minY = -3.7f;
    float maxY = 3.7f;

    bool valid;

	// Use this for initialization
	void Start () {
        Reposition();
	}
	
    public void Reposition() {
        Vector2 newPosition = player.transform.position;

        while (Mathf.Sqrt(Mathf.Pow(newPosition.x - player.transform.position.x, 2) + Mathf.Pow(newPosition.y - player.transform.position.y, 2)) < 4.5f) {
            newPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        }

        transform.position = newPosition;
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
