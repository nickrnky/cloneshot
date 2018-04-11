using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    class Shoot : NetworkBehaviour
    {
        private const string PLAYER_TAG = "Player";
        public float speed = 10.0f;
        public int Damage = 1;
        public int Lifetime = 10;

        public float YRotation = 0;

        private float YSpeed, ZSpeed;

        void Start()
        {
            if (!isServer)
            {
                return;
            }
            StartCoroutine(Life());

            bool NegativeY = true;
            if(YRotation > 90)
            {
                YRotation -= 270;
                NegativeY = false;
                YRotation = 90 - YRotation;
            }

            Debug.Log(YRotation);

            if (YRotation >= 0)
            {
                if(NegativeY)
                {
                    YSpeed = -1 * speed * (float)Math.Sin(YRotation);
                }
                else
                {
                    YSpeed = speed * (float)Math.Sin(YRotation);
                }
                ZSpeed = speed * (float)Math.Cos(YRotation);
            }

            if(ZSpeed < 0)
            {
                ZSpeed *= -1;
            }
            if(YSpeed < 0 && !NegativeY)
            {
                YSpeed *= -1;
            }

            Debug.Log(YSpeed + " " + ZSpeed);
            
        }

        void Update()
        {
            transform.Translate(0, YSpeed * Time.deltaTime, ZSpeed * Time.deltaTime);
        }

        IEnumerator Life()
        {
            yield return new WaitForSeconds(Lifetime);
            NetworkServer.Destroy(gameObject);
        }

        [Client]
        void OnTriggerEnter(Collider other)
        {
            Player target = other.gameObject.GetComponent<Player>();
            if (target != null)
            {
                if (target.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(target.GetPlayerID(), Damage);
                }
            }
            else
            {
                target = other.gameObject.GetComponentInParent<Player>();
                {
                    if(target != null)
                    {
                        if (target.tag == PLAYER_TAG)
                        {
                            CmdPlayerShot(target.GetPlayerID(), Damage);
                        }
                    }
                }
            }

            CmdDestroy();
        }

        [Command]
        void CmdDestroy()
        {
            NetworkServer.Destroy(this.gameObject);
        }

        [Command]
        void CmdPlayerShot(string _playerID, int _damage)
        {

            Debug.Log(_playerID + " has been shot!");

            Player _player = GameManager.GetPlayer(_playerID);
            _player.TakeDamage(_damage);

        }
    }
}
