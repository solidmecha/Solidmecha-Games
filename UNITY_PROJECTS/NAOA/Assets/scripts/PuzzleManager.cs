using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour {

    public GameObject Player;
    public GameObject Tile;
    public List<Sprite> TileSprites = new List<Sprite> { };
    public List<Color> Colors=new List<Color> { };
    public List<Rule> Rules = new List<Rule> { };
    public List<GameObject> RuleObjects = new List<GameObject> { };
    public GameObject[][] Board;
    System.Random RNG;
    int height;
    int width;
    public int PrevTileID;
    public Vector2 PlayerPos;
    Vector2 GoalPos;
    public GameObject Goal;
    public int GoalCount;
    public int Wins;
    public int Resets;
    public Text UI_text;
	// Use this for initialization
	void Start () {
        Wins=PlayerPrefs.GetInt("Wins");
        Resets=PlayerPrefs.GetInt("Resets");
        GUIHandler();
        RNG = new System.Random();
        height = 8;
        width = 8;
        Board = new GameObject[width][];
        setRules();
        makeMaze();
        for(int i=0;i<5;i++)
        {
            if (Rules[i].neverAfter)
            {
                Vector2 V = RuleObjects[i].transform.position;
                GameObject go = Instantiate(Tile, V+Vector2.left, Quaternion.identity) as GameObject;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                int r = Rules[i].otherID;
                sr.sprite = TileSprites[r];
                sr.color = Colors[r];
            }
            else
            {
                Vector2 V = RuleObjects[i].transform.position;
                GameObject go = Instantiate(Tile, V+Vector2.right, Quaternion.identity) as GameObject;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                TileScript ts = (TileScript)go.GetComponent(typeof(TileScript));
                int r = Rules[i].otherID;
                sr.sprite = TileSprites[r];
                sr.color = Colors[r];
            }
        }
	}

   public void GUIHandler()
    {
        PlayerPrefs.SetInt("Wins", Wins);
        PlayerPrefs.SetInt("Resets", Resets);
        UI_text.text = "Resets: " + Resets.ToString() + "    Wins: " + Wins.ToString();
    }
    void setRules()
    {
        for (int i = 0; i < Colors.Count; i++)
        {
          Rule rule = new Rule();
          rule.otherID=RNG.Next(Colors.Count);
            int b = RNG.Next(2);
            if (b == 0)
                rule.neverAfter = true;
            else
                rule.neverAfter = false;
            Rules.Add(rule);
        }
    }

    public struct Rule
    {
       public bool neverAfter;
       public int otherID;
    }

    void makeMaze()
    {
        for (int i = 0; i < width; i++)
        {
            Board[i] = new GameObject[height];
            for (int j = 0; j < height; j++)
            {
                Vector2 V = new Vector2(i, j) + (Vector2)transform.position;
                GameObject go = Instantiate(Tile, V, Quaternion.identity) as GameObject;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                TileScript ts = (TileScript)go.GetComponent(typeof(TileScript));
                int r = RNG.Next(Colors.Count);
                sr.sprite = TileSprites[r];
                sr.color = Colors[r];
                ts.ID = r;
                Board[i][j] = go;
            }
        }
        float x = RNG.Next(width);
        float y = RNG.Next(height);
        PlayerPos = new Vector2(x, y);
        Player.transform.position = PlayerPos + (Vector2)transform.position;
        TileScript ts2 = (TileScript)Board[(int)PlayerPos.x][(int)PlayerPos.y].GetComponent(typeof(TileScript));
        PrevTileID = ts2.ID;
        List<Vector2> path = MakeSolutionPath();
        Vector2 CurrentPos = PlayerPos;
        List<List<int>> PossibleIDs = new List<List<int>> { };
        for (int i = 0; i < 5; i++)
        {
            List<int> AllowedFollows=new List<int> { };
            List<int> Ints = new List<int> { 0, 1, 2, 3, 4 };
           // Ints.Remove(i);
            foreach (int j in Ints)
            {
                if ((Rules[j].neverAfter && Rules[j].otherID != i) || (!Rules[j].neverAfter && Rules[j].otherID == i))
                    AllowedFollows.Add(j);
            }
            if (AllowedFollows.Count >= 2)
                AllowedFollows.Remove(i);
            PossibleIDs.Add(AllowedFollows);
        }
        for (int i = 0; i < 5; i++)
        {
            if (PossibleIDs[i].Count == 0)
            {
                for (int j = 0; j < 5; j++)
                    PossibleIDs[j].Remove(i);
            }
        }
        while (PossibleIDs[ts2.ID].Count==0)
        {
            int r = RNG.Next(5);
            ts2.ID = r;
            SpriteRenderer sr=ts2.GetComponent<SpriteRenderer>();
            sr.sprite = TileSprites[r];
                sr.color = Colors[r];
        }
        int numberOfgoals = RNG.Next(5);
        int counter=0;
        List<int> goalInts = new List<int> { };
        for (int i = 0; i < numberOfgoals; i++)
            goalInts.Add(RNG.Next(path.Count));
        foreach (Vector2 v in path)
        {
            
            TileScript tsc= (TileScript)Board[(int)CurrentPos.x][(int)CurrentPos.y].GetComponent(typeof(TileScript));
            TileScript tsp = (TileScript)Board[(int)CurrentPos.x + (int)v.x][(int)CurrentPos.y + (int)v.y].GetComponent(typeof(TileScript));
            if(PossibleIDs[tsc.ID].Count!=0)            
                tsp.ID = PossibleIDs[tsc.ID][RNG.Next(PossibleIDs[tsc.ID].Count)];
           SpriteRenderer sr = Board[(int)CurrentPos.x + (int)v.x][(int)CurrentPos.y + (int)v.y].GetComponent<SpriteRenderer>();
            sr.sprite = TileSprites[tsp.ID];
            sr.color = Colors[tsp.ID];
            CurrentPos += v;
            if(goalInts.Contains(counter))
            {
                goalInts.Remove(counter);
                Instantiate(Goal, CurrentPos + (Vector2)transform.position, Quaternion.identity);
            }
            counter++;
        }
        Instantiate(Goal, GoalPos+(Vector2)transform.position, Quaternion.identity);
    }

    List<Vector2> MakeSolutionPath()
    {
        List<Vector2> path = new List<Vector2> { };
        List<Vector2> pathPoints = new List<Vector2> { };
        int R=RNG.Next(17,42);
        Vector2 CurrentPos=Player.transform.position-transform.position;
        CurrentPos = new Vector2(Mathf.Round(CurrentPos.x), Mathf.Round(CurrentPos.y));
        for(int i=0; i< R;i++)
        {
            pathPoints.Add(CurrentPos);
            List<Vector2> PossibleMoves = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            List<Vector2> RemoveIndex = new List<Vector2> { };
            for(int j=0;j<4;j++)
            {
                Vector2 V = CurrentPos + PossibleMoves[j];
                if(V.x< 0 || V.x>=width || V.y<0 || V.y>=height || !checkList(V, pathPoints))
                {
                    RemoveIndex.Add(PossibleMoves[j]);
                }
            }
            foreach (Vector2 v in RemoveIndex)
                PossibleMoves.Remove(v);

            if (PossibleMoves.Count == 0)
            {
                break;
            }
            else
            {
                int r = RNG.Next(PossibleMoves.Count);
                CurrentPos += PossibleMoves[r];
                path.Add(PossibleMoves[r]);
            }
        }
        GoalPos = CurrentPos;
        return path;
    }

    bool checkList(Vector2 v, List<Vector2> VL)
    {
        //returns true is v is not in VL
        foreach(Vector2 vt in VL)
        {
            if(v.x==vt.x && v.y ==vt.y)
            {
                return false;
            }
        }
        return true;
    }

   public bool checkMove(Vector2 mv) //sets prevtileID if true
    {
        Vector2 v = PlayerPos + mv;
        if (v.x < 0 || v.x >= width || v.y < 0 || v.y >= height)
            return false;
        TileScript ts = (TileScript)Board[(int)PlayerPos.x+(int)mv.x][(int)PlayerPos.y+(int)mv.y].GetComponent(typeof(TileScript));

        if ((Rules[ts.ID].neverAfter && Rules[ts.ID].otherID != PrevTileID) || (!Rules[ts.ID].neverAfter && Rules[ts.ID].otherID == PrevTileID))
        {
            PrevTileID = ts.ID;
            return true;
        }
        else
        {
            Resets++;
            GUIHandler();
            Application.LoadLevel(0);
            return false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.RightArrow) && checkMove(Vector2.right))
        {
            PlayerPos += Vector2.right;
           Player.transform.position =(Vector2)Player.transform.position+ Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && checkMove(Vector2.left))
        {
            PlayerPos += Vector2.left;
            Player.transform.position = (Vector2)Player.transform.position + Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && checkMove(Vector2.up))
        {
            PlayerPos += Vector2.up;
            Player.transform.position = (Vector2)Player.transform.position + Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && checkMove(Vector2.down))
        {
            PlayerPos += Vector2.down;
            Player.transform.position = (Vector2)Player.transform.position + Vector2.down;
        }
        else
            return;

	}
}
