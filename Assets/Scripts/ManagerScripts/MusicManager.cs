using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    AudioSource titleMusic;
    AudioSource firstLevelMusic;
    AudioSource firstBossMusic;

    float timeToPhaseTitle = 0f;
    float timeToPhaseFirstLevel = 0.75f;
    float timeToPhaseFirstBoss = 1.5f;

    AudioSource playingAudio;
    AudioSource queuedAudio;

    float maxVolume = 0.4f;

	// Use this for initialization
	void Start () {
        AudioSource[] allMusicClips = GetComponents<AudioSource>();
        titleMusic = allMusicClips[0];
        firstLevelMusic = allMusicClips[1];
        firstBossMusic = allMusicClips[2];
	}

    public void PlayTitle() {
        if (playingAudio != null && playingAudio.clip.name.Equals("Title Screen Music")) {
            return;
        }
        queuedAudio = titleMusic;
        StartCoroutine(phaseOutTracks(timeToPhaseTitle));
    }

    public void PlayFirstLevel() {
        if (playingAudio != null && playingAudio.clip.name.Equals("Stage 1 Music")) {
            return;
        }
        //Debug.Log(playingAudio.clip.name);
        queuedAudio = firstLevelMusic;
        StartCoroutine(phaseOutTracks(timeToPhaseFirstLevel));
    }

    public void PlayFirstBoss() {
        if (playingAudio != null && playingAudio.clip.name.Equals("Stage 1 Boss Music")) {
            return;
        }
        queuedAudio = firstBossMusic;
        StartCoroutine(phaseOutTracks(timeToPhaseFirstBoss));
    }

    IEnumerator phaseOutTracks(float timeToPhase) {
        float timeBetweenVolChange = 0.01f;
        float numVolChanges;
        if (timeToPhase == 0) {
            numVolChanges = 1;
        } else {
            numVolChanges = timeToPhase / timeBetweenVolChange;
        }
        float amountPerVolChange = maxVolume / numVolChanges;

        queuedAudio.Play();

        for (int i = 0; i < numVolChanges; i++) {
            if (playingAudio != null) {
                playingAudio.volume -= amountPerVolChange;
            }
            queuedAudio.volume += amountPerVolChange;
            yield return new WaitForSeconds(timeBetweenVolChange);
        }
        if (playingAudio != null) {
            playingAudio.Stop();
        }

        playingAudio = queuedAudio;
    }

    public AudioSource GetPlayingSong() {
        return playingAudio;
    }
}
