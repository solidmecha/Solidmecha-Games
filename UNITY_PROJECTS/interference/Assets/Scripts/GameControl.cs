using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public GameObject Building;
    public System.Random RNG;

	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        int Bcount = RNG.Next(5, 11);
        for(int i=0;i<Bcount;i++)
        {
            Instantiate(Building, new Vector2(RNG.Next(-1000, 1001) / 100f, RNG.Next(-600, 601) / 100f), Quaternion.identity, transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
