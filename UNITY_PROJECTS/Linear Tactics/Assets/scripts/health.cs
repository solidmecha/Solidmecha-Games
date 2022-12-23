using UnityEngine;
using System.Collections;

public class health : MonoBehaviour {

	public int hp;
	public int armor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			Destroy(gameObject);
		}
	
	}
}
