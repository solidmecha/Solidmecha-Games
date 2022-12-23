using UnityEngine;
using System.Collections;

public class WorkerScript : MonoBehaviour {

    public GameObject HomeCommand;
    public GameObject HomeStation;
    public bool isGathering;
    UnitScript us;
    GameObject res;
    public int ResValue;

    public void Gather()
    {
        us.move(HomeCommand.transform.position - transform.position);
        res = Instantiate(us.GM.Resource, (Vector2)transform.position, transform.rotation, transform)as GameObject;
        res.transform.localPosition = (Vector2)res.transform.localPosition + .4f * Vector2.up;
    }
    public void DropOff()
    {
        us.GM.Plat += ResValue;
        us.GM.UpdateGUI();
        Destroy(res);
        us.move(HomeStation.transform.position - transform.position);
    }

	// Use this for initialization
	void Start () {
        us = (UnitScript)GetComponent(typeof(UnitScript));
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isGathering && !us.isMoving)
        {
            if (Vector2.Distance(HomeStation.transform.position,transform.position)<=.01f)
                Gather();
            else if (Vector2.Distance(HomeCommand.transform.position, transform.position) <= .01f)
                DropOff();
            else
            {
                isGathering = false;
                if (res != null)
                    Destroy(res);
                ResourceStationScript rs = (ResourceStationScript)HomeStation.GetComponent(typeof(ResourceStationScript));
                rs.worker = null;
                HomeStation = null;
                HomeCommand = null;
            }
        }
	
	}
}
