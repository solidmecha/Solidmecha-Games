using UnityEngine;
using System.Collections;

public class UIBuildingSelect : MonoBehaviour {

    public BuildingScript Bscript;
    public int id;
    bool DestroyNextFrame;

    void OnMouseDown()
    {
        if (Bscript.GM.Plat >= Bscript.GM.BuildingCosts[id])
            Bscript.BuildBuilding(id);
        else
            Bscript.GM.ShowMessage("Need " + Bscript.GM.BuildingCosts[id].ToString() + " Platinum");
        DestroyNextFrame=true;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(DestroyNextFrame)
            Destroy(transform.parent.gameObject);

    }
}
