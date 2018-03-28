using Assets.Scripts.Recording.PlayerActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Recording
{
    internal class PlayersActionsInRound
    {
        private List<PlayerAction> Actions { get; set; }

        private int CurrentIndex = 0;
        private int FrameNumber = 0;


        public PlayersActionsInRound()
        {
            Actions = new List<PlayerAction>();
        }

        public void AddAction(PlayerAction action)
        {
            Actions.Add(action);
        }

        public List<PlayerAction> GetPlayerActionsForNextFrame()
        {
            FrameNumber++;

            if(Actions[CurrentIndex].FrameNumber != FrameNumber)
            {
                return null;
            }

            List<PlayerAction> RetrievedActions = new List<PlayerAction>();
            
            int MaxIndex = Actions.Count - 1;

            while(CurrentIndex < MaxIndex)
            {
                if(Actions[CurrentIndex].FrameNumber <= FrameNumber)
                {
                    RetrievedActions.Add(Actions[CurrentIndex]);
                }
                else
                {
                    break;
                }
                CurrentIndex++;
            }

            return RetrievedActions;
        }

        public List<PlayerAction> GetPlayerActionsForFrame(int FrameNumber)
        {
            if (Actions != null && Actions.Any())
            {
                try
                {
                    return Actions.Where(x => x.FrameNumber == FrameNumber).ToList();
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
