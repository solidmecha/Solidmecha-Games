using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("C"))
        {
            collision.transform.Rotate(new Vector3(0, 0, 180));
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
