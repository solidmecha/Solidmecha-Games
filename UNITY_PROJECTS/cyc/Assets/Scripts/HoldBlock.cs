using UnityEngine;
using System.Collections;

public class HoldBlock : MonoBehaviour {

    Vector3 LocalPos;


    // Use this for initialization
    void Start () {
        LocalPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = LocalPos;
	}
}
