using UnityEngine;
using System.Collections;

public class EconBuildingScript : MonoBehaviour {
	public int Gold;
	public int goldIncome;
	public int Fuel;
	public int fuelIncome;
	public int VP;
	public int vpIncome;
	public float delay;
	public bool makeMoney; 
	public int previousSelectedIncome; //0=gold, 1=fuel, 2=vp
	// Use this for initialization
	void Start () {
		previousSelectedIncome = 3;
		makeMoney = true;
	}

	public void changeIncome(int i)
	{
		switch (previousSelectedIncome) 
		{
		case 0: goldIncome-=1;break;
		case 1: fuelIncome-=1;break;
		case 2:	vpIncome-=1;break;
		default: break;
		}
		switch(i)
		{
		case 0: goldIncome+=1;break;
		case 1: fuelIncome+=1;break;
		case 2: vpIncome+=1;break;
		default: break;
		}
		previousSelectedIncome = i;
	}

	IEnumerator generateIncome()
	{
		yield return new WaitForSeconds(delay);
		Gold += goldIncome;
		Fuel += fuelIncome;
		VP += vpIncome;
		makeMoney=!makeMoney;

	}


	
	// Update is called once per frame
	void Update () {
		if (makeMoney) {
			makeMoney=!makeMoney;
			StartCoroutine(generateIncome());

		}
	
	}
}
