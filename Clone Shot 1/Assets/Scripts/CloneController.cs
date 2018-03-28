using Assets.Scripts.Recording;
using Assets.Scripts.Recording.PlayerActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    Vector3 offset;
    private Transform master;
    private Transform myTransform;
    private static PlayersActionsInRound Actions;

    private static int CurrentFrameNumber;

    private List<PlayerAction> CurrentRoundActions;

    void Start()
    { 
        master = GameObject.Find("Player").transform;
        myTransform = transform;
        offset = master.position - myTransform.position;

        Actions = new PlayersActionsInRound();

        CurrentFrameNumber = 0;

        CurrentRoundActions = new List<PlayerAction>();
    }

    void Update()
    {
        CurrentFrameNumber++;

        CurrentRoundActions = Actions.GetPlayerActionsForNextFrame();

        if(CurrentRoundActions != null)
        {
            foreach(PlayerAction action in CurrentRoundActions)
            {
                action.PerformActionOnObject(gameObject);
            }
        }
    }

    /// <summary>
    /// Currently is a static funcion until we have hte game controller made
    /// </summary>
    /// <param name="actions"></param>
    internal static void SetActionReader(PlayersActionsInRound actions)
    {
        CurrentFrameNumber = 0;
        Actions = actions;
    }

}
