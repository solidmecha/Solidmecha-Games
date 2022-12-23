using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float Speed;
    public float DistSpeed;
    int index;
    float counter;
    float delay = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.parent.Rotate(new Vector3(0, 0, DistSpeed));
        transform.Translate(Vector2.up * Speed);
	}
}
