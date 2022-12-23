using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHelper : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("S") && transform.parent.CompareTag("P"))
        {
            collision.transform.SetParent(transform.root);
            collision.GetComponent<SpriteRenderer>().color = Color.green;
            collision.tag = "P";
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
