using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject tile;
    public System.Random RNG;
    public List<Sprite> Icons = new List<Sprite> { };
    bool OnDarkTile;
    public Sprite DarkTile;
    public SpellScript CurrentSpell;
    public List<GameObject> SpellBook=new List<GameObject> { };
    public List<GameObject> AllSpells = new List<GameObject> { };
    public GameObject SpellOutline;
    public int SpellIndex;
    public Vector2 SpellLoc;
    public GameObject Page;
    public GameObject Reward;
    public GameObject Curse;
    public Button Forward;
    public Button Backward;
    public Button Reset;
    public GameObject fizzle;

    void NextSpell()
    {
        SpellIndex++;
        UpdateSpell();
    }

    void PrevSpell()
    {
        SpellIndex--;
        UpdateSpell();
    }

    public void UpdateSpell()
    {
        if (SpellBook.Count == 0)
        {
            GameObject go=Instantiate(fizzle,SpellLoc,Quaternion.identity)as GameObject;
            go.GetComponent<SpellScript>().GC = this;
            SpellBook.Add(go);
        }
        if (SpellIndex >= SpellBook.Count)
            SpellIndex = 0;
        else if (SpellIndex < 0)
            SpellIndex = SpellBook.Count - 1;
        if(CurrentSpell!=null)
            CurrentSpell.gameObject.transform.position=new Vector3(-10,-10,-10);
        SpellBook[SpellIndex].transform.position=SpellLoc;
        CurrentSpell = SpellBook[SpellIndex].GetComponent<SpellScript>();
    }

    public GameObject GenSpell(int S, int Starter)
    {
        GameObject go=Instantiate(Page, new Vector3(-10, -10, -10), Quaternion.identity) as GameObject;
        SpellScript ss = go.GetComponent<SpellScript>();
        go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Icons[S];
        if (S > 1)
        {
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
        }
        else
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        ss.Spell = AllSpells[S];
        ss.SpellIndex = S;
        ss.GC = this;
        int r = RNG.Next(20);
        if (Starter == -1)
            r = -1;
        List<Vector2> Vecs = new List<Vector2> { };
        switch(r)
        {
            case -1:
                Vecs.Add(new Vector2(RNG.Next(4), RNG.Next(4)));
                break;
            case 0:
                Vecs.Add(new Vector2(1,0));
                Vecs.Add(new Vector2(1, 1));
                Vecs.Add(new Vector2(1, 2));
                Vecs.Add(new Vector2(1, 3));
                break;
            case 1:
                Vecs.Add(new Vector2(0, 2));
                Vecs.Add(new Vector2(1, 2));
                Vecs.Add(new Vector2(2, 2));
                Vecs.Add(new Vector2(3, 2));
                break;
            case 2:
                Vecs.Add(new Vector2(2, 2));
                Vecs.Add(new Vector2(1, 2));
                Vecs.Add(new Vector2(1, 1));
                Vecs.Add(new Vector2(2, 1));
                break;
            case 3:
                Vecs.Add(new Vector2(1, 1));
                Vecs.Add(new Vector2(2, 2));
                break;
            case 4:
                Vecs.Add(new Vector2(2, 2));
                Vecs.Add(new Vector2(1, 1));
                break;
            default:
                List<Vector2> PossibleV = new List<Vector2> { };
                for(int i=0;i<4;i++)
                {
                    for(int j=0;j<4;j++)
                    {
                        PossibleV.Add(new Vector2(i,j));
                    }
                }
                int O = RNG.Next(1, 4);
                for (int i = 0; i < O; i++)
                {
                    int R = RNG.Next(PossibleV.Count);
                    Vecs.Add(PossibleV[R]);
                    PossibleV.RemoveAt(R);
                }
                break;
        }
        r = RNG.Next(Vecs.Count);
        Vector2 tPos = (Vector2)go.transform.GetChild(1).position + new Vector2(.25f, .25f) + .5f * Vecs[r];
        go.transform.GetChild(1).GetChild(0).position=tPos;
        foreach (Vector2 V in Vecs)
        {
            Vector2 Pos = (Vector2)go.transform.GetChild(1).position + new Vector2(.25f, .25f) + .5f * V;
            Instantiate(SpellOutline, Pos, Quaternion.identity, go.transform);
            ss.AOE.Add(2*(Pos - tPos));
        }
        return go;
    }

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        Forward.onClick.AddListener(delegate { NextSpell(); });
        Backward.onClick.AddListener(delegate { PrevSpell(); });
        Reset.onClick.AddListener(delegate { Application.LoadLevel(0); });
        for (int i=0;i<4;i++)
            SpellBook.Add(GenSpell(i, -1));
        CurrentSpell = SpellBook[0].GetComponent<SpellScript>();
        for (int i=0;i<8;i++)
        {
            OnDarkTile = !OnDarkTile;
            for (int j = 0; j < 8; j++)
            {
                GameObject go = Instantiate(tile, new Vector2(i, j) + (Vector2)transform.position, Quaternion.identity) as GameObject;
                TileScript ts = go.GetComponent<TileScript>();
                ts.GC = this;
                List<int> pOrder = new List<int> { 0, 1, 2, 3 };
                List<int> Order = new List<int> {};
                for (int a = 0; a < 4; a++)
                {
                    int r = RNG.Next(pOrder.Count);
                    Order.Add(pOrder[r]);
                    pOrder.RemoveAt(r);
                }
                for(int k=0;k<4;k++)
                {
                    ts.Chances[Order[k]] = 75 - 20 * k;
                    go.transform.GetChild(1+k).GetComponent<SpriteRenderer>().sprite = Icons[Order[k]];
                    if(Order[k]>1)
                        go.transform.GetChild(1 + k).GetComponent<SpriteRenderer>().color= new Color(1, 1, 1, .5f);
                }
                ts.rewardIndex = RNG.Next(4);
                if(OnDarkTile)
                {
                    go.GetComponent<SpriteRenderer>().sprite = DarkTile;
                }
                OnDarkTile = !OnDarkTile;
            }
        }
        UpdateSpell();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
