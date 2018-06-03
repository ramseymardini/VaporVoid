using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonBomb : MonoBehaviour {

    Color originalColor;
    Color currColor;
    float explosionSizeX = 2.0f;
    float explosionSizeY = 2.0f;

    float timePerBlink = 0.4f;
    int numBlinks = 3;

	// Use this for initialization
	void Start () {
        originalColor = GetComponent<SpriteRenderer>().color;
        currColor = originalColor;
        currColor.a = 0;
        GetComponent<SpriteRenderer>().color = currColor;
        StartCoroutine(Blink());
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    IEnumerator Blink() {
        float currentScaleIncrementerAmount = 0.05f;
        float timeToWait = timePerBlink * currentScaleIncrementerAmount;

        float currentScale = 0f;

        for (int i = 0; i < numBlinks; i++) {
            while (currentScale < 1) {
                currColor.a = Mathf.Lerp(0, 1, currentScale);
                GetComponent<SpriteRenderer>().color = currColor;
                currentScale += currentScaleIncrementerAmount;
                yield return new WaitForSeconds(timeToWait);
            }

            yield return new WaitForSeconds(0.2f);

            while (currentScale > 0) {
                currColor.a = Mathf.Lerp(0, 1, currentScale);
                GetComponent<SpriteRenderer>().color = currColor;
                currentScale -= currentScaleIncrementerAmount;
                yield return new WaitForSeconds(timeToWait);
            }

            yield return new WaitForSeconds(0.2f);
        }

        StartCoroutine(Explode());
    }

    IEnumerator Explode() {
        transform.localScale = new Vector2(explosionSizeX, explosionSizeY);
        currColor = new Color(255, 0, 0);
        GetComponent<SpriteRenderer>().color = currColor;
        yield return new WaitForEndOfFrame();
        GetComponent<PolygonCollider2D>().enabled = true;
        yield return new WaitForEndOfFrame();
        bool colliderDisabled = false;
        while (currColor.a > 0) {
            currColor.a -= 0.02f;
            GetComponent<SpriteRenderer>().color = currColor;

            if (!colliderDisabled && currColor.a < 0.8) {
                colliderDisabled = false;
                yield return new WaitForEndOfFrame();
                GetComponent<PolygonCollider2D>().enabled = false;
            }
            yield return new WaitForSeconds(0.00001f);
        }

        Destroy(gameObject);
    }
}
