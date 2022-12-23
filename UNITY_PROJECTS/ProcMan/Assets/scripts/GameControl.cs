using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
    public int width=21;
    public int height=17;
    public GameObject Tile;
    public GameObject Pip;
    public GameObject SuperPip;
    public GameObject Player;
    public GameObject[][] World;
    public List<int[]> IntersectionList = new List<int[]> { };
    public List<GameObject> Chasers = new List<GameObject> { };
    public System.Random RNG = new System.Random();
    public Button newGame;
    public int pipCount = 0;

    // Use this for initialization
    void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        World =new GameObject[width][];
        newGame.onClick.AddListener(delegate { Application.LoadLevel(0); });
        BuildWorld();
        
	}


    void PlacePips(List<int[]> LIA) //place pips, enemies and player
    {
        foreach (int[] ia in LIA)
        {
            Instantiate(Pip, World[ia[0]][ia[1]].transform.position, Quaternion.identity);
            pipCount++;
        }
        Instantiate(SuperPip, World[1][1].transform.position, Quaternion.identity);
        Instantiate(SuperPip, World[1][height-2].transform.position, Quaternion.identity);
        Instantiate(SuperPip, World[width-2][1].transform.position, Quaternion.identity);
        Instantiate(SuperPip, World[width-2][height-2].transform.position, Quaternion.identity);
        int x = 1;
        int y = 1;
        do
        {
            int r = RNG.Next(LIA.Count);
            x=LIA[r][0];
            y= LIA[r][1];
        }
        while ((x == 1 && y == 1) || (x == 1 && y == height - 2) || (x == width - 2 && y == 1) || (x == width - 2 && y == height - 2)) ;
        Player.transform.position = World[x][y].transform.position;
            for(int i=0;i<8;i++)
            {
            do
            {
                int r = RNG.Next(LIA.Count);
                Chasers[i].transform.position = World[LIA[r][0]][LIA[r][1]].transform.position;
            }
            while (Vector2.Distance(Chasers[i].transform.position, Player.transform.position) < 5);
            }

        for (int i = 0; i < 4; i++)
        {
            if (RNG.Next(2) == 0)
            {
                Destroy(Chasers[i]);
                Chasers.Remove(Chasers[i]);
            }
        }

            foreach(GameObject C in Chasers)
        {
            AIscript ais = (AIscript)C.GetComponent(typeof(AIscript));
            ais.dirIndex = RNG.Next(4);
            ais.SetDirection();
        }
        
    }
	
    void BuildWorld()
    {
        for(int i=0;i<width;i++)
        {
            World[i] = new GameObject[height];
            for(int j=0;j<height;j++)
            {
                GameObject go=Instantiate(Tile, new Vector2(i, j) + (Vector2)transform.position, Quaternion.identity) as GameObject;
                World[i][j] = go;
                if (i!=0 && i!=width-1 && j != 0 && j != height-1)
                {
                    if(i%2 == 0 && j%2==0)
                    {
                        go.GetComponent<SpriteRenderer>().color=Color.black;
                    }
                }
                else if(i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }

        for (int i = 1; i < width; i+=2)
        {
            for (int j = 1; j < height; j+=2)
            {
                int r = RNG.Next(4);
                if(r>3)
                {
                    r -= RNG.Next(1,3);
                }
                List<GameObject> TempList = new List<GameObject> { };
                    if (i - 1 >= 0)
                    { TempList.Add(World[i - 1][j]); }
                    if (i + 1 <= width - 1)
                    { TempList.Add(World[i + 1][j]); }
                    if (j - 1 >= 0)
                    { TempList.Add(World[i][j - 1]); }
                    if (j + 1 <= height - 1)
                    { TempList.Add(World[i][j+1]); }

                for (int k=0;k< r;k++)
                {
                    TempList[RNG.Next(TempList.Count)].GetComponent<SpriteRenderer>().color = Color.black;
                    if (TempList.Count < 3)
                        break;
                }
            }
        }
        
        //dead ends
        for(int i=1;i<width;i++)
        {
            for(int j=1;j<height;j++)
            {
                if(!World[i][j].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                {
                    List<GameObject> Templist = new List<GameObject> { };
                    if (World[i + 1][j].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        Templist.Add(World[i + 1][j]);
                    if (World[i - 1][j].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        Templist.Add(World[i - 1][j]);
                    if (World[i][j + 1].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        Templist.Add(World[i][j + 1]);
                    if (World[i][j - 1].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        Templist.Add(World[i][j - 1]);
                    if(Templist.Count>=3)
                    {
                        int r = RNG.Next(Templist.Count - 2, Templist.Count);
                        if (j - 1 == 0 && Templist.Contains(World[i][j - 1]))
                            Templist.Remove(World[i][j - 1]);
                        if (j + 1 == height-1 && Templist.Contains(World[i][j + 1]))
                            Templist.Remove(World[i][j + 1]);
                        if (i - 1 == 0 && Templist.Contains(World[i - 1][j]))
                            Templist.Remove(World[i - 1][j]);
                        if (i + 1 == width - 1 && Templist.Contains(World[i + 1][j]))
                            Templist.Remove(World[i + 1][j]);                     
                        if (r >= Templist.Count)
                            r = Templist.Count;
                        for (int k = 0; k < r; k++)                      
                            Templist[k].GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }

            }
        }

        //checkConnectivity
        int WhiteCount=0;
        List<GameObject> ConnectedList = new List<GameObject> { };
        List<int[]> WorldLocs = new List<int[]> { };
        ConnectedList.Add(World[1][1]);
        WorldLocs.Add(new int[2] { 1, 1 });
        for(int s=0;s<ConnectedList.Count;s++)
        {
            ConnectList(ConnectedList, s, WorldLocs);
        }
        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                if (!World[i][j].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                {
                    WhiteCount++;
                    int c = 0;
                    if (!World[i+1][j].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        c++;
                    if (!World[i-1][j].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        c++;
                    if (!World[i][j+1].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        c++;
                    if (!World[i][j-1].GetComponent<SpriteRenderer>().color.Equals(Color.black))
                        c++;
                    if (c > 2)
                        IntersectionList.Add(new int[2] { i, j });

                }
            }
        }
        if (WhiteCount != ConnectedList.Count)
            Application.LoadLevel(0);
        PlacePips(WorldLocs);
}

    void ConnectList(List<GameObject> list, int index, List<int[]> locList)
    {
        GameObject curTile = list[index];
        int counter=1;
        while (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
        {
            curTile = World[locList[index][0]][locList[index][1] + counter];
            if(!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
            {
                if(!list.Contains(curTile))
                {
                    list.Add(curTile);
                    int[] ia = new int[2] { locList[index][0], locList[index][1] + counter };
                    locList.Add(ia);
                }
                counter++;
            }
        }

        curTile = list[index];
        counter = 1;
        while (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
        {
            curTile = World[locList[index][0]][locList[index][1] - counter];
            if (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
            {
                if (!list.Contains(curTile))
                {
                    list.Add(curTile);
                    int[] ia = new int[2] { locList[index][0], locList[index][1] - counter };
                    locList.Add(ia);
                }
                counter++;
            }
        }

        curTile = list[index];
        counter = 1;
        while (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
        {
            curTile = World[locList[index][0]+counter][locList[index][1]];
            if (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
            {
                if (!list.Contains(curTile))
                {
                    list.Add(curTile);
                    int[] ia = new int[2] { locList[index][0]+counter, locList[index][1]};
                    locList.Add(ia);
                }
                counter++;
            }
        }

        curTile = list[index];
        counter = 1;
        while (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
        {
            curTile = World[locList[index][0]-counter][locList[index][1]];
            if (!curTile.GetComponent<SpriteRenderer>().color.Equals(Color.black))
            {
                if (!list.Contains(curTile))
                {
                    list.Add(curTile);
                    int[] ia = new int[2] { locList[index][0]-counter, locList[index][1]};
                    locList.Add(ia);
                }
                counter++;
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
