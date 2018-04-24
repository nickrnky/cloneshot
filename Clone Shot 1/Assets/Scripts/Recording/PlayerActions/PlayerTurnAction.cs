using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    /// <summary>
    /// Represents a rotation action
    /// </summary>
    internal class PlayerTurnAction : PlayerAction
    {
        #region Properties

        /// <summary>
        /// The rotation of the action
        /// </summary>
        private Quaternion Rotation;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The rotation value represents how the object should be rotated, the frame number represents when the object should be rotated.
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="frameNumber"></param>
        public PlayerTurnAction(Quaternion rotation, int frameNumber)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Movement;

            Rotation = rotation;
        }

        #endregion Constructor

        #region Public Functions

        /// <summary>
        /// Performs the set rotation on the object.
        /// </summary>
        /// <param name="Object"></param>
        public override void PerformActionOnObject(GameObject Object)
        {
            Object.transform.rotation = Rotation;
        }

        #endregion Public Functions
    }
}


