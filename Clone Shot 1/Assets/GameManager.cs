using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    // Management variables
    private bool RoundInProgress = false;
    private int CurrentRound = 0;
    private List<GameObject> TeamOneClones;
    private List<GameObject> TeamTwoClones;
    private GameObject PlayerOne;
    private GameObject PlayerTwo;

    // Configurable variables
    public int NumberOfRounds = 5;
    public float Spawn1X = 0;
    public float Spawn1Y = 0;
    public float Spawn1Z = 0;
    public float Spawn2X = 0;
    public float Spawn2Y = 0;
    public float Spawn2Z = 0;

    // Use this for initialization
    void Start () {

        if (!isServer)
        {
            return;
        }
        RoundInProgress = false;
        PlayerOne = new GameObject();
        PlayerTwo = new GameObject();

    }
	
	// Update is called once per frame
	void Update () {
		
        // Check server
        if (!isServer)
        {
            return;
        }

        // Wait for players
        if(Network.connections.Length < 2)
        {
            RoundInProgress = false;
            return;
        }

        if(CurrentRound < NumberOfRounds && RoundInProgress == false)
        {
            RoundInProgress = true;
            StartCoroutine(StartRound());
            return;
        }

	}

    // Start a round
    IEnumerator StartRound()
    {
        // Position players

        //player1 = Network.player;

        // Set to start positions

        // Spawn clones

        // Start game

        // Loop and check for game end

        // Clean up and get recording

        // End
        CurrentRound++;
        RoundInProgress = false;
        yield return null;
    }

    // End game
    IEnumerator EndGame()
    {

        // Show ending screen

        yield return null;
    }

    private bool TeamAlive(List<GameObject> clones, GameObject player)
    {
        // Check player for life

        // Check clones for life
        foreach(GameObject x in clones)
        {

        }

        return true;
    }
}
