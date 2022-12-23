using UnityEngine;
using System.Collections;

public class InteractScript : MonoBehaviour {

    public int MenuIndex; //index on WorldControl InteractableMenus
    public int DescriptionID; //buy, trade, type#
    public int UnitID;
    public int Value;
    public bool NowOrNever;
    public const int DescriptionCount = 5;

    public void ShowRecruitMentMenu()
    {
        Destroy(WorldControl.singleton.CurrentWorldMenu);
        WorldControl.singleton.CurrentWorldMenu=Instantiate(WorldControl.singleton.InteractableMenus[1]);
        WorldControl.singleton.CurrentWorldMenu.transform.position = WorldControl.singleton.StatsLoc.position;
        WorldControl.singleton.CurrentWorldMenu.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GetRecruitmentDescription();
        WorldControl.singleton.CurrentWorldMenu.transform.GetChild(0).GetChild(1).GetComponent<WorldMenuButton>().ActionName = "AddUnit";
        WorldControl.singleton.CurrentWorldMenu.transform.GetChild(0).GetChild(2).GetComponent<WorldMenuButton>().ActionName = "DeclineUnit";
    }

    public string GetRecruitmentDescription()
    {
        if (DescriptionID != 2 && PlayerScript.singleton.Team.Count == 10)
        {
            return "Come back when you've dismissed someone";
        }
        else
        {
            switch (DescriptionID)
            {
                case 1:
                    return name + " will join for " + GetComponent<BehaviourScript>().Cost + ". You have " + PlayerScript.singleton.Money + ".";
                case 2:
                    if (PlayerScript.singleton.Team.Count > UnitID)
                    {
                        NowOrNever = true;
                        return name + " will trade for " + PlayerScript.singleton.Team[UnitID].name + ". Now or Never.";
                    }
                    else
                        return "Come back when you have more Team Members.";
                case 3:
                    return name + " will join if you have at least " + Value +" "+ SkillScript.TypeByID(UnitID)+" units.";
                case 4:
                    return name + " will join if you have no " + SkillScript.TypeByID(UnitID) + " units";
                default: return name + " will join you if you ask.";
            }
        }
    }

    public bool ConditionMet()
    {
        if (DescriptionID != 2 && PlayerScript.singleton.Team.Count == 10)
            return false;
            switch (DescriptionID)
        {
            case 1:
                if (PlayerScript.singleton.Money >= GetComponent<BehaviourScript>().Cost)
                {
                    PlayerScript.singleton.Money -= GetComponent<BehaviourScript>().Cost;
                    return true;
                }
                else return false;
            case 2:
                if (PlayerScript.singleton.Team.Count > UnitID)
                {
                    for (int i = UnitID + 1; i < PlayerScript.singleton.Team.Count; i++)
                        PlayerScript.singleton.Team[i].Index--;
                    PlayerScript.singleton.Team.RemoveAt(UnitID);
                    return true;
                }
                else
                    return false;
            case 3:
                return Value <= PlayerScript.singleton.TypeCount(UnitID);
            case 4:
                return PlayerScript.singleton.TypeCount(UnitID) == 0;
            default:
                return true;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
