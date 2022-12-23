using UnityEngine;
using System.Collections.Generic;

public class ModuleControl : MonoBehaviour {

    Color mainColor;
    public List<Sprite> Icons;
    public GameObject tile;
    System.Random RNG=new System.Random();
    public GameObject empty;

    public GameObject CreateModule(int w, int h, int R)
    {
        bool[][] flags=new bool[w][];
        if (RNG.Next(19) == 4)
            R = 2;//5% chance of trimino;
        for(int i=0; i<w; i++)
        {
            flags[i] = new bool[h];

            for(int j=0;j<h;j++)
            {
                flags[i][j] = false;
            }
        }

        flags[w/2][h/2] = true;
        int[] CurrentNode = new int[2] { w/2, h/2 };
        List<int[]> PossiblePoints = new List<int[]> { };
        List<int[]> Points = new List<int[]> { new int[2] { w/2, h/2 } };
        while(R>0)
        {
            if (CurrentNode[0] + 1<w && !flags[CurrentNode[0]+1][CurrentNode[1]])
                PossiblePoints.Add(new int[2] { CurrentNode[0] + 1, CurrentNode[1]});
            if (CurrentNode[1] + 1 < h && !flags[CurrentNode[0]][CurrentNode[1]+1])
                PossiblePoints.Add(new int[2] { CurrentNode[0], CurrentNode[1]+1 });
            if (CurrentNode[0] - 1 >= 0 && !flags[CurrentNode[0] - 1][CurrentNode[1]])
                PossiblePoints.Add(new int[2] { CurrentNode[0] - 1, CurrentNode[1] });
            if (CurrentNode[1] - 1 >= 0 && !flags[CurrentNode[0]][CurrentNode[1]-1])
                PossiblePoints.Add(new int[2] { CurrentNode[0], CurrentNode[1]-1});
            if (PossiblePoints.Count == 0)
                CurrentNode = Points[RNG.Next(Points.Count)];
            else
            {
                int r = RNG.Next(PossiblePoints.Count);
                flags[PossiblePoints[r][0]][PossiblePoints[r][1]] = true;
                Points.Add(new int[2] { PossiblePoints[r][0], PossiblePoints[r][1] });
                CurrentNode = Points[RNG.Next(Points.Count)];
                R--;
                PossiblePoints.Clear();
            }

        }
        GameObject rootTile = Instantiate(empty);
        GameObject Outline = Instantiate(empty, rootTile.transform.position, Quaternion.identity, rootTile.transform) as GameObject;
        rootTile.transform.position = new Vector2(Mathf.Round(w / 2-.5f), Mathf.Round(h / 2-.5f));
        for (int i = 0; i < w; i++)
        {

            for (int j = 0; j < h; j++)
            {
                if(flags[i][j])
                {
                    GameObject go = Instantiate(tile, rootTile.transform) as GameObject;
                    go.transform.localPosition = new Vector2(i-w/2, j-h/2);
                    int r = RNG.Next(Icons.Count);
                    go.GetComponent<SpriteRenderer>().sprite = Icons[r];
                    go.GetComponent<NodeID>().ID = r;
                    if (i-1>=0 && flags[i - 1][j])
                        Destroy(go.transform.GetChild(2).gameObject);
                    if (i + 1 < w && flags[i + 1][j])
                        Destroy(go.transform.GetChild(0).gameObject);
                    if (j + 1 < h && flags[i][j+1])
                        Destroy(go.transform.GetChild(1).gameObject);
                    if (j - 1 >= 0 && flags[i][j - 1])
                        Destroy(go.transform.GetChild(3).gameObject);
                    for (int t = go.transform.childCount-1; t >=0; t--)
                        go.transform.GetChild(t).SetParent(Outline.transform);
                }
            }
        }
        Outline.transform.SetSiblingIndex(rootTile.transform.childCount - 1);
        GetComponent<GameControl>().Modules.Add(rootTile.transform);
        return rootTile;

    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 0; i++)
        {
            int x = RNG.Next(3, 4);
            int y = RNG.Next(3, 4);
            CreateModule(x, y, RNG.Next(4, 5));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
