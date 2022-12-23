using UnityEngine;
using System.Collections;

public class RescaleIt : MonoBehaviour {

    public Vector3 delta;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = transform.localScale + delta * Time.deltaTime;
	}
}
