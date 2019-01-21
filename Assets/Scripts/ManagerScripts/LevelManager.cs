using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
    public GameObject musicManager;
    public GameObject soundManager;

    MusicManager musicManagerScript;

    public void Start()
    {
        if (musicManager == null) {
            musicManager = GameObject.FindGameObjectWithTag("MusicManager");
        }

        musicManagerScript = musicManager.GetComponent<MusicManager>();

        if (musicManagerScript.GetPlayingSong() == null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Menu")) {
            musicManagerScript.PlayTitle();
        }

        if (soundManager == null) {
            soundManager = GameObject.FindGameObjectWithTag("SoundManager");
        }

    }

    public void LoadGame() {
        //DontDestroyOnLoad(musicManager);
        //DontDestroyOnLoad(soundManager);
        //musicManagerScript.PlayFirstLevel();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    }

    public void LoadMenu() {
        Destroy(musicManager);
        //DontDestroyOnLoad(musicManager);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
