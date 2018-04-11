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

        void Start()
        {
            if (!isServer)
            {
                return;
            }
            StartCoroutine(Life());
        }

        void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
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
