using UnityEngine;
using System.Collections;

public class PieceControl : MonoBehaviour {

    public GameObject Mirror;

    void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().SetSelection(gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
