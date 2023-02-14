using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager instance { get; private set; }

    [SerializeField] private AudioClip[] keyboardSounds;
    [SerializeField] private AudioClip[] spacebarSounds;
    [SerializeField] private AudioClip[] tabSounds;

    [SerializeField] private AudioClip[] purchaseSounds;
    [SerializeField] private AudioClip[] incompleteSounds;
    [SerializeField] private AudioClip[] wildcharSounds;
    [SerializeField] private AudioClip[] autocompleteSounds;
    [SerializeField] private AudioClip[] autofillSounds;

    [SerializeField] private AudioClip[] cheatSounds;

    private AudioSource audioSource;

    public void PlayKeyboard()
    {
        audioSource.PlayOneShot(keyboardSounds[Random.Range(0, keyboardSounds.Length)]);
    }

    public void PlaySpacebar()
    {
        audioSource.PlayOneShot(spacebarSounds[Random.Range(0, spacebarSounds.Length)]);
    }

    public void PlayTab()
    {
        audioSource.PlayOneShot(tabSounds[Random.Range(0, tabSounds.Length)]);
    }

    public void PlayPurchase()
    {
        audioSource.PlayOneShot(purchaseSounds[Random.Range(0, purchaseSounds.Length)]);
    }

    public void PlayIncomplete()
    {
        audioSource.PlayOneShot(incompleteSounds[Random.Range(0, incompleteSounds.Length)]);
    }

    public void PlayWildchar()
    {
        audioSource.PlayOneShot(wildcharSounds[Random.Range(0, wildcharSounds.Length)]);
    }

    public void PlayAutocomplete()
    {
        audioSource.PlayOneShot(autocompleteSounds[Random.Range(0, autocompleteSounds.Length)]);
    }

    public void PlayAutofill()
    {
        audioSource.PlayOneShot(autofillSounds[Random.Range(0, autofillSounds.Length)]);
    }

    public void PlayCheat()
    {
        audioSource.PlayOneShot(cheatSounds[Random.Range(0, cheatSounds.Length)]);
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
