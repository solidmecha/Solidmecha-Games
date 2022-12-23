using UnityEngine;
using System.Collections.Generic;

public class BotControl : MonoBehaviour {

    public int HandIndex;
    public int AnnounceBehavior;

    // Use this for initialization
    void Start() {

    }

    public void ChooseCard(int PackIndex)
    {
        GameControl GC = GetComponent<GameControl>();
        int r = GC.RNG.Next(GC.PackIndex[PackIndex].Count);
        if (GC.CardSet[GC.PackIndex[PackIndex][r]].GetComponent<CardScript>().ID == 0)
            AnnounceBehavior = -1;//set lying if operative
        else if (GC.CardSet[GC.PackIndex[PackIndex][r]].GetComponent<CardScript>().ID == 9)
            AnnounceBehavior = 1; //set truth if cleric
        GC.CardSet[GC.PackIndex[PackIndex][r]].GetComponent<CardSelect>().SelectCard(HandIndex);
    }

    public void ChooseDiscard()
    {
        GameControl GC = GetComponent<GameControl>();
        int r = GC.RNG.Next(GC.PlayerHands[HandIndex].Count);
        GC.PlayerHands[HandIndex][r].GetComponent<CardSelect>().DiscardCard();
        GC.PlayerHands[HandIndex].RemoveAt(r);
    }

    public void ChooseRole()
    {
        GameControl GC = GetComponent<GameControl>();
        int r = GC.RNG.Next(GC.PlayerHands[HandIndex].Count);
        if (GC.PlayerHands[HandIndex][r].FactionID == 2)
            GC.ChangeFaction(GC.RNG.Next(2), GC.PlayerHands[HandIndex][r]);
        GC.PlayerHands[HandIndex][r].GetComponent<CardSelect>().SelectRole(HandIndex);
    }


    public void AnnouceFaction()
    {
        GameControl GC = GetComponent<GameControl>();
        int r = 0;
        switch (AnnounceBehavior)
        {
            case -1:
                if (GC.CardSet[GC.RoleIndex[HandIndex]].GetComponent<CardScript>().FactionID == 0)
                    r = 1;
                else
                    r = 0;
                break;
            case 1:
                r = GC.CardSet[GC.RoleIndex[HandIndex]].GetComponent<CardScript>().FactionID;
                break;
            default:
                r = GC.RNG.Next(2);
                break;
        }
        GC.CheckFaction(r, GC.CardSet[GC.RoleIndex[HandIndex]].GetComponent<CardScript>(), HandIndex);
        GC.RoleLocations.transform.GetChild(HandIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = GC.SpriteIcons[r];
    }

    public void Accuse()
    {
        List<int> tempL = new List<int> { 0, 1, 2, 3 };
        tempL.Remove(HandIndex);
        GameControl GC = GetComponent<GameControl>();
        CardScript CS = GC.CardSet[GC.RoleIndex[tempL[GC.RNG.Next(3)]]].GetComponent<CardScript>();
        if (!CS.truth)
        {
            GC.BoonCounts[HandIndex]++;
            if (GC.CardSet[GC.RoleIndex[HandIndex]].GetComponent<CardScript>().ID == 1)
                GC.BoonCounts[HandIndex]++;
            GC.ShowBoon();
        }
        else
        {
            GC.StrikeCounts[HandIndex]++;
            GC.ShowStrikes();
        }
    }

    public void GiveBoon()
    {
        List<int> tempL = new List<int> { 0, 1, 2, 3 };
        tempL.Remove(HandIndex);
        GameControl GC = GetComponent<GameControl>();
        CardScript CS = GC.CardSet[GC.RoleIndex[tempL[GC.RNG.Next(3)]]].GetComponent<CardScript>();
        GC.BoonCounts[CS.HandIndex]++;
        GC.ShowBoon();
        if (GC.CardSet[GC.RoleIndex[HandIndex]].GetComponent<CardScript>().FactionID == CS.FactionID)
            GC.BoonBonus[HandIndex] = 1;

    }

    public void GiveStrike()
    {
        List<int> tempL = new List<int> { 0, 1, 2, 3 };
        tempL.Remove(HandIndex);
        GameControl GC = GetComponent<GameControl>();
        CardScript CS = GC.CardSet[GC.RoleIndex[tempL[GC.RNG.Next(3)]]].GetComponent<CardScript>(); ;
        GC.StrikeCounts[CS.HandIndex]++;
        if (CS.FactionID == GC.CardSet[GC.RoleIndex[HandIndex]].GetComponent<CardScript>().FactionID)
            GC.StrikePenalty[HandIndex] = 1;
    }

    public void AddSoldierPower()
    {
        GameControl GC = GetComponent<GameControl>();
        bool canBuff=false;
        CardScript CS=GetComponent<CardScript>();
        for(int i=0;i<GC.PlayerHands[HandIndex].Count;i++)
        {
            if(GC.PlayerHands[HandIndex][i].FactionID==1 && GC.PlayerHands[HandIndex][i].Power<4)
            {
                canBuff = true;
                CS = GC.PlayerHands[HandIndex][i];
            }
        }
        if (canBuff)
        {
            GC.BoonCounts[HandIndex] += CS.Power;
            GC.ShowBoon();
        }
    }

    public void Reveal()
    {
        List<int> tempL = new List<int> { 0, 1, 2, 3 };
        tempL.Remove(HandIndex);
        GameControl GC = GetComponent<GameControl>();
        CardScript CS = GC.CardSet[GC.RoleIndex[tempL[GC.RNG.Next(3)]]].GetComponent<CardScript>(); ;
        if (CS.FactionID == 1)
        {
            GC.BoonCounts[0]++;
            GC.ShowBoon();
        }
    }

    public void ShowHand()
    {
        List<int> tempL = new List<int> { 0, 1, 2, 3 };
        tempL.Remove(HandIndex);
        GameControl GC = GetComponent<GameControl>();
        int HIndex = tempL[GC.RNG.Next(3)];
        foreach (CardScript c in GC.PlayerHands[HIndex])
        {
            if (c.FactionID != 1)
                GC.BoonCounts[HandIndex]++;
        }
        GC.ShowBoon();
    }



	
	// Update is called once per frame
	void Update () {
	
	}
}
