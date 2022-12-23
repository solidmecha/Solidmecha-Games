using UnityEngine;
using System.Collections;

public class PassTurn : MonoBehaviour {

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().PassTurn();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
