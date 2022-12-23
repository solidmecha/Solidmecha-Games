using UnityEngine;
using System.Collections;

public class TorpedoScript : MonoBehaviour {

	public float speed;
	public float lifeTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector2.up*speed*Time.deltaTime);

		lifeTime=lifeTime-Time.deltaTime;
		if(lifeTime<0)
		{Destroy(gameObject);}
	}
}
