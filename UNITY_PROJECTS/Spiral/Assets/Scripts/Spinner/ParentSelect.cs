using UnityEngine;
using System.Collections;

public class ParentSelect : MonoBehaviour {

    void OnMouseDown()
    {
        Transform T = transform.root;
        transform.SetParent(null);
        T.SetParent(transform);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
