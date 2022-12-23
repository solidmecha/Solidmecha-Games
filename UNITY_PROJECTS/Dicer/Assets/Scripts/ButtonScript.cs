using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public Text text;
	public Action skillMethod;
	public GameObject ControlObj;
	public DiceControl DC;
	public bool isReset;
	public int id;


	void Start()
	{
		setUpButton();
	}

	public void setUpButton()
	{
		DC=(DiceControl)ControlObj.GetComponent(typeof(DiceControl));
		if(isReset)
		{skillMethod=DC.reset;}
		else
		{skillMethod=DC.scoreBoard;}
		GetComponent<Button>().onClick.AddListener(delegate{skillMethod();});
	}

	public void removeListeners()
	{
		GetComponent<Button>().onClick.RemoveAllListeners();
	}

}
