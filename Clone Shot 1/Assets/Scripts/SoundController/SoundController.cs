using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour {

    // Use this for initialization
    public int MaxAudioDistance { get; set; }

    private AudioSource currentSource;

    void Start()
    {
        currentSource = GetComponent<AudioSource>();

        currentSource.maxDistance = MaxAudioDistance;
   }
    
    public void LaunchTestAllSoundsCouritine()
    {
        StartCoroutine("TestAllSounds");
        Debug.Log("Starting test all sounds couritine!");
    }

    private IEnumerator TestAllSounds()
    {
        if(currentSource == null)
        {
            currentSource = GetComponent<AudioSource>();

            currentSource.maxDistance = MaxAudioDistance;
        }

        AudioClip tempClip;

        int clipNumber = 0;

        foreach(SoundEffects SoundEffect in Enum.GetValues(typeof(SoundEffects)))
        {
            Debug.Log("Attempting to load clip #" + clipNumber);
            tempClip = SoundEffectManager.GetClip(SoundEffect);
            if(tempClip != null)
            {
                Debug.Log("Playing clip #" + clipNumber);
                currentSource.clip = tempClip;
                currentSource.Play();
                while(currentSource.isPlaying)
                {
                    yield return null;
                }
            }

            clipNumber++;
            yield return null;
        }

    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
