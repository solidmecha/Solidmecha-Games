using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

public int[] Resources=new int[8]{0,0,0,0,0,0,0,0};
public float[] Income=new float[8]{0,0,0,0,0,0,0,0};
public int[] Favor=new int[6]{0,0,0,0,0,0};
public int[] FavorIncome=new int[6]{0,0,0,0,0,0};
public int[] Hostility=new int[6]{0,0,0,0,0,0};
public int[] HostilityIncome=new int[6]{0,0,0,0,0,0};

/*
0- atk
1- def
2- wealth
3- happiness
4- food/water
5- health
6- energy
7- population
*/


	// Use this for initialization
	void Start () {
		switch (name) 
		{
		case "mercury":
	changeResource(0, 10,25);
	changeResource(1, 25,75);
	changeResource(2, 25, 75);
	changeResource(3, 25, 75);
	changeResource(4, 25,50);
	changeResource(5, 25,75);
	changeResource(6, 75, 100);
	changeResource(7, 2500,5000);
			break;
		case "venus":
	
			break;
		case "earth":
		
			break;
		case "mars":
		
			break;
		case "jupiter":
		
			break;

		case "saturn":
				
			break;}
	
	}
	
//method for changing resources given an exclusive high and inclusive low, neg ints for losses, resource pointer i
public void changeResource(int i, int l, int h)
{
	System.Random pluto=new System.Random(ThreadSafeRandom.Next());
	Resources[i]+=pluto.Next(l,h);
}

public void changeIncome(int i, int l, int h)
{
	System.Random pluto=new System.Random(ThreadSafeRandom.Next());
	Income[i]+=pluto.Next(l,h);
}

public void changeFavorIncome(int i, int l, int h)
{
	System.Random pluto=new System.Random(ThreadSafeRandom.Next());
	FavorIncome[i]+=pluto.Next(l,h);
}

public void changeFavor(int i, int l, int h)
{
	System.Random pluto=new System.Random(ThreadSafeRandom.Next());
	Favor[i]+=pluto.Next(l,h);
}

public void changeHostility(int i, int l, int h)
{
	System.Random pluto=new System.Random(ThreadSafeRandom.Next());
	Hostility[i]+=pluto.Next(l,h);
}

public void changeHostilityIncome(int i, int l, int h)
{
	System.Random pluto=new System.Random(ThreadSafeRandom.Next());
	HostilityIncome[i]+=pluto.Next(l,h);
}
	// Update is called once per frame
	void Update () {

	}
}
