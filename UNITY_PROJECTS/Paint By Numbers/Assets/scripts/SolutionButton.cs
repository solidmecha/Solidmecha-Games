using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SolutionButton: MonoBehaviour {

	public GameObject Leader;
	public PuzzleLeader PL;
	void Start()
	{
		PL=(PuzzleLeader) Leader.GetComponent(typeof(PuzzleLeader));
		GetComponent<Button>().onClick.AddListener(delegate{PL.checkSolution();});
	}


}