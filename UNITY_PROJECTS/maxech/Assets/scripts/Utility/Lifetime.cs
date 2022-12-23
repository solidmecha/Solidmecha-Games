using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {

    public float counter;
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter <= 0)
            Destroy(gameObject);
	}
}
