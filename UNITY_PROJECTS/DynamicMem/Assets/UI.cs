using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	void OnGUI()
	{
	
		if(GUI.Button(new Rect (1,1,100,50), "Exit"))
		{Application.Quit();}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
