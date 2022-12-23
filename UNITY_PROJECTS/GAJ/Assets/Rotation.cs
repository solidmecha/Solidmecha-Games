using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {
	float rotationMod;
	public float Rspeed=20f;
	// Use this for initialization
	void Start () {
		switch (name) 
		{
		case "Mercury":
		rotationMod=365.25f/87.97f;
			break;
		case "Venus":
		rotationMod=1f/0.62f;
			break;
		case "Earth":
		rotationMod=1;
			break;
		case "Mars":
		rotationMod=365f/687f;
			break;
		case "Jupiter":
		rotationMod=1f/11.9f;
			break;

		case "Saturn":
				rotationMod=1f/29.7f;
			break;}
	
	}
	
	// Update is called once per frame
	void Update () {
		float temp = transform.rotation.z;
		Vector3 rotate = new Vector3 (0, 0, temp + rotationMod*Time.time*Rspeed);
		gameObject.transform.rotation=Quaternion.Euler(rotate);
	
	}
}
