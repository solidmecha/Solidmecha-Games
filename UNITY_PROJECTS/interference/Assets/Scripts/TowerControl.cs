using UnityEngine;
using System.Collections;

public class TowerControl : MonoBehaviour {

    bool isMoving;

    private void OnMouseDown()
    {
        isMoving = !isMoving;
        GetComponentInChildren<MessagingScript>().enabled = !GetComponentInChildren<MessagingScript>().enabled;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	if(isMoving)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
	}
}
