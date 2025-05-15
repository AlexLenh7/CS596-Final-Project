using System.Collections.Generic;
using Unity.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource currentPreview;

    [SerializeField] private AudioSource AudioSourceObject;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void playSound(AudioClip clip, Transform transform, float volume)
    {
        // spawn in game object to play sound
        AudioSource audioSource = Instantiate(AudioSourceObject, transform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        // destroy the gameObject after it finishes playing
        Destroy(audioSource.gameObject, clip.length);
    }

    public void songPreview(AudioClip clip, float startTime, float duration, float volume)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }

        AudioSource audioSource = Instantiate(AudioSourceObject, transform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.time = startTime;
        audioSource.volume = volume;
        audioSource.Play();

        currentPreview = audioSource;
        Destroy(audioSource.gameObject, duration);
    }
}
