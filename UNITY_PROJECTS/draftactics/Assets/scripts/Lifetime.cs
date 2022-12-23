using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {

    public float Counter;
	
	// Update is called once per frame
	void Update () {
        Counter -= Time.deltaTime;
        if (Counter <= 0)
            Destroy(gameObject);
	}
}
