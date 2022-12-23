using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ColorButton : NetworkBehaviour {

    public int ID;
    public PlayerControl p;

    public void SetColor()
    {
        p.CmdSyncPlayerColors();
        if (GameControl.singleton.PlayerColorAvailble(ID))
        {
            p.CmdSetPlayerColor(ID);
            p.ColorID = ID;
            p.ShowNameCanvas();
            GameObject.FindGameObjectWithTag("NetObject").GetComponent<NetworkManagerHUD>().showGUI = false;
            Destroy(transform.root.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
