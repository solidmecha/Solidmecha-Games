using UnityEngine;
using System.Collections.Generic;

public class colonist : MonoBehaviour {

  public string Name;
  System.Random RNG; 
	public float spawnTime;
	public float senseRange; 
	public float energy;
	public float decay;
	public int hydration;
	public int groupingStyle; //colony, pack
	public string foodType;  //plant, animal
	public int activeTime;
	public float speed;
public List<Vector3> foodLocations=new List<Vector3>{};
public GameObject[] foodArray;


public bool hasDestination;
public Vector3 Destination;

	// Use this for initialization
	void Start () {
		foodArray=GameObject.FindGameObjectsWithTag(foodType);
		findFood();
		RNG=new System.Random(ThreadSafeRandom.Next());
	}

	void findFood()
	{
		foreach(GameObject g in foodArray)
		{
			foodLocations.Add(g.transform.position);
		}
	}

	void OmNomNom(GameObject food)
	{
		colonist foodColonist=(colonist)food.GetComponent(typeof(colonist));
		energy+=foodColonist.energy;
		Destroy(food);
	}

	// Update is called once per frame
	void Update () {
		energy-=decay*Time.deltaTime;
		if(energy<=0)
		{
			Destroy(gameObject);
		}
		if(!hasDestination)
		{
			Vector3 dir=new Vector3(RNG.Next(-100, 100),RNG.Next(-100, 100), 0);
			transform.Translate(dir.normalized*speed*Time.deltaTime);
		}
		else
		{
			Vector3 dir=Destination-transform.position;
			transform.Translate(dir.normalized*speed*Time.deltaTime);
		}

		if(foodLocations.Count>0 && Vector3.Distance(transform.position, foodLocations[0])<=.1)
		{
			if(foodArray[0])
			OmNomNom(foodArray[0]);
		}
	
	}

}
