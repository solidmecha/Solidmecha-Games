using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    public static PlayerControl singleton;
    float counter;
    public List<CityScript> Cities;

    private void Awake()
    {
        singleton = this;
    }
    // Use this for initialization
    void Start () {
	
	}

    void GenerateCityResources()
    {
        foreach (CityScript c in Cities)
            c.GenerateResources();
    }
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if(counter<=0)
        {
            counter = 1;
            GenerateCityResources();
        }
	}
}
