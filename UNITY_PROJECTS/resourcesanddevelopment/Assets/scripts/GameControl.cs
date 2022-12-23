using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public static GameControl singleton;
    public GameObject Tile;
    public Color[] Colors;
    public System.Random RNG;
    public GameObject ActiveTile;
    public int TransitLineChance;
    public int BridgeChance;
    public GameObject[] TransitLines;
    public Transform[] HandLoc;
    public GameObject[] Pieces;
    public GameObject Dice;
    public Sprite[] DiceFaces;
    public Transform[] DiceLoc;
    public GameObject Resource;
    public Sprite[] ResIcons;
    int PiecesPlaced;
    public GameObject NextTurnButton;
    GameObject ActivePiece;
    Vector2 VillageLoc;
    bool BuildView;
    public Vector2[] nOffsets = new Vector2[6] { new Vector2(.8f, 0), new Vector2(.4f, .709f), new Vector2(-.4f, .709f), new Vector2(-.8f, 0), new Vector2(-.4f, -.709f), new Vector2(.4f, -.709f) };
    List<RobberScript> Robbers=new List<RobberScript> { };
    public GameObject Robber;
    public int[] Scores;
    public UnityEngine.UI.Text ScoreText;

    private void Awake()
    {
        singleton = this;
        PiecesPlaced = 0;
        NextTurnButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { NextTurn(); });
        RNG = new System.Random(ThreadSafeRandom.Next());
    }

    // Use this for initialization
    void Start () {
        FirstTurn();
	}

    void FirstTurn()
    {
        foreach (Transform t in HandLoc)
        {
            GameObject go = Instantiate(Tile, t.position, Quaternion.identity) as GameObject;
            TileScript TS = go.GetComponent<TileScript>();
            TS.ResourceID = RNG.Next(Colors.Length);
            go.GetComponent<SpriteRenderer>().color = Colors[TS.ResourceID];
            TS.BuildVillage();      
        }
    }

    void FirstBridges()
    {
        foreach (Transform t in HandLoc)
        {
            GameObject go = Instantiate(Tile, t.position, Quaternion.identity) as GameObject;
            TileScript TS = go.GetComponent<TileScript>();
            TS.ResourceID = RNG.Next(Colors.Length);
            go.GetComponent<SpriteRenderer>().color = Colors[TS.ResourceID];
            int RRindex = RNG.Next(TransitLines.Length - 1);
            GameObject r = Instantiate(TransitLines[RRindex]);
            r.transform.SetParent(go.transform);
            r.transform.localPosition = Vector2.zero;
            List<int> RailEdges = new List<int> { };
            if (RRindex == 0)
            {
                RailEdges.Add(0);
                RailEdges.Add(3);
                TS.EdgeTypes[0] = 1;
                TS.EdgeTypes[3] = 1;
            }
            if (RRindex == 1)
            {
                RailEdges.Add(1);
                RailEdges.Add(3);
                TS.EdgeTypes[1] = 1;
                TS.EdgeTypes[3] = 1;
            }
            if (RRindex == 2)
            {
                RailEdges.Add(1);
                RailEdges.Add(3);
                RailEdges.Add(5);
                TS.EdgeTypes[1] = 1;
                TS.EdgeTypes[3] = 1;
                TS.EdgeTypes[5] = 1;
            }

            List<int> pos = new List<int> { 0, 1, 2, 3, 4, 5 };
            foreach (int i in RailEdges)
                pos.Remove(i);
            while (pos.Count > 2)
            {
                pos.RemoveAt(RNG.Next(pos.Count));
            }
            foreach (int i in pos)
            {
                GameObject g = Instantiate(TransitLines[TransitLines.Length - 1]);
                g.transform.SetParent(go.transform);
                g.transform.localEulerAngles = new Vector3(0, 0, 60 * i);
                g.transform.localPosition = Vector3.zero;
                TS.EdgeTypes[i] = 2;
            }
        }
    }

    public void NextTurn()
    {
        ActivePiece = null;
        if (ActiveTile != null)
            Destroy(ActiveTile);
        CleanHand();
        CleanDice();
        GenerateHand();
        GenerateDice();
        for(int i=0;i<Robbers.Count;i++)
        {
            Robbers[i].Move();
        }
        if (PiecesPlaced > 3 && RNG.Next(3) == 0 && Robbers.Count<=8)
            GenerateRobber();
    }

    void CleanHand()
    {
        foreach (Transform t in HandLoc)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<TileScript>().isLocked == false)
                Destroy(hit.collider.gameObject);
        }
    }

    void CleanDice()
    {
        foreach (Transform t in DiceLoc)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.zero);
            if (hit.collider != null)
                Destroy(hit.collider.gameObject);
        }
    }

    void GenerateDice()
    {
        foreach (Transform t in DiceLoc)
        {
            GameObject D= Instantiate(Dice, t.position, Quaternion.identity) as GameObject;
            D.GetComponent<DiceScript>().TurnDice = true;
        }
    }

    void GenerateHand()
    {
        foreach (Transform t in HandLoc)
            GenerateTile(t.position);
    }

    bool ValidatePlacement(TileScript T, Vector2 Loc, List<TileScript> Neighbors, List<int> NeighborIndex)
    {
        foreach(RobberScript r in Robbers)
        {
            if (Vector2.SqrMagnitude((Vector2)r.transform.position - Loc) < .1f)
                return false;
        }
        for(int i=0;i<Neighbors.Count;i++)
        {
            if ((T.EdgeTypes[NeighborIndex[i]] == 3 && Neighbors[i].ResourceID == 0 && Neighbors[i].EdgeTypes[(NeighborIndex[i] + 3) % 6]==0) || (T.ResourceID == 0 && T.EdgeTypes[NeighborIndex[i]]==0 && Neighbors[i].EdgeTypes[(NeighborIndex[i] + 3) % 6] == 3))
                continue;
            else if (T.EdgeTypes[NeighborIndex[i]] != Neighbors[i].EdgeTypes[(NeighborIndex[i] + 3) % 6])
                return false;
        }
        return true;
    }

    void GenerateRobber()
    {
        int y = RNG.Next(-290, 450);
        int x = RNG.Next(-750, 750);
        if (-350 < x && x >= 350 && y > -185 && y < 275)
        {
            y = RNG.Next(-290, 450);
            x = RNG.Next(-750, 750);
        }
        Vector2 v = find_Hex_XY(new Vector2((float)x / 100f, (float)y / 100f));
        RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);
        if (hit.collider == null || (hit.collider.GetComponent<TileScript>().GamePiece == null && hit.collider.GetComponent<TileScript>().StockPile[0] == 0))
        {
            GameObject go = Instantiate(Robber, v, Quaternion.identity) as GameObject;
            RobberScript R = go.GetComponent<RobberScript>();
            R.GCIndex = Robbers.Count;
            Robbers.Add(R);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<TileScript>().GamePiece = go;
                R.CurrentTile = hit.collider.GetComponent<TileScript>();
            }
        }
    }

    public void RemoveRobber(int index)
    {
        Destroy(Robbers[index].gameObject);
        Robbers.RemoveAt(index);
        if(index<Robbers.Count)
        {
            for (int i = index; i < Robbers.Count; i++)
                Robbers[i].GCIndex--;
        }
    }

    void GenerateTile(Vector2 SpawnLoc)
    {
        GameObject go = Instantiate(Tile, SpawnLoc, Quaternion.identity) as GameObject;
        TileScript TS=go.GetComponent<TileScript>();
        TS.ResourceID=RNG.Next(Colors.Length);
        go.GetComponent<SpriteRenderer>().color= Colors[TS.ResourceID];
        if(RNG.Next(BridgeChance)==0)
        {
            int RRindex = RNG.Next(TransitLines.Length - 1);
            GameObject t = Instantiate(TransitLines[RRindex]);
            t.transform.SetParent(go.transform);
            t.transform.localPosition = Vector2.zero;
            List<int> RailEdges = new List<int> {};
            if(RRindex==0)
            {
                RailEdges.Add(0);
                RailEdges.Add(3);
                TS.EdgeTypes[0] = 1;
                TS.EdgeTypes[3] = 1;
            }
            if (RRindex == 1)
            {
                RailEdges.Add(1);
                RailEdges.Add(3);
                TS.EdgeTypes[1] = 1;
                TS.EdgeTypes[3] = 1;
            }
            if (RRindex == 2)
            {
                RailEdges.Add(1);
                RailEdges.Add(3);
                RailEdges.Add(5);
                TS.EdgeTypes[1] = 1;
                TS.EdgeTypes[3] = 1;
                TS.EdgeTypes[5] = 1;
            }

            List<int> pos = new List<int> { 0, 1, 2, 3, 4, 5 };
            foreach (int i in RailEdges)
                pos.Remove(i);
            bool rollRoad = true;
            while(pos.Count>2 && rollRoad)
            {
                pos.RemoveAt(RNG.Next(pos.Count));
                rollRoad = RNG.Next(3) == RNG.Next(3);
            }
            foreach (int i in pos)
            {
                GameObject g = Instantiate(TransitLines[TransitLines.Length - 1]);
                g.transform.SetParent(go.transform);
                g.transform.localEulerAngles = new Vector3(0, 0, 60 * i);
                g.transform.localPosition = Vector3.zero;
                TS.EdgeTypes[i] = 2;
            }


            return;
        }
        else if(RNG.Next(TransitLineChance)==0)
        {
            if (RNG.Next(2) == 0) //railroad
            {
                int RRindex = RNG.Next(TransitLines.Length - 1);
                GameObject t = Instantiate(TransitLines[RRindex]);
                t.transform.SetParent(go.transform);
                t.transform.localPosition = Vector2.zero;
                if (RRindex == 0)
                {
                    TS.EdgeTypes[0] = 1;
                    TS.EdgeTypes[3] = 1;
                }
                if (RRindex == 1)
                {
                    TS.EdgeTypes[1] = 1;
                    TS.EdgeTypes[3] = 1;
                }
                if (RRindex == 2)
                {
                    TS.EdgeTypes[1] = 1;
                    TS.EdgeTypes[3] = 1;
                    TS.EdgeTypes[5] = 1;
                }
            }
            else //road or river
            {
                bool isWater = (RNG.Next(2) == 0 && TS.ResourceID != 0);
                List<int> pos = new List<int> { };
                for(int i=0;i<6;i++)
                {
                    if(RNG.Next(5)==4)
                    {
                        int r = RNG.Next(6);
                        if (!pos.Contains(r))
                            pos.Add(r);
                    }
                }
                if(pos.Count==0)
                    pos.Add(RNG.Next(6));
                if (pos.Count == 1)
                {
                    int p = RNG.Next(6);
                    while (p == pos[0])
                        p = RNG.Next(6);
                    pos.Add(p);
                }
                foreach(int i in pos)
                {
                    GameObject g = Instantiate(TransitLines[TransitLines.Length - 1]);
                    if (isWater)
                    {
                        g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
                        TS.EdgeTypes[i] = 3;
                    }
                    else
                        TS.EdgeTypes[i] = 2;
                    g.transform.SetParent(go.transform);
                    g.transform.localEulerAngles = new Vector3(0, 0, 60 * i);
                    g.transform.localPosition = Vector3.zero;
                }
           
            }
        }
        else
        {
            if (TS.ResourceID != 0)
            {
                GameObject d = Instantiate(Dice, go.transform) as GameObject;
                d.transform.localPosition = Vector2.zero;
                d.transform.localScale = new Vector2(.75f, .75f);
                TS.GamePiece = d;
            }
        }
    }

    bool ValidatePiece(TileScript TS)
    {
        int id=ActiveTile.GetComponent<PieceScript>().ID;
        if (id == 3 && TS.ResourceID == 0)
            return true;
        foreach(int i in TS.EdgeTypes)
        {
            if (i == id)
                return true;
        }
        return false;
    }

    void HandlePieceMove(RaycastHit2D hit)
    {
        if(hit.collider.GetComponent<TileScript>().GamePiece != null && hit.collider.GetComponent<TileScript>().GamePiece.CompareTag("Robber"))
        {
            RemoveRobber(hit.collider.GetComponent<TileScript>().GamePiece.GetComponent<RobberScript>().GCIndex);
            Scores[1]++;
        }
        hit.collider.GetComponent<TileScript>().GamePiece = ActivePiece;
        PieceScript ps = ActivePiece.GetComponent<PieceScript>();
        ps.Movement--;
        if (ps.Movement > 0)
            ps.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = DiceFaces[ps.Movement - 1];
        else
            ps.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        ps.transform.position = hit.collider.transform.position;
    }

    Vector2 find_Hex_XY(Vector2 V)
    {
        int row = Mathf.RoundToInt(V.y / .709f);
        float new_X=0;
        float new_Y=row*.709f;
        if (row%2==0)
        {
            new_X = Mathf.Round(V.x / .8f) * .8f;
        }
        else
        {
            new_X = .4f+Mathf.Round((V.x-.4f) / .8f) * .8f;
        }
        return new Vector2(new_X, new_Y);
    }

    void BackFromBuildView()
    {
        BuildView = false;
        CleanDice();
        ResetStuff();
        NextTurnButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Next Turn";
        NextTurnButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        NextTurnButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { NextTurn(); });
    }
    void MoveTurnStuffOffscreen()
    {
        List<GameObject> temp = new List<GameObject> { };
        foreach (Transform t in HandLoc)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.zero);
            if (hit.collider != null)
                temp.Add(hit.collider.gameObject);
        }
        foreach (Transform t in DiceLoc)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.zero);
            if (hit.collider != null)
                temp.Add(hit.collider.gameObject);
        }
        foreach (GameObject g in temp)
            g.transform.SetParent(transform);
        transform.position = new Vector3(-100, -100, -100);
    }

    private void ResetStuff()
    {
        transform.position = Vector3.zero;
        transform.DetachChildren();
    }

    void ShowScore()
    {
       ScoreText.text= "Tiles Placed: "+PiecesPlaced.ToString() + ", Villages: "+ Scores[0].ToString()+ ", Robbers Caught: " + Scores[1].ToString(); 
    }

    void FadeScore()
    {
        ScoreText.text = "";
    }

    // Update is called once per frame
    void Update () {
        if (ActiveTile != null)
        {
            Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ActiveTile.transform.position = new Vector3(v.x,v.y,0);
        }
     if (Input.GetMouseButtonDown(0) && ActiveTile==null)
        {
            // GenerateTile(find_Hex_XY(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player") && !hit.collider.GetComponent<TileScript>().isLocked)
                {
                    //pickup tile in hand
                    ActiveTile = hit.collider.gameObject;
                    ActiveTile.GetComponent<TileScript>().isLocked = true;
                    CleanHand();
                    ActiveTile.GetComponent<TileScript>().isLocked = false;
                }
                else if(hit.collider.CompareTag("Player") && hit.collider.GetComponent<TileScript>().isLocked)
                {
                    //interact with placed tile
                    if (hit.collider.GetComponent<TileScript>().GamePiece != null && hit.collider.GetComponent<TileScript>().GamePiece.CompareTag("Village") && PiecesPlaced > 1 && hit.collider.GetComponent<TileScript>().GamePiece.GetComponent<PieceScript>().HeldID == 0)
                    {
                        VillageLoc = hit.collider.transform.position;
                        if (!BuildView)
                        {
                            BuildView = true;
                            NextTurnButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
                            NextTurnButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { BackFromBuildView(); });
                            NextTurnButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Back";
                            MoveTurnStuffOffscreen();
                            for (int i = 0; i < 3; i++)
                            {
                                Instantiate(Pieces[i + 2], DiceLoc[i].position, Quaternion.identity);
                            }
                        }
                    }
                    else if (hit.collider.GetComponent<TileScript>().GamePiece != null && hit.collider.GetComponent<TileScript>().GamePiece.CompareTag("Piece"))
                    {
                        ActivePiece = hit.collider.GetComponent<TileScript>().GamePiece;
                    }
                    else if (ActivePiece != null && ActivePiece.GetComponent<PieceScript>().Movement > 0 && hit.collider.GetComponent<TileScript>().StockPile[0] == 0 && Vector2.SqrMagnitude(hit.collider.transform.position - ActivePiece.transform.position) < 1f)
                    {
                        //movement
                        Vector2 diff = hit.collider.transform.position - ActivePiece.transform.position;
                        if (ActivePiece.GetComponent<PieceScript>().ID == 3 && hit.collider.GetComponent<TileScript>().ResourceID == 0)
                        {
                            HandlePieceMove(hit);
                            Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                        }
                        if (diff.x > 0 && diff.y > 0)
                        {
                            if (ActivePiece.GetComponent<PieceScript>().ID == hit.collider.GetComponent<TileScript>().EdgeTypes[4])
                            {
                                HandlePieceMove(hit);
                                Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                            }
                        }
                        else if (diff.x > 0 && diff.y == 0)
                        {
                            if (ActivePiece.GetComponent<PieceScript>().ID == hit.collider.GetComponent<TileScript>().EdgeTypes[3])
                            {
                                HandlePieceMove(hit);
                                Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                            }
                        }
                        else if (diff.x > 0 && diff.y < 0)
                        {
                            if (ActivePiece.GetComponent<PieceScript>().ID == hit.collider.GetComponent<TileScript>().EdgeTypes[2])
                            {
                                HandlePieceMove(hit);
                                Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                            }
                        }
                        else if (diff.x < 0 && diff.y < 0)
                        {
                            if (ActivePiece.GetComponent<PieceScript>().ID == hit.collider.GetComponent<TileScript>().EdgeTypes[1])
                            {
                                HandlePieceMove(hit);
                                Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                            }
                        }
                        else if (diff.x < 0 && diff.y == 0)
                        {
                            if (ActivePiece.GetComponent<PieceScript>().ID == hit.collider.GetComponent<TileScript>().EdgeTypes[0])
                            {
                                HandlePieceMove(hit);
                                Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                            }
                        }
                        else if (diff.x < 0 && diff.y > 0)
                        {
                            if (ActivePiece.GetComponent<PieceScript>().ID == hit.collider.GetComponent<TileScript>().EdgeTypes[5])
                            {
                                HandlePieceMove(hit);
                                Physics2D.Raycast((Vector2)ActivePiece.transform.position - diff, Vector2.zero).collider.GetComponent<TileScript>().GamePiece = null;
                            }
                        }

                    }
                    if (ActivePiece != null && ActivePiece.GetComponent<PieceScript>().HeldID == 0 && hit.collider.GetComponent<TileScript>().GamePiece != null && hit.collider.GetComponent<TileScript>().GamePiece.CompareTag("Resource") && Vector2.SqrMagnitude(hit.collider.transform.position - ActivePiece.transform.position) < 1f)
                    {
                        //pick up Resource
                        hit.collider.GetComponent<TileScript>().GamePiece.transform.position = ActivePiece.transform.position;
                        hit.collider.GetComponent<TileScript>().GamePiece.transform.SetParent(ActivePiece.transform);
                        hit.collider.GetComponent<TileScript>().GamePiece.transform.localScale = hit.collider.GetComponent<TileScript>().GamePiece.transform.localScale / 2;
                        ActivePiece.GetComponent<PieceScript>().HeldID = hit.collider.GetComponent<TileScript>().StockPile[0];
                        hit.collider.GetComponent<TileScript>().GamePiece = null;
                        hit.collider.GetComponent<TileScript>().StockPile[0] = 0;

                    }
                    else if (ActivePiece != null && ActivePiece.GetComponent<PieceScript>().HeldID > 0 && hit.collider.GetComponent<TileScript>().StockPile[0] != 0 && Vector2.SqrMagnitude(hit.collider.transform.position - ActivePiece.transform.position) < 1f)
                    {
                        //stockpile
                        TileScript ts = hit.collider.GetComponent<TileScript>();
                        int HID = ActivePiece.GetComponent<PieceScript>().HeldID;
                        if (ts.StockPile[0] != HID &&
                            ts.StockPile[1] != HID &&
                            ts.StockPile[2] != HID &&
                            ts.StockPile[3] != HID
                            )
                        {
                            int nextindex = 1;
                            while (ts.StockPile[nextindex] != 0)
                                nextindex++;
                            ts.StockPile[nextindex] = HID;
                            switch (nextindex)
                            {
                                case 1:
                                    ts.GamePiece.transform.localScale = ts.GamePiece.transform.localScale / 2;
                                    ts.GamePiece.transform.localPosition = new Vector2(0, .25f);
                                    ActivePiece.transform.GetChild(1).position = (Vector2)ts.transform.position + new Vector2(.25f, .1f);
                                    ActivePiece.transform.GetChild(1).SetParent(ts.transform);
                                    ts.GamePiece = null;
                                    break;
                                case 2:
                                    ActivePiece.transform.GetChild(1).position = (Vector2)ts.transform.position + new Vector2(.16f, -.16f);
                                    ActivePiece.transform.GetChild(1).SetParent(ts.transform);
                                    break;
                                case 3:
                                    ActivePiece.transform.GetChild(1).position = (Vector2)ts.transform.position + new Vector2(-.16f, -.16f);
                                    ActivePiece.transform.GetChild(1).SetParent(ts.transform);
                                    break;
                                case 4:
                                    Destroy(ts.transform.GetChild(ts.transform.childCount - 1).gameObject);
                                    Destroy(ts.transform.GetChild(ts.transform.childCount - 2).gameObject);
                                    Destroy(ts.transform.GetChild(ts.transform.childCount - 3).gameObject);
                                    Destroy(ts.transform.GetChild(ts.transform.childCount - 4).gameObject);
                                    Destroy(ActivePiece.transform.GetChild(1).gameObject);
                                    ts.BuildVillage();
                                    Scores[0]++;
                                    break;
                            }
                            ActivePiece.GetComponent<PieceScript>().HeldID = 0;
                        }
                    }
                    else if (ActivePiece != null && ActivePiece.GetComponent<PieceScript>().HeldID > 0 && hit.collider.GetComponent<TileScript>().StockPile[0] == 0 && hit.collider.GetComponent<TileScript>().GamePiece== null && Vector2.SqrMagnitude(hit.collider.transform.position - ActivePiece.transform.position) < 1f)
                    {
                        TileScript ts = hit.collider.GetComponent<TileScript>();
                        if (ts.ResourceID != 0 &&
                            ts.EdgeTypes[0] == 0 &&
                            ts.EdgeTypes[1] == 0 &&
                            ts.EdgeTypes[2] == 0 &&
                            ts.EdgeTypes[3] == 0 &&
                            ts.EdgeTypes[4] == 0 &&
                            ts.EdgeTypes[5] == 0)
                        {
                            
                            ts.GamePiece = ActivePiece.transform.GetChild(1).gameObject;
                            int HID = ActivePiece.GetComponent<PieceScript>().HeldID;
                            ts.StockPile[0] = HID;
                            ts.GamePiece.transform.position = (Vector2)ts.transform.position;
                            ts.GamePiece.transform.SetParent(ts.transform);
                            ts.GamePiece.transform.localScale *= 2;
                            ActivePiece.GetComponent<PieceScript>().HeldID = 0;
                        }
                    }
                }
                else
                {
                    //pickup dice or piece
                    ActiveTile = hit.collider.gameObject;   
                }
            }
        }
     else if(Input.GetMouseButtonDown(0) && ActiveTile != null)
        {
            Vector2 tryPos= find_Hex_XY(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tryPos.y > 4.5f || tryPos.y < -3f)
                return;
            if (tryPos.x < -7.7f || tryPos.x > 7.7f)
                return;
            ActiveTile.transform.position = new Vector3(100, 100, 100);
            RaycastHit2D hit = Physics2D.Raycast(tryPos, Vector2.zero);
            if (hit.collider == null && ActiveTile.CompareTag("Player"))
            {
                List<TileScript> nt = new List<TileScript> { };
                List<int> nIndex=new List<int> { };
                
                for(int i=0;i<6;i++)
                {
                    hit = Physics2D.Raycast(tryPos+nOffsets[i], Vector2.zero);
                    if (hit.collider != null)
                    {
                        nt.Add(hit.collider.gameObject.GetComponent<TileScript>());
                        nIndex.Add(i);
                    }
                }
                if(ValidatePlacement(ActiveTile.GetComponent<TileScript>(), tryPos, nt, nIndex))
                {
                    ActiveTile.transform.position = tryPos;
                    ActiveTile.GetComponent<TileScript>().isLocked = true;
                    ActiveTile = null;
                    PiecesPlaced++;
                    if(PiecesPlaced==1)
                    {
                        FirstBridges();
                    }
                    else if(PiecesPlaced==2)
                    {
                        NextTurnButton.transform.localScale = Vector3.one;
                        GenerateHand();
                        GenerateDice();
                    }
                }
            }
            else if(hit.collider != null)
            {
                //Dice Placement
                if(ActiveTile.CompareTag("Dice") && hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.GetComponent<TileScript>().GenValue == ActiveTile.GetComponent<DiceScript>().Val)
                    {
                        //Resource gen
                        Destroy(ActiveTile);
                        ActiveTile = null;
                        Destroy(hit.collider.transform.GetChild(0).gameObject);
                        GameObject g = Instantiate(Resource, hit.collider.transform.position, Quaternion.identity) as GameObject;
                        g.transform.SetParent(hit.collider.transform);
                        g.GetComponent<SpriteRenderer>().sprite = ResIcons[g.transform.parent.GetComponent<TileScript>().ResourceID - 1];
                        hit.collider.GetComponent<TileScript>().GenValue = -1;
                        hit.collider.GetComponent<TileScript>().GamePiece = g;
                        hit.collider.GetComponent<TileScript>().StockPile[0] = hit.collider.GetComponent<TileScript>().ResourceID;
                    }
                    else if(hit.collider.GetComponent<TileScript>().GamePiece != null && hit.collider.GetComponent<TileScript>().GamePiece.CompareTag("Piece"))
                    {
                        //movement assignment
                        hit.collider.GetComponent<TileScript>().GamePiece.GetComponent<PieceScript>().Movement = ActiveTile.GetComponent<DiceScript>().Val+1;
                        SpriteRenderer sr = hit.collider.GetComponent<TileScript>().GamePiece.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        sr.sprite = DiceFaces[ActiveTile.GetComponent<DiceScript>().Val];
                        sr.enabled = true;
                        Destroy(ActiveTile);
                        ActivePiece = hit.collider.GetComponent<TileScript>().GamePiece;
                    }
                }
                //piece Placement
                else if(hit.collider.CompareTag("Player") && hit.collider.GetComponent<TileScript>().GamePiece==null && Vector2.SqrMagnitude((Vector2)hit.collider.transform.position-VillageLoc)<1f && ValidatePiece(hit.collider.GetComponent<TileScript>()))
                {
                    ActiveTile.transform.position = hit.collider.transform.position;
                    hit.collider.GetComponent<TileScript>().GamePiece = ActiveTile;
                    Destroy(ActiveTile.GetComponent<BoxCollider2D>());
                    ActivePiece = ActiveTile;
                    ActiveTile = null;
                    BackFromBuildView();
                    TileScript vil = Physics2D.Raycast(VillageLoc, Vector2.zero).collider.GetComponent<TileScript>();
                    vil.GamePiece.GetComponent<PieceScript>().HeldID = 1;
                    GameObject g = Instantiate(ActivePiece, vil.transform)as GameObject;
                    g.transform.localPosition = Vector2.zero;
                    g.transform.localScale /= 2;
                }

            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit= Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider !=null && hit.collider.CompareTag("Player") && !hit.collider.GetComponent<TileScript>().isLocked)
            {
                hit.collider.gameObject.transform.Rotate(new Vector3(0, 0, -60));
                hit.collider.GetComponent<TileScript>().HandleRotation();
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowScore();
            Invoke("FadeScore", 5);
        }
        else if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
    }
}
