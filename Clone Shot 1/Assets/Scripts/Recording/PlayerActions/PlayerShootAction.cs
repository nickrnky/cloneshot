using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    internal class PlayerShootAction: PlayerAction
    {
        public PlayerShootAction(int frameNumber)
            : base(frameNumber)
        {
            ActionType = PlayerActions.Shoot;
        }

        public override void PerformActionOnObject(GameObject Object)
        {
            PlayerCharacter Player = Object.GetComponent<PlayerCharacter>();
            if(Player != null)
            {
                Player.Shoot();
            }
        }
    }
}
