using UnityEngine;
using System.Collections;

public class SpikeHelper : MonoBehaviour {

    bool spin;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Platform")
            transform.parent.parent.SetParent(other.transform);
    }

    // Use this for initialization
    void Start () {
      //  spin = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (spin)
            transform.parent.parent.Rotate(new Vector3(0, 0, 90));
	}
}
