using UnityEngine;
using System.Collections.Generic;

public class ResourceStationScript : MonoBehaviour {
    GameManager GM;
   public GameObject worker;
    void OnMouseDown()
    {
        if (GM.CurrentSelection.Count == 1 && GM.CurrentSelection[0].name.Equals("worker(Clone)") && worker ==null)
        {
            worker = GM.CurrentSelection[0];
            WorkerScript ws = (WorkerScript)GM.CurrentSelection[0].GetComponent(typeof(WorkerScript));
            if (ws.HomeStation == null)
            {
                ws.HomeStation = gameObject;
                GameObject[] CommandStations = GameObject.FindGameObjectsWithTag("command");
                GameObject C = CommandStations[0].transform.parent.gameObject;
                foreach (GameObject g in CommandStations)
                {
                    if (Vector2.Distance(transform.position, g.transform.parent.position) < Vector2.Distance(C.transform.position, transform.position))
                        C = g.transform.parent.gameObject;
                }
                ws.HomeCommand = C;
                ws.isGathering = true;
                UnitScript us = (UnitScript)GM.CurrentSelection[0].GetComponent(typeof(UnitScript));
                us.move(transform.position - GM.CurrentSelection[0].transform.position);
            }
        }

    }

	// Use this for initialization
	void Start () {
	GM = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
