using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleMenuScript : MonoBehaviour {

    public BehaviourScript BS;
    public Text Name;
    public Text[] Abilities;

	// Use this for initialization
	void Start () {
        //Name.text = BS.name;
        for (int i = 0; i < 4; i++)
        {
            Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Index = i;
            Abilities[i].text = BS.Skills[i].Name;
            Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Action = BS.Skills[i].Action;
            //Find Targets of the ability
            if (BS.Skills[i].TargetCount==0)
            {
                if(!BS.Skills[i].Support)
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, BS.Skills[i].Ranged, !BS.PlayerControlled));
                else
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, BS.Skills[i].Ranged, BS.PlayerControlled));
            }
            else if(BS.Skills[i].TargetCount==2)
            {
                if (!BS.Skills[i].Support)
                {
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, BS.Skills[i].Ranged, !BS.PlayerControlled));
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, !BS.Skills[i].Ranged, !BS.PlayerControlled));
                }
                else
                {
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, BS.Skills[i].Ranged, BS.PlayerControlled));
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, !BS.Skills[i].Ranged, BS.PlayerControlled));
                }
            }
            else if(BS.Skills[i].TargetCount==3)
            {
                if (!BS.Skills[i].Support)
                {
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, BS.Skills[i].Ranged, !BS.PlayerControlled));
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID + 1, BS.Skills[i].Ranged, !BS.PlayerControlled));
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID - 1, BS.Skills[i].Ranged, !BS.PlayerControlled));
                }
                else
                {
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, BS.Skills[i].Ranged, BS.PlayerControlled));
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID + 1, BS.Skills[i].Ranged, BS.PlayerControlled));
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID - 1, BS.Skills[i].Ranged, BS.PlayerControlled));
                }
            }
            else if (BS.Skills[i].TargetCount == 5)
            {
                if(!BS.Skills[i].Support)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(j, BS.Skills[i].Ranged, !BS.PlayerControlled));
                    }
                }
                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(j, BS.Skills[i].Ranged, BS.PlayerControlled));
                    }
                }
            }
            for (int j = Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Count-1; j >-1; j--)
            {
                if (Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets[j] == null)
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.RemoveAt(j);
            }
            if(Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Count==0)
            {
                if (!BS.Skills[i].Support)
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, !BS.Skills[i].Ranged, !BS.PlayerControlled));
                else
                    Abilities[i].transform.parent.GetComponent<BattleButtonScript>().Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, !BS.Skills[i].Ranged, BS.PlayerControlled));
            }
        }
        //HP.text = BS.HP[0] + " / " + BS.HP[1];
        //MP.text= BS.MP[0] + " / " + BS.MP[1];
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
