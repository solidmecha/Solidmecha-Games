using UnityEngine;
using System.Collections;

public class ScaleChange : MonoBehaviour {
    public Vector3 Delta;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += (Delta * Time.deltaTime);
	}
}
