using UnityEngine;
using System.Collections;

public class Capturescript : MonoBehaviour {

	void OnMouseDown()
	{
		if (BoardManager.SelectedPiece.transform.localEulerAngles.y == 0) {
			BoardManager.SelectedPiece.transform.localEulerAngles = new Vector3 (0, 180, 0);
		} 
		else {
			BoardManager.SelectedPiece.transform.localEulerAngles=Vector3.zero;
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
