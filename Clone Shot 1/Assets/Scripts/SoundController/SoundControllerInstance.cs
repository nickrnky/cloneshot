using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.SoundController
{
    /// <summary>
    /// Immediate will play immediately, 
    /// Block will block any sound effects qued as wait, 
    /// Wait will wait for any blocking sound effects to end,
    /// Wait and block willw ait for any block sounds to finish, then play and block.
    /// </summary>
    public enum PlayMode {Immediate, WaitAndBlock, Wait, Block }

    class SoundControllerInstance
    {
        public PlayMode Mode;
        public AudioClip Clip;

        public SoundControllerInstance(PlayMode mode, AudioClip clip)
        {
            Mode = mode;
            Clip = clip;
        }

        public SoundControllerInstance(PlayMode mode, SoundEffects SoundEffect)
        {
            Mode = mode;
            Clip = SoundEffectManager.GetClip(SoundEffect);
        }
    }
}
