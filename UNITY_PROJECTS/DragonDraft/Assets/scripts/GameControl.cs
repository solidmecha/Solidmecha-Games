using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {


    public GameObject Dragon;
    public GameObject StatWindowPrefab;
    public GameObject CurrentStatWindow;
    public GameObject BanSymbol;
    public List<List<Vector2>> PlayerPoints=new List<List<Vector2>> { };
    public List<DragonScript> Player1_Dragons = new List<DragonScript> { };
    public List<DragonScript> Player2_Dragons = new List<DragonScript> { };
    public List<List<DragonScript>> Dragons = new List<List<DragonScript>> { };
    public bool isEoT;
    public bool inDraftMode;
    public int AbilityCount;
    public List<Sprite> Sprites = new List<Sprite> { };
    public List<GameObject> DragonPool = new List<GameObject> { };
    public int roundNumber;
    public int ActivePlayerID;
    public GameObject plusOne;
    public GameObject BattleCanvas;
    public GameObject ContinueCanvas;
    GameObject CurrentBattleCanvas;
    public List<Action> p1Actions=new List<Action> { };
    public List<Action> p2Actions=new List<Action> { };
    public List<GameObject> WinMarkers=new List<GameObject> { };

	// Use this for initialization
	void Start () {
        List<Vector2> p1 = new List<Vector2> { new Vector2(-6.3f, -4), new Vector2(-4.3f, -4) , new Vector2(-2.3f, -4) , new Vector2(-.3f, -4) , new Vector2(1.7f, -4)};
        List<Vector2> p2 = new List<Vector2> { new Vector2(-6.3f, 4), new Vector2(-4.3f, 4), new Vector2(-2.3f, 4), new Vector2(-.3f, 4), new Vector2(1.75f, 4) };
        PlayerPoints.Add(p1);
        PlayerPoints.Add(p2);
        Dragons.Add(Player1_Dragons);
        Dragons.Add(Player2_Dragons);
        GenerateDragons();
        inDraftMode = true;
        roundNumber = 1;
	}

    public void SetDraftState()
    {
        switch(roundNumber)
        {
            case 7: ActivePlayerID = 0;
                break;
            case 8:
                ActivePlayerID = 1;
                break;
            case 9:
                ActivePlayerID = 1;
                break;
            case 10:
                ActivePlayerID = 0;
                break;
            case 11:
                ActivePlayerID = 0;
                break;
            case 12:
                ActivePlayerID = 1;
                break;
            case 13:
                ActivePlayerID = 1;
                break;
            case 14:
                ActivePlayerID = 0;
                break;
            case 15:
                ActivePlayerID = 0;
                break;
            case 16:
                ActivePlayerID = 1;
                break;
            case 17:
                inDraftMode = false;
                foreach(GameObject D in DragonPool)
                {
                    DragonScript ds = (DragonScript)D.GetComponent(typeof(DragonScript));
                    if (!Player1_Dragons.Contains(ds) && !Player2_Dragons.Contains(ds))
                        Destroy(D);
                }
                roundNumber = 0;
                BeginBattle();
                break;

        }
    }

    void BeginBattle()
    {
        if (roundNumber != 0)
        {
            foreach (GameObject g in WinMarkers)
                Destroy(g);
            if (CurrentBattleCanvas != null)
                Destroy(CurrentBattleCanvas);
            isEoT = true;
            foreach (Action A in p1Actions)
                A();
            foreach (Action B in p2Actions)
                B();
            p1Actions.Clear();
            p2Actions.Clear();
            isEoT = false;
            Vector3 Pos = Dragons[1][0].transform.position;
            for(int i=0;i<4;i++)
            {
                Dragons[1][i].transform.position = Dragons[1][i + 1].transform.position;
            }
            Dragons[1][4].transform.position = Pos;
            DragonScript DS = Dragons[1][4];
            Dragons[1].RemoveAt(4);
            Dragons[1].Insert(0, DS);
        }
        CurrentBattleCanvas = Instantiate(BattleCanvas) as GameObject;
        CurrentBattleCanvas.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { determineRound(); });

    }

    void determineRound()
    {
        Destroy(CurrentBattleCanvas);
        int p2WinCount=0;
        foreach(Action A in p1Actions)     
            A();        
        foreach (Action B in p2Actions)
            B();
        for(int i=0;i<5;i++)
        {
            int W=determineFight(Dragons[0][i], Dragons[1][i]);
            Vector2 V = Dragons[W][i].transform.position;
            WinMarkers.Add(Instantiate(plusOne, V, Quaternion.identity) as GameObject);
            p2WinCount += W;
        }
        roundNumber++;
        CurrentBattleCanvas = Instantiate(ContinueCanvas) as GameObject;
        CurrentBattleCanvas.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { BeginBattle(); });
    }

    int determineFight(DragonScript ds1, DragonScript ds2) //0 if player 1 wins, 1 if player 2 
    {
        float Temp_ds1DPS= ds1.DPS;
        float Temp_ds2DPS=ds2.DPS;
        if (ds1.StrongEleID == ds2.elementID)
        { Temp_ds1DPS *= ds1.StrongMultiplier; }
        if(ds1.elementID==ds2.WeakEleID)
        { Temp_ds1DPS *= ds2.WeakMultiplier; }
        if (ds2.StrongEleID==ds1.elementID)
        { Temp_ds2DPS *= ds2.StrongMultiplier; }
        if (ds2.elementID == ds1.WeakEleID)
        { Temp_ds2DPS *= ds1.WeakMultiplier; }
        float t1 = ds1.HP / Temp_ds2DPS;
        float t2 = ds2.HP / Temp_ds1DPS;
        if (t2 > t1)
            return 1;
        else
            return 0;
    }

    void GenerateDragons()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        for (int i = 0; i < 17; i++)
        {
            GameObject D = Instantiate(Dragon, transform.GetChild(i).position, Quaternion.identity) as GameObject;
            DragonScript ds = (DragonScript)D.GetComponent(typeof(DragonScript));
            ds.GC = this;
            ds.HP = RNG.Next(500, 1001);
            ds.DPS = RNG.Next(50, 101);
            ds.AbilityID = RNG.Next(AbilityCount);
            DragonPool.Add(D);
        }
        setElements();
        setElementStrong();
        setElementWeak();

        for (int i = 0; i < 17; i++)
        {
            DragonScript ds = (DragonScript)DragonPool[i].GetComponent(typeof(DragonScript));
            DragonPool[i].GetComponent<SpriteRenderer>().sprite = Sprites[ds.elementID];
            ds.gameObject.AddComponent<BoxCollider2D>();
            if (ds.elementID !=1)
            {
                DragonPool[i].transform.localScale = new Vector2(1.6f, 1.6f);
            }
            
        }
    }

    void setElements()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        List<int> IndexList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int r = RNG.Next(IndexList.Count);
                DragonScript dst = (DragonScript)DragonPool[IndexList[r]].GetComponent(typeof(DragonScript));
                dst.elementID = i;
                IndexList.Remove(IndexList[r]);
            }
        }
        DragonScript dsrt = (DragonScript)DragonPool[IndexList[0]].GetComponent(typeof(DragonScript));
        dsrt.elementID = RNG.Next(4);
    }

    void setElementWeak()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        List<int> IndexList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int r = RNG.Next(IndexList.Count);
                DragonScript dst = (DragonScript)DragonPool[IndexList[r]].GetComponent(typeof(DragonScript));
                dst.WeakEleID = i;
                dst.WeakMultiplier = (float)RNG.Next(11, 21) / (float)10;
                IndexList.Remove(IndexList[r]);
            }
        }
        DragonScript dsrt = (DragonScript)DragonPool[IndexList[0]].GetComponent(typeof(DragonScript));
        dsrt.WeakEleID = RNG.Next(4);
        dsrt.WeakMultiplier = (float)RNG.Next(11, 21) / (float)10;
    }

    void setElementStrong()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        List<int> IndexList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int r = RNG.Next(IndexList.Count);
                DragonScript dst = (DragonScript)DragonPool[IndexList[r]].GetComponent(typeof(DragonScript));
                dst.StrongEleID = i;
                dst.StrongMultiplier = (float)RNG.Next(11, 21) / (float)10;
                IndexList.Remove(IndexList[r]);
            }
        }
        DragonScript dsrt = (DragonScript)DragonPool[IndexList[0]].GetComponent(typeof(DragonScript));
        dsrt.StrongEleID = RNG.Next(4);
        dsrt.StrongMultiplier = (float)RNG.Next(11, 21) / (float)10;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
