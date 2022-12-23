using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public Vector2 targetVec;
	public float speed;
	public int dmg;
	public GameObject target;
	health targetHealth;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.name.Equals ("turret")) {
			dmg -= targetHealth.armor;
			if (dmg > 0)
				targetHealth.hp -= dmg;
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		targetHealth = (health)target.GetComponent (typeof(health));
		rigidbody2D.velocity=targetVec.normalized*speed;
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
