using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameButton : MonoBehaviour {

    public PlayerControl p;

    public void SubmitName()
    {
        string N = transform.root.GetChild(0).GetComponent<UnityEngine.UI.InputField>().text;
        p.PlayerName = N;
        p.CmdSetPlayerName(N);
        p.isReady = false;
        Destroy(transform.root.gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
