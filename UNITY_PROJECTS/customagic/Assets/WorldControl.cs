using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldControl : MonoBehaviour {

    public static WorldControl singleton;
    public System.Random RNG;
    public Slider[] Sliders;
    int[] Values;
    int[] Costs;
    public GameObject Player;
    public GameObject StartCanvas;
    public int Money;
    public Text MoneyText;
    public GameObject[] Icons;
    public GameObject Platform;
    public GameObject Spikes;
    public GameObject ActivePlayer;
    public GameObject[] Weap;
    public Transform[] ports;
    public Transform[] UIElements;
    public Sprite[] WeaponSprites;
    public GameObject Crystal;
    public GameObject[] Spawners;
    public List<Vector2> EnemyCrystalSpawns=new List<Vector2> { };
    public bool SpawningEnemies;
    float EnemyCounter;
    public float ESpawnDelay;
    public List<Vector2[]> PossiblePaths;
    public Text MsgText;
    public Text[] CostText;

    // Use this for initialization
    void Start() {
        singleton = this;
        RNG = new System.Random();
        PossiblePaths = new List<Vector2[]> { };
        Costs = new int[8];
        Values = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0};
        Money = RNG.Next(900, 2300);
        Costs[0] = RNG.Next(50, 300);
        Costs[1] = RNG.Next(50, 300);
        Costs[2] = RNG.Next(50, 300);
        Costs[3] = RNG.Next(50, 300);
        Costs[4] = RNG.Next(50, 300);
        Costs[5] = RNG.Next(5, 30);
        Costs[6] = RNG.Next(50, 300);
        Costs[7] = RNG.Next(3, 15);
        for(int i=0;i<8;i++)
        {
            CostText[i].text = "(" + Costs[i].ToString() + ")";
        }
        MoneyText.text = Money.ToString();
    }
      
    
    void GeneratePath(Vector2 Start, Vector2 End) //crystals
    {
        int StepCount = ((int)(End - Start).magnitude)/5 +2;
        Vector2[] P = new Vector2[StepCount];
        P[0] = Start;
        P[1] = Start + (End - Start).normalized*5f+new Vector2(RNG.Next(-2,3), RNG.Next(-2,3));
        for(int i=2;i<StepCount;i++)
        {
            Vector2 v;
            if (i==StepCount-1)
            {
                v = End;
            }
            else
                v=P[i-1]+(End-P[i-1]).normalized*5f+ new Vector2(RNG.Next(-3, 4), RNG.Next(-3, 4));
            P[i] = v;
        }
        PossiblePaths.Add(P);
    }

    void GeneratePatternPath(Vector2 Start, Vector2 End, PatternMovement P)
    {
        int StepCount = ((int)(End - Start).magnitude) + 2;
        P.Path = new Vector2[StepCount];
        P.Path[0] = Start;
        P.Path[1] = Start + (End - Start).normalized + new Vector2(RNG.Next(-2, 3), RNG.Next(-2, 3));
        for (int i = 2; i < StepCount; i++)
        {
            Vector2 v;
            if (i == StepCount - 1)
            {
                v = End;
            }
            else
                v = P.Path[i - 1] + (End - P.Path[i - 1]).normalized + new Vector2(RNG.Next(-3, 4), RNG.Next(-3, 4));
            P.Path[i] = v;
        }
    }

    void SpawnEnemy()
    {
        for(int i=0; i<EnemyCrystalSpawns.Count;i++)
        {
            WeaponScript w = (Instantiate(Weap[RNG.Next(Weap.Length)], EnemyCrystalSpawns[i], Quaternion.identity) as GameObject).transform.GetChild(0).GetComponent<WeaponScript>();
            w.hostile = true;
            w.GetComponent<SpriteRenderer>().color = Color.red;
            w.gameObject.AddComponent<BotScript>().PathIndex =i*4+RNG.Next(4);
        }
    }

    public void StartSpawningEnemies()
    {
        foreach(Vector2 ec in EnemyCrystalSpawns)
        {
            foreach(Vector2 pc in GetComponent<WorldSpawnerScript>().CrysLocs)
            {
                GeneratePath(ec, pc);
            }
        }
        SpawningEnemies = true;
    }

    public void ClearCrystal(Vector2 V)
    {
        int index = 0;
        for(int i=0; i<EnemyCrystalSpawns.Count; i++)
        {
            if ((EnemyCrystalSpawns[i] - V).sqrMagnitude < .5f)
            {
                index = i;
                break;
            }
        }
        PossiblePaths.RemoveRange(index * 4, 4);
        EnemyCrystalSpawns.RemoveAt(index);
    }

    public void ShowMessage(string s, float time)
    {
        MsgText.text = s;
        CancelInvoke();
        Invoke("ClearMessage", time);
    }

    public void ClearMessage()
    {
        MsgText.text = "";
    }

    public void Reset()
    {
        Application.LoadLevel(0);
    }

    public void HandleSlideChange(int i)
    {
        if(Values[i]<Sliders[i].value && Money > Costs[i]*((int)Sliders[i].value-Values[i]))
        {
            Money -= (Costs[i] * ((int)Sliders[i].value - Values[i]));
            Values[i] = (int)Sliders[i].value;
            MoneyText.text = Money.ToString();
        }
        else if(Values[i] < Sliders[i].value)
        {
            Sliders[i].value = Values[i];
        }
        if(Values[i]>Sliders[i].value)
        {
            Money += (Costs[i] * (Values[i] - (int)Sliders[i].value));
            Values[i] = (int)Sliders[i].value;
            MoneyText.text = Money.ToString();
        }

    }

    public void GameOver()
    {
        ShowMessage("Watch your health. Game Over! Restarting in 3...2..1.", 3f);
        Invoke("Reset", 3f);
    }

    public void StartGame()
    {
        Vector2 RoomStart = new Vector2(RNG.Next(4) * 17.14f, RNG.Next(5) * 9.87f);
        ActivePlayer=Instantiate(Player, new Vector2(-7.9f, -2f)+RoomStart, Quaternion.identity) as GameObject;
        ports[0].transform.position = RoomStart + new Vector2(-7.3f, .87f);
        ports[1].transform.position = RoomStart + new Vector2(7.2f, .87f);
        CharControl C = ActivePlayer.GetComponent<CharControl>();
        C.speed[1] = 2+Sliders[0].value;
        C.JumpCharges[1] += (int)Sliders[1].value;
        C.ProjSize = new Vector2(.55f+.25f*Sliders[2].value, .55f+.25f*Sliders[2].value);
        C.ProjSpeed = 1.5f+Sliders[3].value;
        C.exploSize = Sliders[4].value * 1.5f;
        C.Dmg = 10+(int)Sliders[5].value;
        C.FireDelay = 2.15f - .35f * Sliders[6].value;
        C.HP[1] = 100 + (int)Sliders[7].value;
        C.HP[0] = 100 + (int)Sliders[7].value;
        C.ReadyIcons[0] = (Instantiate(Icons[0], Camera.main.transform) as GameObject).GetComponent<SpriteRenderer>();
        for (int i = 0; i < C.JumpCharges[1]; i++)
        {
            C.ReadyIcons[i + 1] = (Instantiate(Icons[1], Camera.main.transform) as GameObject).GetComponent<SpriteRenderer>();
            C.ReadyIcons[i + 1].transform.localPosition = C.ReadyIcons[i + 1].transform.localPosition + new Vector3(-.5f* i,0,10f);
        }
        Destroy(StartCanvas);
        ShowMessage("Attune the four Crystals.", 4f);
        Camera.main.GetComponent<CameraControl>().Snap(RoomStart);
        for (int i=0;i<50;i++)
            SpawnOpponents();
    }

    public void SpawnOpponents()
    {
        Vector2 Start = new Vector2(RNG.Next(4) * 17.14f, RNG.Next(5) * 9.87f);
        Vector2 End=Start+ new Vector2(RNG.Next(-6, 7), RNG.Next(-4, 4));
        Start += new Vector2(RNG.Next(-6, 7), RNG.Next(-4, 4));
        WeaponScript w=(Instantiate(Weap[RNG.Next(Weap.Length)], Start, Quaternion.identity)as GameObject).transform.GetChild(0).GetComponent<WeaponScript>();
        w.hostile = true;
        w.GetComponent<SpriteRenderer>().color = Color.red;
        GeneratePatternPath(Start, End,w.gameObject.AddComponent<PatternMovement>());
        w.currentState = WeaponScript.state.pattern;
    }
	
	// Update is called once per frame
	void Update () {
        if(SpawningEnemies)
        {
            EnemyCounter -= Time.deltaTime;
            if(EnemyCounter<=0)
            {
                EnemyCounter = ESpawnDelay;
                SpawnEnemy();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	}
}
