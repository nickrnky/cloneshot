using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    /// <summary>
    /// This class represents any action that a player could do. Whenever a new action is added, an enum should be added to PlayerActions enum.
    /// </summary>
    internal abstract class PlayerAction
    {
        #region Properties

        /// <summary>
        /// Enum that represents the different types of actions that can be performed by the player.
        /// </summary>
        public enum PlayerActions { Movement, Turn, Jump, Shoot }

        /// <summary>
        /// Represents the type of action that the current object represents.
        /// </summary>
        public PlayerActions ActionType { get; set; }

        /// <summary>
        /// The frame number that the action occured on.
        /// </summary>
        public int FrameNumber { get; set; }

        #endregion Properties

        #region Constructors

        public PlayerAction(int frameNumber)
        {
            FrameNumber = frameNumber;
        }

        #endregion Constructors

        #region Abstract Methods

        public abstract void PerformActionOnObject(GameObject Object);

        #endregion Abstract Methods
    }
}


