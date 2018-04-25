using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;
using Assets.Scripts;
using Assets.Scripts.SoundController;

[RequireComponent(typeof(Collider))]
public class Player : Character
{
    #region Properties
    
    /// <summary>
    /// Object used to record all of the players action in a round.
    /// </summary>
    private PlayersActionsInRound ActionsInRound;

    [SerializeField]
    public Camera MainCamera;

    /// <summary>
    /// Represents the color that the screen should flash when the character is damaged
    /// </summary>
    public Color DamageFlashColour { get; set; }

    /// <summary>
    /// Represents the speed at which the screen should flash the damage color when damaged
    /// </summary>
    public float DamageFlashSpeed = 5f;

    [SerializeField]
    public float JumpForce = 10;

    public int ClipSize = 30;
    private int NumberBullets;
    private bool reloading = false;

    /// <summary>
    /// The script being used to handle the players movement.
    /// </summary>
    public FPSInput PlayerMovement;

    private Vector3 PreviousMovement;
    private Quaternion PreviousRotation;


    [SerializeField]
    public SoundController SoundManager;

    private Text RoundText;
    private Text WinText;
    private int TotalWins = 0;


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

        NumberBullets = ClipSize;

        RoundText = this.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>();
        WinText = this.transform.Find("Canvas").transform.Find("Text (1)").GetComponent<Text>();

        RoundText.text = "Round: 0";
        WinText.text = "Wins: 0";
    }

    /// <summary>
    /// Function that runs after every frame.
    /// </summary>
    void Update()
    {
        CurrentFrameNumber++;
        Damaged = false;

        // Set win text
        if (isLocalPlayer)
        {
            WinText.text = "Wins: " + TotalWins;
        }
        else
        {
            WinText.text = "";
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!reloading)
            {
                reloading = true;
                StartCoroutine(RunReload());
            }
        }

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
            // Number of bullets check
            if(NumberBullets == 0)
            {
                return;
            }
            else
            {
                NumberBullets--;
            }

            SoundManager.ProcessSoundEffect(Assets.Scripts.SoundController.PlayMode.Immediate, SoundEffects.PlasmaShot);
            
            Vector3 point = new Vector3(MainCamera.pixelWidth / 2, MainCamera.pixelHeight / 2, 0);
            Ray Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            point = Ray.direction;

            Vector3 StartingPoint;
            if(ShootingZone != null)
            {
                StartingPoint = ShootingZone.transform.position;
            }
            else
            {
                StartingPoint = gameObject.transform.position;
            }

            CmdShoot(point, StartingPoint);

            ActionsInRound.AddAction(new PlayerShootAction(CurrentFrameNumber, point, StartingPoint));
        }
        else if(GroundCheck != null)
        {
            IsJumping = Physics.Raycast(transform.position, -Vector3.up, DistanceToTheGround + 0.1f);
        }
    }


    #endregion Unity Events

    #region Private Methods
    IEnumerator RunReload()
    {
        // Start sound
        SoundManager.ProcessSoundEffect(Assets.Scripts.SoundController.PlayMode.Immediate, SoundEffects.Reload);
        yield return new WaitForSeconds(3.0f);
        NumberBullets = ClipSize;
        reloading = false;
        yield return null;
    }

    public override void Die()
    {
        PlayerMovement.AllowMovement = false;
        IsDead = true;
        Debug.Log("Player " + GetPlayerID() + " is dead!");
        RpcPlayDeathSound();
    }

    [ClientRpc]
    public void RpcPlaySound(Assets.Scripts.SoundController.PlayMode Mode, SoundEffects SoundEffect)
    {
        if (isLocalPlayer)
        {
            if (SoundManager != null)
            {
                SoundManager.ProcessSoundEffect(Mode, SoundEffect);
            }
        }
    }

    // Respawn player called on server but run on client
    [ClientRpc]
    public void RpcRespawn(Vector3 location)
    {
        this.transform.position = location;
        PlayerMovement.AllowMovement = true;
        ActionsInRound = new PlayersActionsInRound();
        CurrentHealth = MaxHealth;
        IsDead = false;

        CurrentFrameNumber = 0;
    }

    // Set Round
    [ClientRpc]
    public void RpcSetRound(int RoundNumber)
    {
        RoundText.text = "Round: " + RoundNumber;
    }

    // Increment win
    [ClientRpc]
    public void RpcAddWin()
    {

        TotalWins++;
        WinText.text = "Wins: " + TotalWins;
        
    }

    internal PlayersActionsInRound GetPlayerActions()
    {
        return ActionsInRound;
    }

    internal void ResetActions()
    {
        ActionsInRound = new PlayersActionsInRound();
    }

    #endregion Private Methods
}
