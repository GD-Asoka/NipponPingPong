using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgm1, bgm2, bgm3;
    public AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        SetBGM();
    }

    public void SetBGM()
    {
        float rand = Random.value;
        if(rand < 0.33)
        {
            audioSource.clip = bgm1;
            audioSource.Play();
        }
        if(rand < 0.66)
        {
            audioSource.clip = bgm2;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = bgm3;
            audioSource.Play();
        }
    }
}
