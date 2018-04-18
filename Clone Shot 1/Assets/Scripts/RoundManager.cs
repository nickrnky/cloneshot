using Assets.Scripts.Recording;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoundManager : NetworkBehaviour {

    [SerializeField]
    public GameObject ClonePrefab;

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    // Management variables
    private int RoundInProgress = 0;
    private int CurrentRound = 0;
    private List<GameObject> TeamOneClones = new List<GameObject>();
    private List<GameObject> TeamTwoClones = new List<GameObject>();

    [SerializeField]
    GameManager DaManager;

    [SerializeField]
    GameObject Spawner1;

    [SerializeField]
    GameObject Spawner2;

    // Configurable variables
    public int NumberOfRounds = 5;
    Vector3 PlayerOneStart = new Vector3();
    Vector3 PlayerTwoStart = new Vector3();

    // Use this for initialization
    void Start () {

        if (!isServer)
        {
            return;
        }
        RoundInProgress = 0;
        PlayerOneStart = Spawner1.transform.position;
        PlayerOneStart = Spawner2.transform.position;

    }
	
	// Update is called once per frame
	void Update () {

        // Check server
        if (!isServer)
        {
            return;
        }
        //Debug.Log("Connections: " + Network.connections.Length);

        players = DaManager.GetComponent<GameManager>().GetAllPlayers();

        // Wait for players
        if (DaManager.GetComponent<GameManager>().GetNumberPlayers() < 2)
        {
            RoundInProgress = 0;
            return;
        }

        if (CurrentRound < NumberOfRounds && RoundInProgress == 0)
        {
            RoundInProgress = 1;
            Debug.Log("Starting Round " + CurrentRound);
            StartCoroutine(StartRound());
            return;
        }

        // Loop and check for game end
        if(RoundInProgress == 2)
        {
            int count = 0;
            foreach (Player y in players.Values)
            {
                if (count % 2 == 0 && !TeamAlive(TeamOneClones, y))
                {
                    RoundInProgress = 3;
                    Debug.Log("No longer fighting");
                    break;
                }
                else if (count % 2 != 0 && !TeamAlive(TeamTwoClones, y))
                {
                    RoundInProgress = 3;
                    Debug.Log("No longer fighting");
                    break;
                }
                count++;
            }
            
        }
        
        // Post processing
        if(RoundInProgress == 3)
        {
            Debug.Log("Round over " + CurrentRound);
            RoundInProgress = 4;
            StartCoroutine(PostProcessing());
        }
        

    }

    // Start a round
    IEnumerator StartRound()
    {
        // Position players
        Debug.Log("round 1");

        int count = 0;
        foreach (Player person in players.Values)
        {
            if (count % 2 == 0)
            {
                //person.transform.position = PlayerOneStart;
                person.RpcRespawn(PlayerOneStart);
            }
            else
            {
                //person.transform.position = PlayerTwoStart;
                person.RpcRespawn(PlayerTwoStart);
            }
        }

        Debug.Log("round 2");

        // Spawn clones
        foreach (GameObject x in TeamOneClones)
        {
            ResetClone(x);
            x.SetActive(true);
        }
        Debug.Log("round 3");

        foreach (GameObject x in TeamTwoClones)
        {
            ResetClone(x);
            x.SetActive(true);
        }
        Debug.Log("round 4");

        foreach(Player x in players.Values)
        {
            x.ResetActions();
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

        Debug.Log("round 5");
        RoundInProgress = 2;
        yield return null;
    }

    // Post round
    IEnumerator PostProcessing() {
        // Clean up and get recording
        int count = 0;
        foreach (Player y in players.Values)
        {
            PlayersActionsInRound action = new PlayersActionsInRound();
            action = y.GetPlayerActions();

            if (count % 2 == 0)
            {
                GameObject clone = new GameObject();
                clone = CreateClone(PlayerOneStart, action);
                TeamOneClones.Add(clone);
                CmdSpawnClone(clone);
            }
            else
            {
                GameObject clone = new GameObject();
                clone = CreateClone(PlayerOneStart, action);
                TeamTwoClones.Add(clone);
                CmdSpawnClone(clone);
            }
        }

        // Set players to alive
        foreach(Player y in players.Values)
        {
            y.IsDead = false;
            //y.PlayerMovement.AllowMovement = true;
        }

        Debug.Log("round 6");

        // End
        CurrentRound++;
        RoundInProgress = 0;
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
        if (!player.IsAlive())
        {
            MainPlayer = false;
        }

        // Check clones for life
        foreach (GameObject x in clones)
        {
            if (x.GetComponent<CloneController>().IsAlive())
            {
                CloneLife = true;
            }
            else
            {
                x.SetActive(false);
            }
        }

        // Return based on results
        if (!MainPlayer && !CloneLife)
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

        GameObject Clone = (GameObject)Instantiate(ClonePrefab);

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

    [Command]
    private void CmdSpawnClone(GameObject Clone)
    {
        NetworkServer.Spawn(Clone);
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
            controller.IsDead = false;
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
