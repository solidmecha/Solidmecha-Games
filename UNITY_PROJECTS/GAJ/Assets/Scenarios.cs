using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Scenarios : MonoBehaviour {

	public int ScenarioID;
	int numberOfButtons;
	GameObject words;
	List<GameObject> CanvasPointers=new List<GameObject>{};
	public GameObject scenario;
	public GameObject OneButtonCanvas;
	public GameObject TwoButtonCanvas;
	public GameObject Sol;

	GameManager manageGame;
	Text sceneText;
	bool loadScenario;
	// Use this for initialization
	void Start () {
	loadScenario=true; //need logic for this


	manageGame=(GameManager) Sol.GetComponent(typeof(GameManager));
	}
	

	//prompts here
	string setUpPrompt()
	{
		switch(ScenarioID)
		{
		case 0:
		return "Yay->randomly change randomly, \n Button->toggles population change";
		break;

		case 1:
		return "Rebels are incoming \n will you 1) prepare fleet or  2)offer shelter";
		break;
		default:return ":)";
break;
		}
	}

void ScenarioLogic()
{
	ScenarioID=1;
	numberOfButtons=2;

switch(numberOfButtons){
case 1:
	CanvasPointers.Add((GameObject)Instantiate(OneButtonCanvas));
	break;
case 2:
CanvasPointers.Add((GameObject)Instantiate(TwoButtonCanvas));
break;
}

}



//for T
/*
0- atk
1- def
2- wealth
3- happiness
4- food/water
5- health
6- energy
7- population
*/
	public void choiceResultsAndConsequences(int b)//b is the number of which button was pressed
	{
		System.Random bobcat=new System.Random(ThreadSafeRandom.Next());
		switch(ScenarioID)
		{
			case 0:
			switch(b)
			{	case 1:
				int t=bobcat.Next(0,8);
				int l=bobcat.Next(-50, 51);
				int h=bobcat.Next(51, 100);
				manageGame.planetLookUp(0).changeResource(t, l, h);
				break;
				default:
				manageGame.dynamicPopulation=!manageGame.dynamicPopulation;
				break;
			}
			break;

			case 1:
			switch(b)
			{
				case 1:
				int t=0;
				int l=bobcat.Next(-50, 51);
				int h=bobcat.Next(51, 100);
				manageGame.planetLookUp(0).changeResource(t, l, h);
				break;
				case 2:
				t=7;
				 l=bobcat.Next(50, 51);
				h=bobcat.Next(51, 100);
				manageGame.planetLookUp(0).changeResource(t, l, h);
				break;
				default:
				break;
			}
			break;

		}

	}

	// Update is called once per frame
	void Update () {
		if(loadScenario)
		{
			ScenarioLogic();
			CanvasPointers.Add((GameObject)Instantiate(scenario));
			words=GameObject.Find("ScenarioText");
			sceneText= words.GetComponent<Text>();
			sceneText.text=setUpPrompt();
			loadScenario=false;
		}
	
	}
}
