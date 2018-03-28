using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Recording.PlayerActions
{
    internal abstract class PlayerAction
    {
        public enum PlayerActions {Movement, Turn, JumpStart, JumpEnd, Fire  }

        public PlayerActions ActionType { get; set; }

        public int FrameNumber { get; set; }

        public PlayerAction(int frameNumber)
        {
            FrameNumber = frameNumber;
        }

        public abstract void PerformActionOnObject(GameObject Object);
    }
}
