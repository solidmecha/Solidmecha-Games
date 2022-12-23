using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiSpaceControl : NetworkBehaviour {

    public GameObject Ship;
    public GameObject MoveOutline;
    public GameObject ShotOutline;
    public GameObject Static;
    public List<MultiSpacePlayerScript> Players=new List<MultiSpacePlayerScript> { };
    public bool ShipMove;
    System.Random RNG;
    public GameObject SelectedShip;
    public bool[][] World = new bool[12][];
    public List<List<Transform>> ShipPositions = new List<List<Transform>> { };

    [SyncVar]
    public int PlayerTurn;

    private void Awake()
    {
        /*
        if (!isLocalPlayer || !isServer)
        {
            Destroy(gameObject);
        }*/
    }


    public void SpawnPlayerShips(MultiSpacePlayerScript M)
    {
        List<Transform> T = new List<Transform> { };
        ShipPositions.Add(T);

        for (int i = 0; i < 3; i++)
        {
            Vector2 v = FreeSpace();
            GameObject go = Instantiate(Ship, v, Quaternion.identity);
            World[(int)v.x + 6][(int)v.y + 6] = false;
            RpcUpdateWorld(v + new Vector2(6, 6), false);
            go.GetComponent<MultiSpaceShipControl>().Index = ShipPositions[M.LocalPlayerID].Count;
            ShipPositions[M.LocalPlayerID].Add(go.transform);
            NetworkServer.SpawnWithClientAuthority(go, M.gameObject);
            go.GetComponent<MultiSpaceShipControl>().RpcUpdateColor(M.color);
            go.GetComponent<MultiSpaceShipControl>().RpcUpdateName(M.PlayerName);
            go.GetComponent<MultiSpaceShipControl>().ControllerID = M.LocalPlayerID;
        }
    }

    public List<Vector2> PossibleMovement(int x, int y)
    {
        List<Vector2> MoveLocations = new List<Vector2> { };

        //right
        int currentX = x + 1;
        int currentY = y;

        while (currentX < 6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentX++;
        }
        currentX = x - 1;
        //left
        while (currentX >= -6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentX--;
        }
        currentX = x;
        //up
        currentY += 1;
        while (currentY < 6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY++;
        }
        //down
        currentY = y - 1;
        while (currentY >= -6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY--;
        }
        //UL
        currentY = y + 1;
        currentX = x - 1;
        while (currentY < 6 && currentX >= -6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY++;
            currentX--;
        }
        //UR
        currentX = x + 1;
        currentY = y + 1;
        while (currentY < 6 && currentX < 6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY++;
            currentX++;
        }
        //DL
        currentX = x - 1;
        currentY = y - 1;
        while (currentY >= -6 && currentX >= -6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY--;
            currentX--;
        }
        //DR
        currentX = x + 1;
        currentY = y - 1;
        while (currentY >= -6 && currentX < 6 && World[currentX+6][currentY+6])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY--;
            currentX++;
        }
        return MoveLocations;
    }

    public void ClearMovement()
    {
        var Tiles = GameObject.FindGameObjectsWithTag("M");
        foreach (GameObject g in Tiles)
            Destroy(g);
    }
    [ClientRpc]
    public void RpcClearClientMovement()
    {
        ClearMovement();
    }


    public void DisplayMovement(int x, int y)
    {
            var List = PossibleMovement(x, y);
            foreach (Vector2 v in List)
                Instantiate(MoveOutline, v, Quaternion.identity);
    }

    [ClientRpc]
    public void RpcUpdateWorld(Vector2 V, bool B)
    {
        World[(int)V.x][(int)V.y] = B;
    }

    [ClientRpc]
    public void RpcInitilizeWorld()
    {
        for (int i = 0; i < 12; i++)
        {
            World[i] = new bool[12];
            for (int j = 0; j < 12; j++)
            {
                World[i][j] = true;
            }
        }
    }
    [ClientRpc]
    public void RpcMessage(string message, int index)
    {
            Transform C = GameObject.FindGameObjectWithTag("MainCanvas").transform;
            C.GetChild(index).GetComponent<UnityEngine.UI.Text>().text = message;
    }

    Vector2 FreeSpace()
    {
        Vector2 v = Vector2.zero;
        do
        {
            v = new Vector2(RNG.Next(-6, 6), RNG.Next(-6, 6));
        } while (Physics2D.Raycast(v, Vector2.zero).collider != null);
        return v;
    }

    

    // Use this for initialization
    void Start () {
        RNG = new System.Random();
        if (GameObject.FindGameObjectsWithTag("MultiControl").Length == 2)
        {
            Destroy(GameObject.FindGameObjectWithTag("MultiControl"));
        }
        if (isServer)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("S"))
                Destroy(g);
            for (int i = 0; i < 12; i++)
            {
                World[i] = new bool[12];
                for (int j = 0; j < 12; j++)
                {
                    World[i][j] = true;
                }
            }
            RpcInitilizeWorld();

            foreach (PlayerControl p in GameControl.singleton.Players)
            {
                p.RpcUpdatePosition(new Vector2(-100, -100));
                MultiSpacePlayerScript MSP = p.GetComponent<MultiSpacePlayerScript>();
                MSP.MC = GameObject.FindGameObjectWithTag("MultiControl").GetComponent<MultiSpaceControl>();
                MSP.color = GameControl.singleton.Colors[p.ColorID];
                MSP.LocalPlayerID = p.PlayerIndex;
                MSP.PlayerName = p.PlayerName;
                MSP.PlayingMultiSpace = true;
                MSP.RpcStartGame();
                Players.Add(MSP);
                SpawnPlayerShips(MSP);
            }
        }

        ShipMove = true;
        //Destroy(GameControl.singleton.gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
