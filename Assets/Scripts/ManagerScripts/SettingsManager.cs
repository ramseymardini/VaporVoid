using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    
    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public Camera camera;
    string platform;
    int screenWidthPixels;
    int screenHeightPixels;


    private void Start()
    {
        PlaceWalls();
        platform = Application.platform.ToString();
    }

    public void PlaceWalls() {
        screenWidthPixels = Screen.width;
        screenHeightPixels = Screen.height;
        Debug.Log(camera.ScreenToWorldPoint(new Vector3(screenWidthPixels, screenHeightPixels, 0)));
        topWall.transform.position = camera.ScreenToWorldPoint(new Vector3(screenWidthPixels / 2, screenHeightPixels, 0)) + new Vector3(0, topWall.transform.localScale.y / 2, 0);
        rightWall.transform.position = camera.ScreenToWorldPoint(new Vector3(screenWidthPixels, screenHeightPixels / 2, 0)) + new Vector3(rightWall.transform.localScale.x / 2, 0, 0);
        bottomWall.transform.position = -camera.ScreenToWorldPoint(new Vector3(screenWidthPixels / 2, screenHeightPixels, 0)) - new Vector3(0, bottomWall.transform.localScale.y / 2, 0);
        leftWall.transform.position = -camera.ScreenToWorldPoint(new Vector3(screenWidthPixels, screenHeightPixels / 2, 0)) - new Vector3(leftWall.transform.localScale.x / 2, 0, 0);


        //topWall.transform.position.y = screenHe

    }

    public float GetTopWallPositionY() {
        return topWall.transform.position.y - topWall.transform.localScale.y / 2;
    }

    public float GetBottomWallPositionY() {
        return bottomWall.transform.position.y + bottomWall.transform.localScale.y / 2;
    }

    public float GetLeftWallPositionX() {
        return leftWall.transform.position.x + leftWall.transform.localScale.x / 2;
    }

    public float GetRightWallPositionX() {
        return rightWall.transform.position.x - rightWall.transform.localScale.x / 2;
    }

    public float GetScreenHeight() {
        return GetTopWallPositionY() - GetBottomWallPositionY();
    }

    public float GetScreenWidth() {
        return GetRightWallPositionX() - GetLeftWallPositionX();
    }

    public string GetPlatform() {
        return platform;
    }
}
