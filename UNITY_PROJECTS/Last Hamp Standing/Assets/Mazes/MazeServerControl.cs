using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MazeServerControl : NetworkBehaviour {

    public static MazeServerControl singleton;
    public int PlayerSteps;
    public List<bool[]> Maps=new List<bool[]> { };
    public int PlayerCount;
    public List<MazePlayerScript> Players=new List<MazePlayerScript> { };
    public GameObject[] Objects;
    public Color[] Colors;
    public System.Random RNG;
    public int MonsterIndex;
    string GlobalMsg;
    public GameObject PlayerObject;

    public void SetMaps()
    {
        if(isServer)
        {
            int x = RNG.Next(1, 12)-6;
            int y = RNG.Next(1, 12)-6;
            while(Physics2D.Raycast(new Vector2(x,y), Vector2.zero).collider != null)
            {
                 x = RNG.Next(1, 12)-6;
                 y = RNG.Next(1, 12)-6;
            }
            int C=RNG.Next(1,Colors.Length);
            GameObject go = Instantiate(Objects[0], new Vector2(x, y), Quaternion.identity);
            go.GetComponent<KeyScript>().ID = C;
            NetworkServer.Spawn(go);
             
             x = RNG.Next(1, 12) - 6;
             y = RNG.Next(1, 12) - 6;
            while (Physics2D.Raycast(new Vector2(x, y), Vector2.zero).collider != null)
            {
                x = RNG.Next(1, 12) - 6;
                y = RNG.Next(1, 12) - 6;
            }

            go = Instantiate(Objects[1], new Vector2(x, y), Quaternion.identity);
            go.GetComponent<KeyScript>().ID = C;
            NetworkServer.Spawn(go);
        }
    }

    public bool[] GetMap(int index)
    {
        return Maps[index];
    }


    private void Awake()
    {
        RNG = new System.Random();
        singleton = this;
    }

    public void UpdateStepDisplay()
    {
        foreach (MazePlayerScript m in Players)
            m.RpcMessage("Player steps: " + PlayerSteps, 24);
        if (isServer)
            RpcSetSteps(PlayerSteps);
    }

    [ClientRpc]
    void RpcSetSteps(int S)
    {
        PlayerSteps = S;
    }

    public void UpdateGlobalMessage(string s)
    {
        GlobalMsg =GlobalMsg + s + '\n';
        foreach (MazePlayerScript m in Players)
            m.RpcMessage(GlobalMsg, 25);
    }

    // Use this for initialization
    void Start () {

	}

    public void MonsterJump(int ML, int Pindex)
    {
        int L = ML+1;
        if (L == PlayerCount)
        {
            L = 0;
        }
            Players[Pindex].MonsterLocation = L;
            Players[Pindex].RpcClearMaze();
            Players[Pindex].RpcShowMaze(Maps[L]);
    }

    public void UpdateMaps(int MapIndex, Vector2[] Toggles)
    {
        for(int i=0;i<PlayerCount;i++)
        {
            if (Players[i].MonsterLocation != MapIndex)
            {
                foreach (Vector2 v in Toggles)
                {
                    Players[i].RpcUpdateMap(v);
                    int x = Mathf.RoundToInt(v.x);
                    int y = Mathf.RoundToInt(v.y);
                    Maps[Players[i].MonsterLocation][y * 13 + x]=!Maps[Players[i].MonsterLocation][y * 13 + x];
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {

        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                //MonsterIndex = RNG.Next(PlayerCount);
                //Players[MonsterIndex].isMonster = true;
                //Players[MonsterIndex].RpcUpdateSprite();
                PlayerSteps = 144*PlayerCount;
                foreach (MazePlayerScript p in Players)
                    p.isPlaying = true;
                //MonsterJump(-1);
            }
            if (Input.GetKeyDown(KeyCode.F5))
                NetworkManager.singleton.ServerChangeScene("Maze");
            if (Input.GetKeyDown(KeyCode.F1))
            {
                NetworkManager.singleton.playerPrefab = PlayerObject;
                NetworkManager.singleton.ServerChangeScene("Main");
            }
        }
    }
}
