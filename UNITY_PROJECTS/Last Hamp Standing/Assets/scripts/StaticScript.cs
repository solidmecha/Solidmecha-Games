using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StaticScript : NetworkBehaviour {

    [SyncVar(hook ="UpdateColor")]
    public int ColorID;

    void UpdateColor(int change)
    {
        GetComponent<SpriteRenderer>().color=GameControl.singleton.Colors[change];
    }

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = GameControl.singleton.Colors[ColorID];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
