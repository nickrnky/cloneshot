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
            
            Debug.Log(YRotation);

            bool neg = true;
            if (YRotation > 270)
            {
                neg = false;
                YRotation -= 270;
                YRotation = 90 - YRotation;
            }

            YSpeed = 30 * YRotation / 90;

            if(neg)
            {
                YSpeed *= -1;
            }
            
        }

        void Update()
        {
            transform.Translate(0, YSpeed * Time.deltaTime, speed * Time.deltaTime);
        }

        IEnumerator Life()
        {
            yield return new WaitForSeconds(Lifetime);
            NetworkServer.Destroy(gameObject);
        }

        //[Command]
        void OnTriggerEnter(Collider other)
        {
            if (!isServer)
            {
                return;
            }

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
            _player.RpcTakeDamage(_damage);

        }
    }
}
