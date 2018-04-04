using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;

public class Player : NetworkBehaviour
{

    #region Properties

    /// <summary>
    /// The attached camera
    /// </summary>
    private Camera _camera;

    [SerializeField]
    private GameObject bulletPrefab;

    /// <summary>
    /// Represents the current health of the player
    /// </summary>
    /// 
    [SyncVar]
    private int CurrentHealth;

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
    /// Sound thats played when the weapon is fired.
    /// </summary>
    public AudioSource FireSound;

    /// <summary>
    /// Is set to true if the player is dead.
    /// </summary>
    private bool IsDead { get; set; }

    /// <summary>
    /// Represents the maximum health the player can have.
    /// </summary>
    /// 
    [SerializeField]
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


        _camera = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

            Shoot();

            ActionsInRound.AddAction(new PlayerShootAction(CurrentFrameNumber));
        }

        if (CurrentFrameNumber == 200)
        {
            CloneController.SetActionReader(ActionsInRound);
        }
    }

    #endregion Unity Events

    #region Private Methods

    public void TakeDamage(int damage)
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

    private void Die()
    {
        PlayerMovement.AllowMovement = false;
    }

    public void Shoot()
    {
        GameObject _gunshot = Instantiate(bulletPrefab) as GameObject;
        _gunshot.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
        _gunshot.transform.rotation = transform.rotation;
    }

    //private IEnumerator SphereIndicator(Vector3 pos)
    //{
    //    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    sphere.transform.position = pos;

    //    yield return new WaitForSeconds(1);

    //    Destroy(sphere);
    //}

    internal PlayersActionsInRound GetPlayerActions()
    {
        return ActionsInRound;
    }

    #endregion Private Methods
}
