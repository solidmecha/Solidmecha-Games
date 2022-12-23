using UnityEngine;
using System.Collections;

public class TrainSelect : MonoBehaviour {

    public string tooltip;
    
    private void OnMouseDown()
    {
        if(GameControl.singleton.CurrentState==GameControl.Gamestate.Train)
        {
            int Val = GameControl.singleton.RNG.Next(11, 21);
            GameControl.singleton.SelectedCard.Stats[transform.GetSiblingIndex()] += Val;
            GameControl.singleton.CurrentState = GameControl.Gamestate.FX;
            GameControl.singleton.ShowSelectedCard();
            GameControl.singleton.PreviewMsg.text = "Gained " + Val.ToString() + " " + tooltip;
            GameControl.singleton.AdvanceTurn(1);
            foreach (FadeIn f in transform.parent.GetComponentsInChildren<FadeIn>())
                f.stop();
        }
        else if (GameControl.singleton.CurrentState==GameControl.Gamestate.Quest)
        {
            float outcome=(float)GameControl.singleton.SelectedCard.Stats[transform.GetSiblingIndex()]/10f+ GameControl.singleton.RNG.Next(100);
            GameControl.singleton.ActionButtonObj.GetComponent<QuestScript>().ResolveOutcome((int)outcome, transform.GetSiblingIndex());
            GameControl.singleton.CurrentState = GameControl.Gamestate.FX;
            GameControl.singleton.AdvanceTurn(1);
            foreach (FadeIn f in transform.parent.GetComponentsInChildren<FadeIn>())
                f.stop();

        }

    }

    private void OnMouseEnter()
    {
        switch(GameControl.singleton.CurrentState)
        {
            case GameControl.Gamestate.Quest:
                GameControl.singleton.PreviewMsg.text = tooltip;
                break;
            default:
                break;
        }
    }

    private void OnMouseExit()
    {
        
    }



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
