using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

    public int index;
    public int[] Regen;
    string RegenDescription;

    private void OnMouseEnter()
    {
        if (GameControl.singleton.CurrentState == GameControl.GameState.TeamSelect)
        {
            GameControl.singleton.MessageText.text = "Click to select " + name+". "+RegenDescription;
        }
        if (GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
        {
            if(DiceControl.singleton.CheckSameMatch(1))
            {
                GameControl.singleton.MessageText.text="Give " + (DiceControl.singleton.SelectedDiceTypes.Count*2).ToString() + " mana.";
            }
            else
                GameControl.singleton.MessageText.text = "Click to target " + name;
        }
    }

    private void OnMouseDown()
    {
        if (GameControl.singleton.CurrentState == GameControl.GameState.TeamSelect)
        {
            index = GameControl.singleton.SelectedCharacters.Count;
            transform.position = new Vector3(100, 100, 100);
            if (name.Equals("Fighter"))
            {
                GameControl.singleton.ResourceGeneratingCharacterIndicies[0] = index;
            }
            else if (name.Equals("Diviner"))
            {
                GameControl.singleton.ResourceGeneratingCharacterIndicies[1] = index;
            }
            GameControl.singleton.SelectedCharacters.Add(gameObject);
            GameControl.singleton.CharacterOptions.Remove(gameObject);
            GameControl.singleton.NextTeamSelect();
        }
        if (GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
        {
            if (DiceControl.singleton.CheckSameMatch(1))
            {
                GetComponent<StatScript>().UpdateMP(-2 * DiceControl.singleton.SelectedDiceTypes.Count);
                DiceControl.singleton.DestroyUsedDice();
                GameControl.singleton.MessageText.text = "Click to target " + name;
            }
            else
            {
                GameControl.singleton.Target = gameObject;
                GameControl.singleton.TargetText.text = "Current Target: " + name;
            }
        }
    }

    // Use this for initialization
    void Start () {
        name = name.Substring(0, name.Length-7);
        int r = GameControl.singleton.RNG.Next(4);
        Regen[r] = GameControl.singleton.RNG.Next(1, 3);
        if (r == 3)
            RegenDescription = "Skills recover 1 CD per turn.";
        else if(r==0)
        {
            RegenDescription = "Recovers " + Regen[r].ToString() + " HP per turn.";
        }
        else if(r==1)
        {
            RegenDescription = "Gain " + Regen[r].ToString() + " Armor per turn.";
        }
        else
            RegenDescription = "Gain " + Regen[r].ToString() + " Mana per turn.";
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
