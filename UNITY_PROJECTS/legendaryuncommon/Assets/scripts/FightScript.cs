using UnityEngine;
using System.Collections;

public class FightScript : MonoBehaviour {

    public void Fight()
    {
        GameControl.singleton.CurrentState = GameControl.Gamestate.Fighting;
        GameControl.singleton.PreviewMsg.text = "Select target.";
        GameControl.singleton.MoveActions(new Vector2(100, 100));

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
