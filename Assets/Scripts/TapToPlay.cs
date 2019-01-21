using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlay : MonoBehaviour
{
    private UnityEngine.UI.Text textComponent;
    float timePerBlink = 0.5f;
    float changeAmount = 0.005f;

    // Update is called once per frame
    void Start() {
        textComponent = GetComponent<UnityEngine.UI.Text>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink() {
        float r = textComponent.color.r;
        float g = textComponent.color.g;
        float b = textComponent.color.b;
        float a;

        float waitTime = timePerBlink / 2 * changeAmount;
        bool isDecreasing = true;

        while (true)
        {
            a = textComponent.color.a;
            if (a < 0.0001f) isDecreasing = false;
            if (a > 0.999f) isDecreasing = true;

            if (isDecreasing) {
                textComponent.color = new Color(r, g, b, a - changeAmount);
            } else {
                textComponent.color = new Color(r, g, b, a + changeAmount);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
