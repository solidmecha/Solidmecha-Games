using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CastleControl : MonoBehaviour {
    public List<Button> plusbuttons=new List<Button> { };
    List<int> ints = new List<int> { -10, -5, -1, 1, 5, 10 };
    public Button Play;
    public Button Reset;
    public Text RemainingCount;
    public Text ScoreText;
    public List<Text> RivalText;
    public CastleScript CurrentCastle;
    public GameObject Outline;
    int[] TroopCount = new int[10];
    int[] RivalOrders;
    int RemainingTroops = 100;
    System.Random RNG;


    // Use this for initialization
    void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        for(int i=0;i<6;i++)
        {
            int a = i;
            plusbuttons[i].onClick.AddListener(delegate { changeTroopCount(ints[a]); });
        }
        Play.onClick.AddListener(delegate { DetermineScore(); });
        Reset.onClick.AddListener(delegate { Application.LoadLevel(0); });
    }

    void changeTroopCount(int T)
    {
        if(RemainingTroops-T>=0)
        {
            if (TroopCount[CurrentCastle.id] + T >= 0)
            {
                TroopCount[CurrentCastle.id] += T;
                RemainingTroops -= T;
            }
            else
            {
                RemainingTroops += TroopCount[CurrentCastle.id];
                TroopCount[CurrentCastle.id] = 0;
            }
        }
        else
        {
           TroopCount[CurrentCastle.id] += RemainingTroops;
           RemainingTroops = 0;
        }
        UpdateGUI();
    }

    void UpdateGUI()
    {
        CurrentCastle.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = TroopCount[CurrentCastle.id].ToString();
        RemainingCount.text = "Troops:\n"+RemainingTroops.ToString();
    }

    void SetRivalOrders()
    {
        int R = RNG.Next(21);
        if (RNG.Next(2,5) == 4)
            R += 20; //pushes to default case
        switch(R)
        {
            case 0:
                RivalOrders = new int[10] { 0,0,0,0,25,25,0,25,25,0}; 
                break;
            case 1:
                RivalOrders = new int[10] { 1, 1, 1, 1, 1, 1, 12, 13, 33, 36 }; 
                break;
            case 2:
                RivalOrders = new int[10] { 0, 0, 5, 5, 5, 5, 10, 10, 20, 40 };
                break;
            case 3:
                RivalOrders = new int[10] { 5, 5, 5, 5, 5, 15, 15, 15, 15, 15};
                break;
            case 4:
                RivalOrders = new int[10] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
                break;
            case 5:
                RivalOrders = new int[10] { 0, 0, 5, 5, 20, 20, 5, 20, 20, 5 };
                break;
            case 6:
                RivalOrders = new int[10] { 4, 0, 0, 0, 0, 0, 0, 32, 32, 32 };
                break;
            case 7:
                RivalOrders = new int[10] { 1, 3, 5, 5, 5, 5, 5, 16, 21, 34 };
                break;
            case 8:
                RivalOrders = new int[10] { 5, 6, 7, 8, 9, 11, 12, 13, 14, 15 };
                break;
            case 9:
                RivalOrders = new int[10] { 14, 14, 14, 14, 14, 15, 15, 0, 0, 0};
                break;
            case 10:
                RivalOrders = new int[10] { 5, 5, 5, 5, 5, 10, 15, 15, 15, 20};
                break;
            case 11:
                RivalOrders = new int[10] { 1, 1, 1, 1, 1, 2, 3, 30, 30, 30 };
                break;
            case 12:
                RivalOrders = new int[10] { 11, 11, 11, 11, 11, 11, 11, 11, 12, 0 };
                break;
            case 13:
                RivalOrders = new int[10] { 0, 2, 2, 2, 22, 22, 6, 22, 22, 0 };
                break;
            case 14:
                RivalOrders = new int[10] { 1, 1, 1, 1, 1, 1, 16, 26, 26, 26};
                break;
            case 15:
                RivalOrders = new int[10] {1,2,2,2,5,15,16,23,32,2 };
                break;
            case 16:
                RivalOrders = new int[10] { 1, 1, 1, 1, 5, 15, 20, 23, 31, 2 };
                break;
            case 17:
                RivalOrders = new int[10] { 2, 3, 3, 3, 3, 16, 16, 16, 16, 22};
                break;
            case 18:
                RivalOrders = new int[10] { 2, 4, 8, 11, 14, 16, 19, 22, 2, 2};
                break;
            case 19:
                RivalOrders = new int[10] { 2, 4, 5, 7, 9, 11, 13, 15, 16, 18 };
                break;
            default:
                int c = 100;
                RivalOrders = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                while (c>0)
                {
                    int r = RNG.Next(10);
                    int w = RNG.Next(1, 4);
                    if(c>=r*w)
                    {
                        RivalOrders[r] = RivalOrders[r] + r * w;
                        c = c - r * w;
                    }
                    else
                    {
                        RivalOrders[r] += c;
                        c = 0;
                    }
                }
                break;
        }


    }

    void DetermineScore()
    {
        SetRivalOrders();
        float playerScore=0;
        float rivalScore=0;
        for(int i=0;i<10;i++)
        {
            RivalText[i].text = RivalOrders[i].ToString();
            if(TroopCount[i]>RivalOrders[i])
            {
                playerScore = playerScore + i + 1;
                RivalText[i].gameObject.transform.root.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.white;
            }
            else if(TroopCount[i]==RivalOrders[i])
            {
                float f = (float)(i+1f)/ 2f; 
                playerScore = playerScore + f;
                rivalScore = rivalScore + f;
                RivalText[i].gameObject.transform.root.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.cyan;
            }
            else
            {
                rivalScore = rivalScore + 1 + i;
                RivalText[i].gameObject.transform.root.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.red;
            }
        }
        if (rivalScore >= playerScore)
        { ScoreText.color = Color.red; }
        else
            ScoreText.color = Color.white;
        ScoreText.text = "Player Score: " + playerScore.ToString() + " Rival Score: " + rivalScore.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
