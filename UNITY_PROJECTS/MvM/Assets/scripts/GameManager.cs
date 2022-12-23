using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	public GameObject tile;
	public GameObject mage;
	public GameObject mechanic;
	public GameObject ore;



	public GameObject bot;
	public GameObject magic;

	public GameObject nomad;
	public GameObject miniTank;
	public GameObject golem;
	public GameObject drill;

	public Color tileColor;


	public GameObject UIText;
	public GameObject button;
	public GameObject button1;
	public GameObject button2;

	public int battleWidth=14;
	public int battleHeight=10;
	public GameObject[,] Battlefield;

	// Use this for initialization
	void Start () {

			ButtonScript bScript = (ButtonScript)button.GetComponent (typeof(ButtonScript));
		bScript.setUpButton ();

		SpriteRenderer sr = tile.GetComponent<SpriteRenderer> ();
		tileColor=sr.color;
		Battlefield = new GameObject[battleWidth, battleHeight];
		makeBattlefield ();
		placeUnits ();
		BattleScript.team1moveOrderList.Add (bot);
		BattleScript.team2moveOrderList.Add (magic);
		BattleScript.index=0;
		BattleScript battleScript=(BattleScript)gameObject.GetComponent(typeof(BattleScript));
		battleScript.startBattle=true;

	}

	public struct Skill
{
	public string buttonText;
	public Action buttonAction;

	public Skill(string s, Action a)
	{
		buttonText=s;
		buttonAction=a;
	}

}

	public Skill createSkill(string s, Action a)
	{
		Skill skill=new Skill(s,a);
		return skill;
	}

	void makeBattlefield ()
	{
		for (int i=0; i<battleWidth; i++) {
			for (int j=0;j<battleHeight;j++)
			{
				float posX=i*1.25f;
				float posY=j*1.25f;
				Battlefield[i,j]=Instantiate(tile,new Vector2(posX,posY), Quaternion.identity)as GameObject;
				tileScript ts=(tileScript)Battlefield[i,j].GetComponent(typeof(tileScript));
				ts.Location=new int[2]{i,j};
			}
		}
	}

	void placeUnits ()
	{


	
	BattleScript.mana=10;
	BattleScript.metal=10;
	 magic=Instantiate (mage, Battlefield [2, 0].transform.position, Quaternion.identity)as GameObject;
	 bot=Instantiate (mechanic, Battlefield [5, 5].transform.position, Quaternion.identity)as GameObject;

	 GameObject mark=Instantiate (mechanic, Battlefield [9, 4].transform.position, Quaternion.identity)as GameObject;


	 Instantiate(ore, Battlefield [7, 6].transform.position, Quaternion.identity);
	  Instantiate(ore, Battlefield [2, 0].transform.position, Quaternion.identity);
	   Instantiate(ore, Battlefield [3, 3].transform.position, Quaternion.identity);
	    Instantiate(ore, Battlefield [9, 6].transform.position, Quaternion.identity);
	     Instantiate(ore, Battlefield [8, 2].transform.position, Quaternion.identity);
	      Instantiate(ore, Battlefield [5, 8].transform.position, Quaternion.identity);
	       Instantiate(ore, Battlefield [2, 9].transform.position, Quaternion.identity);
	        Instantiate(ore, Battlefield [11, 4].transform.position, Quaternion.identity);

		EntityScript ES = (EntityScript)magic.GetComponent (typeof(EntityScript));
			Skill move=new Skill("Move", ES.showMovement);
		ES.managerObj = gameObject;
		ES.setManager ();
		ES.location = new int[]{2,0};
		ES.movement = 3;
		ES.NumberOfSkills=1;
		ES.health=123;
		ES.listOfSkills.Add(move);

	MechanicScript ES2 = (MechanicScript)mark.GetComponent (typeof(MechanicScript));

	Skill build=new Skill("Build drill", ES2.buildDrill);
		ES2.managerObj = gameObject;
		ES2.setManager ();
		ES2.location = new int[]{9,4};
		ES2.movement = 1;
		ES2.health=323;
		ES2.NumberOfSkills=2;
		move.buttonAction=ES2.showMovement;
		ES2.listOfSkills.Add(move);
		ES2.listOfSkills.Add(build);
		BattleScript.team1moveOrderList.Add(mark);

		
		MechanicScript ES1 = (MechanicScript)bot.GetComponent (typeof(MechanicScript));
		ES1.managerObj = gameObject;
		ES1.setManager();
		ES1.location = new int[]{5,5};
		ES1.movement = 4;
		ES1.health=234;
		ES1.NumberOfSkills=3;
		move.buttonAction=ES1.showMovement;
		build.buttonAction=ES1.buildDrill;
		Skill buildTank=new Skill("Build Tank", ES1.buildPanzer);
		ES1.listOfSkills.Add(buildTank);
		ES1.listOfSkills.Add(build);
		ES1.listOfSkills.Add(move);

		
	}
	
}
