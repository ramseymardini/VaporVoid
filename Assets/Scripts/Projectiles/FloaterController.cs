using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterController : Projectile {

    bool hasPerched;
    bool isSlowing;

    bool isVertical;
    bool isTopToBottom;
    bool isLeftToRight;

    float correctingForce = 5f;

    readonly float DEFAULT_ACCELERATION_X = 0f;
    readonly float DEFAULT_ACCELERATION_Y = 10f;

    float velocityBeforePerch = 5f;
    float distanceBeforePerch = 2.5f;
    float accelerationToSlowBeforePerch = 5f;
    float distanceToStartSlowing;
    float timeToStayPerched = 0.5f;

    Vector2 originalPos;

    float originalAccelX;
    float originalAccelY;

    bool accelSet;

    GameObject player;

    protected override void Start() {
        base.Start();
        accelerationX = DEFAULT_ACCELERATION_X;
        accelerationY = DEFAULT_ACCELERATION_Y;
        originalPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        rb.velocity = new Vector2(0, -velocityBeforePerch);
        UpdateDistanceToStartSlowing();
    }
   
    protected override void FixedUpdate() {
        if (!hasPerched) {
            if (!isSlowing && FindDistanceTraveled() > distanceToStartSlowing) {
                StartCoroutine(StartSlowing());
            }
            //Debug.Log(Mathf.Sqrt(Mathf.Pow(transform.position.x - originalPos.x, 2) + Mathf.Pow(transform.position.y - originalPos.y, 2)) + " " + distanceToStartSlowing);
            return;
        }
        base.FixedUpdate();
        float newAccelX = originalAccelX + correctingForce * Mathf.Cos(FindAngleToPlayer());
        float newAccelY = originalAccelY + correctingForce * Mathf.Sin(FindAngleToPlayer());
        SetAcceleration(new Vector2(newAccelX, newAccelY));
    }

    public override void SetAcceleration(Vector2 newAccel)
    {
        base.SetAcceleration(newAccel);

        if (!accelSet)
        {
            originalAccelX = newAccel.x;
            originalAccelY = newAccel.y;
        }

        accelSet = true;
    }

    float FindDistanceTraveled() {
        //Debug.Log(Mathf.Sqrt(Mathf.Pow(transform.position.x - originalPos.x, 2) + Mathf.Pow(transform.position.y - originalPos.y, 2)));
        return Mathf.Sqrt(Mathf.Pow(transform.position.x - originalPos.x, 2) + Mathf.Pow(transform.position.y - originalPos.y, 2));
    }


    float FindAngleToPlayer() {
        Vector2 currentPlayerPosition = player.transform.position;
        return Mathf.Atan2((currentPlayerPosition.y - transform.position.y), (currentPlayerPosition.x - transform.position.x));
    }

    IEnumerator StartSlowing() {
        isSlowing = true;
        while (Mathf.Abs(rb.velocity.x) > 0.001f || Mathf.Abs(rb.velocity.y) > 0.001f) {
            Debug.Log(rb.velocity);
            if (rb.velocity.y > 0) {
                rb.AddForce(new Vector2(0, -rb.mass * accelerationToSlowBeforePerch));
            }

            if (rb.velocity.y < 0) {
                rb.AddForce(new Vector2(0, rb.mass * accelerationToSlowBeforePerch));
            }

            if (rb.velocity.x > 0) {
                rb.AddForce(new Vector2(-rb.mass * accelerationToSlowBeforePerch, 0));
            }

            if (rb.velocity.x < 0) {
                rb.AddForce(new Vector2(rb.mass * accelerationToSlowBeforePerch, 0));
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(timeToStayPerched);

        //Debug.Log("Done!");
        hasPerched = true;

    }


    public void SetCorrectingForce(float newCorrectingForce) {
        correctingForce = newCorrectingForce;
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void SetType(string type) {
        if (type.Equals("TopToBottom")) {
            isVertical = true;
            isTopToBottom = true;
        } else if (type.Equals("BottomToTop")) {
            isVertical = true;
            isTopToBottom = false;
        } else if (type.Equals("LeftToRight")) {
            isVertical = false;
            isLeftToRight = true;
        } else {
            isVertical = false;
            isLeftToRight = false;
        }
    }

    public void SetDistanceBeforePerch(float newDistance) {
        distanceBeforePerch = newDistance;
        UpdateDistanceToStartSlowing();
    }

    private void UpdateDistanceToStartSlowing() {
        float timeToStop = velocityBeforePerch / accelerationToSlowBeforePerch;
        distanceToStartSlowing = distanceBeforePerch - (Mathf.Abs(velocityBeforePerch * timeToStop) - Mathf.Abs(0.5f * accelerationToSlowBeforePerch * timeToStop * timeToStop));
        Debug.Log(distanceToStartSlowing);
    }
}
