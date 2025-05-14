using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

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
}
