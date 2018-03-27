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
            ReactiveTarget target = other.gameObject.GetComponent<ReactiveTarget>();
            if (target != null)
            {
                target.ReactToHit(5);
            }
            else
            {
                target = other.gameObject.GetComponentInParent<ReactiveTarget>();
                {
                    if(target != null)
                    {
                        target.ReactToHit(5);
                    }
                }
            }

            Destroy(this.gameObject);
        }
    }
}
