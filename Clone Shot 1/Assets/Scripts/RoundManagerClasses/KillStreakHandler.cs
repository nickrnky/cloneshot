using Assets.Scripts.SoundController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RoundManagerClasses
{
    internal class KillStreakHandler
    {
        private int Streak = 0;
        private Teams TeamWithLastKill = Teams.None;
        
        private int NumberBlueTeamAlive, NumberRedTeamAlive;

        private List<KillStreakBufferInstance> KillStreakBuffer = new List<KillStreakBufferInstance>();

        public bool UpdateKillStreakReturnTrueIfBufferChanged(int NumberOfBlueTeamAlive, int NumberOfRedTeamAlive)
        {
            #region Update Information

            int KillStreakCount = KillStreakBuffer.Count;

            int RedTeamDifference = 0, BlueTeamDifference = 0;

            if(NumberBlueTeamAlive != NumberOfBlueTeamAlive)
            {
                BlueTeamDifference = NumberBlueTeamAlive - NumberOfBlueTeamAlive;
                NumberBlueTeamAlive = NumberOfBlueTeamAlive;
            }

            if(NumberRedTeamAlive != NumberOfRedTeamAlive)
            {
                RedTeamDifference = NumberRedTeamAlive - NumberOfRedTeamAlive;
                NumberRedTeamAlive = NumberOfRedTeamAlive;
            }

            if(BlueTeamDifference + RedTeamDifference == 0)
            {
                return false;
            }
            
            #region First Blood

            if(TeamWithLastKill == Teams.None && (BlueTeamDifference + RedTeamDifference) > 0)
            {
                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.FirstBlood, Teams.Red));
                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.FirstBlood, Teams.Blue));
            }

            #endregion First Blood

            if (BlueTeamDifference == RedTeamDifference)
            {
                TeamWithLastKill = Teams.None;
            }

            if (BlueTeamDifference != 0 && RedTeamDifference == 0)
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

            #endregion Update Information

            Random r = new Random();
            Teams PositiveTeam = Teams.None;
            Teams NegativeTeam = Teams.None;
            if (TeamWithLastKill == Teams.Red)
            {
                PositiveTeam = Teams.Red;
                NegativeTeam = Teams.Blue;
            }
            else if(TeamWithLastKill == Teams.Blue)
            {
                PositiveTeam = Teams.Blue;
                NegativeTeam = Teams.Red;
            }
            else if(TeamWithLastKill == Teams.None)
            {
                return KillStreakCount != KillStreakBuffer.Count;
            }

            #region Multiple kills in a single frame

            if (BlueTeamDifference > 1)
            {
                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Perfect, Teams.Red));
            }

            if(RedTeamDifference > 1)
            {
                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Perfect, Teams.Blue));
            }

            #endregion Multiple kills in a single frame

            #region Kill Streak of One
            if(Streak == 1)
            {
                int random = r.Next(0, 1);
                if (NumberBlueTeamAlive - 1 == NumberOfRedTeamAlive || NumberRedTeamAlive - 1 == NumberOfBlueTeamAlive)
                {
                    if (random == 0)
                    {
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.YouHaveTakenTheLead, PositiveTeam));
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.YouHaveLostTheLead, NegativeTeam));
                    }
                    else
                    {
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.RedLead, PositiveTeam));
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.RedLead, NegativeTeam));
                    }
                }
            }

            #endregion Kill Streak of One

            #region Kill Streaks

            if (KillStreakCount > 4)
            {
                int random = r.Next(0, 2);
                switch(random)
                {
                    case 0:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Godlike, PositiveTeam));
                        break;
                    case 1:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.UltraKill, PositiveTeam));
                        break;
                    case 2:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Rampage, PositiveTeam));
                        break;
                }


                KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Humiliation, NegativeTeam));
                
            }
            else if (KillStreakCount > 3)
            {
                int random = r.Next(0, 2);
                switch (random)
                {
                    case 0:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.MonsterKill, PositiveTeam));
                        break;
                    case 1:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Impressive, PositiveTeam));
                        break;
                    case 2:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Unstoppable, PositiveTeam));
                        break;
                }
            }
            else if (KillStreakCount > 2)
            {
                int random = r.Next(0, 2);
                switch (random)
                {
                    case 0:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Dominating, PositiveTeam));
                        break;
                    case 1:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.Excellent, PositiveTeam));
                        break;
                    case 2:
                        KillStreakBuffer.Add(new KillStreakBufferInstance(PlayMode.WaitAndBlock, SoundEffects.KillingSpree, PositiveTeam));
                        break;
                }
            }

            #endregion Kill Streaks


            return KillStreakCount != KillStreakBuffer.Count;
        }

        public KillStreakBufferInstance GetBottomOfBuffer()
        {
            if (KillStreakBuffer == null || !KillStreakBuffer.Any())
            {
                return null;
            }

            KillStreakBufferInstance Instance = KillStreakBuffer[0];
            KillStreakBuffer.RemoveAt(0);
            return Instance;
        }
    }
}
