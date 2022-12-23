using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MechScript : EntityScript {

public int damage;
public int attackRange;
public override void setUpUI ()
	{
		base.setUpUI ();

	}
public void setUpMech()
{
	health=99;
	movement=2;
	attackRange=2;
	damage=25;
 
		listOfSkills.Add (GM.createSkill ("Move", showMovement));
	//listOfSkills.Add(move);
}

}
