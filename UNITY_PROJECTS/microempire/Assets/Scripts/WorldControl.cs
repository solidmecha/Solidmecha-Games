using UnityEngine;
using System.Collections;

public class WorldControl : MonoBehaviour {

    public static WorldControl singleton;
    public GameObject CityMenu;
    public GameObject BuildingCanvas;
    public GameObject UnitCanvas;

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
