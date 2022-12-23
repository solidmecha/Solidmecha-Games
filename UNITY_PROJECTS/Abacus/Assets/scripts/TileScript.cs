using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    public Vector2 Direction;
    Vector2 Pos;
    public bool isMoving;
    public float speed = 4;

	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter2D()
    {
        isMoving = false;
        transform.localPosition = new Vector2(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isMoving)
        {
            Pos = transform.position;
            transform.Translate(speed * Direction*Time.deltaTime);
        }
	}
}
