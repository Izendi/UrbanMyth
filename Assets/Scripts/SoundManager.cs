using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class was heavily inspired by: https://www.youtube.com/watch?v=DU7cgVsU2rM

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundEffectObject;

    //Make this a singleton:
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

    }

    public void PlaySoundEffect(AudioClip audioClip, Transform audioSourceLocation, float volume)
    {
        //Spawn in game object
        AudioSource audioSource = Instantiate(soundEffectObject, audioSourceLocation.position, Quaternion.identity);

        //assign audio clip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy sound effect object
        Destroy(audioSource.gameObject, clipLength + 0.1f);

    }
}
