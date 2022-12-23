using UnityEngine;
using System.Collections;

public class checkpointScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name.Equals("Player"))
		{
			GetComponent<Renderer>().material.color=Color.green;
			GameManager.checkpoint=gameObject;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
