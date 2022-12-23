using UnityEngine;
using System.Collections;

public class RotateIt : MonoBehaviour {

    public Vector3 rotation;
    public float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotation * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(this);
	}
}
