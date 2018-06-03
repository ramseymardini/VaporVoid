using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterController : Projectile {
    
    bool isVertical;
    bool isTopToBottom;
    bool isLeftToRight;

    float correctingForce = 5f;

    GameObject player;

    private void FixedUpdate() {
        base.FixedUpdate();
        float newAccelX = accelerationX + correctingForce * Mathf.Cos(FindAngleToPlayer());
        float newAccelY = accelerationY + Mathf.Sin(FindAngleToPlayer());
        SetAcceleration(new Vector2(newAccelX, newAccelY));
    }

    public void SetPlayer(GameObject newPlayer) {
        player = newPlayer;
    }

    float FindAngleToPlayer() {
        Vector2 currentPlayerPosition = player.transform.position;
        return Mathf.Atan2((currentPlayerPosition.y - transform.position.y), (currentPlayerPosition.x - transform.position.x));
    }

    public void SetCorrectingForce(float newCorrectingForce) {
        correctingForce = newCorrectingForce;
    }
}
