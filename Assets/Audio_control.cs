using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType{
    Enemy,
    Play,
    Ball,
}

public class Audio_control : MonoBehaviour{

    public AudioClip EnemyClip,PlayClip,BallClip, BellClip;
    private AudioSource AudioSC;
    public static Audio_control AudioControl { get; private set; }

    void Awake(){
        if (AudioControl != null && AudioControl != this)
        {
            Destroy(gameObject);
            return;
        }

        AudioControl = this;

        AudioSC = GetComponent<AudioSource>();
        UpdateAudio(BellClip);
    }

    public void StartClip(SoundType type){
        switch (type)
        {
            case SoundType.Enemy:
                UpdateAudio(EnemyClip);
                break;
            case SoundType.Play:
                UpdateAudio(PlayClip);
                break;
            case SoundType.Ball:
                UpdateAudio(BallClip);
                break;
        }
    }

    void UpdateAudio(AudioClip Audio){

        AudioSC.clip=Audio;
        AudioSC.Play();
    }
}
