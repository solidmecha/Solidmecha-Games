using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {
    public GameManager GM;
    public GameObject Display;
    public bool isSmall;
    public bool isBuilding;
    public UnitScript worker;
    float counter;
    int buildingID;
    bool showProgress;
    void OnMouseDown()
    {
        if (GM.CurrentSelection.Count == 1 && GM.CurrentSelection[0].name.Equals("worker(Clone)"))
            displayUI();
    }
    
    void displayUI()
    {
        GameObject go=Instantiate(Display, transform.position, Quaternion.identity, transform) as GameObject;
        if(isSmall)
        {
            for (int i = 0; i < 4; i++)
            {
                UIBuildingSelect UBS = (UIBuildingSelect)go.transform.GetChild(i).GetComponent(typeof(UIBuildingSelect));
                int t = i;
                UBS.id = t;
                UBS.Bscript = this;
            }
        }
        else
        {
            for (int i = 4; i < 6; i++)
            {
                UIBuildingSelect UBS = (UIBuildingSelect)go.transform.GetChild(i-4).GetComponent(typeof(UIBuildingSelect));
                int t = i;
                UBS.id = t;
                UBS.Bscript = this;
            }
        }

    }

    public void BuildBuilding(int B)
    {
        if (GM.CurrentSelection.Count == 1 && GM.CurrentSelection[0].name.Equals("worker(Clone)"))
        {
            GM.Plat -= GM.BuildingCosts[B];
            GM.UpdateGUI();
            buildingID = B;
            worker = (UnitScript)GM.CurrentSelection[0].GetComponent(typeof(UnitScript));
            worker.move(transform.position - worker.transform.position);
            GM.Deselect(GM.CurrentSelection[0]);
            Destroy(worker.GetComponent<Collider2D>());
            isBuilding = true;
        }
       
    }
	// Use this for initialization
	void Start () {
        GM = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
    }
	
	// Update is called once per frame
	void Update () {
	if(isBuilding && !worker.isMoving)
        {
            if(!showProgress)
            {
                GameObject go=Instantiate(GM.progress, transform.position, Quaternion.identity) as GameObject;
                ProgressScript ps = (ProgressScript)go.GetComponent(typeof(ProgressScript));
                ps.Timer = GM.constructionTime[buildingID];
                showProgress = true;
            }
            counter+= Time.deltaTime;
            if (counter >= GM.constructionTime[buildingID])
            {
                Instantiate(GM.Buildings[buildingID], transform.position, Quaternion.identity);
                worker.move(Vector2.down);
                worker.gameObject.AddComponent<BoxCollider2D>();
                worker.gameObject.GetComponent<BoxCollider2D>().isTrigger=true;
                Destroy(gameObject);
            }
        }
	}
}
