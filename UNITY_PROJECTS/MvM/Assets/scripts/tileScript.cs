using UnityEngine;
using System.Collections.Generic;
using System;

public class tileScript : MonoBehaviour {


	public int oreAmount;
	public int[] Location=new int[2];
	public Action Active;
	public GameObject Piece;
	public List<GameObject> Inscriptions=new List<GameObject>{};
	void OnMouseDown()
	{
		if(GetComponent<SpriteRenderer>().color.Equals(Color.green))
		   {
			Active();
			}
	
	}


	public void move()
	{
		BattleScript.currentUnit.transform.position=transform.position;
		EntityScript es = (EntityScript)BattleScript.currentUnit.GetComponent (typeof(EntityScript));
		tileScript ts=(tileScript)es.GM.Battlefield[es.location[0],es.location[1]].GetComponent(typeof(tileScript));
		ts.Piece=null;
		Piece=BattleScript.currentUnit;
		es.location[0]=Location[0];
		es.location [1] = Location [1];
		es.stopShowingMovement ();
		endTurn();
	}

	public void build()
	{
		
		MechanicScript ms=(MechanicScript)BattleScript.currentUnit.GetComponent(typeof(MechanicScript));
		GameObject mech=Instantiate(ms.Mech, transform.position, Quaternion.identity) as GameObject;
		MechScript mechscript=(MechScript)mech.GetComponent(typeof(MechScript));
		mechscript.location[0]=Location[0];
		mechscript.location[1]=Location[1];
		mechscript.managerObj=ms.GM.gameObject;
		mechscript.setManager();
		mechscript.setUpMech();
		Piece=mech;
		BattleScript.team1moveOrderList.Add(mech);
		BattleScript.metal-=ms.cost;
		ms.stopShowingBuildRange();
		endTurn();
	}


	void endTurn()
	{
		EntityScript es = (EntityScript)BattleScript.currentUnit.GetComponent (typeof(EntityScript));
		es.curTileSR.color=es.GM.tileColor;
		BattleScript.index++;
		BattleScript.wait = false;

	}

}
