using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public GameControl GC;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if(!GC.InBuildMode)
        transform.position = new Vector3(GC.Player.transform.position.x, GC.Player.transform.position.y, -10);
	}
}
