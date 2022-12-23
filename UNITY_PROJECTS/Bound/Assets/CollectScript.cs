using UnityEngine;
using System.Collections;

public class CollectScript : MonoBehaviour {

    void OnCollisionEnter2D()
    {
        Destroy(gameObject);
    }

        // Use this for initialization
        void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
