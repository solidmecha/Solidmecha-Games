using UnityEngine;
using System.Collections;

public class StarCollection : MonoBehaviour {


void OnTriggerEnter2D(Collider2D other)
{
	if(other.gameObject.name.Equals("StarPlayerPrefab(Clone)"))
	{
		Destroy(gameObject);
	}
}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
