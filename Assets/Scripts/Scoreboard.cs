using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

    public GameObject gameManager;
    public GameObject bestScoreIndicator;
    public GameObject best;
    public GameObject yourScore;
    public GameObject levelMessages;

    UnityEngine.UI.Text bestText;
    UnityEngine.UI.Text yourScoreText;
    UnityEngine.UI.Text levelMessagesText;
    UnityEngine.UI.Text gameObjectText;

    bool newHighScore;
    int currScore;
    int highestScore;


	// Use this for initialization
	void Start () {
        currScore = 0;
        highestScore = PlayerPrefs.GetInt("highestScore");
        bestText = best.GetComponent<UnityEngine.UI.Text>();
        bestText.text = "Best: \n" + highestScore;

        yourScoreText = yourScore.GetComponent<UnityEngine.UI.Text>();
        yourScoreText.text = "Your Score: \n" + currScore;

        gameObjectText = gameObject.GetComponent<UnityEngine.UI.Text>();
        newHighScore = false;
	}

    public void IncrementScore() {
        if (!newHighScore && currScore > highestScore) {
            newHighScore = true;
            gameObjectText.color = new Color(255, 255, 255);
        }
        currScore++;
        gameObjectText.text = "" + currScore;

        if (currScore == 20) {
            gameManager.GetComponent<GameManager>().IncrementLevel();
        }
    }

    public void ChangeHighScore() {
        if (currScore > highestScore) {
            PlayerPrefs.SetInt("highestScore", currScore);
        }

        if (newHighScore) {
            bestScoreIndicator.SetActive(true);
            best.transform.position = new Vector2(best.transform.position.x, best.transform.position.y - 10f);
            yourScore.transform.position = new Vector2(yourScore.transform.position.x, yourScore.transform.position.y - 10f);
        }

        bestText.text = "Best: \n" + PlayerPrefs.GetInt("highestScore");
        yourScoreText.text = "Your Score: \n" + currScore;
        Destroy(gameObject);
    }

    public void AddPoints(int points) {
        StartCoroutine(AddPointsHelper(points));
    }

    IEnumerator AddPointsHelper(int points) {
        for (int i = 0; i < points; i++) {
            IncrementScore();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
