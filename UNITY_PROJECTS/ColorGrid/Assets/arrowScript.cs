using UnityEngine;
using System.Collections;

public class arrowScript : MonoBehaviour {

	PuzzleManager pM;
	public PuzzleManager.Snake snakey;
	public PuzzleManager.Direction dir;
	void OnMouseDown()
	{
		snakey.curDirection=dir;
	}

	// Use this for initialization
	void Start () {
		pM=(PuzzleManager) GameObject.Find("Teamleader").GetComponent(typeof(PuzzleManager));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
