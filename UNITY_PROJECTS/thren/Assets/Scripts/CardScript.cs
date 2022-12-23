using UnityEngine;
using System;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {

    public string NameText;
    public string RuleText;
    public int Power;
    public int FactionID;
    public int ID;
    public Action Ability;
    public int HandIndex;
    public bool truth;

    void CardLookUp()
    {
        switch(ID)
        {
            case 0:
                NameText = "Operative";
                RuleText = "+1 Boon per Lie";
                Power = 1;
                FactionID = 0;
                Ability = Operative;
                break;
            case 1:
                NameText = "Interigator";
                RuleText = "+1 Accusation, Double Accusation Bonus";
                Power = 2;
                FactionID = 0;
                Ability = Interigator;
                break;
            case 2:
                NameText = "Spymaster";
                RuleText = "Look at a card, +1 Boon if it's a Rebel";
                Power=3;
                FactionID = 0;
                Ability = SpyMaster;
                break;
            case 3:
                NameText = "Elite";
                RuleText = "Give 2 strikes";
                Power=4;
                FactionID = 0;
                Ability = Elite;
                break;
            case 4:
                NameText = "Emperor";
                RuleText = "Reveal All Cards This Round";
                Power=5;
                FactionID = 0;
                Ability = Emperor;
                break;
            case 5:
                NameText = "Aestetic";
                RuleText = "Can Self Vote. Recieve boons instead of strikes this round";
                Power=5;
                FactionID = 2;
                Ability = Aestetic;
                break;
            case 6:
                NameText = "Medic";
                RuleText = "Remove all strikes";
                Power=2;
                FactionID = 2;
                Ability = Medic;
                break;
            case 7:
                NameText = "Bounty Hunter";
                RuleText = "+2 Boons if voted for different factions";
                Power=3;
                FactionID = 2;
                Ability = BountyHunter;
                break;
            case 8:
                NameText = "Mercenary";
                RuleText = "Double Victory Points from Boons. Ignore Strikes.";
                Power=4;
                FactionID = 2;
                Ability = Mercenary;
                break;
            case 9:
                NameText = "Cleric";
                RuleText = "+1 Boon per Truth";
                Power=1;
                FactionID = 2;
                Ability = Cleric;
                break;
            case 10:
                NameText = "Gavedigger";
                RuleText = "Gain 2 boons and 1 strike";
                Power=6;
                FactionID = 2;
                Ability = GraveDigger;
                break;
            case 11:
                NameText = "Scout";
                RuleText = "Look at a card, +1 Victory point if it's a Rebel";
                Power=1;
                FactionID = 1;
                Ability = Scout;
                break;
            case 12:
                NameText = "Tactician";
                RuleText = "Reveal this and skip the Ability Phase";
                Power=2;
                FactionID = 1;
                Ability = Tactician;
                break;
            case 13:
                NameText = "Surveyor";
                RuleText = "Look at hand, +1 Boon per Non-Rebel";
                Power=3;
                FactionID = 1;
                Ability = Surveyor;
                break;
            case 14:
                NameText = "Soldier";
                RuleText = "Select a weaker Rebel in your hand and gain boons equal to its power";
                Power=4;
                FactionID = 1;
                Ability = Soldier;
                break;
            case 15:
                NameText = "Resistance Leader";
                RuleText = "+1 Victory Point per Rebel Played";
                Power=5;
                FactionID = 1;
                Ability = RebelLeader;
                break;


        }
    }

	// Use this for initialization
	void Start () {
        CardLookUp();
        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = NameText;
        transform.GetChild(1).GetChild(1).GetComponent<Text>().text = Power.ToString();
        transform.GetChild(1).GetChild(2).GetComponent<Text>().text = RuleText;
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GC.SpriteIcons[FactionID];
    }

    void Operative()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.BoonCounts[HandIndex] += GC.LieCounts[HandIndex];
        GC.ShowBoon();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.Accuse);
        }
    }

    void Interigator()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
        {
            GC.CardSelectionCounter = 1;
            GC.UpdateMode(GameControl.Mode.Accuse);
        }
        else
        {
            GC.GetComponents<BotControl>()[HandIndex - 1].Accuse();
        }
    }

    void SpyMaster()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.Reveal);
        }
        else
        {
            GC.GetComponents<BotControl>()[HandIndex - 1].Reveal();
        }
    }

    void Elite()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
        {
            GC.CardSelectionCounter = 1;
            GC.UpdateMode(GameControl.Mode.GiveStrike);
        }
        else
        {
            GC.GetComponents<BotControl>()[HandIndex - 1].GiveStrike();
            GC.GetComponents<BotControl>()[HandIndex - 1].GiveStrike();
        }
    }

    void Emperor()
    {
        //checked for in RoundPlay
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
            GC.UpdateMode(GameControl.Mode.Accuse);
    }

    void Aestetic()
    {
        //resolved in CleanUpRound
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
            GC.UpdateMode(GameControl.Mode.Accuse);
    }

    void Medic()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.VictoryPoints[HandIndex] += GC.StrikeTotals[HandIndex];
        GC.StrikeTotals[HandIndex] = 0;
        GC.ShowVP();

        if (HandIndex == 0)
        {
          GC.UpdateMode(GameControl.Mode.Accuse);
        }
    }

    void BountyHunter()
    {
        //resolved in CleanUpRound
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
            GC.UpdateMode(GameControl.Mode.Accuse);
    }

    void Mercenary()
    {
        //resolved in CleanUpRound
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
            GC.UpdateMode(GameControl.Mode.Accuse);
    }

    void Cleric()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.BoonCounts[HandIndex] = 3 - GC.PlayerHands[HandIndex].Count - GC.LieCounts[HandIndex];
        GC.ShowBoon();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.Accuse);
        }
    }

    void GraveDigger()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.BoonCounts[HandIndex] += 2;
        GC.StrikeCounts[HandIndex] += 1;
        GC.ShowBoon();
        GC.ShowStrikes();
        if (HandIndex == 0)
            GC.UpdateMode(GameControl.Mode.Accuse);
    }

    void Scout()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.Reveal);
        }
        else
        {
            GC.GetComponents<BotControl>()[HandIndex - 1].Reveal();
        }
    }

    void Surveyor()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.ShowHand);
        }
        else
        {
            GC.GetComponents<BotControl>()[HandIndex - 1].ShowHand();
        }
    }

    void Tactician()
    {
        //resolved in PlayRound
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
            GC.UpdateMode(GameControl.Mode.Accuse);
    }

    void Soldier()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.AddSoldierPower);
        }
        else
        {
            GC.GetComponents<BotControl>()[HandIndex - 1].AddSoldierPower();
        }
    }

    void RebelLeader()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.BoonCounts[HandIndex] += GC.RebelCounts[HandIndex];
        GC.ShowBoon();
        if (HandIndex == 0)
        {
            GC.UpdateMode(GameControl.Mode.Accuse);
        }
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
