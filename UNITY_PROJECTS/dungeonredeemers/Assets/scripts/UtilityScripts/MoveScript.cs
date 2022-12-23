using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    public float speed;
    public Vector2 Dir;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Dir * Time.deltaTime);
	}
}
