using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiscBoss : NetworkBehaviour {

    int Counter;
    public UnityEngine.UI.Text Text;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isServer)
        {
            Counter++;
            RpcOnCounterChange(Counter);
        }
    }

    [ClientRpc]
    void RpcOnCounterChange(int C)
    {
           Text.text = C.ToString();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
