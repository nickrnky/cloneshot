using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicPlayer : MonoBehaviour {

    // Use this for initialization
    public int MusicLength { get; set; }
    public int MaxAudioDistance { get; set; }
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.loop = true;
        audio.time = Random.Range(0, MusicLength);
        audio.maxDistance = MaxAudioDistance;

        audio.Play();
   }
    

    // Update is called once per frame
    void Update () {
		
	}
}
