using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PocketScript : NetworkBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isServer)
            Destroy(collision.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
