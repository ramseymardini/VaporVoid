using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterController : Projectile {

    protected bool hasPerched;
    protected bool isSlowing;

    bool isVertical;
    bool isTopToBottom;
    bool isLeftToRight;

    protected float correctingForce = 5f;

    readonly float DEFAULT_ACCELERATION_X = 0f;
    readonly float DEFAULT_ACCELERATION_Y = -10f;

    protected float velocityBeforePerch = 5f;
    protected float distanceBeforePerch = 2.5f;
    protected float accelerationToSlowBeforePerch = 5f;
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
        accelerationX = DEFAULT_ACCELERATION_X;
        accelerationY = DEFAULT_ACCELERATION_Y;
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateDistanceToStartSlowing();
        SetPositionAsOriginalPosition();
    }
   
    protected override void FixedUpdate() {
        if (!hasPerched) {
            if (!isSlowing && FindDistanceTraveled() > distanceToStartSlowing) {
                StartCoroutine(StartSlowing());
            } else {
                //MaintainVelocityBeforePerch();
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
        if (direction.Equals("TopToBottom")) {
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
        }
    }

    public void SetDistanceBeforePerch(float newDistance) {
        distanceBeforePerch = newDistance;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }

    public void SetVelocityBeforePerch(float newVelocity) {
        velocityBeforePerch = newVelocity;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }

    public void SetAccelerationBeforePerch(float newAcceleration) {
        accelerationToSlowBeforePerch = newAcceleration;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }

    public void MaintainVelocityBeforePerch() {
        if (isTopToBottom) {
            rb.velocity = new Vector2(0, -velocityBeforePerch);
        }
        else if (isLeftToRight) {
            rb.velocity = new Vector2(velocityBeforePerch, 0);
        }
        else if (isTopToBottom && !isTopToBottom) {
            rb.velocity = new Vector2(0, velocityBeforePerch);
        }
        else {
            rb.velocity = new Vector2(-velocityBeforePerch, 0);
        }
    }

    protected void UpdateDistanceToStartSlowing() {
        float timeToStop = velocityBeforePerch / accelerationToSlowBeforePerch;
        distanceToStartSlowing = Mathf.Max(0, distanceBeforePerch - (Mathf.Abs(velocityBeforePerch * timeToStop) - Mathf.Abs(0.5f * accelerationToSlowBeforePerch * timeToStop * timeToStop)));
        //Debug.Log("Distance to start slowing: " + distanceToStartSlowing);
    }

    protected void SetPositionAsOriginalPosition() {
        originalPos = transform.position;
        //Debug.Log(originalPos);
    }
}
