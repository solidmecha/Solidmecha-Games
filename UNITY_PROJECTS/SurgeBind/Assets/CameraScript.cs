using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject player;
	float lvl; //vertical level

	// Use this for initialization
	void Start () {
		lvl = 0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position=new Vector3(player.transform.position.x, 9.81f*lvl, -10);


//
//		if (player.transform.position.y >= 8.8f * (lvl + 1f)) 
//		{
//		
//			lvl++;
//			transform.position=Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, 9.81f*lvl, -10), );
//		}
	}
}
