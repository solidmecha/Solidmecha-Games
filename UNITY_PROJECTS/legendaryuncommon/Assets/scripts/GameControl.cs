using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
    public static GameControl singleton;
    public Sprite[] PlayerCards;
    public Sprite[] Legion;
    public Sprite[] Demons;
    public enum Gamestate { Playing, EnemyTurn, FX, Train, Quest, Fighting, Awaken, Raid, Win};
    public Gamestate CurrentState;
    public CardControl SelectedCard;
    public Text[] VirtueStats;
    public Text PreviewMsg;
    public Text SilverText;
    public Text NoticeTxt;
    public GameObject CardPreviewObj;
    public GameObject PlayerCardRoot;
    public int ActiveCardIndex;
    public GameObject PlayerCardOutline;
    public System.Random RNG;
    public bool ShowPreviews;
    public int silver;
    public GameObject ActionButtonObj;
    public string[] VirtueNames;
    int RaidIndex;
    public GameObject[] Raiders;
    public GameObject xThree;
    public GameObject DemonRoot;
    int TurnCount;
    public GameObject Card;
    public GameObject RaidbossRoot;
    public bool playing;
    public enum Goal { TotalSilver, GainedSilver, TotalPartyFourAttr, TotalPartyOneAttr, SingleAttr, DemonsDefeated,
    fullAwakening, TotalAwake, TotalQuests, TotalTrain, DefeatBoss};
    public int GainedSilver;
    public int[] HighestAttributes;
    public int[] CollectiveAttributes;
    public int DefeatCount;
    public int QuestCount;
    public int TrainCount;
    public List<MSQcontrol> MSQs;
    int BossCount;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        VirtueNames = new string[4] {"Wisdom", "Courage", "Charisma", "Kindness"};
    }

    public void CardClick(CardControl C)
    {
        if(CurrentState==Gamestate.Fighting && !C.PlayerOwned)
        {
            if (!C.RaidBoss)
            {
                ResolveFight(SelectedCard, C);
                GameControl.singleton.CurrentState = GameControl.Gamestate.FX;
                GameControl.singleton.AdvanceTurn(1);
            }
            else
            {
                CurrentState = Gamestate.Raid;
                PreviewMsg.text = "Select 3 other Cards to start the raid.";
                RaidIndex = 1;
                Raiders[0] = SelectedCard.gameObject;
                SelectedCard.ID = 0;
                SelectedCard = C;
            }
        }
        else if(CurrentState==Gamestate.Raid && C.PlayerOwned)
        {
            Raiders[RaidIndex] = C.gameObject;
            C.ID = RaidIndex;
            RaidIndex++;
            if (RaidIndex > 3)
                BeginRaid();
        }
    }

    public void BeginRaid()
    {
        CardPreviewObj.transform.position = new Vector3(-199, -199, -199);
        CurrentState = Gamestate.FX;
        SelectedCard.GetComponent<MoveScript>().StartMove(xThree.transform.position, 2);
        RotateIt R= Camera.main.gameObject.AddComponent<RotateIt>();
        R.rotation = new Vector3(-45f / 2f, 0);
        R.timer = 2;
        R = SelectedCard.gameObject.AddComponent<RotateIt>();
        R.rotation = new Vector3(-45f / 2f, 0);
        R.timer = 2;
        MoveScript m;
        foreach (GameObject g in Raiders)
        {
            R = g.AddComponent<RotateIt>();
            R.rotation = new Vector3(-45f/2f, 0);
            R.timer = 2;
            m=g.GetComponent<MoveScript>();
            m.StartMove(xThree.transform.GetChild(g.GetComponent<CardControl>().ID).position, 2);
        }
        Invoke("RaidFight", 2f);
        Invoke("EndRaid", 3f);
    }

    public void RaidFight()
    {
        foreach (GameObject g in Raiders)
        {
            ResolveFight(g.GetComponent<CardControl>(), SelectedCard);
            ResolveFight(SelectedCard, g.GetComponent<CardControl>());
        }
    }

    public void EndRaid()
    {

        CardPreviewObj.transform.position = new Vector3(5.82f, 1.81f, 0);
        Camera.main.transform.eulerAngles = Vector2.zero;
        if (SelectedCard != null)
        {
            SelectedCard.transform.eulerAngles = Vector2.zero;
            SelectedCard.transform.position = SelectedCard.GetComponent<MoveScript>().StartPos;
        }
        foreach (GameObject g in Raiders)
        {
            if (g != null && g.GetComponent<CardControl>().HP[0]>=0)
            {
                g.transform.eulerAngles = Vector2.zero;
                g.transform.position = g.GetComponent<MoveScript>().StartPos;
            }
        }
        PreviewMsg.text = "";
        CurrentState = Gamestate.FX;
        AdvanceTurn(1);
    }

    public void ResolveFight(CardControl atk, CardControl def)
    {
        if (atk.PlayerOwned)
        {
            int dmg = 0;
            int t = 0;
            for (int i = 0; i < 4; i++)
            {
                t = atk.Stats[i];
                if (def.WeaknessType[0] == i)
                    t /= 2;
                else if (def.WeaknessType[1] == i)
                    t *= 2;
                dmg += t;
            }
            def.TakeDmg(dmg);
            if (!def.RaidBoss)
                PreviewMsg.text = "";
        }
        else
        {
            if(atk.RaidBoss)
            {
                float p = RNG.Next(2, 13)/20f;
                def.TakeDmg((int)(p * def.HP[1]));
            }
            else
            {
                float p = RNG.Next(1, 9) / 20f;
                def.TakeDmg((int)(p * def.HP[1]));
            }
        }
    }

    public void MoveActions(Vector2 pos)
    {
        ActionButtonObj.transform.position = pos;
    }


    public void CardPreview(CardControl C)
    {
        if (C.PlayerOwned)
        {
            for (int i = 0; i < 4; i++)
                VirtueStats[i].text = C.Stats[i].ToString();
        }
        else
        {
            for(int i = 0; i < 4; i++)
                VirtueStats[i].text = "???";
            PreviewMsg.text = C.msg;
        }
            CardPreviewObj.GetComponent<SpriteRenderer>().sprite = C.GetComponent<SpriteRenderer>().sprite;
        GameObject fill = CardPreviewObj.transform.GetChild(0).GetChild(0).gameObject;
        fill.transform.localScale = new Vector2(15f * (float)C.HP[0] / (float)C.HP[1], 1);
        fill.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, (float)C.HP[0] / (float)C.HP[1]);
        //if(CurrentState==Gamestate.Fighting)

    }

    public void ShowSelectedCard()
    {
        CardPreview(SelectedCard);
    }

    void UpdateGoals(Goal g, int val)
    {
        foreach (MSQcontrol m in MSQs)
        {
            if (m.goal == Goal.TotalSilver)
                m.GoalCounter = silver;
            m.UpdateGoalCounter(g, val);
        }
    }

    public void MakePlayerCards()
    {
        List<int> PlayerIDs = new List<int> { };
        for(int i=0;i<6;i++)
        {
           CardControl c= PlayerCardRoot.transform.GetChild(i).GetComponent<CardControl>();
            c.Stats[0] = RNG.Next(0, 50);
            c.Stats[1] = RNG.Next(0, 50);
            c.Stats[2] = RNG.Next(0, 50);
            c.Stats[3] = RNG.Next(0, 50);
            int id=RNG.Next(PlayerCards.Length);
            while(PlayerIDs.Contains(id))
                id = RNG.Next(PlayerCards.Length);
            PlayerIDs.Add(id);
            c.GetComponent<SpriteRenderer>().sprite = PlayerCards[id];
        }
    }

    public void DemonSetup()
    {
       int n= RNG.Next(3, 6);
        for(int i=0;i<n;i++)
        {
            CreateDemon();
        }
    }

    public void AdvanceTurn(float delay)
    {
        if (playing)
        {
            ActiveCardIndex++;
            MoveActions(new Vector3(100, 100, 100));
            if (ActiveCardIndex >= PlayerCardRoot.transform.childCount)
                TakeEnemyTurn();
            else
            {
                SelectedCard = PlayerCardRoot.transform.GetChild(ActiveCardIndex).GetComponent<CardControl>();
                PlayerCardOutline.GetComponent<MoveScript>().StartMove(SelectedCard.transform.position, .5f);
                if (SelectedCard.HP[0] < 0)
                    AdvanceTurn(delay);
                else
                    Invoke("SetPlaying", delay);
            }
        }
            
    }

    public void DisplayNotice(string msg, float delay, Color c)
    {
        NoticeTxt.color = c;
        NoticeTxt.text = msg;
        Invoke("ClearNotice", delay);
    }

    public void ClearNotice()
    {
        NoticeTxt.text = "";
    }

    public void SetPlaying()
    {
        CurrentState = Gamestate.Playing;
        ShowSelectedCard();
        ShowPreviews = true;
        MoveActions(Vector2.zero);
    }

    public void TakeEnemyTurn()
    {
        TurnCount++;
        int DemonCount = DemonRoot.transform.childCount - 10;
        if (RNG.Next(1, 11) >= DemonCount)
        {
            int num = RNG.Next(1, 3);
            for (int i = 0; i < num; i++)
                CreateDemon();
            PreviewMsg.text = "New enemies spawned!";
        }
        else
        {
            foreach (CardControl c in DemonRoot.GetComponentsInChildren<CardControl>())
                ResolveFight(c, PlayerCardRoot.GetComponentsInChildren<CardControl>()[RNG.Next(PlayerCardRoot.transform.childCount)]);
            PreviewMsg.text = "The enemy attacked!";
        }
        ActiveCardIndex = -1;
        AdvanceTurn(1);
    }

    public void CreateDemon()
    {
        for(int i=0;i<10;i++)
        {
            if(!DemonRoot.transform.GetChild(i).GetComponent<DemonSpawner>().hasSpawned)
            {
                CardControl c = (Instantiate(Card, DemonRoot.transform.GetChild(i).position, Quaternion.identity) as GameObject).GetComponent<CardControl>();
                c.transform.SetParent(DemonRoot.transform);
                c.GetComponent<SpriteRenderer>().sprite = Demons[RNG.Next(Demons.Length)];
                c.WeaknessType[0] = RNG.Next(4);
                c.WeaknessType[1] = RNG.Next(4);
                c.msg = "Strong vs " + VirtueNames[c.WeaknessType[0]] + ". Weak vs " + VirtueNames[c.WeaknessType[1]] + ".";
                int h = RNG.Next((5 * TurnCount), (20 * TurnCount));
                c.HP[0] += h;
                c.HP[1] += h;
                c.ds = DemonRoot.transform.GetChild(i).GetComponent<DemonSpawner>();
                DemonRoot.transform.GetChild(i).GetComponent<DemonSpawner>().hasSpawned = true;
                c.PlayerOwned = false;
                i = 10;
            }
        }
    }

    public void CreateBoss(int B)
    {
        CardControl c = (Instantiate(Card, RaidbossRoot.transform.GetChild(B).position, Quaternion.identity) as GameObject).GetComponent<CardControl>();
        Destroy(RaidbossRoot.transform.GetChild(B).gameObject);
        c.RaidBoss = true;
        c.transform.localScale = new Vector2(.75f, .75f);
        c.ID = B;
        c.GetComponent<SpriteRenderer>().sprite = Legion[RNG.Next(Legion.Length)];
        c.WeaknessType[0] = RNG.Next(4);
        c.WeaknessType[1] = RNG.Next(4);
        c.msg = "Strong vs " + VirtueNames[c.WeaknessType[0]] + ". Weak vs " + VirtueNames[c.WeaknessType[1]] + ".";
        int h = 1000*(TurnCount+1);
        c.HP[0] += h;
        c.HP[1] += h;
    }

    public void ResolveBossClear(int B)
    {
        foreach(MSQcontrol m in MSQs)
        {
            if(m.goal==Goal.DefeatBoss && m.AttrIndex==B)
            {
                m.gameObject.transform.position=new Vector3(-100,-100,-100);
                BossCount++;
                if (BossCount == 6)
                    WinGame();
            }
        }
    }

    void WinGame()
    {
        CurrentState = Gamestate.Win;
        PreviewMsg.text = "Victory!";
    }

    public void UpdateSilver(int change)
    {

        silver += change;
        if (change > 0)
            UpdateGoals(Goal.GainedSilver, change);
        UpdateGoals(Goal.TotalSilver, silver);
        SilverText.text = silver.ToString();
    }

    // Use this for initialization
    void Start () {
        MakePlayerCards();
        DemonSetup();
        SelectedCard= PlayerCardRoot.transform.GetChild(0).GetComponent<CardControl>();
        ShowSelectedCard();
        CurrentState = Gamestate.Playing;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
