using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {

    private void OnMouseDown()
    {
        transform.SetAsFirstSibling();
        transform.root.GetComponent<PlayerControl>().ChangePlayer();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
