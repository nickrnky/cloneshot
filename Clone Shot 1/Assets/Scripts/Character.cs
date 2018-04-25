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
    [RequireComponent(typeof(AudioSource))]
    public class Character : NetworkBehaviour
    {
        #region Properties

        private AudioSource Audio;

        [SerializeField]
        public GameObject BlueGlow;

        [SerializeField]
        public GameObject RedGlow;

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
        /// Represents the server ID of the player
        /// </summary>
        private string PlayerID;

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
        /// Sound thats played when the weapon is fired.
        /// </summary>
        public AudioSource FireSound;

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

        [SerializeField]
        public GameObject ShootingZone;

        private Teams team;
        public Teams Team
        {
            get
            {
                return team;
            }
            set
            {
                Debug.Log("Team set to: " + value);
                team = value;
                RpcSetGlow(value);
            }
        }
        
        #endregion Properties

        #region Unity Events

        /// <summary>
        /// Function that runs at the start of the scene
        /// </summary>
        void Start()
        {
            Team = Team;

            Audio = GetComponent<AudioSource>();
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

        internal void InitializeGlow()
        {
            Debug.Log("turning glow off");
            if (BlueGlow != null)
            {
                BlueGlow.SetActive(false);
            }
            if (RedGlow != null)
            {
                RedGlow.SetActive(false);
            }
        }

        #region Public Functions

        public bool IsAlive()
        {
            return !IsDead;
        }
        /// <summary>
        /// Sends a shoot command originating from the player
        /// </summary>
        [Command]
        public void CmdShoot(Vector3 PointToTravelTo, Vector3 StartingPoint)
        {
            GameObject _gunshot = (GameObject)Instantiate(bulletPrefab, StartingPoint, transform.rotation);
            Shoot ShootScript = _gunshot.GetComponent<Shoot>();

            if(ShootScript != null)
            {
                ShootScript.PointToTravelTo = PointToTravelTo;
            }
            //NetworkServer.SpawnWithClientAuthority(_gunshot, connectionToClient);
            NetworkServer.Spawn(_gunshot);
        }

        [Command]
        public void CmdDie()
        {
            this.IsDead = true;
            CurrentHealth = 0;
            Die();
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public virtual void Die()
        {
            gameObject.SetActive(false);
            IsDead = true;
            RpcPlayDeathSound();
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

            RpcPlayDamageTakenSound();
        }

        [ClientRpc]
        public void RpcPlayDamageTakenSound()
        {
            AudioClip clip = new AudioClip();

            clip = SoundEffectManager.GetClip(SoundEffects.Owww);
            
            if(Audio == null)
            {
                Audio = GetComponent<AudioSource>();
            }
            Audio.PlayOneShot(clip);
        }

        [ClientRpc]
        public void RpcPlayDeathSound()
        {

            AudioClip clip = new AudioClip();
            switch (UnityEngine.Random.Range(0, 1))
            {
                case 0:
                    clip = SoundEffectManager.GetClip(SoundEffects.Ahhh);
                    break;
                case 1:
                    clip = SoundEffectManager.GetClip(SoundEffects.MyLeg);
                    break;

            }

            if (Audio == null)
            {
                Audio = GetComponent<AudioSource>();
            }
            Audio.PlayOneShot(clip);
        }

        public string GetPlayerID()
        {
            return PlayerID;
        }

        [ClientRpc]
        public void RpcSetGlow(Teams team)
        {
            if (team == Teams.Blue)
            {
                if (BlueGlow != null)
                {
                    BlueGlow.SetActive(true);
                }
                if (RedGlow != null)
                {
                    RedGlow.SetActive(false);
                }
            }
            else if (team == Teams.Red)
            {
                if (BlueGlow != null)
                {
                    BlueGlow.SetActive(false);
                }
                if (RedGlow != null)
                {
                    RedGlow.SetActive(true);
                }
            }
        }

        public void SetPlayerID(string ID)
        {
            if (string.IsNullOrEmpty(PlayerID))
            {
                PlayerID = ID;
            }
            else
            {
                Debug.Log("Cannot set a players ID twice!");
            }
        }

        public virtual void SetTeam(Teams team)
        {
            Team = team;
        }


        #endregion Public Functions
    }
}
