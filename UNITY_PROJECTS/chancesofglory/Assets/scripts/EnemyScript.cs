using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public int DiceCount;
    public int[] Effectiveness;
    public int[] BonusDamage;
    public bool hasSpecialAtk;

    private void OnMouseEnter()
    {
        if (GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
            GameControl.singleton.MessageText.text = "Click to target " + name;
    }

    private void OnMouseDown()
    {
        if (GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
        {
            GameControl.singleton.Target=gameObject;
            GameControl.singleton.TargetText.text = "Current Target: " + name;
        }
    }

    public void AdditionalDiceEffects(int index)
    {

    }

    // Use this for initialization
    void Start () {
        name = name.Substring(0, name.Length - 7);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
