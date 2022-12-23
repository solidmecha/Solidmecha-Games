using UnityEngine;
using System.Collections;

public class HazardScript : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.transform.localScale.x == .5f)
                Destroy(collision.collider.gameObject);
            else
            {
                collision.collider.GetComponent<Rigidbody2D>().mass *= .5f;
                collision.collider.transform.localScale *= .5f;
            }

        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
