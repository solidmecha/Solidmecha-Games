using UnityEngine;
using System.Collections;

public class MoveControl : MonoBehaviour {

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().Move(transform.position);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
