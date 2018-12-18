using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject settingsManager;
    public GameObject startMenu;
    public GameObject lossMenu;
    public GameObject scoreboard;

    public GameObject bestLossMenu;
    public GameObject yourScoreLossMenu;
    public GameObject bestScoreIndicator;

    UnityEngine.UI.Text bestText;
    UnityEngine.UI.Text yourScoreText;

    public GameObject gameManager;

    string currentState;

    // Use this for initialization
    void Start()
    {
        startMenu.SetActive(true);
        currentState = "Start Menu";

        SetLossMenu();
    }

    private void FixedUpdate()
    {
        if(currentState == "Start Menu" && Input.GetMouseButtonDown(0))
        //if (currentState == "Start Menu" && Input.touchCount > 0 && Input.GetTouch(0).position.y > settingsManager.GetComponent<SettingsManager>().GetBottomWallPositionY() + 2f)
        {
            scoreboard.SetActive(true);
            Instantiate(gameManager, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            currentState = "Ingame";
            startMenu.SetActive(false);
        }

        //if (currentState == "Loss Menu" && )
    }

    private void SetLossMenu()
    {
        bestText = bestLossMenu.GetComponent<UnityEngine.UI.Text>();
        bestText.text = "Best: \n" + 0;


        yourScoreText = yourScoreLossMenu.GetComponent<UnityEngine.UI.Text>();
        yourScoreText.text = "Your Score: \n" + 0;
    }

    public void DisplayLossMenu(bool newHighScore, int highestScore, int currScore) {
        if (newHighScore) {
            bestScoreIndicator.SetActive(true);
            bestLossMenu.transform.position = new Vector2(bestLossMenu.transform.position.x, bestLossMenu.transform.position.y - 10f);
            yourScoreLossMenu.transform.position = new Vector2(yourScoreLossMenu.transform.position.x, yourScoreLossMenu.transform.position.y - 10f);
        }
        bestText.text = "Best: \n" + highestScore;
        yourScoreText.text = "Your Score: \n" + currScore;

        lossMenu.SetActive(true);
        currentState = "Loss Menu";
    }
}