using UnityEngine;
using System.Collections;

public class ShipBuildingScript : MonoBehaviour {
    GameManager GM;
    public GameObject Display;
    public int buildingID; //worker,pav, lav, ram
    public bool isSmall;
    bool isBuilding;
    bool showProgress;
    float counter;
    void OnMouseDown()
    {
            if(!isBuilding)
                displayUI();
    }

    void displayUI()
    {

        GameObject go = Instantiate(Display, new Vector3(transform.position.x,transform.position.y,-1), Quaternion.identity, transform) as GameObject;
        if (isSmall)
        {
            for (int i = 0; i < 4; i++)
            {
                ShipBuildingHelper sbh = (ShipBuildingHelper)go.transform.GetChild(i).GetComponent(typeof(ShipBuildingHelper));
                int t = i;
                sbh.id = t;
                sbh.sbs = this;
            }
            go.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = GM.Ships[buildingID].GetComponent<SpriteRenderer>().sprite;
            go.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GM.Ships[buildingID].GetComponent<SpriteRenderer>().sprite;
            go.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = GM.Turrets[buildingID].GetComponent<SpriteRenderer>().sprite;
            go.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = GM.CapitalMods[buildingID].GetComponent<SpriteRenderer>().sprite;

        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                ShipBuildingHelper sbh = (ShipBuildingHelper)go.transform.GetChild(i).GetComponent(typeof(ShipBuildingHelper));
                int t = i;
                sbh.id = t;
                sbh.sbs = this;
            }
        }

    }

    public void UIselected(int id)
    {
        switch(id)
        {
            case 0://build ship
                if (GM.checkPlat(GM.ShipCosts[buildingID]))
                {
                    GM.Plat -= GM.ShipCosts[buildingID];
                    GM.UpdateGUI();
                    isBuilding = true;
                }
                break;
            case 1://upgrade ship
                break;
            case 2://turret
                Instantiate(GM.Turrets[buildingID], transform.position+Vector3.back, Quaternion.identity,transform);
                Destroy(this);
                break;
            case 3://upgrade captialship mod
                break;
        }
    }

    // Use this for initialization
    void Start () {
        GM = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
    }
	
	// Update is called once per frame
	void Update () {
        if (isBuilding)
        {
            if (!showProgress)
            {
                GameObject go = Instantiate(GM.progress, transform.position, Quaternion.identity) as GameObject;
                ProgressScript ps = (ProgressScript)go.GetComponent(typeof(ProgressScript));
                ps.Timer = GM.ShipbuildTime[buildingID];
                showProgress = true;
            }
            counter += Time.deltaTime;
            if (counter >= GM.ShipbuildTime[buildingID])
            {
                Instantiate(GM.Ships[buildingID], transform.position+Vector3.back, Quaternion.identity);
                counter = 0;
                isBuilding = false;
                showProgress = false;
            }
        }
    }
}
