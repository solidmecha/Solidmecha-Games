using UnityEngine;
using System.Collections;

public class DieScript : MonoBehaviour {

    public bool isEnemyDie;
    float counter;
    float RollTime;
    public bool Rolling;
    public int id; //sword, mana, time, shield, scroll, heart
    public bool Saved;

    private void OnMouseDown()
    {
        if(GameControl.singleton.CurrentState==GameControl.GameState.PlayerRoll || GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
        {
            SaveSwap();
            if(GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
            {
                if (Saved)
                    DiceControl.singleton.SelectedDiceTypes.Add(id);
                else
                    DiceControl.singleton.SelectedDiceTypes.Remove(id);
            }
        }
    }

    // Use this for initialization
    void Start () {
        Rollit();
	}

    public void SaveSwap()
    {
        Saved = !Saved;
        if (Saved)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Rollit()
    {
        if (!Saved)
        {
            Rolling = true;
            counter = .1f;
            RollTime = .7f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        RollTime -= Time.deltaTime;
        if (RollTime <= 0f)
            Rolling = false;
        if (Rolling)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                counter = .1f;
                id = (id + GameControl.singleton.RNG.Next(5)) % 6;
                if(!isEnemyDie)
                    GetComponent<SpriteRenderer>().sprite = DiceControl.singleton.PlayerFaces[id];
                else
                    GetComponent<SpriteRenderer>().sprite = DiceControl.singleton.EnemyFaces[id];

            }
        }
	}
}
