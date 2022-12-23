using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MazePlayerScript : NetworkBehaviour {
    [SyncVar]
    int PlayerIndex;
    public GameObject Wall;
    public bool[][] LocalGrid=new bool[13][];
    GameObject[][] Walls = new GameObject[13][];
    int KeyID = -1;
    public Sprite MonSprite;
    public Sprite[] Arrows;
    float counter;
    public GameObject SwitchTile;
    public Vector2[] Toggles=new Vector2[6];
    public bool isMonster;
    public int MonsterLocation;
    [SyncVar]
    public bool isPlaying;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMonster)
        {
            if (collision.CompareTag("Key") && KeyID == -1)
            {
                KeyID = collision.GetComponent<KeyScript>().ID;
                CmdClearObject(transform.position);
            }
            else if (collision.CompareTag("door") && KeyID == collision.GetComponent<KeyScript>().ID)
            { 
                CmdClearObject(transform.position);
                CmdWin();
                transform.position = new Vector3(-10, -10, -10);
            }
            else if (collision.CompareTag("Switch"))
            {
                switch (collision.GetComponent<SwitchTileScript>().ID)
                {
                    case 0:
                        CmdUpdateMap(Toggles);
                        break;
                    case 1:
                        ChangeToggles(Vector2.up);
                        break;
                    case 2:
                        ChangeToggles(Vector2.down);
                        break;
                    case 3:
                        ChangeToggles(Vector2.left);
                        break;
                    case 4:
                        ChangeToggles(Vector2.right);
                        break;
                }
            }
        }
        else if (collision.CompareTag("Player"))
        {
            MazePlayerScript mps = collision.GetComponent<MazePlayerScript>();
            if(mps.isPlaying && mps.PlayerIndex==MonsterLocation)
                collision.GetComponent<MazePlayerScript>().CmdEliminatePlayer();
        }
    }
    void ChangeToggles(Vector2 v)
    {
        for (int i = 0; i < 6; i++)
        {
            Toggles[i] += v;
            if(Toggles[i].x>11.1f)
            {
                Toggles[i].x = 1;
            }
            if (Toggles[i].x < 0.1f)
            {
                Toggles[i].x = 11;
            }
            if (Toggles[i].y > 11.1f)
            {
                Toggles[i].y = 1;
            }
            if (Toggles[i].y < 0.1f)
            {
                Toggles[i].y = 11;
            }
        }
    }

    [Command]
    public void CmdEliminatePlayer()
    {
        MazeServerControl.singleton.UpdateGlobalMessage("Player "+PlayerIndex + " was " + RandomDescript());
        isPlaying = false;
        transform.position = new Vector3(-101, -101, -101);
    }

    string RandomDescript()
    {
        System.Random rng = new System.Random();
        switch(rng.Next(5))
        {
            case 0: return "not too bad.";
            case 1: return "delicious!";
            case 2: return "undercooked?!";
            case 3: return "om nom nom'ed";
            default:
                return "tasty.";
        }
    }

    [ClientRpc]
    public void RpcMessage(string message, int index)
    {
        if (isLocalPlayer)
        {
            Transform C = GameObject.FindGameObjectWithTag("MainCanvas").transform;
            C.GetChild(index).GetComponent<UnityEngine.UI.Text>().text = message;
        }
    }

    [ClientRpc]
    public void RpcUpdateSprite()
    {
        if (isLocalPlayer)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Switch"))
                Destroy(g);
        }
       GetComponent<SpriteRenderer>().sprite = MonSprite;
       isMonster = true;
    }


    [Command]
    void CmdUpdateMap(Vector2[] Tog)
    {
        MazeServerControl.singleton.UpdateMaps(MonsterLocation, Tog);
    }

    [ClientRpc]
    public void RpcUpdateMap(Vector2 v)
    {
        if (isLocalPlayer)
        {
            int x = Mathf.RoundToInt(v.x);
            int y = Mathf.RoundToInt(v.y);
            if (!LocalGrid[x][y])
            {
                Destroy(Walls[x][y]);
            }
            else
            {
                Walls[x][y]=Instantiate(Wall, v-new Vector2(6,6), Quaternion.identity);
                Walls[x][y].GetComponent<StaticScript>().ColorID = 9;
            }
            LocalGrid[x][y] = !LocalGrid[x][y];
        }
    }

    // Use this for initialization
    void Start () {
        if (isLocalPlayer)
        {
            bool[] map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MazeControl>().OneD();
            CmdAddPlayer(map);
            ShowLocalMaze(map);
            BuildSwitches();
            PlacePlayer();  
        }
        //else
            //GetComponent<SpriteRenderer>().enabled = false;
	}

    [Command]
    public void CmdAddPlayer(bool[] M)
    {
        PlayerIndex = MazeServerControl.singleton.PlayerCount;
        MonsterLocation = PlayerIndex;
        RpcMessage("You are Player " + PlayerIndex, 26);
        MazeServerControl.singleton.SetMaps();
        MazeServerControl.singleton.PlayerCount++;
        MazeServerControl.singleton.Players.Add(this);
        MazeServerControl.singleton.Maps.Add(M);
    }

    [Command]
    void CmdAddSteps()
    {
        if(MazeServerControl.singleton.PlayerSteps <= 12)
        {
            MazeServerControl.singleton.PlayerSteps+= MazeServerControl.singleton.PlayerCount;
            MazeServerControl.singleton.UpdateStepDisplay();
        }
    }

    [Command]
    void CmdTakeSteps()
    {
       MazeServerControl.singleton.PlayerSteps--;
       MazeServerControl.singleton.UpdateStepDisplay();
    }

    [Command]
    public void CmdWin()
    {
        isPlaying = false;
        transform.position = new Vector3(-100, -100, -100);
        MazeServerControl.singleton.UpdateGlobalMessage("Player " + PlayerIndex + " lived happily ever after!");
    }

    [Command]
    public void CmdClearObject(Vector2 v)
    {
        if(!Physics2D.Raycast(v, Vector2.zero).collider.CompareTag("Player"))
            Destroy(Physics2D.Raycast(v, Vector2.zero).collider.gameObject);
        else
        {
            GameObject g = Physics2D.Raycast(v, Vector2.zero).collider.gameObject;
            g.transform.position = new Vector2(-100, -100);
            Destroy(Physics2D.Raycast(v, Vector2.zero).collider.gameObject);
            g.transform.position = v;
        }
    }

    [Command]
    public void CmdJump()
    {
        MazeServerControl.singleton.MonsterJump(MonsterLocation, PlayerIndex);
    }

    [ClientRpc]
    public void RpcClearMaze()
    {
        if (isLocalPlayer)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("S"))
                Destroy(g);
        }
    }

    [ClientRpc]
    public void RpcShowMaze(bool[] Grid)
    {
        if (isLocalPlayer)
        {
            ShowLocalMaze(Grid);
        }
    }

    void PlacePlayer()
    {
        int x = MazeServerControl.singleton.RNG.Next(1, 12);
        int y = MazeServerControl.singleton.RNG.Next(1, 12);
        while(!LocalGrid[x][y] || Physics2D.Raycast(new Vector2(x-6,y-6), Vector2.zero).collider != null)
        {
            x = MazeServerControl.singleton.RNG.Next(1, 12);
            y = MazeServerControl.singleton.RNG.Next(1, 12);
        }
        transform.position = new Vector2(x, y) - new Vector2(6, 6);
    }

    void BuildSwitches()
    {
        System.Random rng = new System.Random();
        for (int i=0;i<5;i++)
        {
            int x = rng.Next(1, 12);
            int y = rng.Next(1, 12);

            while (!LocalGrid[x][y] || Physics2D.Raycast(new Vector2(x - 6, y - 6), Vector2.zero).collider != null)
            {
                x = rng.Next(1, 12);
                y = rng.Next(1, 12);
            }
            GameObject go=Instantiate(SwitchTile, new Vector2(x - 6, y - 6), Quaternion.identity);
            go.GetComponent<SwitchTileScript>().ID = i;
            if (i > 0)
                go.GetComponent<SpriteRenderer>().sprite = Arrows[i - 1];

        }
        for (int i = 0; i < 5; i++)
        {
            int x = rng.Next(1, 12);
            int y = rng.Next(1, 12);

            while (!LocalGrid[x][y] || Physics2D.Raycast(new Vector2(x - 6, y - 6), Vector2.zero).collider != null)
            {
                x = rng.Next(1, 12);
                y = rng.Next(1, 12);
            }
            GameObject go = Instantiate(SwitchTile, new Vector2(x - 6, y - 6), Quaternion.identity);
            go.GetComponent<SwitchTileScript>().ID = i;
            if (i > 0)
                go.GetComponent<SpriteRenderer>().sprite = Arrows[i - 1];

        }

        int u = rng.Next(1, 12);
        int v = rng.Next(1, 12);
        Vector2 Change = Vector2.zero;
        if (MazeServerControl.singleton.RNG.Next(2) == 0)
            Change = Vector2.right;
        else
            Change = Vector2.up;
        for (int i = 0; i < 6; i++)
        {
            Vector2 TogV = new Vector2(u, v) + Change * i;
            Toggles[i] = TogV;
            if (Toggles[i].x > 11.1f)
            {
                Toggles[i].x = 1;
            }
            if (Toggles[i].x < 0.1f)
            {
                Toggles[i].x = 11;
            }
            if (Toggles[i].y > 11.1f)
            {
                Toggles[i].y = 1;
            }
            if (Toggles[i].y < 0.1f)
            {
                Toggles[i].y = 11;
            }
        }
    }

    public void ShowLocalMaze(bool[] Grid)
    {
        for (int i = 0; i < Grid.Length; i++)
        {
            int x = i % 13;
            int y = i / 13;
            if (i < 13)
            {
                LocalGrid[i] = new bool[13];
                Walls[i] = new GameObject[13];
            }
            LocalGrid[x][y] = Grid[i];
            {

                if (!Grid[i])
                {
                    Walls[x][y] = Instantiate(Wall, new Vector2(x, y) + new Vector2(-6, -6), Quaternion.identity) as GameObject;
                    Walls[x][y].GetComponent<StaticScript>().ColorID = 9;
                }

            }

        }
    }


    bool CheckMove(Vector2 v)
    {
        int x = Mathf.RoundToInt(transform.position.x+v.x)+6;
        int y= Mathf.RoundToInt(transform.position.y+v.y) + 6;
        return LocalGrid[x][y];
    }
	
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer && isPlaying)
        {
            counter += Time.deltaTime;
            if((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && CheckMove(Vector2.right) && MazeServerControl.singleton.PlayerSteps>0)
            {
                transform.position =(Vector2)transform.position + Vector2.right;
                if(!isMonster)  
                    CmdTakeSteps();
            }
            else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && CheckMove(Vector2.left) && MazeServerControl.singleton.PlayerSteps > 0)
            {
                transform.position = (Vector2)transform.position + Vector2.left;
                if(!isMonster)
                    CmdTakeSteps();
            }
            else if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckMove(Vector2.up) && MazeServerControl.singleton.PlayerSteps > 0)
            {
                transform.position = (Vector2)transform.position + Vector2.up;
                if(!isMonster)
                    CmdTakeSteps();
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && CheckMove(Vector2.down) && MazeServerControl.singleton.PlayerSteps > 0)
            {
                transform.position = (Vector2)transform.position + Vector2.down;
                if(!isMonster)
                    CmdTakeSteps();
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                CmdJump();
                CmdTakeSteps();
                CmdTakeSteps();
            }
            /*
            if (counter >= 1.5f)
            {
                
                counter = 0;
                if (PlayerIndex==0)
                    CmdAddSteps(); 
            }*/
        }
    }
}
