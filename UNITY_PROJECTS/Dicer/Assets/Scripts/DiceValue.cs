using UnityEngine;
using System.Collections.Generic;

public class DiceValue : MonoBehaviour {

	public int Value;
	public int scoreValue;
	public bool Scored;
	public bool linked;
	public List<int[]> linkPointers=new List<int[]>{};
	public DiceControl DC;
	public int[] Pos=new int[2];
	public int[] prevPointer=new int[2];
	public bool linkTarget;

	void OnMouseDown()
	{
		DC.reRoll(Pos);
		//DC.scoreBoard();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
