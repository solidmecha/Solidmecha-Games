using UnityEngine;
using System.Collections;

public class TileLeader : MonoBehaviour {

    BuilderManager Bman;

    void OnMouseDown()
    {
        Bman.V = transform.position;
        Bman.createPiece();
    }

	// Use this for initialization
	void Start () {
        Bman = (BuilderManager)GameObject.Find("Control").GetComponent(typeof(BuilderManager));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
