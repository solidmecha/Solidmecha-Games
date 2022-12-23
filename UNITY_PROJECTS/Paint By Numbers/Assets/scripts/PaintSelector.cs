using UnityEngine;
using System.Collections;

public class PaintSelector : MonoBehaviour {
	public Color color;
	public GameObject PLobj;
	public PuzzleLeader PL;

	void OnMouseDown()
	{
		PL.SelectedColor=color;
	}

	// Use this for initialization
	void Start () {

		PL=(PuzzleLeader)PLobj.GetComponent(typeof(PuzzleLeader));
		SpriteRenderer sr=GetComponent<SpriteRenderer>();
		color=sr.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
