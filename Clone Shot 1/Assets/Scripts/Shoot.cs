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

        //public float YRotation = 0;
        public Vector3 PointToTravelTo;

        private float YSpeed, ZSpeed;

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
            transform.position += PointToTravelTo * speed * Time.deltaTime;
        }

        IEnumerator Life()
        {
            yield return new WaitForSeconds(Lifetime);
            NetworkServer.Destroy(gameObject);
        }

        //[Command]
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger Entered");
            if (!isServer)
            {
                return;
            }

            Player target = other.gameObject.GetComponent<Player>();
            if(target == null)
            {
                target = other.gameObject.GetComponentInParent<Player>();
            }

            if (target != null)
            {
                if (target.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(target.GetPlayerID(), Damage);
                }
            }
            else
            {
                NetworkInstanceId cloneTarget = other.gameObject.GetComponentInParent<NetworkIdentity>().netId;

                CmdCloneShot(cloneTarget, Damage);
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

        [Command]
        void CmdCloneShot(NetworkInstanceId cloneID, int damage)
        {

            Debug.Log("Clone " + cloneID + " has been shot!");
            RoundManager.CloneHit(cloneID, damage);

        }
    }
}
