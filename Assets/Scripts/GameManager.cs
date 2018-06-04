using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject circleEnemy;
    public GameObject hexagonBomb;
    public GameObject lossMenu;
    public GameObject point;
    public GameObject scoreboard;
    public GameObject player;

    public GameObject firstBoss;
    public GameObject secondBoss;
    public GameObject thirdBoss;
    public GameObject fourthBoss;

    bool inMove;
    float timeTillNextMove;
    bool gameEnded;

    static readonly float topEdgeY = 5f;
    static readonly float botEdgeY = -5f;
    static readonly float screenHeight = topEdgeY - botEdgeY;
    static readonly float leftEdgeX = -6.3f;
    static readonly float rightEdgeX = 6.3f;
    static readonly float screenWidth = rightEdgeX - leftEdgeX;

    float vertDropPos = 6;
    float horDropPos = 7;
    float horDropPosFar = 13;
    float farRightCircleDropPosX = 5.5f;
    float farLeftCircleDropPosX = -5.5f;
    float minHorCircleDropPosY = -4.5f;
    float maxHorCircleDropPosY = 4.5f;

    float startingAccelVertCircles = 7.2f;
    float startingAccelHorCircles = 7.2f;
    float startingAccelDefaultCircles = 7.2f;

    float currAccelVertCircles;
    float currAccelHorCircles;
    float currAccelDefaultCircles;

    Vector2 lowerLeftCornerDropPos;
    Vector2 UpperRightCornerDropPos;
    float lowerLeftToTopRightDropAngle;

    float timeGameStarted;
    float timeSinceGameStarted;

    float timeToWaitVertCircleFall;
    float timeToWaitHorCircleFall;

    int numMoves = 11;


	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        //player = GameObject.FindGameObjectWithTag("Player");
        inMove = false;
        timeTillNextMove = 0;
        gameEnded = false;
        currAccelVertCircles = startingAccelVertCircles;
        currAccelHorCircles = startingAccelHorCircles;
        currAccelDefaultCircles = startingAccelDefaultCircles;
        timeGameStarted = Time.time;
        timeSinceGameStarted = 0;
        lowerLeftCornerDropPos = new Vector2(-horDropPos, -vertDropPos);
        UpperRightCornerDropPos = new Vector2(horDropPos, vertDropPos);
        lowerLeftToTopRightDropAngle = Mathf.Atan((UpperRightCornerDropPos.y - lowerLeftCornerDropPos.y) / (UpperRightCornerDropPos.x - lowerLeftCornerDropPos.x));
        /*Debug.Log(lowerLeftToTopRightDropAngle);
        Debug.Log("Bottom left: " + lowerLeftCornerDropPos);
        Debug.Log("Upper Right: " + UpperRightCornerDropPos);*/

    }

    // Update is called once per frame
    void Update () {
        timeSinceGameStarted = Time.time - timeGameStarted;

        if (inMove || gameEnded || timeSinceGameStarted < 1)
        {
            return;
        }
            
        currAccelVertCircles = (Mathf.Log(timeSinceGameStarted) * 0.5f) + startingAccelVertCircles;
        currAccelHorCircles = (Mathf.Log(timeSinceGameStarted) * 0.5f) + startingAccelHorCircles;
        currAccelDefaultCircles = (Mathf.Log(timeSinceGameStarted) * 0.5f) + startingAccelDefaultCircles;

        timeToWaitVertCircleFall = Mathf.Sqrt(2 * screenHeight / currAccelVertCircles);
        timeToWaitHorCircleFall = Mathf.Sqrt(2 * screenWidth / currAccelVertCircles);

        int move = UnityEngine.Random.Range(0, numMoves);
        DoMove(move);
	}

    void DoMove(int move) {
        inMove = true;
        //FirstBoss();
        return;
        switch (move)
        {
            case 0:
                StartCoroutine(BritishFlag());
                break;
            case 1:
                StartCoroutine(StairLeftToRight());
                break;
            case 2:
                StartCoroutine(StairRightToLeft());
                break;
            case 3:
                StartCoroutine(RainSides());
                break;
            case 4:
                StartCoroutine(RainRandomDown());
                break;
            case 5:
                StartCoroutine(BubbleUp());
                break;
            case 6:
                StartCoroutine(RainRandomLeft());
                break;
            case 7:
                StartCoroutine(RainRandomRight());
                break;
            case 8:
                StartCoroutine(RandomMines());
                break;
            case 9:
                StartCoroutine(StairTopToBottom());
                break;
            case 10:
                StartCoroutine(StairBottomToTop());
                break;
        }
    }

    /*private void VerticalCrossingMove()
    {
        inMove = true;
        timeTillNextMove = Time.time + shortTime;

        for (float currX = farLeftCircleDropPosX + 1; currX <= farRightCircleDropPosX; currX += 2f)
        {
            Instantiate(circleEnemy, new Vector2(currX, vertDropPos), new Quaternion(0, 0, 0, 0));
        }

        for (float currX = farLeftCircleDropPosX; currX <= farRightCircleDropPosX - 1; currX += 2f)
        {
            Instantiate(circleEnemy, new Vector2(currX, -vertDropPos), new Quaternion(0, 0, 0, 0));
        }

    }*/

    IEnumerator BritishFlag() {
        //Debug.Log("British Flag");
        float gapTime = 0.1f - Mathf.Log(timeSinceGameStarted) * 0.005f;
        int numBalls = 8;

        for (int i = 0; i < numBalls; i++) {
            GameObject verticalFaller = Instantiate(circleEnemy, new Vector2(0, vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject verticalRiser = Instantiate(circleEnemy, new Vector2(0, -vertDropPos), new Quaternion(0, 0, 0, 0));
            verticalFaller.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, -currAccelVertCircles));
            verticalRiser.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, currAccelVertCircles));

            GameObject circleLeftToRight = Instantiate(circleEnemy, new Vector2(-horDropPos, 0), new Quaternion(0, 0, 0, 0));
            GameObject circleRightToLeft = Instantiate(circleEnemy, new Vector2(horDropPos, 0), new Quaternion(0, 0, 0, 0));
            circleLeftToRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelHorCircles, 0));
            circleRightToLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelHorCircles, 0));

            GameObject circleLowerLeft = Instantiate(circleEnemy, new Vector2(-horDropPos, -vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject circleUpperLeft = Instantiate(circleEnemy, new Vector2(-horDropPos, vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject circleLowerRight = Instantiate(circleEnemy, new Vector2(horDropPos, -vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject circleUpperRight = Instantiate(circleEnemy, new Vector2(horDropPos, vertDropPos), new Quaternion(0, 0, 0, 0));


            //Cos and Sin to make them go from corner to corner and not a 45 degree angle since map is not a square
            circleLowerLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelDefaultCircles * Mathf.Cos(lowerLeftToTopRightDropAngle), currAccelDefaultCircles * Mathf.Sin(lowerLeftToTopRightDropAngle)));
            circleUpperLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelDefaultCircles * Mathf.Cos(lowerLeftToTopRightDropAngle), -currAccelDefaultCircles * Mathf.Sin(lowerLeftToTopRightDropAngle)));
            circleLowerRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelDefaultCircles * Mathf.Cos(lowerLeftToTopRightDropAngle), currAccelDefaultCircles * Mathf.Sin(lowerLeftToTopRightDropAngle)));
            circleUpperRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelDefaultCircles * Mathf.Cos(lowerLeftToTopRightDropAngle), -currAccelDefaultCircles * Mathf.Sin(lowerLeftToTopRightDropAngle)));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator StairLeftToRight() {
        //Debug.Log("Rain Left");
        float gapTime = 0.1f - Mathf.Log(timeSinceGameStarted) * 0.005f;

        for (float currX = farLeftCircleDropPosX; currX <= farRightCircleDropPosX - 1; currX += 1f) {
            GameObject verticalFaller = Instantiate(circleEnemy, new Vector2(currX, vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject verticalRiser = Instantiate(circleEnemy, new Vector2(currX, -vertDropPos), new Quaternion(0, 0, 0, 0));
            verticalFaller.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, -currAccelVertCircles));
            verticalRiser.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, currAccelVertCircles));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator StairRightToLeft() {
        //Debug.Log("Rain Right");
        float gapTime = 0.1f - Mathf.Log(timeSinceGameStarted) * 0.005f;

        for (float currX = farRightCircleDropPosX; currX >= farLeftCircleDropPosX + 1; currX -= 1) {
            GameObject verticalFaller = Instantiate(circleEnemy, new Vector2(currX, vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject verticalRiser = Instantiate(circleEnemy, new Vector2(currX, -vertDropPos), new Quaternion(0, 0, 0, 0));
            verticalFaller.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, -currAccelVertCircles));
            verticalRiser.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, currAccelVertCircles));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator RainSides() {
        //Debug.Log("Rain Sides");
        float gapTime = 0.1f - Mathf.Log(timeSinceGameStarted) * 0.005f;

        for (float currX = farRightCircleDropPosX; currX > 0.5f; currX -= 1) {
            if (currX > farRightCircleDropPosX - 3)
            {
                GameObject circleLeftToRight = Instantiate(circleEnemy, new Vector2(-horDropPos, 0), new Quaternion(0, 0, 0, 0));
                GameObject circleRightToLeft = Instantiate(circleEnemy, new Vector2(horDropPos, 0), new Quaternion(0, 0, 0, 0));
                circleLeftToRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelHorCircles, 0));
                circleRightToLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelHorCircles, 0));
            }
            GameObject verticalFaller_1 = Instantiate(circleEnemy, new Vector2(currX, vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject verticalRiser_1 = Instantiate(circleEnemy, new Vector2(currX, -vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject verticalFaller_2 = Instantiate(circleEnemy, new Vector2(-currX, vertDropPos), new Quaternion(0, 0, 0, 0));
            GameObject verticalRiser_2 = Instantiate(circleEnemy, new Vector2(-currX, -vertDropPos), new Quaternion(0, 0, 0, 0));
            verticalFaller_1.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, -currAccelVertCircles));
            verticalFaller_2.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, -currAccelVertCircles));
            verticalRiser_1.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, currAccelVertCircles));
            verticalRiser_2.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, currAccelVertCircles));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator RainRandomDown() {
        //Debug.Log("Rain Random Down");
        float gapTime = 0.11f - Mathf.Log(timeSinceGameStarted) * 0.015f;
        int numBalls = (int) Mathf.Floor(2.5f / gapTime);

        for (int i = 0; i < numBalls; i++) {
            GameObject verticalFaller_1 = Instantiate(circleEnemy, new Vector2(UnityEngine.Random.Range(farLeftCircleDropPosX, farRightCircleDropPosX), vertDropPos), new Quaternion(0, 0, 0, 0));
            verticalFaller_1.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, -currAccelVertCircles));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator BubbleUp() {
        //Debug.Log("Bubble Up");
        float gapTime = 0.11f - Mathf.Log(timeSinceGameStarted) * 0.015f;
        int numBalls = (int) Mathf.Floor(2.5f / gapTime);

        for (int i = 0; i < numBalls; i++) {
            GameObject verticalRiser_1 = Instantiate(circleEnemy, new Vector2(UnityEngine.Random.Range(farLeftCircleDropPosX, farRightCircleDropPosX), -vertDropPos), new Quaternion(0, 0, 0, 0));
            verticalRiser_1.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(0, currAccelVertCircles));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator RainRandomLeft() {
        //Debug.Log("Rain Random Left");
        float gapTime = 0.11f - Mathf.Log(timeSinceGameStarted) * 0.015f;
        int numBalls = (int) Mathf.Floor(2.5f / gapTime);

        for (int i = 0; i < numBalls; i++) {
            GameObject circleLeftToRight = Instantiate(circleEnemy, new Vector2(-horDropPos, UnityEngine.Random.Range(minHorCircleDropPosY, maxHorCircleDropPosY)), new Quaternion(0, 0, 0, 0));
            circleLeftToRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelHorCircles, 0));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitHorCircleFall);
        inMove = false;
    }

    IEnumerator RainRandomRight() {
        //Debug.Log("Rain Random Right");
        float gapTime = 0.11f - Mathf.Log(timeSinceGameStarted) * 0.015f;
        int numBalls = (int) Mathf.Floor(2.5f / gapTime);

        for (int i = 0; i < numBalls; i++) {
            GameObject circleRightToLeft = Instantiate(circleEnemy, new Vector2(horDropPos, UnityEngine.Random.Range(minHorCircleDropPosY, maxHorCircleDropPosY)), new Quaternion(0, 0, 0, 0));
            circleRightToLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelHorCircles, 0));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitHorCircleFall);
        inMove = false;
    }

    IEnumerator RandomMines() {
        //Debug.Log("Random Mines");
        float gapTime = 0.8f;
        int numMines = 6;
        float minMinePosX = -5.5f;
        float maxMinePosX = 5.5f;
        float minMinePosY = -4.25f;
        float maxMinePosY = 4.25f;
        Vector2 nextMinePos;

        for (int i = 0; i < numMines; i++) {
            do {
                nextMinePos = new Vector2(UnityEngine.Random.Range(minMinePosX, maxMinePosX), UnityEngine.Random.Range(minMinePosY, maxMinePosY));
            } while (!isValidPos(nextMinePos));

            Instantiate(hexagonBomb, nextMinePos, new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(gapTime);
        }

        inMove = false;
    }

    bool isValidPos(Vector2 pos) {
        bool isValidPlayer;
        if (player != null) {
            isValidPlayer = Mathf.Abs(pos.x - player.transform.position.x) > 0.3f || Mathf.Abs(pos.y - player.transform.position.y) > 0.3f;
        }
        else {
            isValidPlayer = false;
        }
        bool isValidPoint = Mathf.Abs(pos.x - point.transform.position.x) > 0.3f || Mathf.Abs(pos.y - point.transform.position.y) > 0.3f;

        return (isValidPlayer && isValidPoint);
    }

    IEnumerator StairTopToBottom() {
        float gapTime = 0.1f - Mathf.Log(timeSinceGameStarted) * 0.005f;

        for (float currY = maxHorCircleDropPosY; currY > minHorCircleDropPosY; currY -= 1) {
            GameObject circleLeftToRight = Instantiate(circleEnemy, new Vector2(-horDropPos, currY), new Quaternion(0, 0, 0, 0));
            GameObject circleRightToLeft = Instantiate(circleEnemy, new Vector2(horDropPos, currY), new Quaternion(0, 0, 0, 0));
            circleLeftToRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelHorCircles, 0));
            circleRightToLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelHorCircles, 0));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator StairBottomToTop() {
        float gapTime = 0.1f - Mathf.Log(timeSinceGameStarted) * 0.005f;

        for (float currY = minHorCircleDropPosY; currY < maxHorCircleDropPosY; currY += 1) {
            GameObject circleLeftToRight = Instantiate(circleEnemy, new Vector2(-horDropPos, currY), new Quaternion(0, 0, 0, 0));
            GameObject circleRightToLeft = Instantiate(circleEnemy, new Vector2(horDropPos, currY), new Quaternion(0, 0, 0, 0));
            circleLeftToRight.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(currAccelHorCircles, 0));
            circleRightToLeft.GetComponent<CircleEnemyController>().SetAcceleration(new Vector2(-currAccelHorCircles, 0));
            yield return new WaitForSeconds(gapTime);
        }

        yield return new WaitForSeconds(timeToWaitVertCircleFall);
        inMove = false;
    }

    IEnumerator DiagonalCirclesLeftToRight() {
        float gapTime = 0.3f - Mathf.Log(timeSinceGameStarted) * 0.005f;
        /*for (float currX = minHorCircleDropPosY; currY < maxHorCircleDropPosY; currY += 1)
        {
            new Vector2(currAccelDefaultCircles * Mathf.Cos(lowerLeftToTopRightDropAngle), currAccelDefaultCircles * Mathf.Sin(lowerLeftToTopRightDropAngle)));
        }*/
        yield return new WaitForSeconds(gapTime);
    }

    void FirstBoss() {
        GameObject firstBossController = Instantiate(firstBoss, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        firstBossController.GetComponent<FirstBossController>().SetGameManager(gameObject);
    }

    void SecondBoss() {
        GameObject secondBossController = Instantiate(secondBoss, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
    }

    public void EndFirstBoss() {
        StartCoroutine(EndFirstBossHelper());
    }

    IEnumerator EndFirstBossHelper() {
        yield return new WaitForSeconds(0.5f);
        inMove = false;
    }

    public void EndGame() {
        gameEnded = true;
        Cursor.visible = true;
        lossMenu.SetActive(true);
        SendEndGameMessages();
    }

    void SendEndGameMessages() {
        scoreboard.GetComponent<Scoreboard>().ChangeHighScore();
    }

    public float GetTimeSinceGameStarted() {
        return timeSinceGameStarted;
    }

}
