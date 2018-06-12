﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
    public GameObject musicManager;

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

    }

    public void LoadGame() {
        DontDestroyOnLoad(musicManager);
        //musicManagerScript.PlayFirstLevel();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    }

    public void LoadMenu() {
        DontDestroyOnLoad(musicManager);
        musicManagerScript.PlayTitle();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void ExitGame() {
        Application.Quit();
    }
}