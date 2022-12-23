using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiscServer : NetworkBehaviour {
    public GameObject PlayerObject;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                NetworkManager.singleton.playerPrefab = PlayerObject;
                NetworkManager.singleton.ServerChangeScene("Main");
            }
            else if (Input.GetKeyDown(KeyCode.F6))
                NetworkManager.singleton.ServerChangeScene("disc");
        }
    }
}
