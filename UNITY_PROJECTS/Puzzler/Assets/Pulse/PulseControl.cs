using UnityEngine;
using System.Collections;

public class PulseControl : MonoBehaviour {
	public float inc;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale=new Vector2(transform.localScale.x+inc * Time.deltaTime, transform.localScale.y+inc * Time.deltaTime);
	}
}
