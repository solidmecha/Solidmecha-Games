using UnityEngine;
using System.Collections;

public class PieceSelect : MonoBehaviour {

	public GameObject Manager;
	CryptoManager CM;
	public GameObject Display;
	public int ID;
	void OnMouseDown()
	{
		CM.SelectedPiece=gameObject;
		CM.SelectedDisplay=Display;
		CM.IDpointer=ID;
	}

	// Use this for initialization
	void Start () {
	CM=(CryptoManager)Manager.GetComponent(typeof(CryptoManager));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
