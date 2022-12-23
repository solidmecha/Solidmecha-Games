using UnityEngine;
using System.Collections;

public class WireSelector : MonoBehaviour {

    public WirePuzzle WP;
    public int id;

    void OnMouseDown()
    {
        WP.SelectedWireID = id;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
