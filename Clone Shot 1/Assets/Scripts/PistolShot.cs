using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class PistolShot : MonoBehaviour
    {
        public float speed = 10.0f;
        public int Damage = 5;

        void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            PlayerCharacter target = other.gameObject.GetComponent<PlayerCharacter>();
            if (target != null)
            {
                target.Hurt(5);
            }
            else
            {
                target = other.gameObject.GetComponentInParent<PlayerCharacter>();
                {
                    if(target != null)
                    {
                        target.Hurt(5);
                    }
                }
            }

            Destroy(this.gameObject);
        }
    }
}
