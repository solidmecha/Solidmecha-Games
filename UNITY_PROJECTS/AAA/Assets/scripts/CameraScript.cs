using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject Ship;
	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector2(Ship.transform.position.x-transform.position.x,Ship.transform.position.y-transform.position.y)*speed*Time.deltaTime);
	}
}
