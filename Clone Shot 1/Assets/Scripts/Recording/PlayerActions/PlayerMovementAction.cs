using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    internal class PlayerMovementAction : PlayerAction
    {
        private Vector3 Position;

        public PlayerMovementAction(Vector3 position, int frameNumber)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Movement;

            Position = position;

            FrameNumber = frameNumber;
        }

        public override void PerformActionOnObject(GameObject Object)
        {
            if (Position != null)
            {
                Object.transform.position = Position;
            }        
        }
    }
}
