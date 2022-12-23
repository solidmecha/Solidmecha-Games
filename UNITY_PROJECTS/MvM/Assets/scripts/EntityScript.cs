using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class EntityScript : MonoBehaviour {
		
	public GameObject managerObj;
	public GameManager GM;
	public int movement;
	public int health;
	public int[] location=new int[2];
	protected List<GameObject> tiles = new List<GameObject> {};

	public SpriteRenderer curTileSR;

	public int NumberOfSkills;

	int ID;

	public bool UIset;
	public bool takingTurn;

	public List<GameManager.Skill> listOfSkills=new List<GameManager.Skill>{};

	
public virtual void setUpUI()
	{
		ButtonScript bScript = (ButtonScript)GM.button1.GetComponent (typeof(ButtonScript));
		bScript.removeListeners();
		bScript.setUpButton ();
		bScript.skillMethod = incrementSkillID;

		bScript = (ButtonScript)GM.button2.GetComponent (typeof(ButtonScript));
		bScript.removeListeners();
		bScript.setUpButton ();
		bScript.skillMethod = decrementSkillID;

		ID=0;
		checkSkillID();
		Text uiText=GM.UIText.GetComponent<Text>();
		uiText.text="HP: "+health;
		UIset = true;


	}

	public void incrementSkillID()
	{
		ID++;
		if(ID<NumberOfSkills)
		{checkSkillID();}
		else
		{
			ID=0;
			checkSkillID();
		}

	}

	public void decrementSkillID()
	{
		ID--;
		if(ID<0)
		{	ID=NumberOfSkills-1;
			checkSkillID();}
		else
		{
			checkSkillID();
		}
	}



	void checkSkillID()
	{
		ButtonScript bScript = (ButtonScript)GM.button.GetComponent (typeof(ButtonScript));
		GM.button.transform.GetChild(0).gameObject.GetComponent<Text> ().text = listOfSkills[ID].buttonText;
		bScript.skillMethod = listOfSkills[ID].buttonAction;

	}



public void setManager()
	{
		GM = (GameManager)managerObj.GetComponent (typeof(GameManager));
	}

public void takeTurn()
	{
		curTileSR=GM.Battlefield[location[0],location[1]].GetComponent<SpriteRenderer>();
		curTileSR.color=Color.blue;
			setUpUI ();

		}


	public void showMovement()
	{

		for (int x=0; x<=movement; x++) {
			for (int y=0; y<=movement; y++) {

				if(x!=0 || y!=0)
				{
				if(location [0]-x>=0 && location [1]-y >=0)
				tiles.Add(GM.Battlefield [location [0]-x, location [1]-y]);

				if(location [0]+x<GM.battleWidth && location [1]+y < GM.battleHeight)
				tiles.Add (GM.Battlefield [location [0]+x, location [1]+y]);

				if(location [0]+x<GM.battleWidth && location [1]-y >= 0)
					tiles.Add (GM.Battlefield [location [0]+x, location [1]-y]);

				if(location [0]-x>=0 && location[1]+y < GM.battleHeight)
					tiles.Add (GM.Battlefield [location [0]-x, location [1]+y]);
				}
			}
		}

		foreach (GameObject t in tiles) {
			tileScript ts=(tileScript) t.GetComponent(typeof(tileScript));
			ts.Active=ts.move;
			SpriteRenderer sr=t.GetComponent<SpriteRenderer> ();
			sr.color=Color.green;
		}
	}

	public void stopShowingMovement()
	{
		foreach (GameObject t in tiles) {
			SpriteRenderer s=t.GetComponent<SpriteRenderer> ();
			s.color=GM.tileColor;
		}

		tiles.Clear ();
	}
	
}
