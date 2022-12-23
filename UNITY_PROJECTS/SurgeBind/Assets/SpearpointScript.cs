using UnityEngine;
using System.Collections;

public class SpearpointScript : MonoBehaviour {

	GameObject teamLeader;
	GameManager gameManager;
	// Use this for initialization
	void Start () {
		teamLeader = GameObject.Find ("TeamLeader");
		gameManager = (GameManager)teamLeader.GetComponent (typeof(GameManager));
	}

	void OnCollisionEnter2D (Collision2D other)
	{
				if (other.gameObject.name.Equals ("Player")) {
						gameManager.hScripts[gameManager.activeHeart].takeDamage ();
			gameManager.activeHeart++;
				}
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
