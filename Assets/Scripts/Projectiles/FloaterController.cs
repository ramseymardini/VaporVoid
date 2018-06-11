using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterController : Projectile {

    protected bool hasPerched;
    protected bool isSlowing;

    /*bool isVertical;
    bool isTopToBottom;
    bool isLeftToRight;*/

    protected float correctingForce = 5f;

    readonly float DEFAULT_ACCELERATION_X = 0f;
    readonly float DEFAULT_ACCELERATION_Y = -10f;

    protected Vector2 velocityBeforePerch = new Vector2(0, -10);
    protected float distanceBeforePerch = 2.5f;
    protected Vector2 accelerationToSlowBeforePerch = new Vector2(5, 5);
    protected float distanceToStartSlowing;
    protected float timeToStayPerched = 0.5f;

    protected Vector2 originalPos;

    protected float originalAccelX;
    protected float originalAccelY;

    protected bool accelSet;
    protected bool isEnabled;

    protected GameObject player;

    protected override void Start() {
        base.Start();
        /*accelerationX = DEFAULT_ACCELERATION_X;
        accelerationY = DEFAULT_ACCELERATION_Y;*/
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateDistanceToStartSlowing();
        SetPositionAsOriginalPosition();
    }
   
    protected override void FixedUpdate() {
        if (!hasPerched) {
            if (!isSlowing && FindDistanceTraveled() > distanceToStartSlowing) {
                StartCoroutine(StartSlowing());
            } else if (!isSlowing){
                MaintainVelocityBeforePerch();
            }
            //Debug.Log(Mathf.Sqrt(Mathf.Pow(transform.position.x - originalPos.x, 2) + Mathf.Pow(transform.position.y - originalPos.y, 2)) + " " + distanceToStartSlowing);
            return;
        }
        base.FixedUpdate();
        float newAccelX = originalAccelX + correctingForce * Mathf.Cos(FindAngleToPlayer());
        float newAccelY = originalAccelY + correctingForce * Mathf.Sin(FindAngleToPlayer());
        SetAcceleration(new Vector2(newAccelX, newAccelY));
    }

    public override void SetAcceleration(Vector2 accel)
    {
        base.SetAcceleration(accel);

        if (!accelSet)
        {
            originalAccelX = accel.x;
            originalAccelY = accel.y;
        }

        accelSet = true;
    }

    protected float FindDistanceTraveled() {
        //Debug.Log("Distance Traveled " + Mathf.Sqrt(Mathf.Pow(transform.position.x - originalPos.x, 2) + Mathf.Pow(transform.position.y - originalPos.y, 2)));
        return Mathf.Sqrt(Mathf.Pow(transform.position.x - originalPos.x, 2) + Mathf.Pow(transform.position.y - originalPos.y, 2));
    }


    protected float FindAngleToPlayer() {
        Vector2 currentPlayerPosition = player.transform.position;
        return Mathf.Atan2((currentPlayerPosition.y - transform.position.y), (currentPlayerPosition.x - transform.position.x));
    }

    protected IEnumerator StartSlowing() {
        isSlowing = true;
        //Debug.Log("Started slowing at " + FindDistanceTraveled());

        while (Mathf.Abs(rb.velocity.x) > 0.001f || Mathf.Abs(rb.velocity.y) > 0.001f) {
            if (rb.velocity.y > 0) {
                rb.AddForce(new Vector2(0, -rb.mass * accelerationToSlowBeforePerch.y));
            }

            if (rb.velocity.y < 0) {
                rb.AddForce(new Vector2(0, rb.mass * accelerationToSlowBeforePerch.y));
            }

            if (rb.velocity.x > 0) {
                rb.AddForce(new Vector2(-rb.mass * accelerationToSlowBeforePerch.x, 0));
            }

            if (rb.velocity.x < 0) {
                rb.AddForce(new Vector2(rb.mass * accelerationToSlowBeforePerch.x, 0));
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(timeToStayPerched);

        hasPerched = true;

    }


    public void SetCorrectingForce(float newCorrectingForce) {
        correctingForce = newCorrectingForce;
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void SetDirection(string direction) {
        /*if (direction.Equals("TopToBottom")) {
            isVertical = true;
            isTopToBottom = true;
        } else if (direction.Equals("BottomToTop")) {
            isVertical = true;
            isTopToBottom = false;
        } else if (direction.Equals("LeftToRight")) {
            isVertical = false;
            isLeftToRight = true;
        } else {
            isVertical = false;
            isLeftToRight = false;
        }*/
    }

    public void SetDistanceBeforePerch(float newDistance) {
        distanceBeforePerch = newDistance;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }

    public void SetVelocityBeforePerch(Vector2 vel) {
        velocityBeforePerch = vel;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }

    public void SetAccelerationBeforePerch(Vector2 accel) {
        accelerationToSlowBeforePerch = accel;
        UpdateDistanceToStartSlowing();
    }

    public void MaintainVelocityBeforePerch() {
        rb.velocity = velocityBeforePerch;
    }

    protected void UpdateDistanceToStartSlowing() {
        float timeToStop = Mathf.Max(velocityBeforePerch.x / accelerationToSlowBeforePerch.x, velocityBeforePerch.y / accelerationToSlowBeforePerch.y);
        float distanceTraveledWhileSlowingX = Mathf.Abs(velocityBeforePerch.x * timeToStop) - Mathf.Abs(0.5f * accelerationToSlowBeforePerch.x * timeToStop * timeToStop);
        float distanceTraveledWhileSlowingY = Mathf.Abs(velocityBeforePerch.y * timeToStop) - Mathf.Abs(0.5f * accelerationToSlowBeforePerch.y * timeToStop * timeToStop);
        float distanceTraveledWhileSlowing = Mathf.Sqrt(Mathf.Pow(distanceTraveledWhileSlowingX, 2) + Mathf.Pow(distanceTraveledWhileSlowingY, 2));
        distanceToStartSlowing = Mathf.Max(0, distanceBeforePerch - distanceTraveledWhileSlowing);
        //Debug.Log("Distance to start slowing: " + distanceToStartSlowing);
    }

    protected void SetPositionAsOriginalPosition() {
        originalPos = transform.position;
        //Debug.Log(originalPos);
    }

    protected void ResetConditionals()
    {
        hasPerched = false;
        isSlowing = false;
    }
}
