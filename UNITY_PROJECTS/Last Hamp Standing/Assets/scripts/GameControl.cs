using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameControl : NetworkBehaviour
{
    bool MapSet;
    public static GameControl singleton;
    public Color[] Colors;
    public GameObject[] PickUps;
    public int MovementBuff;
    public int CritBuff;
    public int HitBuff;
    public int DmgBuff;
    public int HPBuff;
    public GameObject Bullet;
    public int ReadyCount;
    public int PlayerCount;
    public List<PlayerControl> Players;
    System.Random RNG;
    public enum GameState { Moving, Targeting, ShowFire};

    public GameState CurrentState;
    string TurnMessage;
    public GameObject MultiSpaceController;
    public GameObject MazePlayer;
    public GameObject Disc;
    public GameObject Cube;

    private void Awake()
    {
        singleton = this;
        CurrentState = GameState.Moving;
        RNG = new System.Random();
    }

    public void SyncPlayerColors()
    {
        foreach (PlayerControl p in Players)
        {
            p.RpcSetColor(p.ColorID);
        }
    }


    public bool PlayerColorAvailble(int ID)
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerControl p = g.GetComponent<PlayerControl>();
            if (p.ColorID == ID)
                return false;
        }
        return true;
    }


    public void UpdatePlayerColors()
    {
        foreach (PlayerControl p in Players)
        {
            if(p.ColorID>=0)
                p.GetComponent<SpriteRenderer>().color = Colors[p.ColorID];
            p.RpcSetColor(p.ColorID);
        }
    }

    public void UpdatePlayerNames()
    {
        foreach(PlayerControl p in Players)
        {
            p.RpcSetName(p.PlayerName);
        }
    }


    // Use this for initialization
    void Start () {
		
	}

    bool EqualVec2(Vector2 a, Vector2 b)
    {
        return Mathf.RoundToInt(a.x) == Mathf.RoundToInt(b.x) && Mathf.RoundToInt(a.y) == Mathf.RoundToInt(b.y);
    }

    void HandlePlayerMovement()
    {
        List<Vector2> Moves=new List<Vector2> { };
        for(int i=0; i<PlayerCount;i++)
        {
            for(int j=0; j<Moves.Count;j++)
            {
                if(EqualVec2(Players[i].TargetPosition, Moves[j]))
                {
                    Players[j].TargetPosition = Players[j].transform.position;
                    Players[i].TargetPosition = Players[i].transform.position;
                }
            }
            Moves.Add(Players[i].TargetPosition);
        }
            foreach (PlayerControl p in Players)
        {
            RaycastHit2D hit = Physics2D.Raycast(p.TargetPosition, Vector2.zero);
            if (hit.collider!=null && hit.collider.CompareTag("P"))
            {
                switch(hit.collider.GetComponent<PickUpScript>().ID)
                {
                    case 0:
                        p.Movement+=MovementBuff;
                        break;
                    case 1:
                        p.hitChance += HitBuff;
                        break;
                    case 2:
                        p.CritChance += CritBuff;
                        break;
                    case 3:
                        p.Dmg+=DmgBuff;
                        break;
                    case 4:
                        p.HP += HPBuff;
                        break;
                }
                Destroy(hit.collider.gameObject);
            }
            p.RpcUpdatePosition(p.TargetPosition);
            p.isReady = false;
            p.isFiring = true;
            p.RpcRevealRole("Targeting Phase", 24);
        }
        CurrentState = GameState.Targeting;
        //Invoke("UpdateStats", .05f);
    }

    void UpdateTargets()
    {
        foreach (PlayerControl p in Players)
        {
            GameObject go = Instantiate(Bullet, p.transform.position, Quaternion.identity);
            go.GetComponent<BulletScript>().Target=p.TargetPosition;
            Destroy(go, 1.5f);
            TurnMessage += p.PlayerName;
            if(DetermineHit(p.TargetPosition, p.hitChance))
            {
                if (Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.CompareTag("Player"))
                {
                    if (RNG.Next(100) <= p.CritChance)
                    {
                        Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PlayerControl>().HP -= p.Dmg * 2;
                        go.GetComponent<BulletScript>().TargetColor = Color.red;
                        TurnMessage += " CRIT!!! " + Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PlayerControl>().PlayerName;
                    }
                    else
                    {
                        Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PlayerControl>().HP -= p.Dmg;
                        go.GetComponent<BulletScript>().TargetColor = Color.green;
                        TurnMessage += " hit " + Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PlayerControl>().PlayerName;
                    }
                    //Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PlayerControl>().Invoke("RpcUpdateStats",.01f);
                }
                else
                {
                    PickUpScript pus = Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PickUpScript>();
                    if (pus != null)
                    {
                        TurnMessage += " destroyed " + " ";
                        switch (pus.ID)
                        {
                            case 0:
                                TurnMessage += " move buff ";
                                break;
                            case 1:
                                TurnMessage += " hit% buff ";
                                break;
                            case 2:
                                TurnMessage += " crit% buff ";
                                break;
                            case 3:
                                TurnMessage += " dmg buff ";
                                break;
                            case 4:
                                TurnMessage += " HP buff ";
                                break;
                        }
                        Destroy(pus.gameObject, 1.5f);
                    }
                    else
                    { TurnMessage += " shot nothing"; }
                    TurnMessage+= " at " + GetFile(Mathf.RoundToInt(p.TargetPosition.x))+(p.TargetPosition.y+7).ToString();
                    go.GetComponent<BulletScript>().TargetColor = Color.red;
                }

            }
            else
            {
                TurnMessage += " missed ";
                if (Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider != null &&  Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.CompareTag("Player"))
                { TurnMessage += Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PlayerControl>().PlayerName; }
                    else if(Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider != null)
                {
                    switch(Physics2D.Raycast(p.TargetPosition, Vector2.zero).collider.GetComponent<PickUpScript>().ID)
                    {
                        case 0:
                            TurnMessage += " move buff ";
                            break;
                        case 1:
                            TurnMessage += " hit% buff ";
                            break;
                        case 2:
                            TurnMessage += " crit% buff ";
                            break;
                        case 3:
                            TurnMessage += " dmg buff ";
                            break;
                        case 4:
                            TurnMessage += " HP buff ";
                            break;
                    }

                }
                else { TurnMessage += " nothing "; }

                TurnMessage += " at " + GetFile(Mathf.RoundToInt(p.TargetPosition.x)) + (p.TargetPosition.y + 7).ToString();

            }
            TurnMessage += '\n';
            NetworkServer.Spawn(go);
        }

        foreach (PlayerControl p in Players)
        {
            p.RpcRevealRole("Resolving Hits", 24);
            p.RpcRevealRole(TurnMessage, 25);
        }
        CurrentState = GameState.ShowFire;
        Invoke("SwitchToMoveState", 1.5f);
        TurnMessage = "";

        //Invoke("UpdateStats", .06f);
    }

    string GetFile(int x)
    {
        switch(x)
        {
            case -6:
                return "A";
            case -5:
                return "B";
            case -4:
                return "C";
            case -3:
                return "D";
            case -2:
                return "E";
            case -1:
                return "F";
            case 0:
                return "G";
            case 1:
                return "H";
            case 2:
                return "I";
            case 3:
                return "J";
            case 4:
                return "K";
            case 5:
                return "L";
            default:
                return "?";
        }
    }

    void SwitchToMoveState()
    {
        List<PlayerControl> elims = new List<PlayerControl> { };
        foreach (PlayerControl p in Players)
        {
            if (p.HP <= 0)
                elims.Add(p);
            p.isReady = false;
            p.isFiring = false;
            p.RpcRevealRole("Moving Phase", 24);
        }
        foreach (PlayerControl p in elims)
            EliminatePlayer(p);
        CurrentState = GameState.Moving;
    }

    void EliminatePlayer(PlayerControl p)
    {
        p.RpcUpdatePosition(new Vector2(-100, -100));
        Players.Remove(p);
        PlayerCount--;
    }

    bool DetermineHit(Vector2 V, int chance)
    {
        RaycastHit2D hit = Physics2D.Raycast(V, Vector2.zero);
        if(hit.collider !=null)
        {
            if (hit.collider.CompareTag("Player"))
                return RNG.Next(100) <= chance;
            else
                return RNG.Next(100) <= chance;
        }
        return false;
    }

    void DoNextTurn()
    {
        ReadyCount = 0;
        switch (CurrentState)
        {
            case GameState.Moving:
                HandlePlayerMovement();
                break;
            case GameState.Targeting:
                UpdateTargets();
                break;
            case GameState.ShowFire:
                break;

        }
    }

    /*
    void UpdateStats()
    {
        foreach (PlayerControl p in Players)
            p.RpcUpdateStats();
    }*/

    public void PositionPlayer(int I)
    {
        Vector2 v = Vector2.zero;
        do {
            v = new Vector2(RNG.Next(-6, 6), RNG.Next(-6, 6));
        } while (Physics2D.Raycast(v, Vector2.zero).collider != null);
        Players[I].transform.position = v;
        Players[I].RpcUpdatePosition(v);
    }

    [Server]
    public void ResetGame()
    {
        if (GameObject.FindGameObjectsWithTag("MultiControl") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("MultiControl"));
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("S"))
                Destroy(g);
        }
        MapSet = false;
        Players.Clear();
        PlayerCount = 0;
        CurrentState = GameState.Moving;
        ReadyCount = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            g.GetComponent<MultiSpacePlayerScript>().PlayingMultiSpace = false;
            Players.Add(g.GetComponent<PlayerControl>());
            PlayerCount++;
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("P"))
            Destroy(g);
        for (int i = 0; i < PlayerCount; i++)
        {
            Players[i].RpcRevealRole("",24);
            Players[i].RpcRevealRole("", 25);
            Players[i].HP = 5;
            Players[i].Movement = 2;
            Players[i].hitChance = 50;
            Players[i].CritChance = 10;
            Players[i].Dmg = 1;
            Players[i].isReady = false;
            Players[i].isFiring = false;
            PositionPlayer(i);
        }

    }

    string SpyFallLocation(int Index)
    {
        switch (Index)
        {
            case 0:
                return "Beach";
            case 1:
                return "Broadway Theater";
            case 2:
                return "Circus Tent";
            case 3:
                return "Casino";
            case 4:
                return "Bank";
            case 5:
                return "Day Spa";
            case 6:
                return "Hotel";
            case 7:
                return "Restaurant";
            case 8:
                return "Supermarket";
            case 9:
                return "Service Station";
            case 10:
                return "Hospital";
            case 11:
                return "Embassy";
            case 12:
                return "Police Station";
            case 13:
                return "School";
            case 14:
                return "University";
            case 15:
                return "Airplane";
            case 16:
                return "Ocean Liner";
            case 17:
                return "Passenger Train";
            case 18:
                return "Submarine";
            case 19:
                return "Cathedral";
            case 20:
                return "Corporate Party";
            case 21:
                return "Movie Studio";
            case 22:
                return "Crusader Army";
            case 23:
                return "Pirate Ship";
            case 24:
                return "Polar Station";
            case 25:
                return "Space Station";
            case 26:
                return "Military Base";
            default:
                return "California";

        }
    }

    void SetUpMultispace()
    {
       GameObject go = Instantiate(MultiSpaceController);
       NetworkServer.Spawn(go);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("P"))
            Destroy(g);
    }


    void SetUpSpyFall()
    {
        int SpyIndex = RNG.Next(PlayerCount);
        string s = SpyFallLocation(RNG.Next(26));
        for(int i=0;i<PlayerCount;i++)
        {
            if (i != SpyIndex)
                Players[i].RpcRevealRole(s, 24);
            else
            {
                string Spy = "You are the spy!";
                string locs= @"Possible Locations: Beach, Circus Tent, 
Broadway Theater, Casino, Bank,
Day Spa, Hotel, Restaurant, Supermarket, Service Station, Hospital, Embassy, Police Station, School, University, Airplane, Cathedral, Ocean Liner, Passenger Train, Submarine, Corporate Party, Movie Studio, Crusader Army, Pirate Ship, 
Polar Station,
Space Station, Military Base ";
                Players[i].RpcRevealRole(Spy, 24);
                Players[i].RpcRevealRole(locs, 25);
            }
        }
    }

    void SetUpMaze()
    {
        NetworkManager.singleton.playerPrefab = MazePlayer;
       NetworkManager.singleton.ServerChangeScene("Maze");
    }

    void LoadLevel(GameObject Player, string Level)
    {
        NetworkManager.singleton.playerPrefab = Player;
        NetworkManager.singleton.ServerChangeScene(Level);
    }

    void TwoAndOneSetup()
    {
        int RoomMax = 0;
        int RoomCount=0;
        int BlueCount=0;
        int BlueMax=0;
        int GamblerIndex = -1;
        int PrezIndex = -1;
        int BomberIndex = -1;
        if (PlayerCount % 2 == 0)
        {
            RoomMax = PlayerCount / 2;
            BlueMax = PlayerCount / 2;
            BomberIndex = RNG.Next(PlayerCount / 2)+1;
        }
        else
        {
            GamblerIndex = RNG.Next(PlayerCount);
            if (RNG.Next(2) == 1)
                RoomMax = (PlayerCount + 1) / 2;
            else
                RoomMax = (PlayerCount - 1) / 2;
            BlueMax = (PlayerCount - 1) / 2;
            BomberIndex = RNG.Next(PlayerCount-BlueMax)+1;
        }
        PrezIndex = RNG.Next(BlueMax)+1;
        List<PlayerControl> BlueTeam = new List<PlayerControl> { };
        List<PlayerControl> RedTeam = new List<PlayerControl> { };
        
        for (int i=0;i<PlayerCount;i++)
        {
            string message = "";
            if (i == GamblerIndex)
            {
                message += "Grey Team ";
                switch(RNG.Next(5))
                {
                    case 0:
                        message = "Green Team Zombie";
                        break;
                    case 1:
                        message = "Grey Team Detective";
                        break;
                    default:
                        message = "Grey Team Gambler";
                        break;
                }
            }
            else
            {
                if (BlueCount < BlueMax && i - BlueCount < PlayerCount - BlueMax)
                {
                    if (RNG.Next(2) == 1)
                    {
                        message += "Blue Team";
                        BlueTeam.Add(Players[i]);
                        if (BlueTeam.Count == PrezIndex)
                            message += " President";
                        BlueCount++;
                    }
                    else
                    {
                        message += "Red Team";
                        RedTeam.Add(Players[i]);
                        if (RedTeam.Count == BomberIndex)
                            message += " Bomber";
                    }
                }
                else if (BlueCount == BlueMax)
                {
                    RedTeam.Add(Players[i]);
                    message += "Red Team";
                    if (RedTeam.Count == BomberIndex)
                        message += " Bomber";

                }
                else
                {
                    message += "Blue Team";
                    BlueTeam.Add(Players[i]);
                    if (BlueTeam.Count == PrezIndex)
                        message += " President";
                    BlueCount++;
                }
            }

            if (RoomCount < RoomMax && i - RoomCount < PlayerCount - RoomMax)
            {
                if ((RNG.Next(2) == 1))
                {
                    message += " Room 1";
                    RoomCount++;
                }
                else
                    message += " Room 2";

            }
            else if (RoomCount == RoomMax)
            {
                message += " Room 2";
            }
            else
            {
                message += " Room 1";
                RoomCount++;
            }
            Players[i].RpcRevealRole(message, 24);
        }

    }

    // Update is called once per frame
    void Update () {
        if (!isServer)
        {
            return;
        }
        else
        {
            if (ReadyCount>0 && ReadyCount == PlayerCount)
            {
                DoNextTurn();
            }

            if(!MapSet)
            {
                MapSet = true;
                int r = RNG.Next(84, 121);
                for (int i=0;i<r;i++)
                {
                    Vector2 v = new Vector2(RNG.Next(-6, 6), RNG.Next(-6, 6));
                    if (Physics2D.Raycast(v, Vector2.zero).collider == null)
                    {
                        GameObject go = Instantiate(PickUps[RNG.Next(5)], v, Quaternion.identity);
                        NetworkServer.Spawn(go);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.F1))
            {
                ResetGame();
            }
            else if(Input.GetKeyDown(KeyCode.F2))
            {
                SetUpSpyFall();
            }
            else if(Input.GetKeyDown(KeyCode.F3))
            {
                TwoAndOneSetup();
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                SetUpMultispace();
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                SetUpMaze();
            }
            else if(Input.GetKeyDown(KeyCode.F6))
            {
                NameHolder N = GameObject.FindGameObjectWithTag("NameHolder").GetComponent<NameHolder>();
                DontDestroyOnLoad(N.gameObject);
                N.Names.Clear();
                N.Colors.Clear();
                foreach (PlayerControl p in Players)
                {
                    N.Names.Add(p.PlayerName);
                    N.Colors.Add(Colors[p.ColorID]);
                }
                LoadLevel(Disc, "disc");
            }
            else if(Input.GetKeyDown(KeyCode.F7))
            {
                LoadLevel(Cube, "ilcet");
            }
        }
	}
}
