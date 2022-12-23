using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<PlayerScript> Players = new List<PlayerScript> { };
    public int PlayerPointer;
    public PlayerScript ActivePlayer;
    public List<Vector3> Hand_Positions;
    public int Start_Life;
    public int Gem_Goal;
    public GameObject Card;
    public GameObject CardBack;
    public List<GameObject> UI_Objects; //Steam, Gears, Life, Gems
    public List<Sprite> ArtSprites = new List<Sprite> { };
    public System.Random RNG;

    public GameObject AssemblyButton;
    public GameObject ToHandButton;
    public GameObject EndTurnButton;
    public GameObject RerollButton;
    public GameObject RerollLoc;

    public GameObject[] AssemblyLine = new GameObject[6];
    public GameObject[] MachinationSpots = new GameObject[5];

    public GameObject AssemblyView;
    public GameObject HandView;
    public GameObject PlayerName;
    public GameObject PlayerListObj;
    public GameObject Panel;
    public GameObject PL_MSG;

    public int PlayerCount;
    public bool isPlaying;

    // Use this for initialization
    void Start()
    {
        RNG = new System.Random(ThreadSafeRandom.Next());
        isPlaying = true;
        AssemblyButton.GetComponent<Button>().onClick.AddListener(delegate { switchView(); });
        ToHandButton.GetComponent<Button>().onClick.AddListener(delegate { switchView(); });
        EndTurnButton.GetComponent<Button>().onClick.AddListener(delegate { EndTurn(); });
        RerollButton.GetComponent<Button>().onClick.AddListener(delegate { RerollLine(); });
        //SetUpGame();
       // StartTurn();
    }

    public void SetUpGame()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            PlayerScript PS = new PlayerScript();
            PS.name = "Player " + i.ToString();
            Players.Add(PS);
        }
        for (int j=0;j<PlayerCount;j++)
        {
            Players[j].GM = this;
            for (int i = 0; i < 5; i++)
                Players[j].MachinationOpen[i] = true;
            Players[j].Resources[2] = Start_Life;
            CreateStartingDeck(Players[j]);

            GameObject go=Instantiate(Panel, PlayerListObj.transform.GetChild(j).position, Quaternion.identity) as GameObject;
            go.transform.SetParent(PlayerListObj.transform);
            PanelScript ps = (PanelScript)go.GetComponent(typeof(PanelScript));
            ps.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Players[j].name;
            ps.PS = Players[j];
        }

        StartTurn();
    }

    void StartTurn()
    {
        if (PlayerPointer >= Players.Count)
        {
            PlayerPointer = 0;
        }

        for(int i=0; i<6;i++)
        {
            if(AssemblyLine[i]==null)
            {
                AssemblyLine[i] = GenerateCard(i);
                AssemblyLine[i].transform.position = AssemblyView.transform.GetChild(i).position;
                AssemblyLine[i].transform.SetParent(AssemblyView.transform);
            }
        }
        ActivePlayer = Players[PlayerPointer];
        RerollButton.transform.position = RerollLoc.transform.position;
        UpdateUI();
       // Shuffle(ActivePlayer.Deck);
        AssemblyView.transform.position = new Vector3(10, 10, -11);
        HandView.transform.position = Vector3.zero;
        for(int i=0;i<5;i++)
        {
            if(!ActivePlayer.MachinationOpen[i])
            {
                CardScript CS = (CardScript)ActivePlayer.Machinations[i].GetComponent(typeof(CardScript));
                CS.Activated = false;
                if (CS.CardBack != null)
                    Destroy(CS.CardBack);
                CS.transform.position = MachinationSpots[i].transform.position;
            }
        }
        ActivePlayer.DrawCards();
    }

    void EndTurn()
    {
        for (int i = 0; i < ActivePlayer.Hand.Count; i++)
        {
           ActivePlayer.Hand[i].transform.position = new Vector3(10, 10, -11);
            ActivePlayer.Graveyard.Add(ActivePlayer.Hand[i]);
        }
        ActivePlayer.Hand.Clear();

        if (ActivePlayer.Deck.Count < 5)
        {
            Shuffle(ActivePlayer.Graveyard);
            for (int i = 0; i < ActivePlayer.Graveyard.Count; i++)
            {
                ActivePlayer.Deck.Add(ActivePlayer.Graveyard[i]);
            }
            ActivePlayer.Graveyard.Clear();
        }

        for (int i = 0; i < 5; i++)
        {
            if (!ActivePlayer.MachinationOpen[i])
            {
                CardScript CS = (CardScript)ActivePlayer.Machinations[i].GetComponent(typeof(CardScript));
                CS.transform.position = new Vector3(10,10,-11);
            }
        }

        ActivePlayer.Resources[0] = 0;
        ActivePlayer.Resources[1] = 0;
        PlayerPointer++;
        StartTurn();
    }

    public void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RNG.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void RerollLine()
    {
        RerollButton.transform.position = new Vector3(10, 10, -11);
        for(int i=0; i<6; i++)
        {
            Destroy(AssemblyLine[i]);
            AssemblyLine[i] = GenerateCard(i);
            AssemblyLine[i].transform.position = AssemblyView.transform.GetChild(i).position;
            AssemblyLine[i].transform.SetParent(AssemblyView.transform);
        }
    }

    public GameObject GenerateCard(int ID)
    {
        GameObject go = Instantiate(Card) as GameObject;
        CardScript CS = (CardScript)go.GetComponent(typeof(CardScript));
        int r;
        if (ID == 0 || ID == 1)
            r = 1;
        else
             r= RNG.Next(2);

        if (r > 0)
        {
            //production
            int[] Buy_Costs1 = new int[2];
            int[] prod = new int[2];

            if (ID == 0 || ID == 1)
                r = RNG.Next(2);
            else
                r = RNG.Next(5);

            int rand;

            switch (r)
            {
                //steam
                case 0:
                    rand = RNG.Next(2, 4);

                    if (ID == 0 || ID == 1)
                        rand = 2;

                    if (ID == 0)
                        Buy_Costs1[0] = 0;
                    else if (ID == 1)
                        Buy_Costs1[0] = 1;
                    else
                        Buy_Costs1[0] = RNG.Next(2);

                    Buy_Costs1[1] = rand + 1;
                    if (rand > 2)
                        Buy_Costs1[1]++;
                    if (rand == 3)
                        Buy_Costs1[1]++;
                    prod[0] = 0;
                    prod[1] = rand;
                    CS.Buying_Costs.Add(Buy_Costs1);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //gear
                case 1:
                    rand = RNG.Next(2, 4);

                    if (ID == 0 || ID == 1)
                        rand = 2;


                    if (ID == 0)
                        Buy_Costs1[0] = 0;
                    else if (ID == 1)
                        Buy_Costs1[0] = 1;
                    else
                    Buy_Costs1[0] = RNG.Next(2);

                    Buy_Costs1[1] = rand + 1;
                    if (rand > 2)
                        Buy_Costs1[1]++;
                    if (rand == 3)
                        Buy_Costs1[1]++;
                    prod[0] = 1;
                    prod[1] = rand;

                    CS.Buying_Costs.Add(Buy_Costs1);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //heal
                case 2:
                    rand = RNG.Next(1, 3);
                    Buy_Costs1[0] = RNG.Next(2);
                    Buy_Costs1[1] = rand + 4;
                    prod[0] = 2;
                    prod[1] = rand;

                    CS.Buying_Costs.Add(Buy_Costs1);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //gem
                case 3:
                    rand = RNG.Next(1, 4);
                    Buy_Costs1[0] = RNG.Next(2);
                    Buy_Costs1[1] = rand + 5;
                    prod[0] = 3;
                    prod[1] = rand;

                    CS.Buying_Costs.Add(Buy_Costs1);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //hit
                case 4:
                    rand = 4;
                    Buy_Costs1[0] = RNG.Next(2);
                    Buy_Costs1[1] = 5;
                    prod[0] = 4;
                    prod[1] = rand;

                    CS.Buying_Costs.Add(Buy_Costs1);
                    CS.Production = prod;
                    CS.playAction = CS.Hit;
                    break;

            }
            CS.gameObject.transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>().sprite = ArtSprites[prod[0] + 7];

        }
        else
        {
            //machination
            CS.isMachanation = true;
            int[] Act_Costs0 = new int[2];
            int[] Act_Costs1 = new int[2];
            int[] Buy_Costs0 = new int[2];
            int[] Buy_Costs1 = new int[2];
            int[] prod = new int[2];

            r = RNG.Next(10);
            int rand;
            switch (r)
            {
                //Gear to steam
                case 0:
                    rand= RNG.Next(1, 5);
                    Act_Costs0[0] = 1;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand;
                    prod[0] = 0;
                    prod[1] = rand + 1;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //steam to gear
                case 1:
                    rand = RNG.Next(1, 5);
                    Act_Costs0[0] = 0;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand;
                    prod[0] = 1;
                    prod[1] = rand + 1;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //gear to gems
                case 2:
                    rand = RNG.Next(2, 5);
                    Act_Costs0[0] = 1;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand + 1;
                    prod[0] = 3;
                    prod[1] = rand - 1;
                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //steam to gems
                case 3:
                    rand = RNG.Next(2, 5);
                    Act_Costs0[0] = 0;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand + 1;
                    prod[0] = 3;
                    prod[1] = rand - 1;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //life to gems
                case 4:
                    rand = RNG.Next(-4, 0);
                    Act_Costs0[0] = 2;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = 3 - rand;
                    prod[0] = 3;
                    prod[1] = 1 - rand;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //steam and gear to gems

                // life and steam to gems

                //life and gears to gems

                //gear to hit
                case 5:
                    rand = 4;
                    Act_Costs0[0] = 1;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand + 1;
                    prod[0] = 4;
                    prod[1] = 1;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Hit;
                    break;

                //steam to hit
                case 6:
                    rand = 4;
                    Act_Costs0[0] = 0;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand + 1;
                    prod[0] = 4;
                    prod[1] = 1;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Hit;
                    break;

                //gems to hit
                case 7:
                    rand = 2;
                    Act_Costs0[0] = 3;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = 4;
                    prod[0] = 4;
                    prod[1] = 1;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Hit;
                    break;

                //gear and steam to hit

                //gear to life
                case 8:
                    rand = RNG.Next(3, 5);
                    Act_Costs0[0] = 1;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand + 1;
                    prod[0] = 2;
                    prod[1] = rand - 2;

                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //steam to life
                case 9:
                    rand = RNG.Next(3, 5);
                    Act_Costs0[0] = 0;
                    Act_Costs0[1] = rand;
                    Buy_Costs0[0] = RNG.Next(2);
                    Buy_Costs0[1] = rand + 1;
                    prod[0] = 2;
                    prod[1] = rand - 2;


                    CS.Activation_Costs.Add(Act_Costs0);
                    CS.Buying_Costs.Add(Buy_Costs0);
                    CS.Production = prod;
                    CS.playAction = CS.Produce;
                    break;

                //gear and steam to life

                default:
                    break;
            }
            CS.gameObject.transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>().sprite = ArtSprites[prod[0] + 2];
        }

        CS.isOnLine = true;
        CS.ID = ID;
        CS.GM = this;
        return go;
    }

    void CreateStartingDeck(PlayerScript PS)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject Go = Instantiate(Card) as GameObject;
            CardScript CS = (CardScript)Go.GetComponent(typeof(CardScript));
            CS.Controlling_Player = PS;
            CS.Production[0] = 0;
            CS.Production[1] = 1;
            CS.playAction = CS.Produce;
            CS.GM = this;
            Go.transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>().sprite = ArtSprites[0];

            GameObject Go1 = Instantiate(Card) as GameObject;
            CardScript CS1 = (CardScript)Go1.GetComponent(typeof(CardScript));
            CS1.Controlling_Player = PS;
            CS1.Production[0] = 1;
            CS1.Production[1] = 1;
            CS1.playAction = CS1.Produce;
            CS1.GM = this;
            Go1.transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>().sprite = ArtSprites[1];
            PS.Deck.Add(Go);
            PS.Deck.Add(Go1);
        }
    }

    public void switchView()
    {
        if(HandView.transform.position.x==0)
        {
            HandView.transform.position = new Vector3(10, 10, -11);
            AssemblyView.transform.position = Vector3.zero;
        }
        else
        {
            AssemblyView.transform.position = new Vector3(10, 10, -11);
            HandView.transform.position = Vector3.zero;
        }
    }

    public void Produce(int[] ia)
    {
               
        ActivePlayer.Resources[ia[0]] += ia[1];
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < 4; i++)
        {
            UI_Objects[i].GetComponent<Text>().text = ActivePlayer.Resources[i].ToString();
        }
        PlayerName.GetComponent<Text>().text = ActivePlayer.name;

        if (ActivePlayer.Resources[3]>=Gem_Goal || Players.Count==1)
        {
            //winning stuff
            isPlaying = false;
            HandView.transform.position = new Vector3(10, 10, -11);
            AssemblyView.transform.position = new Vector3(10, 10, -11);
            PlayerName.GetComponent<Text>().text = "";
            PlayerListObj.transform.position = new Vector3(10, 10, -11);
            PlayerListObj.transform.position = Vector3.zero;
            if(ActivePlayer.Resources[3] >= Gem_Goal)
            PL_MSG.GetComponent<Text>().text = ActivePlayer.name + " WINS!!!";
            else
                PL_MSG.GetComponent<Text>().text = Players[0].name + " WINS!!!";
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
