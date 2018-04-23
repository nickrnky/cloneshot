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
        private Vector3 PointToTravelTo;
        private Vector3 StartingPoint;

        #region Constructor

        /// <summary>
        /// The frame number represents when the action took place
        /// </summary>
        /// <param name="frameNumber"></param>
        public PlayerShootAction(int frameNumber, Vector3 pointToTravelTo, Vector3 startingPoint)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Shoot;
            PointToTravelTo = pointToTravelTo;
            StartingPoint = startingPoint;
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
                Player.CmdShoot(PointToTravelTo, StartingPoint);
            }
        }

        #endregion Public Functions
    }
}


