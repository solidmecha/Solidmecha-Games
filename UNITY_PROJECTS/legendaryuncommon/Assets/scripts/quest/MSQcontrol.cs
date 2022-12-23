using UnityEngine;
using System.Collections;

public class MSQcontrol : MonoBehaviour {

    public string description;
    public GameControl.Goal goal;
    public int GoalCounter;
    public int GoalTarget;
    public int AttrIndex;

    private void OnMouseEnter()
    {
        if(GameControl.singleton.ShowPreviews)
            GameControl.singleton.PreviewMsg.text = description;
    }

    private void OnMouseExit()
    {
        if (GameControl.singleton.ShowPreviews)
            GameControl.singleton.PreviewMsg.text = "";
    }

    void CheckGoalStatus()
    {
                if (GoalCounter >= GoalTarget)
                    ResolveGoal();
    }

    void ResolveGoal()
    {
        description = "Defeat the boss";
        goal = GameControl.Goal.DefeatBoss;
        AttrIndex = transform.GetSiblingIndex();
        GameControl.singleton.CreateBoss(transform.GetSiblingIndex());
    }

    public void UpdateGoalCounter(GameControl.Goal G, int val)
    {
        if (goal == G)
        {
            GoalCounter += val;
        }
        CheckGoalStatus();
        UpdateDescription();
    }

    void UpdateDescription()
    {
        switch(goal)
        {
            case GameControl.Goal.TotalSilver:
                description="Have " + GoalTarget.ToString()+ " silver. ("+ GoalCounter.ToString()+")";
                break;
            case GameControl.Goal.GainedSilver:
                description = "collect a total of " + GoalTarget.ToString() + " silver. (" + GoalCounter.ToString() + ")";
                break;
            case GameControl.Goal.TotalPartyFourAttr:
                description = "Have a total of " + GoalTarget.ToString() + " attributes across your party. (" + GoalCounter.ToString() + ")";
                break;
            case GameControl.Goal.TotalPartyOneAttr:
                description = "Have a total of " + GoalTarget.ToString()+" "+ GameControl.singleton.VirtueNames[AttrIndex]+ ". (" + GoalCounter.ToString() + ")";
                break;
            case GameControl.Goal.SingleAttr:
                description = "Have a total of " + GoalTarget.ToString() + " " + GameControl.singleton.VirtueNames[AttrIndex] + ". (" + GoalCounter.ToString() + ")";
                break;
            case GameControl.Goal.DemonsDefeated:
                break;
            case GameControl.Goal.fullAwakening:
                break;
            case GameControl.Goal.TotalAwake:
                break;
            case GameControl.Goal.TotalQuests:
                break;
            case GameControl.Goal.TotalTrain:
                break;
        }
    }



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
