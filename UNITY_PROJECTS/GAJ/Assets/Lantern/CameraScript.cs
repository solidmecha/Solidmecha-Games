using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(transform.localPosition.z!=-10f)
	transform.localPosition=new Vector3(player.transform.position.x,player.transform.position.y,-10f);
	}
}
