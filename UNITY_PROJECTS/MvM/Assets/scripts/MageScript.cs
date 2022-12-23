using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MageScript : EntityScript {

	public override void setUpUI ()
	{
		base.setUpUI ();
		Text uiText=GM.UIText.GetComponent<Text>();
		uiText.text=uiText.text+" Mana: "+BattleScript.mana;
		UIset = true;

	}

	void cast()
	{
	}
	
}
