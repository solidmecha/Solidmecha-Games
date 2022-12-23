using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {


    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localScale += (Vector3.up * Time.deltaTime * 5);
            GetComponent<Rigidbody2D>().freezeRotation = true;
        }
        else if (Input.GetMouseButton(1))
        {
            transform.localScale += (Vector3.down * Time.deltaTime * 5);
            GetComponent<Rigidbody2D>().freezeRotation = true;
        }
        else
            GetComponent<Rigidbody2D>().freezeRotation = false;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        /*
        float f = transform.localEulerAngles.z;
        transform.localEulerAngles = Vector3.zero;
        transform.parent.Rotate(new Vector3(0,0,f)); */
	}
}
