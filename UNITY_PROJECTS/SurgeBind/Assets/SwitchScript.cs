using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	public GameObject movingWall;
	public GameObject startPos;
	public GameObject endPos;

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.name.Equals ("Player")) {
			transform.localEulerAngles = new Vector3 (0, 0, transform.localEulerAngles.z + 180);
			if (movingWall.transform.position.x == startPos.transform.position.x && movingWall.transform.position.y == startPos.transform.position.y) {
				movingWall.transform.position = endPos.transform.position;
			} else {
				movingWall.transform.position = startPos.transform.position;
			}
	

			Destroy (other.gameObject);
		}


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
