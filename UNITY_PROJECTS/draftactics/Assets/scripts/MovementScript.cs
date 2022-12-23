using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

    public Vector3 Dir;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Dir * Time.deltaTime);
	}
}
