using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour {

int Number;
GameObject Sol;

Scenarios scenario;
	// Use this for initialization
	void Start () {
		Sol=GameObject.Find("Sol");
		scenario=(Scenarios)Sol.GetComponent(typeof(Scenarios));
		setupBtn();
	}

	 public void setupBtn()
    {
        	switch(name)
		{
			case "Button1":
			Number=1;
			break;
			case "Button2":
			Number=2;
			break;
			case "Button3":
			Number=3;
			break;

		}
        gameObject.GetComponent<Button>().onClick.AddListener(delegate{btnClicked(Number);});
    }
       
    void btnClicked(int i)
    {
       scenario.choiceResultsAndConsequences(i);
    }
 
	
	// Update is called once per frame
	void Update () {
	
	}
}
