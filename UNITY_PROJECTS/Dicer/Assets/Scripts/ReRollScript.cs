using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class ReRollScript : MonoBehaviour {

	public GameObject ControlObj;
	public DiceControl DC;
	public bool isRow;
	public int id;


	void Start()
	{
		setUpButton();
	}

	public void setUpButton()
	{
		DC=(DiceControl)ControlObj.GetComponent(typeof(DiceControl));
		GetComponent<Button>().onClick.AddListener(delegate{reRoll();});
	}

	public void reRoll()
	{
		if(isRow)
		{DC.reRollRow(id);}
		else
		{DC.reRollCol(id);}
	}

	public void removeListeners()
	{
		GetComponent<Button>().onClick.RemoveAllListeners();
	}

}
