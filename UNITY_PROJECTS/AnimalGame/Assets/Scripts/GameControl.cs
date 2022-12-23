using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject Tile;
    public GameObject Piece;
    public List<Sprite> Sprites = new List<Sprite> { };
    public int[][] World;
    GameObject[][] WorldObjects;
    GameObject[][] WorldTiles;
     List<int> Select1=new List<int> { };
     List<int> Select2 = new List<int> { };
    public Vector2 SelectPos;
    public List<List<int>> CurSelect=new List<List<int>> {};
    List<GameObject> SelectedObjects = new List<GameObject> { };
    public int csIndex;
    public int height;
    public int width;
    System.Random RNG;
    public GameObject Outline;

	// Use this for initialization
	void Start () {
        CurSelect.Add(Select1);
        CurSelect.Add(Select2);
        csIndex = 0;
        SelectPos = new Vector2(-6, -4);
        World = new int[width][];
        WorldObjects = new GameObject[width][];
        WorldTiles = new GameObject[width][];
        RNG = new System.Random(ThreadSafeRandom.Next());
        CreateGrid();
	}

    bool CheckSelect(Vector2 v1, Vector2 v2)
    {
        print(Vector2.Distance(v1, v2));
        if (Vector2.Distance(v1, v2) < 2)
            return true;
        else
            return false;
    }

    void colorGreen(GameObject G)
    {
         G.GetComponent<SpriteRenderer>().color=Color.green;
    }

    void colorWhite(GameObject G)
    {
        G.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void CreateGrid()
    {
        for (int i=0;i<width; i++)
        {
            World[i] = new int[height];
            WorldTiles[i] = new GameObject[height];
            WorldObjects[i] = new GameObject[height];
            for(int j=0;j< height;j++)
            {
                GameObject T=Instantiate(Tile, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                WorldTiles[i][j] = T;
               GameObject go= Instantiate(Piece, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                WorldObjects[i][j] = go;
                int r = RNG.Next(Sprites.Count);
                World[i][j] = r;
                go.GetComponent<SpriteRenderer>().sprite = Sprites[r];
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	if(Input.GetMouseButton(0))
        {
            Vector2 v=Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
            v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
            if (v.x >= 0 && v.x < width && v.y >= 0 && v.y < height)
            {
                //need to check if selection is adjacent
                if (!SelectedObjects.Contains(WorldObjects[(int)v.x][(int)v.y]) )
                {
                    CurSelect[csIndex].Add(World[(int)v.x][(int)v.y]);
                    GameObject go = Instantiate(Piece, SelectPos - Vector2.down * CurSelect[csIndex].Count, Quaternion.identity) as GameObject;
                    SelectedObjects.Add(go);
                    SelectedObjects.Add(WorldObjects[(int)v.x][(int)v.y]);
                    go.GetComponent<SpriteRenderer>().sprite = Sprites[World[(int)v.x][(int)v.y]];
                }
            }
        }
        if (CurSelect[0].Count >= 4 && CurSelect[0].Count==CurSelect[1].Count)
        {
            foreach(GameObject g in SelectedObjects)
            { Destroy(g); }
            SelectedObjects.Clear();
            CurSelect[0].Clear();
            CurSelect[1].Clear();
        }

    }
}
