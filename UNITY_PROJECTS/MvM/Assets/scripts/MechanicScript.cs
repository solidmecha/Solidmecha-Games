using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System;

public class MechanicScript : EntityScript {

	public GameObject Mech;

	public int cost;

	int BuildRange=1;

	public override void setUpUI ()
	{
		base.setUpUI ();
			Text uiText=GM.UIText.GetComponent<Text>();
		uiText.text=uiText.text+" Metal: "+BattleScript.metal;
		UIset = true;

	}


void setMech(int mechID)
{
	switch (mechID)
	{
	case 0:
	Mech=GM.drill;
	cost=6;
	break;
	case 1:
	Mech=GM.miniTank;
	cost=4;
	break;
	case 2:
	Mech=GM.nomad;
	cost=7;
	break;
	case 3:
	Mech=GM.golem;
	cost=15;
	break;
}

}


public void buildDrill()
{
	setMech(0);
	showBuildRange();
}

public void buildPanzer()
{
	setMech(1);
	showBuildRange();
}

public void showBuildRange()
	{

		for (int x=0; x<=BuildRange; x++) {
			for (int y=0; y<=BuildRange; y++) {

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
			ts.Active=ts.build;
			SpriteRenderer sr=t.GetComponent<SpriteRenderer> ();
			sr.color=Color.green;
		}
	}

	public void stopShowingBuildRange()
	{
		foreach (GameObject t in tiles) {
			SpriteRenderer s=t.GetComponent<SpriteRenderer> ();
			s.color=GM.tileColor;
		}

		tiles.Clear ();
	}

}
