using Assets.Scripts.SoundController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{

    // Use this for initialization
    public int MaxAudioDistance { get; set; }

    private AudioSource BlockedAudioSource;

    private List<SoundControllerInstance> AudioClipBuffer = new List<SoundControllerInstance>();

    private Semaphore AudioClipBufferLock = new Semaphore(1, 1);

    private float BlockTimeInSeconds = 0;

    private bool Blocked = false;

    void Start()
    {
        BlockedAudioSource = GetComponent<AudioSource>();

        BlockedAudioSource.maxDistance = MaxAudioDistance;
   }
    
    public void LaunchTestAllSoundsCouritine()
    {
        StartCoroutine("TestAllSounds");
        Debug.Log("Starting test all sounds couritine!");
    }

    private IEnumerator TestAllSounds()
    {
        if(BlockedAudioSource == null)
        {
            BlockedAudioSource = GetComponent<AudioSource>();

            BlockedAudioSource.maxDistance = MaxAudioDistance;
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
                BlockedAudioSource.clip = tempClip;
                BlockedAudioSource.Play();
                while(BlockedAudioSource.isPlaying)
                {
                    yield return null;
                }
            }

            clipNumber++;
            yield return null;
        }

    }

    public void ProcessSoundEffect(Assets.Scripts.SoundController.PlayMode Mode, SoundEffects SoundEffect)
    {
        AudioClip clip = SoundEffectManager.GetClip(SoundEffect);
        if(clip != null)
        {
            SoundControllerInstance instance = new SoundControllerInstance(Mode, clip);
            ProcessSoundEffect(instance);
        }
    }

    internal void ProcessSoundEffect(SoundControllerInstance Instance)
    {
        if(Instance.Mode == Assets.Scripts.SoundController.PlayMode.Immediate || Instance.Mode == Assets.Scripts.SoundController.PlayMode.Block)
        {
            AudioSource source = new AudioSource();
            source.clip = Instance.Clip;
            source.Play();
        }

        if(Instance.Mode == Assets.Scripts.SoundController.PlayMode.Block)
        {
            BlockTimeInSeconds = Instance.Clip.length;
            StartCoroutine("BlockAudioSource");
        }

        if(Instance.Mode == Assets.Scripts.SoundController.PlayMode.Wait ||
            Instance.Mode == Assets.Scripts.SoundController.PlayMode.WaitAndBlock)
        {
            GetAudioClipBufferLock();
            AudioClipBuffer.Add(Instance);
            ReleaseAudioClipBufferLock();
        }
    }
    
    private IEnumerator BlockAudioSource()
    {
        Blocked = true;
        
        yield return new WaitForSeconds((float)BlockTimeInSeconds);

        Blocked = false;
    }

    // Update is called once per frame
    void Update ()
    {
		if(!Blocked)
        {
            GetAudioClipBufferLock();
            SoundControllerInstance instance = null;
            if (AudioClipBuffer.Any())
            {
                instance = new SoundControllerInstance();
                instance = AudioClipBuffer[0];
                AudioClipBuffer.RemoveAt(0);
            }
            ReleaseAudioClipBufferLock();

            if(instance != null)
            {
                PlayOnBlockedAudioSource(instance);
            }
        }
	}

    private void PlayOnBlockedAudioSource(SoundControllerInstance Instance)
    {
        BlockedAudioSource.clip = Instance.Clip;
        BlockedAudioSource.Play();
        
        if (Instance.Mode == Assets.Scripts.SoundController.PlayMode.WaitAndBlock)
        {
            BlockTimeInSeconds = Instance.Clip.length;
            StartCoroutine("BlockAudioSource");
        }
    }

    private void GetAudioClipBufferLock()
    {
        AudioClipBufferLock.WaitOne();
    }

    private void ReleaseAudioClipBufferLock()
    {
        AudioClipBufferLock.Release();
    }
}
