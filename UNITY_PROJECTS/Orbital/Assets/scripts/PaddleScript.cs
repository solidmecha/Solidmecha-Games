using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {


    public Vector2 normal;



    void OnTriggerEnter2D(Collider2D other)
    {
        CircleScript CS = (CircleScript)other.gameObject.GetComponent(typeof(CircleScript));
        CS.direction =Vector2.Reflect(CS.direction, normal);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        normal = -transform.position.normalized;
	}
}
