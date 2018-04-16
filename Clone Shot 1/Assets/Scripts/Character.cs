using Assets.Scripts.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{

    [RequireComponent(typeof(Collider))]
    public class Character : NetworkBehaviour
    {
        #region Properties


        [SerializeField]
        public GameObject bulletPrefab;

        /// <summary>
        /// The attached camera
        /// </summary>
        internal Camera _camera;

        /// <summary>
        /// Represents the collider of the parent object
        /// </summary>
        internal Collider SelfCollider;

        /// <summary>
        /// Represents what frame is currently being run.
        /// </summary>
        public int CurrentFrameNumber = 0;

        /// <summary>
        /// Represents the current health of the player
        /// </summary>
        /// 
        [SyncVar]
        internal int CurrentHealth;

        /// <summary>
        /// Is set to true if the character was damaged, set to false when the damage has been processed.
        /// </summary>
        internal bool Damaged { get; set; }

        /// <summary>
        /// Represents the distance of the object to the ground
        /// </summary>
        public float DistanceToTheGround;

        /// <summary>
        /// The object used to check if the player is on the ground
        /// </summary>
        public Transform GroundCheck;

        /// <summary>
        /// Is set to true if the player is dead.
        /// </summary>
        [SyncVar]
        internal bool IsDead;

        /// <summary>
        /// Returns true if the player is jumping
        /// </summary>
        internal bool IsJumping { get; set; }

        /// <summary>
        /// Represents the maximum health the player can have.
        /// </summary>
        /// 
        [SerializeField]
        public int MaxHealth = 100;

        internal Rigidbody SelfRigidBody;



        #endregion Properties

        #region Unity Events

        /// <summary>
        /// Function that runs at the start of the scene
        /// </summary>
        void Start()
        {
            IsJumping = false;
            IsDead = false;
            Damaged = false;
            CurrentHealth = 100;

            _camera = GetComponent<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelfRigidBody = GetComponent<Rigidbody>();
            SelfCollider = GetComponent<Collider>();

            DistanceToTheGround = SelfCollider.bounds.extents.y;
        }

        #endregion Unity Events

        #region Public Functions

        /// <summary>
        /// Sends a shoot command originating from the player
        /// </summary>
        [Command]
        public void CmdShoot(Vector3 PointToTravelTo)
        {
            GameObject _gunshot = (GameObject)Instantiate(bulletPrefab, transform.TransformPoint(0,.7f,1.5f), transform.rotation);
            Shoot ShootScript = _gunshot.GetComponent<Shoot>();

            if(ShootScript != null)
            {
                ShootScript.PointToTravelTo = PointToTravelTo;
            }
            //NetworkServer.SpawnWithClientAuthority(_gunshot, connectionToClient);
            NetworkServer.Spawn(_gunshot);
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public virtual void Die()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Performs a jump actions
        /// </summary>
        /// <param name="Force"></param>
        public void Jump(Vector3 Force)
        {
            //SelfRigidBody.AddForce(Force);
        }

        /// <summary>
        /// Causes the character to take damage.
        /// </summary>
        /// <param name="damage"></param>
        [ClientRpc]
        public void RpcTakeDamage(int damage)
        {
            CurrentHealth -= damage;

            Damaged = true;

            if (CurrentHealth <= 0)
            {
                if (!IsDead)
                {
                    Die();
                }
            }
        }

        #endregion Public Functions
    }
}
