using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestruction : MonoBehaviour {

    public float counter;

	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter <= 0)
            Destroy(gameObject);
	}
}
