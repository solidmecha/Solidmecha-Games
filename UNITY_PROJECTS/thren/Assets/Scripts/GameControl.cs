using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameControl : MonoBehaviour {

    public GameObject Card;
    public Sprite[] SpriteIcons;
    public List<GameObject> CardSet = new List<GameObject> { };
    public List<List<int>> PackIndex = new List<List<int>> { };
    public List<GameObject> PackLocations;
    public List<GameObject> HandLocations;
    public GameObject RoleLocations;
    public List<List<CardScript>> PlayerHands = new List<List<CardScript>> { };
    public int PassIndex; //index for active player's pack
    public int RoundNumber;
    public System.Random RNG;
    public List<int> DiscardIndex = new List<int> { };
    public int[] LieCounts;
    public int[] RebelCounts;
    public int[] FactionScore;
    public int[] VictoryPoints;
    public int[] BoonCounts;
    public int[] StrikeCounts;
    public int[] StrikeTotals;
    public int[] BoonBonus;
    public int[] StrikePenalty;
    public GameObject FactionChoice;
    public Text Message;
    public Text[] BoonText;
    public Text[] StrikeText;
    public Text[] VPText;
    public Text[] StikeTotalText;
    public enum Mode { Default, Draft, Discard, SetRole, Reveal, AddSoldierPower, ShowHand, Accuse, BecomeRole, GiveStrike, GiveBoon, Continue, EndGame}
    public Mode CurrentMode;
    public int[] RoleIndex;
    public int CardSelectionCounter;
    public List<GameObject> FlippedCards=new List<GameObject> { };


    // Use this for initialization
    void Start() {
        BoonBonus = new int[4];
        StrikeTotals = new int[4];
        StrikePenalty = new int[4];
        BoonCounts = new int[4];
        StrikeCounts = new int[4];
        VictoryPoints = new int[4];
        LieCounts = new int[4];
        RebelCounts = new int[4];
        FactionScore = new int[2];
        RoleIndex = new int[4];
        RNG = new System.Random();
        List<CardScript> Hand0 = new List<CardScript> { };
        List<CardScript> Hand1 = new List<CardScript> { };
        List<CardScript> Hand2 = new List<CardScript> { };
        List<CardScript> Hand3 = new List<CardScript> { };
        PlayerHands.Add(Hand0);
        PlayerHands.Add(Hand1);
        PlayerHands.Add(Hand2);
        PlayerHands.Add(Hand3);
        List<int> Indices = new List<int> { };
        for (int i = 0; i < 16; i++)
        {
            GameObject Go = Instantiate(Card, (Vector2)transform.position + 2.5f * i * Vector2.right, Quaternion.identity) as GameObject;
            Go.GetComponent<CardScript>().ID = i;
            SetFactionIcon(Go);
            CardSet.Add(Go);
            Indices.Add(i);
        }

        for (int i = 0; i < 4; i++)
        {
            List<int> pack = new List<int> { };
            for (int j = 0; j < 4; j++)
            {
                int r = RNG.Next(Indices.Count);
                pack.Add(Indices[r]);
                CardSet[Indices[r]].transform.position = PackLocations[i].transform.GetChild(j).position;
                if (i != 0)
                    CardSet[Indices[r]].transform.Rotate(new Vector3(0, 180, 0));
                else
                    CardSet[Indices[r]].GetComponent<CardSelect>().enabled = true;
                CardSet[Indices[r]].GetComponent<CardSelect>().PackNumber = i;
                CardSet[Indices[r]].GetComponent<CardSelect>().PackValue = Indices[r];
                Indices.RemoveAt(r);
            }
            PackIndex.Add(pack);
        }
        for (int i = 0; i < 3; i++)
        {
            int p = GetComponents<BotControl>()[i].HandIndex - PassIndex;
            GetComponents<BotControl>()[i].ChooseCard(p);
        }
        UpdateMode(Mode.Draft);
    }

    public void UpdateMode(Mode M)
    {
        CurrentMode = M;
        switch(CurrentMode)
        {
            case GameControl.Mode.Draft:
                Message.text = "Select card to draft";
                break;
            case GameControl.Mode.Discard:
                Message.text = "Select card to discard";
                break;
            case GameControl.Mode.SetRole:
                Message.text = "Select role for the round";
                break;
            case GameControl.Mode.Reveal:
                {
                    Message.text = "Select a role card to reveal";
                }
                break;
            case GameControl.Mode.AddSoldierPower:
                Message.text = "Select card to help";
                break;
            case GameControl.Mode.ShowHand:
                {
                    Message.text = "Select hand to reveal";
                }
                break;
            case GameControl.Mode.Accuse:
                Message.text = "Select a role card to accuse";
                break;
            case GameControl.Mode.BecomeRole:
                {
                    Message.text = "Select a card to become";
                }
                break;
            case GameControl.Mode.GiveBoon:
                {
                    Message.text = "Select a role card to give a boon";
                }
                break;
            case GameControl.Mode.GiveStrike:
                {
                    Message.text = "Select a role card to give a strike";
                }
                break;
            default:
                break;
        }

    }

    public void PlayRound()
    {
        bool isTactical=false;
        bool isImperial = false;
        for (int i = 0; i < 4; i++)
        {
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().FactionID == 1)
                RebelCounts[i]++;
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().ID == 12)
            {
                CardSet[RoleIndex[i]].transform.eulerAngles=Vector3.zero;
                UpdateMode(Mode.Accuse);
                isTactical = true;
            }
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().ID == 4)
            {
                isImperial = true;
            }
        }

        if (!isTactical)
        {
            if(isImperial)
            {
                for (int c = 1; c < 4; c++)
                    CardSet[RoleIndex[c]].transform.eulerAngles=Vector3.zero;
            }
            for (int i = 0; i < 4; i++)
            {
                CardSet[RoleIndex[i]].GetComponent<CardScript>().Ability();
            }
        }


}

    public void CleanUpRound()
    {
        for(int i=0;i<4;i++)
        {
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().ID==5)//Aestetic
            {
                BoonCounts[i] += StrikeCounts[i];
                StrikeCounts[i] = 0;
            }
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().ID == 7)//Bounty Hunter
            {
                if(StrikePenalty[i]==BoonBonus[i])
                {
                    BoonCounts[i] += 2;
                }
            }
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().ID == 8)//Merc
            {
                BoonCounts[i] *= 2;
                StrikeCounts[i] = 0;
            }
            BoonCounts[i] += BoonBonus[i];
            StrikeCounts[i] += StrikePenalty[i];
        }
        ShowBoon();
        ShowStrikes();

        for (int i = 0; i < 4; i++)
        {
            CardSet[RoleIndex[i]].transform.eulerAngles=Vector3.zero;
            FactionScore[CardSet[RoleIndex[i]].GetComponent<CardScript>().FactionID] += CardSet[RoleIndex[i]].GetComponent<CardScript>().Power;
            FactionScore[CardSet[RoleIndex[i]].GetComponent<CardScript>().FactionID] += BoonCounts[i];
            FactionScore[CardSet[RoleIndex[i]].GetComponent<CardScript>().FactionID] -= StrikeCounts[i];
        }
        Message.text = "Rebels: " + FactionScore[1].ToString() + "   " + "Imperials: " + FactionScore[0].ToString();
        if(FlippedCards.Count>0)
        {
            foreach (GameObject g in FlippedCards)
                g.transform.Rotate(new Vector3(0, 180, 0));
            FlippedCards.Clear();
        }

        StartCoroutine(NextFrameContinue(Mode.Continue));
    }

    IEnumerator NextFrameContinue(Mode M)
    {
        yield return new WaitForEndOfFrame();
        UpdateMode(M);
    }

    public void EndRound()
    {
        int winID = 9;
        if (FactionScore[0] > FactionScore[1])
            winID = 0;
        else if (FactionScore[0] < FactionScore[1])
            winID = 1;
        for (int i = 0; i < 4; i++)
        {
            if (CardSet[RoleIndex[i]].GetComponent<CardScript>().FactionID == winID)
                VictoryPoints[i]++;
            VictoryPoints[i] += BoonCounts[i];
            VictoryPoints[i] -= StrikeCounts[i];
            StrikeTotals[i] += StrikeCounts[i];
            Destroy(CardSet[RoleIndex[i]]);
            BoonBonus[i] = 0;
            BoonCounts[i] = 0;
            StrikeCounts[i] = 0;
            StrikePenalty[i] = 0;
        }
        FactionScore[0] = 0;
        FactionScore[1] = 0;
        ShowBoon();
        ShowStrikes();
        ShowVP();
        foreach (SpriteRenderer s in RoleLocations.GetComponentsInChildren<SpriteRenderer>())
            s.sprite = null;
        if (PlayerHands[0].Count == 0)
        {
            int winningScore = 0;
            int winner = 0;
            for (int i = 0; i < 4; i++)
            {

                if (VictoryPoints[i] >= winningScore)
                {
                    winningScore = VictoryPoints[i];
                    winner = i;
                }
            }
                
                if (winner == 0)
                    Message.text = "Victory!";
                else
                    Message.text = "Maybe Next Time...";
            StartCoroutine(NextFrameContinue(Mode.EndGame));

        }
        else {

            UpdateMode(Mode.SetRole);
        }
        
    }

    public void SetFactionIcon(GameObject Go)
    {
        Go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = SpriteIcons[Go.GetComponent<CardScript>().FactionID];
    }

    public void ChooseFaction(CardScript CS)
    {
        GameObject Go = Instantiate(FactionChoice) as GameObject;
        Go.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ChangeFaction(1, CS); });
        Go.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ChangeFaction(0, CS); });
        Go.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { AnnounceFaction(CS); });
        Go.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { AnnounceFaction(CS); });
        Go.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Destroy(Go); });
        Go.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Destroy(Go); });
    }
    public void AnnounceFaction(CardScript CS)
    {
        GameObject Go = Instantiate(FactionChoice) as GameObject;
        Go.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Announce:";
        Go.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { CheckFaction(1, CS,0); });
        Go.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { CheckFaction(0, CS,0); });
        Go.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Destroy(Go); });
        Go.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Destroy(Go); });
        Go.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { PlayRound(); });
        Go.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { PlayRound(); });
    }

    public void ChangeFaction(int i, CardScript CS)
    {
        CS.FactionID = i;
        SetFactionIcon(CS.gameObject);       
    }

    public void CheckFaction(int i, CardScript CS, int HandIndex)
    {
        if (CS.FactionID != i)
            LieCounts[HandIndex]++;
        else
            CS.truth = true;
    }

    public void ShowBoon()
    {
        for(int i=0;i<4;i++)
            BoonText[i].text = BoonCounts[i].ToString();        
    }

    public void ShowStrikes()
    {
        for (int i = 0; i < 4; i++)
            StrikeText[i].text = StrikeCounts[i].ToString();
    }

    public void ShowVP()
    {
        for (int i = 0; i < 4; i++)
        {
            VPText[i].text = VictoryPoints[i].ToString();
            StikeTotalText[i].text = "(-" + StrikeTotals[i].ToString() + ")";
        }
    }

    public void PassPack()
    {
        PassIndex++;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < PackIndex[i].Count; j++)
            {
                int P = i + PassIndex;
                if (P > 3)
                    P -= 4;
                CardSet[PackIndex[i][j]].transform.position=PackLocations[P].transform.GetChild(j).position;

                if (P == 0)
                {
                    CardSet[PackIndex[i][j]].transform.Rotate(new Vector3(0, 180, 0));
                    CardSet[PackIndex[i][j]].GetComponent<CardSelect>().enabled = true;
                }
                else
                    CardSet[PackIndex[i][j]].GetComponent<CardSelect>().enabled = false;
            }
        }
        for(int i=0;i<3;i++)
        {
            int p = GetComponents<BotControl>()[i].HandIndex - PassIndex;
            if (p < 0)
                p += 4;
            GetComponents<BotControl>()[i].ChooseCard(p);
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CurrentMode == GameControl.Mode.EndGame)
            Application.LoadLevel(0);
        else if (Input.GetKeyDown(KeyCode.Mouse0) && CurrentMode == GameControl.Mode.Continue)
            EndRound();

    }
}
