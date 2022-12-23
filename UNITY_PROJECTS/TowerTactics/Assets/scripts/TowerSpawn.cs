using UnityEngine;
using System.Collections;

public class TowerSpawn : MonoBehaviour {

    public GameManager GM;
    public bool[] tierBuilt = new bool[5];

    void OnMouseDown()
    {
        GM.SwitchOffMode();
        GetComponent<SpriteRenderer>().color = Color.blue;
        GM.SelectedCastle = gameObject;
        GM.towerMode = true;
    }


    public void TryBuild(Vector2 v)
    {
        foreach(PlayerControl p in GM.PClist)
        {
            foreach(Vector2 tv in p.TargetVecs)
            {
                if (tv.x == v.x+GM.transform.position.x && tv.y == v.y+GM.transform.position.y)
                    return;
            }
        }
        Vector2 Dir=(Vector2)transform.position-v- (Vector2)GM.transform.position;
        float D = Vector2.Distance(v, transform.position - GM.transform.position);
        int t=-1;
        if (Mathf.Abs(Dir.x) > Mathf.Abs(Dir.y))
            t = (int)Mathf.Abs(Dir.x) - 1;
        else
            t = (int)Mathf.Abs(Dir.y) - 1;
        if ((Mathf.Abs(Dir.x)<=5 && Mathf.Abs(Dir.y) <= 5) && D !=0 && !tierBuilt[t])
        {
            GameObject go=Instantiate(GM.Tower, (Vector2)GM.transform.position+v, Quaternion.identity) as GameObject;
            PlayerControl pc=go.GetComponent<PlayerControl>();
            GM.PClist.Add(pc);
            pc.GM = GM;
            pc.boolIndex = t;
            pc.ts = this;
            pc.tier = t;          
            tierBuilt[pc.tier] = true;
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GM.tierList[pc.tier];
            pc.tier++;
            //pc.SelectTower();

        }
    }

	// Use this for initialization
	void Start () {
        GM = GameObject.Find("GameControl").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
