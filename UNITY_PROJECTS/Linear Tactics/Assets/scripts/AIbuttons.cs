using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class AIbuttons : MonoBehaviour {
	public GameObject label;
	Text labelText;
	// Use this for initialization
	void Start () {
		labelText = label.GetComponent<Text> ();
		if (name.Equals ("minus")) {
			AIScript.delay=12f;
			GetComponent<Button>().onClick.AddListener(delegate{minusAI();});
			updateGUI();
		} 
		else {
			GetComponent<Button>().onClick.AddListener(delegate{plusAI();});
		}
	
	}

	void minusAI()
	{
		AIScript.delay -= 0.01f;
		updateGUI ();
	}
	void plusAI()
	{AIScript.delay += 0.01f;
		updateGUI ();
	}
	void updateGUI()
	{
		labelText.text="AI Delay: "+AIScript.delay;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
