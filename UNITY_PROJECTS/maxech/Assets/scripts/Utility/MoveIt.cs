using UnityEngine;
using System.Collections;

public class MoveIt : MonoBehaviour {
    public Vector3 move;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(move * Time.deltaTime);
	}
}
