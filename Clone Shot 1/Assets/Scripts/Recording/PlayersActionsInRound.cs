using Assets.Scripts.Recording.PlayerActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording
{
    internal class PlayersActionsInRound
    {
        #region Properties

        /// <summary>
        /// The list of actions that were performed
        /// </summary>
        private List<PlayerAction> Actions { get; set; }

        /// <summary>
        /// The index number of the last action processed. (Used for optimization purposes).
        /// </summary>
        private int CurrentIndex = 0;

        /// <summary>
        /// The frame number. Is incremented each time GetPlayerActionsForNextFrame is called.
        /// </summary>
        private int FrameNumber = 0;

        #endregion Properties

        #region Constructors

        public PlayersActionsInRound()
        {
            Actions = new List<PlayerAction>();
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Adds an action to the list of actions
        /// </summary>
        /// <param name="action"></param>
        internal void AddAction(PlayerAction action)
        {
            Actions.Add(action);
        }

        internal void Test()
        {
            if(Actions == null)
            {
                Debug.Log("Null");

            }
            else if(!Actions.Any())
            {
                Debug.Log("Empty");
            }
            else
            {
                Debug.Log("Nothing was rwong");
            }
        }

        /// <summary>
        /// Gets the actions that would occur on the next frame, and increments the frame counter.
        /// </summary>
        /// <returns></returns>
        internal List<PlayerAction> GetPlayerActionsForNextFrame()
        {
            FrameNumber++;
            if (Actions == null || !Actions.Any() || Actions.Count <= CurrentIndex)
            {
                return null;
            }

            if (Actions[CurrentIndex].FrameNumber != FrameNumber)
            {
                return null;
            }

            List<PlayerAction> RetrievedActions = new List<PlayerAction>();

            int MaxIndex = Actions.Count - 1;

            while (CurrentIndex < MaxIndex)
            {
                if (Actions[CurrentIndex].FrameNumber <= FrameNumber)
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

        /// <summary>
        /// Gets the actions that would occur on the current frame.
        /// </summary>
        /// <param name="FrameNumber"></param>
        /// <returns></returns>
        internal List<PlayerAction> GetPlayerActionsForFrame(int FrameNumber)
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

        /// <summary>
        /// Resets the frame counter back to 0.
        /// </summary>
        internal void ResetActions()
        {
            CurrentIndex = 0;
            FrameNumber = 0;
        }

        /// <summary>
        /// Sorts the actions using each actions frame number. Sort type is ascending.
        /// </summary>
        internal void SortActions()
        {
            Actions = Actions.OrderBy(x => x.FrameNumber).ToList();
        }

        #endregion Internal Methods
    }
}


