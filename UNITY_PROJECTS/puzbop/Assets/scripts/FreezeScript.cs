using UnityEngine;
using System.Collections;

public class FreezeScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collision.GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
