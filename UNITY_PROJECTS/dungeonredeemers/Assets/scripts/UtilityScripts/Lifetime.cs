using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {

    public float Counter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Counter -= Time.deltaTime;
        if(Counter<=0)
        {
            Destroy(gameObject);
        }
	}
}
