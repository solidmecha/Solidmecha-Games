using UnityEngine;
using System.Collections;

public class BuildModuleScript : MonoBehaviour {

    public CaptialShipHelper CSH;
    public int ID;

    void OnMouseDown()
    {
        CSH.BuildModule(ID);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
