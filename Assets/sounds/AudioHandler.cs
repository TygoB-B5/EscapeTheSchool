using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioClip Step;
    public AudioClip Scream;
    public AudioSource aud;

    public void PlaySound(string sound)
    {
        if(sound == "Step")
        {
            aud.PlayOneShot(Step, 0.5f);
        }

        if (sound == "Scream")
        {
            aud.PlayOneShot(Scream, 1);
        }
    }

    public void StopSound()
    {
        aud.Stop();
        AudioSource[] allAud = FindObjectsOfType<AudioSource>();
        foreach (AudioSource aud_ in allAud)
        {
            aud_.Stop();
        }
    }
}
