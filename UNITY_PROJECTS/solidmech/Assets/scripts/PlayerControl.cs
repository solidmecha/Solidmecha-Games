using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    Rigidbody rb;
    public float JumpMagnitude;
    public float speed;
    public GameObject Bullet;
    public Transform ShotOrigin;
    public Vector3 Destination;
    bool moving;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Destination = hit.point;
                moving = true;
                transform.LookAt(Destination);
            }
        }

        if(moving)
        {
            float dist = Vector3.Magnitude(Destination - transform.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (dist <= Vector3.Magnitude(Destination - transform.position))
            {
                transform.position = Destination;
                moving = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(Bullet, ShotOrigin.position, ShotOrigin.rotation);
        }

        /*
		if(Input.GetAxis("Horizontal") !=0)
        {
            transform.Rotate(new Vector3(0, 90, 0) * Input.GetAxis("Horizontal") * Time.deltaTime);
            
        }
        if(Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(Vector3.right*-1f* Input.GetAxis("Vertical") * speed*Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * JumpMagnitude);
            }
        if(Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(Bullet, ShotOrigin.position, ShotOrigin.rotation);
        }
        */
    }
}
