using Assets.Scripts.Recording;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public GameObject ClonePrefab;

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
    public float Spawn1X = -5;
    public float Spawn1Y = 2;
    public float Spawn1Z = 0;
    public float Spawn2X = 5;
    public float Spawn2Y = 2;
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
            Debug.Log("Starting Round");
            StartCoroutine(StartRound());
            return;
        }

	}

    // Registers the player
    public static void RegisterPlayer(string _netID, Player _player)
    {

        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.SetPlayerID(_playerID);
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
        Vector3 PlayerOneStart = new Vector3(Spawn1X, Spawn1Y, Spawn1Z);
        Vector3 PlayerTwoStart = new Vector3(Spawn2X, Spawn2Y, Spawn2Z);

        int count = 0;
        foreach(Player person in players.Values)
        {
            if(count % 2 == 0)
            {
                person.transform.position = PlayerOneStart;
            }
            else
            {
                person.transform.position = PlayerTwoStart;
            }
        }

        // Spawn clones
        foreach(GameObject x in TeamOneClones)
        {
            ResetClone(x);
        }

        foreach(GameObject x in TeamTwoClones)
        {
            ResetClone(x);
        }

        // Start game
        foreach (GameObject x in TeamOneClones)
        {
            StartClone(x);
        }

        foreach (GameObject x in TeamTwoClones)
        {
            StartClone(x);
        }

        // Loop and check for game end
        count = 0;
        bool StillFighting = true;
        while (StillFighting)
        {
            foreach(Player y in players.Values)
            {
                if(count % 2 == 0 && !TeamAlive(TeamOneClones, y)){
                    StillFighting = false;
                }
                else if (count % 2 == 0 && !TeamAlive(TeamTwoClones, y)){
                    StillFighting = false;
                }
            }
        }

        // Clean up and get recording
        count = 0;
        foreach(Player y in players.Values)
        {
            if(count % 2 == 0)
            {
                TeamOneClones.Add(CreateClone(PlayerOneStart, y.GetPlayerActions()));
            }
            else
            {
                TeamTwoClones.Add(CreateClone(PlayerTwoStart, y.GetPlayerActions()));
            }
        }

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

    private bool TeamAlive(List<GameObject> clones, Player player)
    {
        // Temp variables
        bool MainPlayer = true;
        bool CloneLife = false;

        // Check player for life
        if (player.IsAlive())
        {
            MainPlayer = false;
        }

        // Check clones for life
        foreach(GameObject x in clones)
        {
            if (x.GetComponent<Player>().IsAlive())
            {
                CloneLife = true;
            }
        }

        // Return based on results
        if(!MainPlayer && !CloneLife)
        {
            return false;
        }

        return true;
    }

    #region Clone Functions

    /// <summary>
    /// Creates a new clone object at the specified starting position, that will use the passed in actions
    /// </summary>
    /// <param name="StartingPosition"></param>
    /// <param name="ActionReader"></param>
    /// <returns></returns>
    private GameObject CreateClone(Vector3 StartingPosition, PlayersActionsInRound ActionReader)
    {
        if (StartingPosition == null || ActionReader == null)
        {
            Debug.Log("Cannot pass in a null starting position or action reader when creating a clone");
            return null;
        }

        if (ClonePrefab == null)
        {
            Debug.Log("Must set the clone prefab before creating a clone");
            return null;
        }

        GameObject Clone = Instantiate(ClonePrefab) as GameObject;

        CloneController Controller = Clone.GetComponent<CloneController>();

        if (Controller == null)
        {
            Debug.Log("Cannot create a clone instance using a prefab that doesn't have the clone controller component!");
            return null;
        }

        Controller.SetStartingPosition(StartingPosition);
        Controller.SetActionReader(ActionReader);

        return Clone;
    }

    /// <summary>
    /// Resets a clone to its starting position, and sets its actions counter back to frame 0.
    /// </summary>
    /// <param name="Clone"></param>
    private void ResetClone(GameObject Clone)
    {
        CloneController controller = Clone.GetComponent<CloneController>();

        if (controller != null)
        {
            controller.ResetActions();
        }
        else
        {
            Debug.Log("Cannot reset clone actions for an object without a CloneController component!");
            return;
        }
    }

    /// <summary>
    /// Starts a clones action counter. This will make the clone actually start preforming recorded actions.
    /// </summary>
    /// <param name="Clone"></param>
    private void StartClone(GameObject Clone)
    {
        CloneController controller = Clone.GetComponent<CloneController>();

        if (controller != null)
        {
            controller.StartActions();
        }
        else
        {
            Debug.Log("Cannot start clone actions for an object without a CloneController component!");
            return;
        }
    }

    /// <summary>
    /// Stops the clones action counter, so that it will no longer perform actions.
    /// </summary>
    /// <param name="Clone"></param>
    private void StopClone(GameObject Clone)
    {
        CloneController controller = Clone.GetComponent<CloneController>();

        if (controller != null)
        {
            controller.StopActions();
        }
        else
        {
            Debug.Log("Cannot stop clone actions for an object without a CloneController component!");
            return;
        }
    }

    #endregion Clone Functions
}

