using UnityEngine;
using System.Collections;

public class TileSelect : MonoBehaviour {
	float towerHieght;
	void OnMouseDown()
	{
		BoardManager.SelectedPiece.transform.position =new Vector3(transform.position.x,transform.position.y,-towerHieght);
		towerHieght += .001f;
	}
	// Use this for initialization
	void Start () {
		towerHieght = .001f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
