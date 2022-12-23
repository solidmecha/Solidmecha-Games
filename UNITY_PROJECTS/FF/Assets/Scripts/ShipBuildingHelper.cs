using UnityEngine;
using System.Collections;

public class ShipBuildingHelper : MonoBehaviour {
    public ShipBuildingScript sbs;
    public int id;
    bool DestroyNextFrame;

    void OnMouseDown()
    {
        sbs.UIselected(id);
        DestroyNextFrame = true;
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
