using UnityEngine;
using System.Collections;

public class NovaFX : MonoBehaviour {
    float[] counter = new float[2] { .5f, .5f };
    bool visible;
    Vector3 scale;
	// Use this for initialization
	void Start () {
        scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        counter[0] -= Time.deltaTime;
        if (counter[0] <= 0)
        {
            visible = !visible;
            if (visible)
                transform.localScale=scale;
            else
                transform.localScale=Vector3.zero;
            counter[0] = counter[1];
        }

	}
}
