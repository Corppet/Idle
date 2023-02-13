using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager instance { get; private set; }

    [SerializeField] private AudioClip[] correctSounds;
    [SerializeField] private AudioClip[] incorrectSounds;

    private AudioSource audioSource;

    public void PlayCorrect()
    {
        audioSource.PlayOneShot(correctSounds[Random.Range(0, correctSounds.Length)]);
    }

    public void PlayIncorrect()
    {
        audioSource.PlayOneShot(incorrectSounds[Random.Range(0, incorrectSounds.Length)]);
    }

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

        audioSource = GetComponent<AudioSource>();
    }
}
