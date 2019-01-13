using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMessagesController : MonoBehaviour {

    UnityEngine.UI.Text text;

    int numBlinks = 3;
    float timeToBlink = 0.8f;

    private void Start() {
        text = GetComponent<UnityEngine.UI.Text>();
    }

    public void DisplayFirstStageCompleted() {
        text.text = "First Stage Completed";
        StartCoroutine(BlinkMessage());
    }

    public void DisplaySecondStageCompleted() {
        text.text = "Second Stage Completed";
        StartCoroutine(BlinkMessage());
    }

    public void DisplayThirdStageCompleted() {
        text.text = "Third Stage Completed";
        StartCoroutine(BlinkMessage());
    }

    public void DisplayFourthStageCompleted() {
        text.text = "Game Completed!";
        StartCoroutine(BlinkMessage());
    }

    IEnumerator BlinkMessage() {
        for (int i = 0; i < numBlinks; i++) {
            text.enabled = true;
            yield return new WaitForSeconds(timeToBlink);
            text.enabled = false;
            yield return new WaitForSeconds(timeToBlink / 2);
        }
    }
}
