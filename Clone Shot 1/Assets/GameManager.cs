using Assets.Scripts.Recording;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    // Management variables
    
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private const string PLAYER_ID_PREFIX = "Player ";
    private static int NumberPlayers = 0;
    //private GameObject PlayerOne;
    //private GameObject PlayerTwo;
    //[SerializeField]
    //private RoundManager roundManager = new RoundManager();

    // Use this for initialization
    void Start () {

        //PlayerOne = new GameObject();
        //PlayerTwo = new GameObject();

    }
	
	// Update is called once per frame
	void Update () {

	}

    public Dictionary<string, Player> GetAllPlayers()
    {
        return players;
    }

    public int GetNumberPlayers()
    {
        return NumberPlayers;
    }

    // Registers the player
    public static void RegisterPlayer(string _netID, Player _player)
    {

        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.SetPlayerID(_playerID);
        _player.transform.name = _playerID;
        NumberPlayers++;

    }

    public static void UuRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
        NumberPlayers--;
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

    
}

