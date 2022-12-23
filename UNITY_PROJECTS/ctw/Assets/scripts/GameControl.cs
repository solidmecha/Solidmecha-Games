using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public int ActionCount;
    public GameObject Road;
    System.Random RNG;
    public NodeScript ActivePlayerLocation;
    public GameObject[] Players;
    public Text ActionText;
    public Transform Outline;
    public List<GameObject> PlayerPrefabs;
    public GameObject HQ;
    public Text Description;
    public Text AbilityDescription;
    public Text ActionCounterTime;
    float ActionCounter;
    public float ActionCD;
    public Color[] Colors;
    public GameObject Germ;
    public int SpreadCount;
    public Button AbilityButton;
    public Button ProgressButton;
    public int HQcount;
    public GameObject BioHazard;
    public GameObject Book;
    public float InfectRate= 0.0125f;
    public float[] HealRates = new float[] { -0.0125f, -0.0125f, -0.0125f, -0.0125f };
    public bool[] Cures=new bool[4] { false, false, false, false };
    float epiCounter;
    public Text EpiText;

	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        ConnectTheWorld();
        while(!checkConnectivity())
        {
            ConnectTheWorld();
        }
        SetPlayers();
        BuildHQ(transform.GetChild(RNG.Next(32)).GetComponent<NodeScript>());
        UpdateActionText(0);
        int[] StarLocs = new int[] {0,0,0,0,0,0};
        while (!AllUnique(StarLocs))
        {
           for (int i = 0; i < 6; i++)
            {
                int r = RNG.Next(32);
                while(transform.GetChild(r).GetComponent<NodeScript>().PlayerIndex != -1)
                    r = RNG.Next(32);
                StarLocs[i] = r;
            }
        }
        for(int i=0;i<6;i++)
        {
            Infect(transform.GetChild(StarLocs[i]).GetComponent<NodeScript>(), i%4, transform.GetChild(StarLocs[i]).transform.position);
        }
    }

    public void UpdateActionText(int c)
    {
        ActionCount += c;
        ActionText.text = "Action(s): " + ActionCount.ToString();
    }

    void SetPossibleSiblings()
    {
        for(int T=0; T<transform.childCount;T++)
        {
            for (int T2=0; T2 < transform.childCount; T2++)
            {
                if (T != T2 && Vector2.Distance(transform.GetChild(T).position, transform.GetChild(T2).position) < 3.5)
                {
                    if (!transform.GetChild(T).GetComponent<NodeScript>().PossibleSiblingIndex.Contains(T2))
                        transform.GetChild(T).GetComponent<NodeScript>().PossibleSiblingIndex.Add(T2);
                    if(!transform.GetChild(T2).GetComponent<NodeScript>().PossibleSiblingIndex.Contains(T))
                        transform.GetChild(T2).GetComponent<NodeScript>().PossibleSiblingIndex.Add(T);
                }
            }
        }
    }

    void ConnectTheWorld()
    {
        foreach(NodeScript n in GetComponentsInChildren<NodeScript>())
        {
            if (n.PossibleSiblingIndex.Count > 0)
            {
                BuildRoad(n);

                if(RNG.Next(3)==2 && n.PossibleSiblingIndex.Count > 0)
                {
                    BuildRoad(n);
                }
            }
        }
    }

    void BuildRoad(NodeScript n)
    {
        int I = RNG.Next(n.PossibleSiblingIndex.Count);
        I = n.PossibleSiblingIndex[I];
        n.ConnectedSiblings.Add(I);
        n.PossibleSiblingIndex.Remove(I);
        NodeScript n2 = transform.GetChild(I).GetComponent<NodeScript>();
        int S = n.transform.GetSiblingIndex();
        n2.ConnectedSiblings.Add(S);
        n2.PossibleSiblingIndex.Remove(S);
        GameObject r = Instantiate(Road) as GameObject;
        r.GetComponent<LineRenderer>().SetPositions(new Vector3[] { n.transform.position, transform.GetChild(I).position });
    }


    bool checkConnectivity()
    {
        List<int> ToCheck = new List<int> { };
        List<int> ConnectedList= new List<int> { };
        ConnectedList.Add(0);
        foreach (int i in transform.GetChild(0).GetComponent<NodeScript>().ConnectedSiblings)
        {
            ConnectedList.Add(i);
            ToCheck.Add(i);
        }
        while(ToCheck.Count>0)
        {
            foreach (int i in transform.GetChild(ToCheck[0]).GetComponent<NodeScript>().ConnectedSiblings)
            {
                if (!ConnectedList.Contains(i))
                {
                    ConnectedList.Add(i);
                    ToCheck.Add(i);
                }
            }
            ToCheck.RemoveAt(0);
        }
        return ConnectedList.Count == 32;
    }

    public void BuildHQ(NodeScript n)
    {
        n.hasHQ = true;
        n.Immune = true;
       GameObject go=Instantiate(HQ, n.transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(n.transform);
    }

    bool AllUnique(int[] I)
    {
        var hashSet = new HashSet<int>();
        foreach (var x in I)
        {
            if (!hashSet.Add(x))
            {
                return false;
            }
        }
        return true;
    }

    void SetPlayers()
    {
        int[] StartLocs = new int[4] { 0, 0, 0, 0 };
        while(!AllUnique(StartLocs))
        {
            for (int i = 0; i < 4; i++)
                StartLocs[i] = RNG.Next(32);
        }
        for(int i=0;i<4;i++)
        {
            int r = RNG.Next(PlayerPrefabs.Count);
            int c = StartLocs[i];
            Players[i] = Instantiate(PlayerPrefabs[r], transform.GetChild(c).position, Quaternion.identity) as GameObject;  
            transform.GetChild(c).GetComponent<NodeScript>().PlayerIndex=i;
            PlayerPrefabs.RemoveAt(r);
        }
    }

    public void SetActivePlayer(NodeScript n, int i)
    {
        ActivePlayerLocation = n;
        Description.text = Players[i].GetComponent<PlayerScript>().Description;
        AbilityDescription.text = Players[i].GetComponent<PlayerScript>().AbilityText;
        AbilityButton.onClick.RemoveAllListeners();
        AbilityButton.onClick.AddListener(delegate { Players[i].GetComponent<PlayerScript>().Ability(); });
        Outline.position = n.transform.position;
    }

    public void UpdateActionCounter()
    {
        int a = (int)ActionCounter;
        ActionCounterTime.text = a.ToString();
    }

    public void Infect(NodeScript n, int ID, Vector2 Origin)
    {
        if (!n.Immune && n.PlayerIndex == -1 && n.Germs.Count<4)
        {
            if(n.ActiveGermIndex<n.Germs.Count)
            {
                if(n.Germs[n.ActiveGermIndex].transform.localScale.x < .5f)
                {
                    n.Germs[n.ActiveGermIndex].transform.localScale = new Vector2(.5f, .5f);
                    n.ActiveGermIndex++;
                }
            }
            GameObject go = Instantiate(Germ, Origin, Quaternion.identity) as GameObject;
            go.AddComponent<Travel>().Dir = (Vector2)n.transform.position + GermPlacementByIndex(n.Germs.Count) - Origin;
            go.GetComponent<Travel>().Dest = (Vector2)n.transform.position + GermPlacementByIndex(n.Germs.Count);
            n.Germs.Add(go);
            n.Germs[n.Germs.Count - 1].GetComponent<SpriteRenderer>().color = Colors[ID];
            n.InfectIDIndex[n.Germs.Count - 1] = ID;
        }
        else if(n.Immune || n.PlayerIndex != -1)
        {
            GameObject go = Instantiate(Germ, Origin, Quaternion.identity) as GameObject;
            go.AddComponent<Travel>().Dir = (Vector2)n.transform.position + GermPlacementByIndex(n.Germs.Count) - Origin;
            go.GetComponent<Travel>().Dest = (Vector2)n.transform.position + GermPlacementByIndex(n.Germs.Count);
            go.GetComponent<Travel>().immuned = true;
            go.GetComponent<SpriteRenderer>().color = Colors[ID];
        }
    }

    Vector2 GermPlacementByIndex(int i)
    {
        switch(i)
        {
            case 0:
                return new Vector2(-.25f,.25f);
            case 1:
                return new Vector2(.25f, .25f);
            case 2:
                return new Vector2(-.25f, -.25f);
            case 3:
                return new Vector2(.25f, -.25f);
            default:
                return Vector2.zero;
        }
        
    }

    void SetAreaInfectType()
    {

    }


    private void ItGetsWorse()
    {
       //switch(RNG.Next(1))
        //{
          //  case 0:
                int i = RNG.Next(32);
                NodeScript n = transform.GetChild(i).GetComponent<NodeScript>();
                List<int> possible = new List<int> { };
                for(int j=0;j<4;j++)
                {
                    if (!Cures[j])
                        possible.Add(j);
                }
                if (possible.Count > 0)
                    Infect(n, possible[RNG.Next(possible.Count)], n.transform.position);
            //    break;
       // }
    }
    void UpdateEpiCounter()
    {
        int e = (int)epiCounter;
        EpiText.text = e.ToString();
    }


    // Update is called once per frame
    void Update () {
        epiCounter -= Time.deltaTime;
        UpdateEpiCounter();
        if(epiCounter<=0)
        {
            ItGetsWorse();
            epiCounter = 8;
        }

        if (ActionCount < 10)
        {
            ActionCounter -= Time.deltaTime;
            UpdateActionCounter();
            if (ActionCounter <= 0)
            {
                ActionCounter = ActionCD;
                UpdateActionText(1);
            }
        }
        if (Input.GetKey(KeyCode.R))
            Application.LoadLevel(0);
	}
}
