
using UnityEngine;
using System.Collections;

public class QuestScript : MonoBehaviour {

    public GameObject Outlines;
    public string tooltip;
    public int[] QuestIDs;
    public int[] Difficulty;

    public void Quest()
    {
        GameControl.singleton.ShowPreviews = false;
        GameControl.singleton.CurrentState = GameControl.Gamestate.Quest;
        foreach (FadeIn f in Outlines.GetComponentsInChildren<FadeIn>())
            f.fading = true;
        for(int i=0;i<4;i++)
            GenerateQuests(i);
        GameControl.singleton.PreviewMsg.text = tooltip;
    }

    public void GenerateQuests(int index)
    {
        int QuestID = GameControl.singleton.RNG.Next(9);
        QuestIDs[index] = QuestID;
        Difficulty[index] = GameControl.singleton.RNG.Next(4);
        Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip = Description(4*index+Difficulty[index])+" Complete this " + difficultyLookup(Difficulty[index]) + " to be rewarded with ";
        switch (QuestID)
        {
            case 0:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "silver.";
                break;
            case 1:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "increased wisdom.";
                break;
            case 2:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "increased courage.";
                break;
            case 3:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "increased charisma.";
                break;
            case 4:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "increased kindness.";
                break;
            case 5:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "healing.";
                break;
            case 6:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "silver.";
                break;
            case 7:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "silver.";
                break;
            case 8:
                Outlines.transform.GetChild(index).GetComponent<TrainSelect>().tooltip += "silver.";
                break;
        }
    }

    public string Description(int ID)
    {
        switch(ID)
        {
            case 0:
                return "Read a book.";
            case 1:
                return "Formulate a theory.";
            case 2:
                return "Help solve problems.";
            case 3:
                return "Negotiate a peace treaty.";
            case 4:
                return "Show off your bravado.";
            case 5:
                return "Protect the village.";
            case 6:
                return "Stop some pirates.";
            case 7:
                return "Overthrow a tyrant.";
            case 8:
                return "Go to a swanky party.";
            case 9:
                return "Give a public speach.";
            case 10:
                return "Fundraise for a good cause.";
            case 11:
                return "Gain favor with lords and ladies at court.";
            case 12:
                return "Rescue lost fluffy animals.";
            case 13:
                return "Track down missing people.";
            case 14:
                return "Provide transportation with your ship.";
            case 15:
                return "Help cook delicious meals.";
            default:
                return "";
        }
    }

    public void ResolveOutcome(int outcome, int index)
    {
        int rank = DetermineRank(outcome, index);
        switch (rank)
        {
            case 0:
                GameControl.singleton.DisplayNotice("Rank C?", 1f, Color.red);
                break;
            case 1:
                GameControl.singleton.DisplayNotice("Rank B...", 1f, Color.magenta);
                break;
            case 2:
                GameControl.singleton.DisplayNotice("Rank A", 1f, Color.white);
                break;
            case 3:
                GameControl.singleton.DisplayNotice("Rank S!", 1f, Color.green);
                break;
        }
        switch(QuestIDs[index])
        {
            case 1:
                GameControl.singleton.PreviewMsg.text = "Gained " + RewardVirtue(rank, 0, Difficulty[index])+ " wisdom.";
                break;
            case 2:
                GameControl.singleton.PreviewMsg.text = "Gained " + RewardVirtue(rank, 1, Difficulty[index]) + " courage.";
                break;
            case 3:
                GameControl.singleton.PreviewMsg.text = "Gained " + RewardVirtue(rank, 2, Difficulty[index]) + " charisma.";
                break;
            case 4:
                GameControl.singleton.PreviewMsg.text = "Gained " + RewardVirtue(rank, 3, Difficulty[index]) + " kindness.";
                break;
            case 5:
                heal(rank);
                GameControl.singleton.PreviewMsg.text = "You feel better.";
                break;
            default:
                GameControl.singleton.PreviewMsg.text = "Gained " + RewardSilver(rank, Difficulty[index]).ToString() + " silver.";
                break;

        }
    }

    public int DetermineRank(int outcome, int index)
    {
        switch(Difficulty[index])
        {
            case 0: //easy
                if (outcome > 55)
                    return 3;
                else if (outcome > 35)
                    return 2;
                else if (outcome > 15)
                    return 1;
                else
                    return 0;
            case 1: //moderate
                if (outcome > 70)
                    return 3;
                else if (outcome > 50)
                    return 2;
                else if (outcome > 30)
                    return 1;
                else
                    return 0;
            case 2: //difficult
                if (outcome > 85)
                    return 3;
                else if (outcome > 65)
                    return 2;
                else if (outcome > 45)
                    return 1;
                else
                    return 0;
            case 3: //difficult
                if (outcome > 95)
                    return 3;
                else if (outcome > 75)
                    return 2;
                else if (outcome > 55)
                    return 1;
                else
                    return 0;
            default:
                return 0;
               
        }
    }

    public int RewardSilver(int rank, int diff)
    {
       int val = GameControl.singleton.RNG.Next(50 + 100 * rank+10*diff, 100 + 100 * rank+20*diff);
        GameControl.singleton.UpdateSilver(val);
       return val;
    }

    public int RewardVirtue(int rank, int index, int diff)
    {
        int val = GameControl.singleton.RNG.Next(1 + 2 * rank+1*(diff), 2 + 2 * rank+2*diff);
        GameControl.singleton.SelectedCard.Stats[index] += val;
        print(val);
        GameControl.singleton.ShowSelectedCard();
        return val;
    }

    public void heal(int rank)
    {
        switch(rank)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    public string difficultyLookup(int diffID)
    {
        switch(diffID)
        {
            case 0:
                return "easy task";
            case 1:
                return "moderate challenge";
            case 2:
                return "difficult trial";
            case 3:
                return "near impossible feat";
            default:
                return "";
        }
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
