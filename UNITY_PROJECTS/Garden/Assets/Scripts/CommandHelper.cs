using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommandHelper : MonoBehaviour {

    public int index;
    CommandUI ComUI;

    void OnMouseDown()
    {
        ComUI.CommandSprite.transform.position = transform.position;
        ComUI.CommandSprite.GetComponent<SpriteRenderer>().sprite = ComUI.cellui.Sprites[ComUI.Drop.value];
        if (ComUI.cellui.isShowingActived)
        {
            ComUI.cellui.CS.Actions[ComUI.NH.index]=ComUI.cellui.CS.PossibleActions[ComUI.Drop.value];
            ComUI.cellui.CS.ActionsID[ComUI.NH.index] = ComUI.Drop.value;
            ComUI.cellui.CS.ActionParams[ComUI.NH.index] = index;
        }
        else
        {
            ComUI.cellui.CS.SupressedActions[ComUI.NH.index] = ComUI.cellui.CS.PossibleActions[ComUI.Drop.value];
            ComUI.cellui.CS.SupressedActionsID[ComUI.NH.index] = ComUI.Drop.value;
            ComUI.cellui.CS.SupressedActionParams[ComUI.NH.index] = index;
        }
        ComUI.cellui.SetCommandSprites();
        ComUI.cellui.InvokeResetActive();
        Destroy(ComUI.cellui.CS.WS.CommandWindow);
    }

	// Use this for initialization
	void Start () {
        ComUI = (CommandUI)transform.parent.parent.gameObject.GetComponent(typeof(CommandUI));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
