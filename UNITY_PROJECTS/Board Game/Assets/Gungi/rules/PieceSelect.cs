using UnityEngine;
using System.Collections;

public class PieceSelect : MonoBehaviour {

	void OnMouseDown()
	{
		BoardManager.SelectedPiece = transform.parent.gameObject;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
