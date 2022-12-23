using UnityEngine;
using System.Collections;

public class CaptureScript : MonoBehaviour {

    bool PlayerOwned;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<Renderer>().material.color = Color.blue;
            foreach (Renderer r in transform.GetComponentsInChildren<Renderer>())
                r.material.color = Color.blue;
            GetComponent<RotateIt>().speed *= -1;
            other.GetComponent<PlayerScript>().ResetPoint = transform.position + Vector3.up;
            if(!PlayerOwned)
            {
                PlayerOwned = true;
                other.GetComponent<PlayerScript>().CapturePoint();
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
