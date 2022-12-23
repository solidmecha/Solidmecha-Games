using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public GameObject[] Entities=new GameObject[2]; 

	// Use this for initialization
	void Start () {
		makeWorld();
	}

	void makeWorld()
	{
		GameObject temp=Instantiate(Entities[1], new Vector2(0,0), Quaternion.identity) as GameObject;
		GameObject plant=Instantiate(Entities[0], new Vector2(10,10), Quaternion.identity) as GameObject;

		SpriteRenderer sr=temp.GetComponent<SpriteRenderer>();
		SpriteRenderer psr=plant.GetComponent<SpriteRenderer>();
		sr.color=Color.blue;
		psr.color=Color.green;
		colonist thisColonist=(colonist)temp.GetComponent(typeof(colonist));
		colonist plantColonist=(colonist)plant.GetComponent(typeof(colonist));
		thisColonist.speed=4f;
		thisColonist.Destination=new Vector3(10f,10f,0);
		thisColonist.hasDestination=true;
		thisColonist.energy=100f;
		thisColonist.decay=5f;
		thisColonist.foodType=plant.tag;

		plantColonist.energy=1000f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
