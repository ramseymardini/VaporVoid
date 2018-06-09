using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCoordinateManager : MonoBehaviour {

    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;

    public float getTopWallPositionY() {
        return topWall.transform.position.y - topWall.transform.localScale.y / 2;
    }

    public float getBottomWallPositionY() {
        return bottomWall.transform.position.y + bottomWall.transform.localScale.y / 2;
    }

    public float getLeftWallPositionX() {
        return leftWall.transform.position.x + leftWall.transform.localScale.x / 2;
    }

    public float getRightWallPositionX() {
        return rightWall.transform.position.x - rightWall.transform.localScale.x / 2;
    }

    public float getScreenHeight() {
        return getTopWallPositionY() - getBottomWallPositionY();
    }

    public float getScreenWidth() {
        return getRightWallPositionX() - getLeftWallPositionX();
    }
}
