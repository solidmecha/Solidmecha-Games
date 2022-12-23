using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    public string Phobia;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Phobia))
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
