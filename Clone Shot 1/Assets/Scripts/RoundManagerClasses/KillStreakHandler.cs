using Assets.Scripts.SoundController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RoundManagerClasses
{
    public class KillStreakHandler
    {
        private int Streak = 0;
        private Teams TeamWithLastKill;

        private int PreviousNumberBlueTeamAlive, PreviousNumberRedTeamAlive;
        private int NumberBlueTeamAlive, NumberRedTeamAlive;

        private List<KillStreakBufferInstance> KillStreakBuffer = new List<KillStreakBufferInstance>();

        public bool UpdateKillStreakReturnTrueIfBufferChanged(int NumberOfBlueTeamAlive, int NumberOfRedTeamAlive)
        {
            int KillStreakCount = KillStreakBuffer.Count;

            int RedTeamDifference = 0, BlueTeamDifference = 0;

            if(NumberBlueTeamAlive != NumberOfBlueTeamAlive)
            {
                BlueTeamDifference = NumberBlueTeamAlive - NumberOfBlueTeamAlive;
                PreviousNumberBlueTeamAlive = NumberBlueTeamAlive;
                NumberBlueTeamAlive = NumberOfBlueTeamAlive;
            }

            if(NumberRedTeamAlive != NumberOfRedTeamAlive)
            {
                RedTeamDifference = NumberRedTeamAlive - NumberOfRedTeamAlive;
                PreviousNumberRedTeamAlive = NumberRedTeamAlive;
                NumberRedTeamAlive = NumberOfRedTeamAlive;
            }

            if(BlueTeamDifference + RedTeamDifference == 0)
            {
                return false;
            }

            if(BlueTeamDifference != 0 && RedTeamDifference == 0)
            {
                if(TeamWithLastKill == Teams.Red)
                {
                    Streak++;
                }
                else
                {
                    TeamWithLastKill = Teams.Red;
                    Streak = 1;
                }
            }

            if(RedTeamDifference != 0 && BlueTeamDifference == 0)
            {
                if(TeamWithLastKill == Teams.Blue)
                {
                    Streak++;
                }
                else
                {
                    TeamWithLastKill = Teams.Blue;
                    Streak = 1;
                }
            }

            if(BlueTeamDifference > 1)
            {
                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Perfect, Teams.Red));
            }

            if(RedTeamDifference > 1)
            {
                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Perfect, Teams.Blue));
            }



            return KillStreakCount == KillStreakBuffer.Count;
        }


    }
}
