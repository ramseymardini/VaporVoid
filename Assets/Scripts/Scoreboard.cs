using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

    public GameObject gameManager;
    public GameObject bestScoreIndicator;
    public GameObject bestText;
    public GameObject yourScoreText;
    bool newHighScore;
    int currScore;
    int highestScore;
    UnityEngine.UI.Text text;


	// Use this for initialization
	void Start () {
        currScore = 0;
        highestScore = PlayerPrefs.GetInt("highestScore");
        bestText.GetComponent<UnityEngine.UI.Text>().text = "Best: \n" + highestScore;
        yourScoreText.GetComponent<UnityEngine.UI.Text>().text = "Your Score: \n" + currScore;
        text = gameObject.GetComponent<UnityEngine.UI.Text>();
        newHighScore = false;
	}

    public void IncrementScore() {
        if (!newHighScore && currScore > highestScore) {
            newHighScore = true;
            text.color = new Color(255, 255, 255);
        }
        currScore++;
        text.text = "" + currScore;

        if (currScore % 5 == 0) {
            gameManager.GetComponent<GameManager>().IncrementLevel();
        }
    }

    public void ChangeHighScore() {
        if (currScore > highestScore) {
            PlayerPrefs.SetInt("highestScore", currScore);
        }

        if (newHighScore) {
            bestScoreIndicator.SetActive(true);
            bestText.transform.position = new Vector2(bestText.transform.position.x, bestText.transform.position.y - 10f);
            yourScoreText.transform.position = new Vector2(yourScoreText.transform.position.x, yourScoreText.transform.position.y - 10f);
        }

        bestText.GetComponent<UnityEngine.UI.Text>().text = "Best: \n" + PlayerPrefs.GetInt("highestScore");
        yourScoreText.GetComponent<UnityEngine.UI.Text>().text = "Your Score: \n" + currScore;
        Destroy(gameObject);
    }
}
