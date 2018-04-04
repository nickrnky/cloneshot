using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    internal class PlayerJumpAction : PlayerAction
    {
        #region Properties

        /// <summary>
        /// The position that a player should be moved to.
        /// </summary>
        private Vector3 Force;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The position is the position that object should be moved to, the frame number is when the object should be moved.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="frameNumber"></param>
        public PlayerJumpAction(Vector3 force, int frameNumber)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Jump;

            Force = force;

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
            Character Player = Object.GetComponent<Character>();
            if (Player != null)
            {
                Player.Jump(Force);
            }
        }

        #endregion Public Functions
    }
}
