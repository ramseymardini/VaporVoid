using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterController : Projectile {

    protected bool hasPerched;
    protected bool isSlowing;

    protected float correctingForce = 5f;

    protected float speedBeforePerch;
    protected Vector2 velocityBeforePerch;

    protected float accelerationToSlowBeforePerch;
    protected float distanceBeforePerch = 2f;
    protected float distanceToStartSlowing;
    protected float timeToStayPerched = 0.5f;

    protected float minSpeed = 2f;

    protected Vector2 originalPos;

    protected float accelDive;

    //protected bool accelSet;
    protected bool isEnabled;

    protected GameObject player;

    protected bool isLeftToRight;
    protected bool isRightToLeft;
    protected bool isTopToBottom;
    protected bool isBottomToTop;

    bool canReverse;

    protected override void Start() {
        base.Start();
        //accelerationX = DEFAULT_ACCELERATION_X;
        //accelerationY = DEFAULT_ACCELERATION_Y;
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateDistanceToStartSlowing();
        SetPositionAsOriginalPosition();

        SetDistanceBeforePerch(2f);
        distanceToStartSlowing = Mathf.Infinity;

        //velocityBeforePerch = new Vector2(-2.5f, 0);
        SetDirection();
        SetSpeedBeforePerch(2f);
        SetAccelerationBeforePerch(5f);
    }
   
    protected override void FixedUpdate() {
        if (!hasPerched) {
            if (!isSlowing && FindDistanceTraveled() >= distanceToStartSlowing) {
                StartCoroutine(StartSlowing());
            } else if (!isSlowing){
                MaintainVelocityBeforePerch();
            }
            return;
        }

        base.FixedUpdate();
        float accelX = correctingForce * Mathf.Cos(FindAngleToPlayer());
        float accelY = correctingForce * Mathf.Sin(FindAngleToPlayer());
        if (!canReverse) {
            PreventReverse();
        }
        SetAcceleration(new Vector2(accelX, accelY));
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

        isSlowing = false;

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

    /*public void SetDirection(string direction) {
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
    }*/

    public void SetDistanceBeforePerch(float newDistance) {
        distanceBeforePerch = newDistance;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }

    public void SetVelocityBeforePerch() {
        if (isLeftToRight) {
            velocityBeforePerch = new Vector2(speedBeforePerch, 0);
        } else if (isBottomToTop) {
            velocityBeforePerch = new Vector2(0, speedBeforePerch);
        } else if (isRightToLeft) {
            velocityBeforePerch = new Vector2(-1 * speedBeforePerch, 0);
        } else if (isTopToBottom) {
            velocityBeforePerch = new Vector2(0, -1 * speedBeforePerch);
        }

        MaintainVelocityBeforePerch();

        UpdateDistanceToStartSlowing();
    }

    /*public void SetVelocityBeforePerch(Vector2 vel) {
        velocityBeforePerch = vel;
        MaintainVelocityBeforePerch();
        UpdateDistanceToStartSlowing();
    }*/

    public void SetSpeedBeforePerch(float speed) {
        speedBeforePerch = speed;
        SetVelocityBeforePerch();
    }

    public void SetAccelerationBeforePerch(float accel) {
        accelerationToSlowBeforePerch = accel;
        UpdateDistanceToStartSlowing();
    }

    protected void SetAccelDive(float accel) {
        accelDive = accel;
    }

    /*public void SetAccelerationBeforePerch(Vector2 accel) {
        accelerationToSlowBeforePerch = accel;
        UpdateDistanceToStartSlowing();
    }*/

    public void MaintainVelocityBeforePerch() {
        //Debug.Log(rb.name);
        //Debug.Log(velocityBeforePerch);
        //rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocityBeforePerch;
    }

    protected void UpdateDistanceToStartSlowing() {
        /*float timeToStop = Mathf.Max(velocityBeforePerch.x / accelerationToSlowBeforePerch, velocityBeforePerch.y / accelerationToSlowBeforePerch);
        Debug.Log(timeToStop);
        float distanceTraveledWhileSlowingX = Mathf.Max(0, Mathf.Abs(velocityBeforePerch.x * timeToStop) - Mathf.Abs(0.5f * accelerationToSlowBeforePerch * timeToStop * timeToStop));
        Debug.Log("Distance traveled while slowing x" + distanceTraveledWhileSlowingX);
        float distanceTraveledWhileSlowingY = Mathf.Max(0, Mathf.Abs(velocityBeforePerch.y * timeToStop) - Mathf.Abs(0.5f * accelerationToSlowBeforePerch * timeToStop * timeToStop));
        Debug.Log("Distance traveled while slowing y" + distanceTraveledWhileSlowingY);
        float distanceTraveledWhileSlowing = Mathf.Sqrt(Mathf.Pow(distanceTraveledWhileSlowingX, 2) + Mathf.Pow(distanceTraveledWhileSlowingY, 2));*/

        float timeToStop = speedBeforePerch / accelerationToSlowBeforePerch;
        //Debug.Log("Time to stop" + rb.name + timeToStop);

        float distanceTraveledWhileSlowing = (speedBeforePerch * timeToStop) - (0.5f * accelerationToSlowBeforePerch * Mathf.Pow(timeToStop, 2));

        distanceToStartSlowing = Mathf.Max(0, distanceBeforePerch - distanceTraveledWhileSlowing);
        //Debug.Log(rb.name + " " + distanceToStartSlowing);
        //Debug.Log(distanceToStartSlowing);
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

    protected void SetDirection() {
        float angleToPlayer = FindAngleToPlayer();
        if (angleToPlayer <= Mathf.PI / 4 && angleToPlayer > -1 * Mathf.PI / 4) {
            isLeftToRight = true;
        } else if (angleToPlayer <= Mathf.PI * 3 / 4 && angleToPlayer > Mathf.PI / 4) {
            isBottomToTop = true;
        } else if (angleToPlayer <= -1 * Mathf.PI * 3 / 4 && angleToPlayer > Mathf.PI * 3 / 4) {
            isRightToLeft = true;
        } else {
            isTopToBottom = true;
        }

        UpdateDistanceToStartSlowing();
    }

    public void SetCanReverse(bool canReverse) {
        this.canReverse = canReverse;
    }

    //It might be better to do this by keeping track of a maximum speed reached and prevent the ball from ever slowing down. ex: if leftToRight velocity will always have the max x velocity it reached.
    protected void PreventReverse() {
        if (isLeftToRight) {
            SetVelocity(new Vector2(Mathf.Max(minSpeed, rb.velocity.x), rb.velocity.y));
        } else if (isBottomToTop) {
            SetVelocity(new Vector2(rb.velocity.x, Mathf.Max(minSpeed, rb.velocity.y)));
        } else if (isRightToLeft) {
            SetVelocity(new Vector2(Mathf.Min(-1 * minSpeed, rb.velocity.x), rb.velocity.y));
        } else if (isTopToBottom) {
            SetVelocity(new Vector2(rb.velocity.x, Mathf.Min(-1 * minSpeed, rb.velocity.y)));
        }
    }
}
