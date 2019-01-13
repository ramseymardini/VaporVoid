using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

    public GameObject gameManager;
    public GameObject gamePlayManager;
    public GameObject best;
    public GameObject yourScore;
    //public GameObject stageMessages;


    UnityEngine.UI.Text gameObjectText;

    StageMessagesController stageMessagesScript;

    int highestScore;
    bool newHighScore;
    int currScore;

    readonly int SCORE_FOR_FIRST_BOSS = 5;
    readonly int SCORE_FOR_SECOND_BOSS = 10;

	// Use this for initialization
	void Start () {
        currScore = 0;
        gameManager = GameObject.Find("Game Manager");
        highestScore = PlayerPrefs.GetInt("highestScore");

        gameObjectText = gameObject.GetComponent<UnityEngine.UI.Text>();
        newHighScore = false;

        //stageMessagesScript = stageMessages.GetComponent<StageMessagesController>();
	}

    public void IncrementScore() {
        if (!newHighScore && currScore > highestScore) {
            newHighScore = true;
            gameObjectText.color = new Color(255, 255, 255);
        }
        currScore++;
        gameObjectText.text = "" + currScore;

        if (currScore == SCORE_FOR_FIRST_BOSS || currScore == SCORE_FOR_SECOND_BOSS) {
            if (gamePlayManager == null) {
                SetGamePlayManager();
            }

            gamePlayManager.GetComponent<GameplayManager>().IncrementLevel();
            //stageMessagesScript.DisplayFirstStageCompleted();
        }

    }

    public void ChangeHighScore() {
        if (currScore > highestScore) {
            PlayerPrefs.SetInt("highestScore", currScore);
        }

        gameManager.GetComponent<GameManager>().DisplayLossMenu(newHighScore, highestScore, currScore);

        Destroy(gameObject);
    }

    public void AddPoints(int points) {
        StartCoroutine(AddPointsHelper(points));
    }

    void SetGamePlayManager() {
        gamePlayManager = GameObject.FindGameObjectWithTag("GameController");
    }

    IEnumerator AddPointsHelper(int points) {
        for (int i = 0; i < points; i++) {
            IncrementScore();
            yield return new WaitForSeconds(0.1f);
        }
    }


}
