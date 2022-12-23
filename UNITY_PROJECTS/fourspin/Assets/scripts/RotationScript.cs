using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    public float RotationSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
	}
}
