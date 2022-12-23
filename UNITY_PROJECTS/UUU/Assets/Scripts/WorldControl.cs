using UnityEngine;
using System.Collections.Generic;

public class WorldControl : MonoBehaviour {
    public static WorldControl singleton;
    public GameObject[] InteractableMenus;
    public GameObject CurrentWorldMenu;
    public GameObject StatScreen;
    public GameObject LiveStatScreen;
    public int Level;
    public GameObject Unit;
    public System.Random RNG = new System.Random();
    public GameObject LanePreview;
    public Transform StatsLoc;
    public List<GameObject> WildUnits;
    public GameObject DismissCanvas;

    void Recruit()
    {
        PlayerScript.singleton.InteractableObject.ShowRecruitMentMenu();
    }

    void Scan()
    {

    }

    void Battle()
    {
        if(PlayerScript.singleton.Team.Count==10) 
            BattleScript.singleton.StartBattle();
    }

    void Cancel()
    {

    }

    void Confirm()
    {

    }

    public void Preview()
    {
        Camera.main.transform.SetParent(null);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        PlayerScript.singleton.isMoving = false;
        //PlayerScript.singleton.transform.position = new Vector3(-10, -10, -10);
        Instantiate(BattleScript.singleton.LanesBG);
        foreach (UnitStats U in PlayerScript.singleton.Team)
        {
            GameObject g = Instantiate(Unit) as GameObject;
            BehaviourScript B = g.GetComponent<BehaviourScript>();
            B.SetStats(U);
            BattleScript.singleton.PlaceUnit(B);
            BattleScript.singleton.Units.Add(B);
        }
        Instantiate(BattleScript.singleton.SwapCanvas);
    }

    public void ExitPreview()
    {
        Camera.main.transform.SetParent(PlayerScript.singleton.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        //PlayerScript.singleton.transform.position = Vector3.zero;
        PlayerScript.singleton.isMoving = true;
        Destroy(GameObject.FindGameObjectWithTag("LanesBG"));
        foreach (BehaviourScript b in BattleScript.singleton.Units)
            if (b != null)
                Destroy(b.gameObject);
        BattleScript.singleton.Units.Clear();
    }

    public void Dismiss()
    {
        Camera.main.transform.SetParent(null);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        PlayerScript.singleton.isMoving = false;
        //PlayerScript.singleton.transform.position = new Vector3(-10, -10, -10);
        Instantiate(BattleScript.singleton.LanesBG);
        foreach (UnitStats U in PlayerScript.singleton.Team)
        {
            GameObject g = Instantiate(Unit) as GameObject;
            BehaviourScript B = g.GetComponent<BehaviourScript>();
            B.SetStats(U);
            BattleScript.singleton.PlaceUnit(B);
            BattleScript.singleton.Units.Add(B);
        }
        Instantiate(DismissCanvas);
    }

    public void ExitDismiss()
    {
        Camera.main.transform.SetParent(PlayerScript.singleton.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        //PlayerScript.singleton.transform.position = Vector3.zero;
        PlayerScript.singleton.isMoving = true;
        Destroy(GameObject.FindGameObjectWithTag("LanesBG"));
        foreach (BehaviourScript b in BattleScript.singleton.Units)
            if (b != null)
                Destroy(b.gameObject);
        BattleScript.singleton.Units.Clear();
    }

    void AddUnit(bool IgnoreCondition)
    {
        if (IgnoreCondition || PlayerScript.singleton.InteractableObject.ConditionMet())
        {
            PlayerScript.singleton.Team.Add(new UnitStats(PlayerScript.singleton.InteractableObject.GetComponent<BehaviourScript>()));
            PlayerScript.singleton.HandleTeamAddition();
            Destroy(PlayerScript.singleton.InteractableObject.gameObject);
        }
    }

    void AddUnit()
    {
        if (PlayerScript.singleton.InteractableObject.ConditionMet())
        {
            PlayerScript.singleton.Team.Add(new UnitStats(PlayerScript.singleton.InteractableObject.GetComponent<BehaviourScript>()));
            PlayerScript.singleton.HandleTeamAddition();
            Destroy(PlayerScript.singleton.InteractableObject.gameObject);
        }
    }

    void DeclineUnit()
    {
        if (PlayerScript.singleton.InteractableObject.NowOrNever)
            Destroy(PlayerScript.singleton.InteractableObject.gameObject);
    }

    public GameObject GenerateUnit()
    {
        int x = RNG.Next(0, 14);
        int y = RNG.Next(0, 11);
        while(x<7 && y<6)
        {
            x = RNG.Next(0, 14);
            y = RNG.Next(0, 11);
        }
        if (RNG.Next(2) == 0)
            x *= -1;
        if (RNG.Next(2) == 0)
            y *= -1;
        GameObject g=Instantiate(Unit, new Vector2(x, y), Quaternion.identity) as GameObject;

        int r= RNG.Next(1, InteractScript.DescriptionCount);
        if (RNG.Next(3) == 0)
            r = 1;//Buy
        g.GetComponent<InteractScript>().DescriptionID = r;
        if(r==2) //trade
            g.GetComponent<InteractScript>().UnitID = RNG.Next(10);
        else if(r==3) //need X of Type
        {
            g.GetComponent<InteractScript>().UnitID = RNG.Next(3);
            int va= RNG.Next(2, 6);
            int vb= RNG.Next(2, 6);
            if (vb < va)
                va = vb;
            g.GetComponent<InteractScript>().Value = va;
        }
        else if(r==4) //need 0 of Type
            g.GetComponent<InteractScript>().UnitID = RNG.Next(3);
        g.GetComponent<BehaviourScript>().SpawnStats();
        return g;
    }

    void LevelWilds()
    {
        foreach (GameObject g in WildUnits)
            g.GetComponent<BehaviourScript>().LvlUp(Level);
    }

    public void NextLvl()
    {
        foreach (GameObject g in WildUnits)
            if (g != null)
                Destroy(g);
        WildUnits.Clear();
        Level++;
        int r = RNG.Next(10, 21);
        if (Level == 1)
            r = 25;
        for(int i=0;i<r;i++)
        {
            WildUnits.Add(GenerateUnit());
        }
        LevelWilds();
    }

    public string RandomName()
    {
        const string constanants = "bcdfghjklmnpqrstvwxyz";
        const string vowels = "aeiou";
        string[] strings = new string[2] { constanants, vowels };
        string N = "";
        int L=RNG.Next(2,7);
        int L2 = RNG.Next(2, 7);
        if (L < L2)
            L = L2;
        int off = 0;
        if (RNG.Next(10) == 4)
            off = 1;
        for (int i = 0; i < L; i++)
        {
            N += OneOrTwoChar(strings[(i+off) % 2]);
        }
        N=N[0].ToString().ToUpper()+N.Remove(0, 1);
        return N;
    }

    string OneOrTwoChar(string s)
    {
        string n = s[RNG.Next(s.Length)].ToString();
        if (RNG.Next(3) == 2)
        {
            n+= s[RNG.Next(s.Length)].ToString();
        }
        return n;
    }

    private void Awake()
    {
        singleton = this;

    }
    // Use this for initialization
    void Start () {
        NextLvl();
      
        /*
	for(int i=0;i<10;i++)
        {
            //PlayerScript.singleton.InteractableObject=GenerateUnit().GetComponent<InteractScript>();
            //AddUnit(true);
        }
        */
	}
	
	// Update is called once per frame
	void Update () {
    }
}
