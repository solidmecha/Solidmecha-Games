using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
    public static GameControl singleton;
    public System.Random RNG;
    public GameObject Tile;
    public Sprite[] Shapes;
    public Sprite[] Patterns;
    public Sprite[] Numbers;
    public Color[] Colors;
    public GameObject Outline;
    public List<GameObject> Outlines;
    public List<TileScript> PotentialMatches;
    public TileScript ActiveTile;
    public int score;
    public UnityEngine.UI.Text ScoreText;
    public bool isMatching;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        isMatching = true;
    }

    // Use this for initialization
    void Start () {
        List<int[]> IDs = new List<int[]> { };
        for(int i=0;i<7;i++)
        {
            for(int j=0;j<7; j++)
            {
                IDs.Add(new int[4]);
            }
        }
        for(int i=0;i<7;i++)
        {
            bool[] isSame = new bool[4];
            isSame[0] = RNG.Next(6) == 1;
            isSame[1] = RNG.Next(6) == 1;
            isSame[2] = RNG.Next(6) == 1;
            isSame[3] = RNG.Next(6) == 1;
            for (int t = 0; t < 4; t++)
            {
                List<int> Vals = new List<int> { };
                for (int j = 0; j < 7; j++)
                {
                    if (j == 0)
                    {
                        if (isSame[t])
                        {
                            int v = RNG.Next(7);
                            for (int x = 0; x < 7; x++)
                                Vals.Add(v);
                        }
                        else
                        {
                            List<int> sev = new List<int> { 0, 1, 2, 3, 4, 5, 6};
                            for (int x = 0; x < 7; x++)
                            {
                                int q = sev[RNG.Next(sev.Count)];
                                Vals.Add(q);
                                sev.Remove(q);
                            }
                        }
                    }
                    IDs[i*7+j][t] = Vals[j];
                }

            }
        }
        for(int i=0;i<7;i++)
        {
            for(int j=0;j<7;j++)
            {
              TileScript t= (Instantiate(Tile, new Vector2(i, j) * 1.5f, Quaternion.identity) as GameObject).GetComponent<TileScript>();
                int index = RNG.Next(IDs.Count);
                SetID(t, IDs[index]);
                IDs.RemoveAt(index);
            }
        }
	}

    public void SetID(TileScript ts, int[] ia)
    {
        ts.IDs = new int[4];
        ts.IDs[0] = ia[0];
        ts.IDs[1] = ia[1];
        ts.IDs[2] = ia[2];
        ts.IDs[3] = ia[3];
    }

    void ClearOutlines()
    {
        foreach (GameObject g in Outlines)
            Destroy(g);
        Outlines.Clear();
    }

    void ResolveMatches()
    {
        int Matches = 0;
        for (int i = 0; i < 4; i++)
        {
            if (CheckMatch(i))
                Matches++;
        }
        if(Matches>1)
        {
            foreach (TileScript t in PotentialMatches)
                t.gameObject.AddComponent<FXScript>();
            UpdateScore(Matches);

        }
        else
        {
            UpdateScore(-9999);
        }
        PotentialMatches.Clear();
    }

    void UpdateScore(int m)
    {
        int s = m;
        if (m > 0)
        {
            for (int i = 0; i < PotentialMatches.Count; i++)
                s *= m;
        }
        score += s;
        ScoreText.text = score.ToString();
    }

    bool CheckMatch(int index)
    {
        bool isSame = true;
        bool isDiff = true;
        int c = PotentialMatches[0].IDs[index];
        List<int> Checklist = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        foreach(TileScript t in PotentialMatches)
        {
            if(c!=t.IDs[index] && isSame)
            {
                isSame = false;
            }
            else
            {
                c = t.IDs[index];
            }
            if (Checklist.Contains(t.IDs[index]) && isDiff)
            {
                Checklist.Remove(t.IDs[index]);
            }
            else
                isDiff = false;
        }
        return isSame || isDiff;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) && !isMatching)
        {
                ActiveTile.transform.position = ActiveTile.destination;
                ActiveTile.GetComponent<BoxCollider2D>().enabled = true;
                ActiveTile = null;
            isMatching = true;
        }   
        else if (Input.GetMouseButton(0) && !isMatching)
            ActiveTile.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if(Input.GetMouseButtonUp(1) && isMatching)
        {
            ClearOutlines();
            foreach (TileScript t in PotentialMatches)
                t.marked = false;
            if (PotentialMatches.Count > 3)
            {
                ResolveMatches();
            }
            else
            {
                PotentialMatches.Clear();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	
	}
}
