using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnemyController : Projectile {
    
    public Sprite blue;
    public Sprite yellow;
    public Sprite green;
    public Sprite red;

    Sprite thisSprite;

    // Use this for initialization
    void Start() {
        base.Start();
        int numColors = 4;
        int color = Random.Range(0, numColors);
        switch (color) {
            case 0:
                GetComponent<SpriteRenderer>().sprite = blue;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = yellow;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = green;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = red;
                break;
        }
    }
}
