using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;

public class PlayerCharacter : MonoBehaviour {

    #region Properties

    /// <summary>
    /// Represents the current health of the player
    /// </summary>
    private int CurrentHealth { get; set; }

    /// <summary>
    /// Is set to true if the character was damaged, set to false when the damage has been processed.
    /// </summary>
    private bool Damaged { get; set; }

    /// <summary>
    /// Represents the color that the screen should flash when the character is damaged
    /// </summary>
    public Color DamageFlashColour { get; set; }

    /// <summary>
    /// Represents the speed at which the screen should flash the damage color when damaged
    /// </summary>
    public float DamageFlashSpeed = 5f;

    /// <summary>
    /// Represents what frame is currently being run.
    /// </summary>
    private int CurrentFrameNumber = 0;

    /// <summary>
    /// Is set to true if the player is dead.
    /// </summary>
    private bool IsDead { get; set; }

    /// <summary>
    /// Represents the maximum health the player can have.
    /// </summary>
    public int MaxHealth = 100;

    /// <summary>
    /// Object used to record all of the players action in a round.
    /// </summary>
    private PlayersActionsInRound ActionsInRound;

    /// <summary>
    /// The script being used to handle the players movement.
    /// </summary>
    public FPSInput PlayerMovement;

    private Vector3 PreviousMovement;
    private Quaternion PreviousRotation;



    #endregion Properties
    
    #region Unity Events

    /// <summary>
    /// Function that runs at the start of the scene
    /// </summary>
    void Start()
    {
        IsDead = false;
        ActionsInRound = new PlayersActionsInRound();
        Damaged = false;
		CurrentHealth = 100;
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

        if(PreviousMovement != transform.position)
        {
            PreviousMovement = transform.position;
            ActionsInRound.AddAction(new PlayerMovementAction(PreviousMovement, CurrentFrameNumber));
        }
        
        if(PreviousRotation != transform.rotation)
        {
            PreviousRotation = transform.rotation;
            ActionsInRound.AddAction(new PlayerTurnAction(PreviousRotation, CurrentFrameNumber));
        }

        if(CurrentFrameNumber == 200)
        {
            CloneController.SetActionReader(ActionsInRound);
        }
    }

    #endregion Unity Events

    #region Private Methods

    public void Hurt(int damage)
    {
		CurrentHealth -= damage;

        Damaged = true;
        
        if(CurrentHealth <= 0)
        {
            if(!IsDead)
            {
                Die();
            }
        }
	}

    private void Die()
    {
        PlayerMovement.AllowMovement = false;
    }

    #endregion Private Methods
}
