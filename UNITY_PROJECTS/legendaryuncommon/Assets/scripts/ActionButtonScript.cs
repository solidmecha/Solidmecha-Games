using UnityEngine;
using System.Collections;

public class ActionButtonScript : MonoBehaviour {

    public void Awaken()
    {
        if (GameControl.singleton.SelectedCard.Awakening < 6)
        {
            GameControl.singleton.SelectedCard.Awakening++;
            for(int i=0;i<4;i++)
                GetComponent<QuestScript>().RewardVirtue(2, i, 2);
            GameControl.singleton.SelectedCard.transform.GetChild(1).GetChild(GameControl.singleton.SelectedCard.Awakening - 1).GetComponent<SpriteRenderer>().color = Color.white;
            GameControl.singleton.SelectedCard.HP[0] = GameControl.singleton.SelectedCard.HP[1];
            GameControl.singleton.ShowSelectedCard();
        }
        GameControl.singleton.CurrentState = GameControl.Gamestate.FX;
        GameControl.singleton.AdvanceTurn(1);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
