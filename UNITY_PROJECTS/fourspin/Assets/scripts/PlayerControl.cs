using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.Space))
        {
            GameScript.singleton.SpawnBlock();
        }
    else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            GameScript.singleton.Rotate(90);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            GameScript.singleton.Rotate(-90);
        }
    }
}
