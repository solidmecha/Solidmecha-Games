using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ColorSync : NetworkBehaviour {

    [ClientRpc]
    public void RpcSetColor(Color c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
