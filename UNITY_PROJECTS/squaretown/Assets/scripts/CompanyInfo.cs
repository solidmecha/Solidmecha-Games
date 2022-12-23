using UnityEngine;
using System.Collections;

public class CompanyInfo {

    public int Price;
    public int voltility;
    public int AvailableShares;

    public CompanyInfo(int P, int V, int AS)
    {
        Price = P;
        voltility = V;
        AvailableShares = AS;
    }


    public void UpdatePrice()
    {
        Price = Price + voltility * EconControl.singleton.RNG.Next(-5, 11);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
