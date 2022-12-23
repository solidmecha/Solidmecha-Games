using UnityEngine;
using System.Collections;

public class PegControl : MonoBehaviour {

    bool isLeft;

    void OnMouseDown()
    {
        if (isLeft)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            isLeft = false;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -90));
            isLeft = true;
        }
    }
	// Use this for initialization
	void Start () {
        if (name.Contains("Clone"))
        transform.Rotate(new Vector3(0, 0, 45));

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
