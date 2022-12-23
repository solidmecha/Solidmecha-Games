using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class BattleButtonScript : MonoBehaviour {

    public Action Action;
    public int Index;
    public List<BehaviourScript> Targets = new List<BehaviourScript> { };
    // Use this for initialization
    void Start() {
        GetComponent<Button>().onClick.AddListener(delegate { SetAction(); });
    }

    void SetAction()
    {
        if (BattleScript.singleton.HasMana(Index))
        {
            BattleScript.singleton.Units[BattleScript.singleton.UnitIndex].PayMana(BattleScript.singleton.Units[BattleScript.singleton.UnitIndex].Skills[Index].ManaCost);
            BattleScript.singleton.ActiveSkillIndex = Index;
            BattleScript.singleton.SetTargets(Targets);
            Action();
            if (BattleScript.singleton.inBattle)
                BattleScript.singleton.NextTurn();
            Destroy(transform.root.gameObject);
            //  BattleScript.singleton.ChooseTarget();
        }
    }

    public void ShowSkillDesc()
    {
        if (WorldControl.singleton.LiveStatScreen != null)
            Destroy(WorldControl.singleton.LiveStatScreen);
        GameObject go = Instantiate(WorldControl.singleton.StatScreen) as GameObject;
        go.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = BattleScript.singleton.Units[BattleScript.singleton.UnitIndex].Skills[Index].Name;
        go.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Text>().text = BattleScript.singleton.Units[BattleScript.singleton.UnitIndex].Skills[Index].ManaCost + " MP";
        go.transform.GetChild(0).GetChild(4).GetComponent<UnityEngine.UI.Text>().text = BattleScript.singleton.Units[BattleScript.singleton.UnitIndex].Skills[Index].Description;
        go.transform.position = WorldControl.singleton.StatsLoc.position;
        WorldControl.singleton.LiveStatScreen = go;
    }

    public void  StopSkillDesc()
    {
        if (WorldControl.singleton.LiveStatScreen != null)
            Destroy(WorldControl.singleton.LiveStatScreen);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
