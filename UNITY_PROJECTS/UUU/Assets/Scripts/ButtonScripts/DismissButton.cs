using UnityEngine;
using System.Collections;

public class DismissButton : MonoBehaviour {

    public int LaneID;
    public bool backRow;

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Dismiss(); });
	}

    void Dismiss()
    {
        if (LaneID > -1 && !PlayerScript.singleton.OpenSpot(LaneID, backRow))
        {
            PlayerScript.singleton.RemoveUnit(LaneID, backRow);
            Destroy(BattleScript.singleton.FindUnit(LaneID, backRow, true).gameObject);
        }
        else
        {
            WorldControl.singleton.ExitDismiss();
            Destroy(transform.root.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
