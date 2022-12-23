using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static int curlvl;
	public static int frameCount;
	public static bool WallLashed;
	public static bool inverted;
	public static bool lashForward;
	public static bool canLashObj;
	public static GameObject checkpoint;

	public GameObject player;
	public GameObject[] Hearts;
	public HeartScript[] hScripts;
	public  int activeHeart;

	void Awake()
	{	
	
		}

	// Use this for initialization
	void Start () {
		frameCount = 0;
		lashForward = false;
		WallLashed = false;
		inverted = false;
		canLashObj=false;
	Hearts=GameObject.FindGameObjectsWithTag("heart");
		hScripts = new HeartScript[Hearts.Length];
		for (int i=0; i<Hearts.Length; i++) 
		{
			hScripts[i] = (HeartScript)Hearts[i].GetComponent (typeof(HeartScript));
		}
		activeHeart = 0;
	}
	
	// Update is called once per frame
	void Update () {
		frameCount++;

		if (activeHeart == Hearts.Length) 
		{
			player.transform.position=checkpoint.transform.position;

			activeHeart=0;
			foreach(HeartScript h in hScripts)
			{
				h.heal();
			}

		}

		if(frameCount>100)
		{
			//hScript.heal();
		}
		if (frameCount > 179)
		{frameCount = 0;
		//	hScript.takeDamage();
		}

	
	}
}
