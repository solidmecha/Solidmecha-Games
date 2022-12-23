using UnityEngine;
using System.Collections;

public class WorldScript : MonoBehaviour {

    public GameObject Pusher;
    System.Random RNG;


	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        int count = RNG.Next(10, 20);
        for(int i=0;i<count;i++)
        {
            Quaternion Q = Quaternion.Euler(new Vector3(0, 0, RNG.Next(360)));
            Instantiate(Pusher,new Vector2(RNG.Next(-7, 8), RNG.Next(-5, 6)), Q);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
