using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    /// <summary>
    /// Represents a shooting action.
    /// </summary>
    internal class PlayerShootAction : PlayerAction
    {
        private float YRotation;

        #region Constructor

        /// <summary>
        /// The frame number represents when the action took place
        /// </summary>
        /// <param name="frameNumber"></param>
        public PlayerShootAction(int frameNumber, float yRotation)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Shoot;
            YRotation = yRotation;
        }

        #endregion Constructor

        #region Public Functions

        /// <summary>
        /// Performs a shooting action on the game object.
        /// </summary>
        /// <param name="Object"></param>
        public override void PerformActionOnObject(GameObject Object)
        {
            Character Player = Object.GetComponent<Character>();
            if (Player != null)
            {
                Player.CmdShoot(YRotation);
            }
        }

        #endregion Public Functions
    }
}


