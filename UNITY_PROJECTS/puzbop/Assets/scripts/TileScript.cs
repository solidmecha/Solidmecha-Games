using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
    public bool isHandling;
    public bool dropReady;
    public int ID;

    private void OnMouseDown()
    {
        if(dropReady)
        {
            PuzControl.singleton.SpawnDrop((Vector2)transform.position);
            transform.position = (Vector2)transform.position + Vector2.down * .33f;
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            dropReady = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isHandling && !dropReady && collision.collider.CompareTag("Player") && !collision.collider.GetComponent<TileScript>().dropReady)
        {
            PuzControl.singleton.HandleCollision(this, collision.collider.GetComponent<TileScript>());
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
