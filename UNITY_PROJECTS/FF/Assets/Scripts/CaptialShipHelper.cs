using UnityEngine;
using System.Collections;

public class CaptialShipHelper : MonoBehaviour {

    int ModCount;
    UnitScript us;
    GameObject DisplayObj;
    bool DestroyDisplayNextFrame;

    void OnMouseDown()
    {
        if (ModCount < 3 && DisplayObj==null)
            Display();
    }

    void Display()
    {
        DisplayObj=Instantiate(us.GM.CSDisplay, transform.position, Quaternion.identity, transform) as GameObject;
        for (int i = 0; i < 4; i++)
        {
            BuildModuleScript bms = (BuildModuleScript)DisplayObj.transform.GetChild(i).GetComponent(typeof(BuildModuleScript));
            bms.CSH = this;
        }
    }

    public void BuildModule(int i)
    {
        Instantiate(us.GM.CapitalMods[i],transform.GetChild(ModCount + 1).position,transform.rotation, transform);
        ModCount++;
        if (ModCount >= 3)
            DestroyDisplayNextFrame = true;
            
    }
	// Use this for initialization
	void Start () {
        us = (UnitScript)GetComponent(typeof(UnitScript));
    }
	
	// Update is called once per frame
	void Update () {
	    if(DestroyDisplayNextFrame)
            Destroy(DisplayObj);
    }
}
