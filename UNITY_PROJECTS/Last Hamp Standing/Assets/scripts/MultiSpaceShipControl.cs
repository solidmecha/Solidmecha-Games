using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiSpaceShipControl : NetworkBehaviour {

    [SyncVar]
    public int ControllerID;
    [SyncVar]
    public int Index;

    public bool SelectShip(int ID)
    {
        MultiSpaceControl MC = GameObject.FindGameObjectWithTag("MultiControl").GetComponent<MultiSpaceControl>();
        if (ControllerID == ID && MC.PlayerTurn == ID && MC.ShipMove)
        {
            if (MC.SelectedShip != null)
                MC.ClearMovement();
            MC.DisplayMovement((int)transform.position.x, (int)transform.position.y);
            MC.SelectedShip = gameObject;
            return true;
        }
        else
            return false;

    }

    [ClientRpc]
    public void RpcUpdateName(string s)
    {
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = s;
    }

    [ClientRpc]
    public void RpcUpdateColor(Color c)
    {
       GetComponent<SpriteRenderer>().color = c;
    }

    [Command]
    public void CmdMove(Vector2 v)
    {
        transform.position = v;
        RpcUpdatePosition(v);
    }

    [ClientRpc]
    public void RpcUpdatePosition(Vector2 V)
    {
       transform.position = V;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
