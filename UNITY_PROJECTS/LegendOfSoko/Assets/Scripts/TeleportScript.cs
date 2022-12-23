using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    public Vector2 TelePos;



    // Use this for initialization
    void Start () {
        TelePos = transform.GetChild(0).position;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
