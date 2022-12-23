using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SystemControl : MonoBehaviour{
	
	public GameObject leftSignText;
	public GameObject rightSignText; 

	public GameObject rightTrigger;
	public GameObject leftTrigger;

	public GameObject statDisplay;
	Text statText;
	string firstLine, secondLine,thirdLine;

	System.Random RNG;

	public static int choicesMade;
	int numberOfChoices;
	int valueA, valueB, valueC;
	int resultA, resultB, resultC;
	int changeA, changeB, changeC;

	int stamReduce;

	bool rightIsSet;
	public static bool madeChoice;

	public static int[] Stats=new int[3];
	public static int[] startStats=new int[3];

	// Use this for initialization
	void Start () {
		stamReduce = 1;
		choicesMade = 0;
		RNG = new System.Random (ThreadSafeRandom.Next ());
		Stats [0] = RNG.Next (3, 7);
		Stats [1] = RNG.Next (2, 8);
		Stats [2] = RNG.Next (40, 60);
		for (int i=0; i<3; i++) 
		{
			startStats[i]=Stats[i];
				}
		StartCoroutine (stamTickDown ());

		madeChoice = true;

		statText = statDisplay.GetComponent<Text> ();
	}

	IEnumerator stamTickDown()
	{
		yield return new WaitForSeconds (1);
		if(Application.loadedLevel==0)
		{
			if((float)choicesMade/10f>stamReduce)
			{
				stamReduce++;
			}
			Stats [2]-=stamReduce;
			Debug.Log (stamReduce);
		}
		StartCoroutine (stamTickDown ());
		}
	
	void setUpChances()
	{
		int c=RNG.Next(4);
		switch (c) 
		{
		case 0: // 50-50
			valueA=50;
			valueB=50;
			numberOfChoices=2;
			break;
		case 1: //75-25
			valueA=75;
			valueB=25;
			numberOfChoices=2;
			break;
		case 2: //30-30-40
			valueA=30;
			valueB=30;
			valueC=40;
			numberOfChoices=3;
			break;
		case 3: //20-20-60
			valueA=20;
			valueB=20;
			valueC=60;
			numberOfChoices=3;
			break;
		default:
			Debug.Log ("setupChances default");
			break;
				}
		}
	int[] setUpResults()
	{
		int[] temp = new int[2];
		int c=RNG.Next (3);
		switch (c) {
		case 0:
			temp[0]=0;
			temp[1]=RNG.Next(-3,3);
			break;
		case 1:
			temp[0]=1;
			temp[1]=RNG.Next(-3,3);
			break;
		case 2:
			int low=-5-stamReduce;
			int high=8+stamReduce;
			temp[0]=2;
			temp[1]=RNG.Next(low,high);
			break;
				}
		return temp;
		}
	void setUpText(GameObject g)
	{
		Text signText = g.GetComponent<Text> ();
	signText.text=" "+valueA.ToString()+"%: "+ changeA.ToString()+" "+statLookUp(resultA)+"\n "+valueB.ToString()+"%: "+ changeB.ToString()+" "+statLookUp(resultB);
		if (numberOfChoices == 3) 
		{
			signText.text=signText.text+"\n "+valueC.ToString()+"%: "+ changeC.ToString()+" "+statLookUp(resultC);
		}
	}

	string statLookUp(int a)
	{
		switch(a)
		{
		case 0:
			return "HP";
		case 1:
			return "WP";
		case 2:
			return "St";
		default:
			return "BREAK";
		}
		}

	void setUpChoices(GameObject g)
	{
		Choices thisChoice = (Choices)g.GetComponent (typeof(Choices));
		int r = RNG.Next (0, 100);
		if(r<valueA)
		{
		thisChoice.resultChange=changeA;
		thisChoice.resultIndex=resultA;
		}
		else if(r<valueA+valueB)
		{
			thisChoice.resultChange=changeB;
			thisChoice.resultIndex=resultB;
		}
		else{
			thisChoice.resultChange=changeC;
			thisChoice.resultIndex=resultC;
		}

	}
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevel==0)
		{
		if (madeChoice) 
		{
			choicesMade++;
				setUpChances();
				int[] temp=new int[2];
				temp=setUpResults();
				resultA=temp[0];
				changeA=temp[1];
				temp=setUpResults();
				resultB=temp[0];
				changeB=temp[1];
				if(numberOfChoices==3)
				{
					temp=setUpResults();
					resultC=temp[0];
					changeC=temp[1];
				}
				setUpText(rightSignText);
				setUpChoices(rightTrigger);
			temp=setUpResults();
			resultA=temp[0];
			changeA=temp[1];
			temp=setUpResults();
			resultB=temp[0];
			changeB=temp[1];
			if(numberOfChoices==3)
			{
				temp=setUpResults();
				resultC=temp[0];
				changeC=temp[1];
			}
			setUpText(leftSignText);
			setUpChoices(leftTrigger);
			madeChoice=false;
			
		}
	

		firstLine = "<color=#da4545ff>"+"Health: " + Stats [0].ToString ()+"</color>";
		secondLine = "<color=#0066ffff>"+"Willpower: " + Stats [1].ToString ()+"</color>";
		thirdLine = "<color=#00cc00ff>"+"Stamina: " + Stats [2].ToString ()+"</color>";

		statText.text = firstLine+"\n"+secondLine+"\n"+thirdLine;

		if (Stats [0] <= 0 || Stats [1] <= 0 || Stats [2] <= 0) {
			DontDestroyOnLoad(gameObject);
			Application.LoadLevel(1);	
			}
		}
	}
	

}
