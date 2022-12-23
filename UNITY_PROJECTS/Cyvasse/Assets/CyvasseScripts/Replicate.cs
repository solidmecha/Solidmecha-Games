using UnityEngine;
using System.Collections;

public class Replicate : MonoBehaviour {
	GameObject thingy;
	// Use this for initialization
	void Start () {
		thingy = GameObject.Find("MoveQuad");
		copyIt ();
	
	}

	void copyIt()
	{
		for (int i=50; i>0; i--) {
			Instantiate(thingy, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+1, gameObject.transform.position.z-i/10f) , Quaternion.identity);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
