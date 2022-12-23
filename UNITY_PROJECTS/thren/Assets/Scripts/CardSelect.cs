using UnityEngine;
using System.Collections;

public class CardSelect : MonoBehaviour {

    public int PackNumber;
    public int PackValue;



    private void OnMouseDown()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        switch (GC.CurrentMode)
        {
            case GameControl.Mode.Draft:
                { 
            SelectCard(0);
            foreach (int i in GC.PackIndex[PackNumber])
            {
                GC.CardSet[i].transform.Rotate(0, 180, 0);
            }
                    if (GC.PassIndex < 3)
                        GC.PassPack();
                    else
                        GC.UpdateMode(GameControl.Mode.Discard);
            GC.RoundNumber++;
        }
                break;
            case GameControl.Mode.Discard:
        {
            DiscardCard();

            GC.PlayerHands[0].Remove(GetComponent<CardScript>());
            for (int i = 0; i < 3; i++)
            {
                GC.GetComponents<BotControl>()[i].ChooseDiscard();
            }
                    GC.UpdateMode(GameControl.Mode.SetRole);
                }
                break;
            case GameControl.Mode.SetRole:
        {
            SelectRole(0);
            for (int i = 0; i < 3; i++)
            {
                GC.GetComponents<BotControl>()[i].ChooseRole();
                GC.GetComponents<BotControl>()[i].AnnouceFaction();
            }
            GC.RoundNumber++;
        }
                break;
            case GameControl.Mode.Reveal:
        {
                    if (transform.rotation.y != 0)
                        transform.Rotate(new Vector3(0, 180, 0));
                    if(GetComponent<CardScript>().FactionID==1)
                    {
                        GC.BoonCounts[0]++;
                        GC.ShowBoon();
                    }
                    GC.UpdateMode(GameControl.Mode.Accuse);
        }
                break;
            case GameControl.Mode.AddSoldierPower:
                {
                    if (GetComponent<CardScript>().FactionID == 1 && GetComponent<CardScript>().Power < 4)
                    {
                        GC.BoonCounts[0] += GetComponent<CardScript>().Power;
                        GC.ShowBoon();
                    }
                    GC.UpdateMode(GameControl.Mode.Accuse);
                }
                break;
            case GameControl.Mode.ShowHand:
                {
                    int HandIndex=GetComponent<CardScript>().HandIndex;
                    foreach (CardScript c in GC.PlayerHands[HandIndex])
                    {
                        c.transform.Rotate(new Vector3(0, 180, 0));
                        GC.FlippedCards.Add(gameObject);
                        if (c.FactionID != 1)
                            GC.BoonCounts[0]++;
                       
                    }
                    GC.ShowBoon();
                    GC.UpdateMode(GameControl.Mode.Accuse);
                }
                break;
            case GameControl.Mode.Accuse:
                {
                    CardScript CS = GetComponent<CardScript>();
                    if (!CS.truth)
                    {
                        GC.BoonCounts[0]++;
                        if(GC.CardSet[GC.RoleIndex[0]].GetComponent<CardScript>().ID==1)
                            GC.BoonCounts[0]++;
                        GC.ShowBoon();
                    }
                    else
                    {
                        GC.StrikeCounts[0]++;
                        GC.ShowStrikes();
                    }

                    if (GC.CardSelectionCounter > 0)
                        GC.CardSelectionCounter--;
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            GC.GetComponents<BotControl>()[i].Accuse();
                        }
                        GC.UpdateMode(GameControl.Mode.GiveBoon);
                    }
                }
                break;
            case GameControl.Mode.BecomeRole:
                {

                }
                break;
            case GameControl.Mode.GiveBoon:
                {
                    CardScript CS = GetComponent<CardScript>();
                    GC.BoonCounts[CS.HandIndex]++;
                    GC.ShowBoon();
                    if (GetComponent<CardScript>().FactionID == GC.CardSet[GC.RoleIndex[0]].GetComponent<CardScript>().FactionID)
                        GC.BoonBonus[0] = 1;
                    for (int i = 0; i < 3; i++)
                    {
                        GC.GetComponents<BotControl>()[i].GiveBoon();
                    }
                    GC.UpdateMode(GameControl.Mode.GiveStrike);
                }
                break;
            case GameControl.Mode.GiveStrike:
                {
                    CardScript CS = GetComponent<CardScript>();
                    GC.StrikeCounts[CS.HandIndex]++;
                    GC.ShowStrikes();
                    if (GC.CardSet[GC.RoleIndex[0]].GetComponent<CardScript>().ID != 3 || GC.CardSelectionCounter<0)
                    {
                        if (GetComponent<CardScript>().FactionID == GC.CardSet[GC.RoleIndex[0]].GetComponent<CardScript>().FactionID)
                            GC.StrikePenalty[0] = 1;
                        for (int i = 0; i < 3; i++)
                        {
                            GC.GetComponents<BotControl>()[i].GiveStrike();
                        }
                        GC.CleanUpRound();
                    }
                    else if(GC.CardSelectionCounter>0)
                    {
                        GC.CardSelectionCounter--;
                    }
                    else if(GC.CardSelectionCounter==0)
                    {
                        GC.CardSelectionCounter--;
                        GC.UpdateMode(GameControl.Mode.Accuse);
                    }

                }
                break;
            default:
                break;
        }
        
    }

    public void SelectCard(int HandIndex)
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.PlayerHands[HandIndex].Add(GetComponent<CardScript>());
        GetComponent<CardScript>().HandIndex = HandIndex;
        GC.PackIndex[PackNumber].Remove(PackValue);
        transform.position = GC.HandLocations[HandIndex].transform.GetChild(GC.RoundNumber).position;
    }

    public void SelectRole(int HandIndex)
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        transform.position = GC.RoleLocations.transform.GetChild(HandIndex).position;
        GC.PlayerHands[HandIndex].Remove(GetComponent<CardScript>());
        GC.RoleIndex[HandIndex] = PackValue;
        if (HandIndex == 0 && GetComponent<CardScript>().FactionID == 2)
            GC.ChooseFaction(GetComponent<CardScript>());
        else if (HandIndex==0)
            GC.AnnounceFaction(GetComponent<CardScript>());
    }

    public void DiscardCard()
    {
        transform.position = new Vector3(-10, -10, -10);
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.DiscardIndex.Add(PackValue);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
