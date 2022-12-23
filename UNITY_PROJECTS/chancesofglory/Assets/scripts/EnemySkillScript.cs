using UnityEngine;
using System.Collections;

public class EnemySkillScript : MonoBehaviour {

    public int SkillID;

	// Use this for initialization
	void Start () {
	
	}

    public void Execute()
    {
        switch(SkillID)
        {
            case 1:
                if(!CheckPlayerMatch(transform.GetChild(2).GetComponent<DieScript>().id))
                {
                    GameControl.singleton.MessageText.text = "The curse of the forest is felt.";
                    foreach (GameObject g in GameControl.singleton.SelectedCharacters)
                        g.GetComponent<StatScript>().UpdateHP(1);
                }
                if (!CheckPlayerMatch(transform.GetChild(3).GetComponent<DieScript>().id))
                {
                    GameControl.singleton.MessageText.text = "The curse of the forest is felt.";
                    foreach (GameObject g in GameControl.singleton.SelectedCharacters)
                        g.GetComponent<StatScript>().UpdateHP(1);
                }
                transform.GetChild(2).GetComponent<DieScript>().Rollit();
                transform.GetChild(3).GetComponent<DieScript>().Rollit();
                break;
        }
    }

    bool CheckPlayerMatch(int D)
    {
        foreach(int i in DiceControl.singleton.LastPlayerDice)
        {
            if (i == D)
                return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
