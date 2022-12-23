using UnityEngine;
using System.Collections;

public class SquareScript : MonoBehaviour {

	public PuzzleLeader PL;
	public SpriteRenderer SR;
	public Vector4 SolutionVector;

	void OnMouseDown()
	{
		SR.color=PL.SelectedColor;
	}

	// Use this for initialization
	void Start () {
		SR=GetComponent<SpriteRenderer>();
		//SR.color=SolutionVector;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
