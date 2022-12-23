using UnityEngine;
using System.Collections;

public class PieceSelect : MonoBehaviour {
	int number;
	public GameObject manager;
	GameManager gamer;

	void OnMouseDown()
	{
		GameManager.SelectedObject = gameObject;
		gamer.buttonDisplay (number);
	}
	// Use this for initialization
	void Start () {
		setUp();
	}

	void setUp()
	{
		gamer=(GameManager)manager.GetComponent(typeof(GameManager));
		switch (tag) {
		case "turret":
			number=0;
			break;
		case "econ":
			number=1;
			break;
		case "fact":
			number=2;
			break;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
