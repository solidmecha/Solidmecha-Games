using UnityEngine;
using System.Collections;

public class ActionButtonTooltipHelper : MonoBehaviour {

    public string tooltip;

    private void OnMouseEnter()
    {
        if(GameControl.singleton.CurrentState==GameControl.Gamestate.Playing)
            GameControl.singleton.PreviewMsg.text = tooltip;
    }

    private void OnMouseExit()
    {
        if (GameControl.singleton.CurrentState == GameControl.Gamestate.Playing)
            GameControl.singleton.PreviewMsg.text = "";
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
