using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    // Management variables
    private bool RoundInProgress = false;
    private int CurrentRound = 0;
    private List<GameObject> TeamOneClones;
    private List<GameObject> TeamTwoClones;
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private const string PLAYER_ID_PREFIX = "Player ";
    //private GameObject PlayerOne;
    //private GameObject PlayerTwo;

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

        /*
        if (!isServer)
        {
            return;
        }
        */
        RoundInProgress = false;
        //PlayerOne = new GameObject();
        //PlayerTwo = new GameObject();

    }
	
	// Update is called once per frame
	void Update () {
		
        // Check server
        /*
        if (!isServer)
        {
            return;
        }
        */

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

    // Registers the player
    public static void RegisterPlayer(string _netID, Player _player)
    {

        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;

    }

    public static void UuRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }
    
    public static Player GetPlayer (string _playerID)
    {
        return players[_playerID];
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach(string _playerID in players.Keys)
        {

            GUILayout.Label(_playerID + "  -   " + players[_playerID].transform.name);

        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
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
