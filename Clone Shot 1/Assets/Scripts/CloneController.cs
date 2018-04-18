using Assets.Scripts;
using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : Character
{
    #region Properties
    /// <summary>
    /// Represents the actions that the clone will be replicating
    /// </summary>
    private PlayersActionsInRound Actions;

    /// <summary>
    /// Represents the actions that should be performed on the current frame.
    /// </summary>
    private List<PlayerAction> CurrentRoundActions;
    
    /// <summary>
    /// Controls whether or not the controller should be performing actions. Controlled by the start and stop functions.
    /// </summary>
    private bool DoActions = false;

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
    }

    /// <summary>
    /// The method thats called during each frame tick in Unity
    /// </summary>
    void Update()
    {
        if (DoActions)
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
}


