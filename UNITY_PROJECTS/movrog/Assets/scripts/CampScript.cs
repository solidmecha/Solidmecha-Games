using UnityEngine;
using System.Collections.Generic;

public class CampScript : MonoBehaviour {

    public List<UnitScript> Mobs;

    // Use this for initialization
    void Start () {
        foreach (UnitScript m in GetComponentsInChildren<UnitScript>())
            Mobs.Add(m);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
