using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    internal class PlayerTurnAction : PlayerAction
    {
        private Quaternion Rotation;

        public PlayerTurnAction(Quaternion rotation, int frameNumber)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Movement;

            Rotation = rotation;
        }

        public override void PerformActionOnObject(GameObject Object)
        {
            if (Rotation != null)
            {
                Object.transform.rotation = Rotation;
            }
        }
    }
}
