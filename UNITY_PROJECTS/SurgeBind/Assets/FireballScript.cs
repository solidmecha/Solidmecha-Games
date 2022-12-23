using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {
	public float FireSpeed;
	public float delay;
	public int lifeTime;

	GameObject teamLeader;
	GameManager gameManager;
	// Use this for initialization
	void Start () {
	
		teamLeader = GameObject.Find ("TeamLeader");
		gameManager = (GameManager)teamLeader.GetComponent (typeof(GameManager));
		GetComponent<Rigidbody2D>().velocity = new Vector2(-FireSpeed,0);
		StartCoroutine(FireballHandler());
	}

void OnCollisionEnter2D (Collision2D other)
	{ if(other.gameObject.name.Equals("Player"))
		{
			gameManager.hScripts[gameManager.activeHeart].takeDamage ();
			gameManager.activeHeart++;
		}
		Destroy(gameObject);
	}

	IEnumerator FireballHandler()
	{
		//Debug.Log ("actually happening");
		yield return new WaitForSeconds(delay);
		//transform.Translate(new Vector2(-1*FireSpeed,0));
		lifeTime--;
		if (lifeTime == 0) {
						Destroy (gameObject);
				}
		StartCoroutine(FireballHandler());
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
