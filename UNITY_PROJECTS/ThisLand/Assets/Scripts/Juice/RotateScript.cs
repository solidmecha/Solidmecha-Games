using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

    public bool isRotate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(isRotate)
            transform.Rotate(0, 0, -10 * Time.deltaTime);

	}
}
