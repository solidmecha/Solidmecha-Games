using UnityEngine;
using System.Collections.Generic;

public class BuildingScript : MonoBehaviour {

    string Title;
    public List<int> UnitIDs;

    public void ViewBuilding()
    {
        print("seen!");
    }

    public void BuildBuilding(CityScript City)
    {
        City.BuildingsBuilt[transform.GetSiblingIndex()] = true;
        transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "View";
        transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { ViewBuilding(); });
        print("here");
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
