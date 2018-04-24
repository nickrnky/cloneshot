using Assets.Scripts.SoundController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RoundManagerClasses
{
    internal class KillStreakBufferInstance
    {
        public PlayMode Mode;
        public SoundEffects SoundEffect;
        public Teams Team;
        
        public KillStreakBufferInstance(PlayMode mode, SoundEffects soundEffect, Teams team)
        {
            Mode = mode;
            SoundEffect = soundEffect;
            Team = team;
        }
    }
}
