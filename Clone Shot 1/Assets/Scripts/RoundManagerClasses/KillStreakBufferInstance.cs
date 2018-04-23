using Assets.Scripts.SoundController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RoundManagerClasses
{
    class KillStreakBufferInstance
    {
        public SoundControllerInstance Instance;
        public Teams Team;
        
        public KillStreakBufferInstance(PlayMode mode, SoundEffects soundEffect, Teams team)
        {
            Instance = new SoundControllerInstance(mode, soundEffect);
            Team = team;
        }

        public KillStreakBufferInstance(SoundControllerInstance instance, Teams team)
        {
            Instance = instance;
            Team = team;
        }
    }
}
