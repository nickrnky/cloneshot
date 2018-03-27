using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyCounter : MonoBehaviour
    {
        private static int Counter = 0;
        public Text Display;


        void Update()
        {
            Display.text = "Enemy Count: " + Counter;
        }

        public static void AddEnemy()
        {
            Counter++;
        }

        public static void SubtractEnemy()
        {
            Counter--;
        }
    }
}
