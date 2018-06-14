using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    AudioSource shieldGainNoise;
    AudioSource playerDeathNoise;
    AudioSource explosionNoise;

    float volume;

    private void Start()
    {
        AudioSource[] noises = GetComponents<AudioSource>();
        shieldGainNoise = noises[0];
        playerDeathNoise = noises[1];
        explosionNoise = noises[2];
        volume = 0.5f;
    }

    public void PlayShieldGainNoise() {
        shieldGainNoise.pitch = Random.Range(0.95f, 1.15f);
        shieldGainNoise.volume = volume - 0.2f;
        shieldGainNoise.Play();
    }

    public void PlayPlayerDeathNoise() {
        playerDeathNoise.pitch = Random.Range(0.9f, 1.1f);
        playerDeathNoise.volume = volume + 0.2f;
        playerDeathNoise.Play();
    }

    public void PlayExplosionNoise() {
        explosionNoise.pitch = Random.Range(0.9f, 1.1f);
        explosionNoise.volume = volume;
        explosionNoise.Play();
    }
}
