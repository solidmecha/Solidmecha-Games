using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject MoveOutline;
    public GameObject ShotOutline;
    public GameObject Static;
    bool[][] World=new bool[12][];
    public Transform SelectedShip;
    bool shipMove;
    public bool PlayerTurn;
    public GameObject VictorySign;
    public GameObject DefeatSign;

    public List<Vector2> PossibleMovement(int x, int y)
    {
        List<Vector2> MoveLocations = new List<Vector2> { };

        //right
        int currentX = x+1;
        int currentY = y;

        while (currentX < 12 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentX++;
        }
        currentX = x-1;
        //left
        while (currentX >= 0 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentX--;
        }
        currentX = x;
        //up
        currentY += 1;
        while (currentY < 8 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY++;
        }
        //down
        currentY = y - 1;
        while (currentY >= 0 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY--;
        }
        //UL
        currentY = y + 1;
        currentX = x - 1;
        while (currentY < 8 && currentX>=0 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY++;
            currentX--;
        }
        //UR
        currentX = x + 1;
        currentY = y + 1;
        while (currentY < 8 && currentX < 12 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY++;
            currentX++;
        }
        //DL
        currentX = x - 1;
        currentY = y - 1;
        while (currentY >= 0 && currentX >= 0 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY--;
            currentX--;
        }
        //DR
        currentX = x + 1;
        currentY = y - 1;
        while (currentY >= 0 && currentX < 12 && World[currentX][currentY])
        {
            MoveLocations.Add(new Vector2(currentX, currentY));
            currentY--;
            currentX++;
        }
        return MoveLocations;
    }

    public void DisplayMovement(int x, int y)
    {
        var List = PossibleMovement(x, y);
        foreach (Vector2 v in List)
            Instantiate(MoveOutline, v, Quaternion.identity);
    }

    public void DisplayShot(int x, int y)
    {
        var List = PossibleMovement(x, y);
        foreach (Vector2 v in List)
            Instantiate(ShotOutline, v, Quaternion.identity);
    }

    public void ClearMovement()
    {
        var Tiles = GameObject.FindGameObjectsWithTag("M");
        foreach (GameObject g in Tiles)
            Destroy(g);
    }

    void Defeat()
    {
        Instantiate(DefeatSign);
    }

   public void Victory()
    {
        Instantiate(VictorySign);
    }

    public void UpdateWorld(Vector2 Start, Vector2 Finish)
    {
        World[(int)Start.x][(int)Start.y] = true;
        World[(int)Finish.x][(int)Finish.y] = false;
    }

    public void Move(Vector2 v)
    {
        if(shipMove)
        {  
            ClearMovement();
            World[(int)SelectedShip.position.x][(int)SelectedShip.position.y] = true;
            World[(int)v.x][(int)v.y] = false;
            DisplayShot((int)v.x, (int)v.y);
            SelectedShip.transform.position = v;
            shipMove = false;
        }
        else
        {
            if(PlayerTurn)
                ClearMovement();
            World[(int)v.x][(int)v.y] = false;
            Instantiate(Static, v, Quaternion.identity);
            shipMove = true;
            if (PlayerTurn)
            {
                PlayerTurn = false;
                shipMove = false;
                GetComponent<AIScript>().BotMove();
            }
            else
               PlayerTurn = true;
         
        }
    }

    // Use this for initialization
    void Start () {
        //shipMove = true;

	    for(int i=0;i<12;i++)
        {
            World[i] = new bool[8];
            for(int j=0;j<8;j++)
            {
                World[i][j] = true;
            }
        }
        var S = GetComponent<AIScript>().Ships;
        var G = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in G)
            World[(int)g.transform.position.x][(int)g.transform.position.y] = false;
        List<Vector2> Positions = new List<Vector2> { };
        System.Random rand = new System.Random();
        while(Positions.Count<S.Count)
        {
            int x = rand.Next(12);
            int y = rand.Next(8);
            if (World[x][y] && !Positions.Contains(new Vector2(x, y)))
                Positions.Add(new Vector2(x, y));
        }
        for (int i = 0; i < Positions.Count; i++)
            S[i].position = Positions[i];
        foreach (Transform t in S)
            World[(int)t.position.x][(int)t.position.y] = false;

        GetComponent<AIScript>().BotMove();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	}
}
