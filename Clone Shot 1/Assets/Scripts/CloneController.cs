using Assets.Scripts;
using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CloneController : Character
{
    #region Properties
    /// <summary>
    /// Represents the actions that the clone will be replicating
    /// </summary>
    private PlayersActionsInRound Actions;

    private float MinX, MinY, MinZ, MaxX, MaxY, MaxZ;

    /// <summary>
    /// Represents the actions that should be performed on the current frame.
    /// </summary>
    private List<PlayerAction> CurrentRoundActions;

    private CharacterController _charController;

    /// <summary>
    /// Controls whether or not the controller should be performing actions. Controlled by the start and stop functions.
    /// </summary>
    private bool DoActions = false;

    [SerializeField]
    public int ObstacleRange = 6;

    [SerializeField]
    public int Speed = 10;
    public float gravity = -9.8f;
    public float jump_speed = 5.0f;
    public float FallingSpeed = 0;

    /// <summary>
    /// The point that the clone should start its actions at.
    /// </summary>
    private Vector3 StartingPosition;

    #endregion Properties

    #region Unity Methods

    /// <summary>
    /// The method that is called when unity instantiates the object.
    /// </summary>
    void Start()
    {

        CurrentFrameNumber = 0;

        CurrentRoundActions = new List<PlayerAction>();

        _charController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// The method thats called during each frame tick in Unity
    /// </summary>
    void Update()
    {
        if(IsDead)
        {
            DoActions = false;
            gameObject.SetActive(false);
        }
        if (DoActions)
        {
            #region If the clone has prerocorded actions to take
            if (!Actions.FinishedReading)
            {
                CurrentFrameNumber++;

                if (Actions != null)
                {
                    CurrentRoundActions = Actions.GetPlayerActionsForNextFrame();

                    if (CurrentRoundActions != null)
                    {
                        foreach (PlayerAction action in CurrentRoundActions)
                        {
                            action.PerformActionOnObject(gameObject);
                        }
                    }
                }
            }
            #endregion If the clone has prerocorded actions to take

            #region AI actions
            else
            {
                Debug.Log("Clone Finished Reading");

                AIMove();

                if(Random.Range(0, 100) == 1)
                {
                    AITurn();
                }

                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 0.75f, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    Player HitPlayer = hitObject.GetComponent<Player>();
                    if (HitPlayer != null && HitPlayer.Team != Team)
                    {
                        AIShoot();
                    }
                    else if (hit.distance < ObstacleRange)
                    {
                        AITurn();
                    }
                }
            }
            #endregion AI actions
        }
    }

    #endregion Unity Methods

    #region Internal Methods

    /// <summary>
    /// Resets the current frame number to 0, places the clone back at the starting position, and resets the actions to the beggining.
    /// </summary>
    internal void ResetActions()
    {
        CurrentFrameNumber = 0;
        gameObject.transform.position = StartingPosition;
        Actions.ResetActions();
    }

    /// <summary>
    /// Sets the actions that the clone should be imatating.
    /// </summary>
    /// <param name="actions"></param>
    internal void SetActionReader(PlayersActionsInRound actions)
    {
        CurrentFrameNumber = 0;
        Actions = actions;
    }

    /// <summary>
    /// Sets the starting position of the clone.
    /// </summary>
    /// <param name="startingPosition"></param>
    internal void SetStartingPosition(Vector3 startingPosition)
    {
        StartingPosition = startingPosition;
        gameObject.transform.position = StartingPosition;
    }

    /// <summary>
    /// Starts performing actions, and progressing the frame counter.
    /// </summary>
    internal void StartActions()
    {
        DoActions = true;
    }

    /// <summary>
    /// Stops performing actions, and stops progressing the frame counter.
    /// </summary>
    internal void StopActions()
    {
        DoActions = false;
    }

    internal void Test()
    {
        Actions.Test();
    }

    #endregion Internal Methods

    #region Public methods

    private void AIMove()
    {
        float deltaX = Random.Range(0f, Speed);
        float deltaZ = Random.Range(0f, Speed);
        
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        movement = Vector3.ClampMagnitude(movement, Speed);

        if (Random.Range(0, 500) == 1)
        {
            FallingSpeed = jump_speed;
        }

        FallingSpeed = gravity * Time.deltaTime + FallingSpeed;
        movement.y = FallingSpeed;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);


        if (transform.position.y < MinY)
        {
            movement.y = jump_speed;
        }
        else if (transform.position.y > MaxY)
        {
            movement.y = gravity * Time.deltaTime + gravity;
        }

        if (transform.position.x < MinX)
        {
            movement.x = Speed * Time.deltaTime;
        }
        else if (transform.position.x > MaxX)
        {
            movement.x = Speed * -1 * Time.deltaTime;
        }
        if (transform.position.z < MinZ)
        {
            movement.z = Speed * Time.deltaTime;
        }
        else if (transform.position.z > MaxZ)
        {
            movement.z = Speed * -1 * Time.deltaTime;
        }

        _charController.Move(movement);

        if (_charController.isGrounded)
        {
            FallingSpeed = 0;
        }
    }

    private void AIShoot()
    {
        if (FireSound != null)
        {
            FireSound.Play();
        }

        Vector3 point = gameObject.transform.forward;

        Vector3 StartingPoint;
        if (ShootingZone != null)
        {
            StartingPoint = ShootingZone.transform.position;
        }
        else
        {
            StartingPoint = gameObject.transform.position;
        }

        CmdShoot(point, StartingPoint);
    }

    private void AITurn()
    {
        float angle = Random.Range(-110, 110);
        transform.Rotate(0, angle, 0);
    }

    public void CloneTakeDamage(int damage)
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

    public void SetBoundsCheck(Collider Bounds)
    {
        Vector3 MinPoint = Bounds.bounds.min;
        Vector3 MaxPoint = Bounds.bounds.max;

        MinX = MinPoint.x;
        MinY = MinPoint.y;
        MinZ = MinPoint.z;

        MaxX = MaxPoint.x;
        MaxY = MaxPoint.y;
        MaxZ = MaxPoint.z;
    }

    public override void SetTeam(Teams team)
    {
        Team = team;

    }
    #endregion Public methods
}


