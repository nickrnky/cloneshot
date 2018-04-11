using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Player : Character
{
    #region Properties
    
    /// <summary>
    /// Object used to record all of the players action in a round.
    /// </summary>
    private PlayersActionsInRound ActionsInRound;

    /// <summary>
    /// Represents the color that the screen should flash when the character is damaged
    /// </summary>
    public Color DamageFlashColour { get; set; }

    /// <summary>
    /// Represents the speed at which the screen should flash the damage color when damaged
    /// </summary>
    public float DamageFlashSpeed = 5f;

    /// <summary>
    /// Sound thats played when the weapon is fired.
    /// </summary>
    public AudioSource FireSound;

    [SerializeField]
    public float JumpForce = 10;

    /// <summary>
    /// Represents the server ID of the player
    /// </summary>
    private string PlayerID;

    /// <summary>
    /// The script being used to handle the players movement.
    /// </summary>
    public FPSInput PlayerMovement;

    private Vector3 PreviousMovement;
    private Quaternion PreviousRotation;

    private bool PlayerDead = false;



    #endregion Properties

    #region Unity Events

    /// <summary>
    /// Function that runs at the start of the scene
    /// </summary>
    void Start()
    {
        ActionsInRound = new PlayersActionsInRound();
        DamageFlashColour = new Color(1f, 0f, 0f, 0.1f);
        PlayerMovement = GetComponentInParent<FPSInput>();

        PreviousMovement = new Vector3();
        PreviousRotation = new Quaternion();
        PreviousMovement = transform.position;
        PreviousRotation = transform.rotation;
    }

    /// <summary>
    /// Function that runs after every frame.
    /// </summary>
    void Update()
    {
        CurrentFrameNumber++;
        Damaged = false;

        if (PreviousMovement != transform.position)
        {
            PreviousMovement = transform.position;
            ActionsInRound.AddAction(new PlayerMovementAction(PreviousMovement, CurrentFrameNumber));
        }

        if (PreviousRotation != transform.rotation)
        {
            PreviousRotation = transform.rotation;
            ActionsInRound.AddAction(new PlayerTurnAction(PreviousRotation, CurrentFrameNumber));
        }
        if (Input.GetMouseButtonDown(0) && isLocalPlayer)
        {
            if (FireSound != null)
            {
                FireSound.Play();
            }

            CmdShoot();

            ActionsInRound.AddAction(new PlayerShootAction(CurrentFrameNumber));
        }

        if(Input.GetKeyDown(KeyCode.Space) && !IsJumping)
        {
            IsJumping = true;
            Vector3 forceOfJump = transform.up * JumpForce;
            Jump(forceOfJump);
            ActionsInRound.AddAction(new PlayerJumpAction(forceOfJump, CurrentFrameNumber));

        }
        else if(GroundCheck != null)
        {
            IsJumping = Physics.Raycast(transform.position, -Vector3.up, DistanceToTheGround + 0.1f);
        }
    }

    #endregion Unity Events

    #region Private Methods

    public override void Die()
    {
        PlayerMovement.AllowMovement = false;
        PlayerDead = true;
    }

    public bool IsAlive()
    {
        return !PlayerDead;
    }

    public string GetPlayerID()
    {
        return PlayerID;
    }

    public void SetPlayerID(string ID)
    {
        if(string.IsNullOrEmpty(PlayerID))
        {
            PlayerID = ID;
        }
        else
        {
            Debug.Log("Cannot set a players ID twice!");
        }
    }


    internal PlayersActionsInRound GetPlayerActions()
    {
        return ActionsInRound;
    }

    #endregion Private Methods
}
