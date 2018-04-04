using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    /// <summary>
    /// Represents a movement action.
    /// </summary>
    internal class PlayerMovementAction : PlayerAction
    {
        #region Properties

        /// <summary>
        /// The position that a player should be moved to.
        /// </summary>
        private Vector3 Position;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The position is the position that object should be moved to, the frame number is when the object should be moved.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="frameNumber"></param>
        public PlayerMovementAction(Vector3 position, int frameNumber)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Movement;

            Position = position;

            FrameNumber = frameNumber;
        }

        #endregion Constructor

        #region Public Functions

        /// <summary>
        /// Performs the action on the passed in object.
        /// </summary>
        /// <param name="Object"></param>
        public override void PerformActionOnObject(GameObject Object)
        {
            if (Position != null)
            {
                Object.transform.position = Position;
            }
        }

        #endregion Public Functions
    }
}


