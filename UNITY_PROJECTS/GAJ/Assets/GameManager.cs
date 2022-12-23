using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

protected Planet pluto;

public GameObject mercury;
public GameObject venus;
public GameObject earth;
public GameObject mars;
public GameObject jupiter;
public GameObject saturn;

public bool dynamicPopulation;
private Planet mercp;
private Planet venp;
private Planet earthp;
private Planet marsp;
private Planet jupp;
private Planet satp;
	// Use this for initialization
	void Start () {

	dynamicPopulation=true;
	
	mercp=(Planet) mercury.GetComponent(typeof(Planet));
	venp=(Planet) venus.GetComponent(typeof(Planet));
	earthp=(Planet) earth.GetComponent(typeof(Planet));
	marsp=(Planet) mars.GetComponent(typeof(Planet));
	jupp=(Planet) jupiter.GetComponent(typeof(Planet));
	satp=(Planet) saturn.GetComponent(typeof(Planet));
	}

	public Planet planetLookUp(int i)
	{
		switch (i)
		{
			
			case 0:
			return mercp;
			break;
			case 1:
			return venp;
			break;
			case 2:
			return earthp;
			break;
			case 3:
			return marsp;
			break;
			case 4:
			return jupp;
			break;
			case 5:
			return satp;
			break;
			case 9: return pluto; break; //never forget
			default:
			return pluto;
			break;

		}
	}
	
	// Update is called once per frame
	void Update () {
		if(dynamicPopulation)
		{
		mercp.changeResource(7,-10,11);
	}
	//	Debug.Log(mp.population);
	
	}
}
