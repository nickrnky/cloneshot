using Assets.Scripts.Recording;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum Teams { Red, Blue, None};

public class RoundManager : NetworkBehaviour {

    [SerializeField]
    public GameObject ClonePrefab;

    private Dictionary<string, Player> players = new Dictionary<string, Player>();

    // Management variables
    private int StageInProgress = 0;
    private int CurrentRound = 0;
    private static List<GameObject> BlueTeamClones = new List<GameObject>();
    private static List<GameObject> RedTeamClones = new List<GameObject>();

    [SerializeField]
    GameManager DaManager;

    [SerializeField]
    GameObject SpawnerBlue;

    [SerializeField]
    GameObject SpawnerRed;

    // Configurable variables
    public int NumberOfRounds = 5;
    public float TimeBetweenRounds = 5.0f;
    Vector3 BluePlayerStart = new Vector3();
    Vector3 RedPlayerStart = new Vector3();

    // Use this for initialization
    void Start ()
    {

        if (!isServer)
        {
            return;
        }
        StageInProgress = 0;
        BluePlayerStart = SpawnerBlue.transform.position;
        RedPlayerStart = SpawnerRed.transform.position;

    }
	
	// Update is called once per frame
	void Update ()
    {
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
            StageInProgress = 0;
            return;
        }

        if (CurrentRound < NumberOfRounds && StageInProgress == 0)
        {
            StageInProgress = 1;
            Debug.Log("Starting Round " + CurrentRound);
            StartCoroutine(StartRound());
            return;
        }

        // Loop and check for game end
        if(StageInProgress == 2)
        {
            if(!TeamAlive(Teams.Blue) || !TeamAlive(Teams.Red))
            {
                StageInProgress = 3;
                Debug.Log("No longer fighting");
            }
        }
        
        // Post processing
        if(StageInProgress == 3)
        {
            Debug.Log("Round over " + CurrentRound);
            StageInProgress = 4;
            StartCoroutine(PostProcessing());
        }
        

    }

    private void UpdateKillCount()
    {

    }

    // Start a round
    IEnumerator StartRound()
    {
        // Position players
        //Debug.Log("round 1");

        Debug.Log("Ready?");
        yield return new WaitForSeconds(TimeBetweenRounds);
        Debug.Log("Go!!!");

        int count = 0;
        foreach (Player person in players.Values)
        {
            if (count % 2 == 0)
            {
                //person.transform.position = PlayerOneStart;
                person.RpcRespawn(BluePlayerStart);
            }
            else
            {
                //person.transform.position = PlayerTwoStart;
                person.RpcRespawn(RedPlayerStart);
            }
            count++;
        }

        //Debug.Log("round 2");

        // Spawn clones
        foreach (GameObject x in BlueTeamClones)
        {
            ResetClone(x);
            x.SetActive(true);
        }
        //Debug.Log("round 3");

        foreach (GameObject x in RedTeamClones)
        {
            ResetClone(x);
            x.SetActive(true);
        }
        //Debug.Log("round 4");

        foreach(Player x in players.Values)
        {
            x.ResetActions();
        }

        // Start game
        foreach (GameObject x in BlueTeamClones)
        {
            StartClone(x);
        }

        foreach (GameObject x in RedTeamClones)
        {
            StartClone(x);
        }

        //Debug.Log("round 5");
        StageInProgress = 2;
        yield return null;
    }

    // Post round
    IEnumerator PostProcessing()
    {
        // Clean up and get recording
        int count = 0;
        foreach (Player player in players.Values)
        {
            PlayersActionsInRound action = new PlayersActionsInRound();
            action = player.GetPlayerActions();

            GameObject clone = new GameObject();
            if (player.Team == Teams.Blue)
            {
                clone = CreateClone(BluePlayerStart, action);
                BlueTeamClones.Add(clone);
            }
            else
            {
                clone = CreateClone(RedPlayerStart, action);
                RedTeamClones.Add(clone);
            }

            CmdSpawnClone(clone);
        }

        // Set players to alive
        foreach(Player player in players.Values)
        {
            player.IsDead = false;
            //y.PlayerMovement.AllowMovement = true;
        }

        //Debug.Log("round 6");

        // End
        CurrentRound++;
        StageInProgress = 0;
        yield return null;
    }

    // End game
    IEnumerator EndGame()
    {

        // Show ending screen

        yield return null;
    }

    private bool TeamAlive(Teams Team)
    {
        List<GameObject> clones = null;
        Player player = null;
        
        foreach(Player tempPlayer in players.Values)
        {
            if(tempPlayer.Team == Team)
            {
                player = tempPlayer;
                break;
            }
        }

        if(Team == Teams.Blue)
        {
            clones = BlueTeamClones;
        }
        else if(Team == Teams.Red)
        {
            clones = RedTeamClones;
        }

        if(player == null)
        {
            return false;
        }
        if(clones == null)
        {
            return false;
        }
        
        // Check player for life
        if(player.IsAlive())
        {
            return true;
        }

        // Check clones for life
        foreach (GameObject x in clones)
        {
            if (x.GetComponent<CloneController>().IsAlive())
            {
                return true;
            }
        }

        return false;
    }

    public static void CloneHit(NetworkInstanceId CloneID, int damage)
    {
        foreach(GameObject clone in BlueTeamClones)
        {
            if(clone.GetComponent<NetworkIdentity>().netId == CloneID)
            {
                clone.GetComponent<CloneController>().CloneTakeDamage(damage);
                return;
            }
        }

        foreach (GameObject clone in RedTeamClones)
        {
            if (clone.GetComponent<NetworkIdentity>().netId == CloneID)
            {
                clone.GetComponent<CloneController>().CloneTakeDamage(damage);
                return;
            }
        }

        return;
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
