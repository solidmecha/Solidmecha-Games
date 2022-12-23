using UnityEngine;
using System.Collections;

public class CircleScript : MonoBehaviour {

    public Vector2 direction;
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(direction * speed * Time.deltaTime);
	}
}
