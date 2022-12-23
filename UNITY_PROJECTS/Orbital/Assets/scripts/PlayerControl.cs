using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 0, speed));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, 0, -speed));
        }


    }
}
