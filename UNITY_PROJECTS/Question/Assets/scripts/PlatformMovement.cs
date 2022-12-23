using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour {

    public float Xspeed;
    public float Yspeed;
    public float speed;
    Vector2 V;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name.Equals("Reverser(Clone)") || other.gameObject.name.Equals("EmptyTile(Clone)"))
            V = V * -1;
       // GetComponent<Rigidbody2D>().velocity = speed * V;

    }

  

    // Use this for initialization
    void Start () {
        V = new Vector2(Xspeed, Yspeed);
      //  GetComponent<Rigidbody2D>().velocity = speed * V;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
         transform.Translate(V * Time.deltaTime);
       

    }
}
