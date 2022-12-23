using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    public GameManager GM;
    public List<Vector2> TargetVecs=new List<Vector2> { };
    public int tier;
    public bool Selected;
    public TowerSpawn ts;
    public int boolIndex;

    void OnMouseOver()
    {
        if(GM.TargetDisplay.Count>0)
        {
            foreach (GameObject g in GM.TargetDisplay)
                Destroy(g);
            GM.TargetDisplay.Clear();
        }
        foreach(Vector2 v in TargetVecs)
        {
            GameObject go = Instantiate(GM.Target, v, Quaternion.identity) as GameObject;
            GM.TargetDisplay.Add(go);
        }
    }

    void OnMouseExit()
    {
            if (GM.TargetDisplay.Count > 0)
            {
                foreach (GameObject g in GM.TargetDisplay)
                    Destroy(g);
                GM.TargetDisplay.Clear();
            }
            if(GM.SelectedTower != null)
        {
            foreach (Vector2 v in GM.SelectedTower.GetComponent<PlayerControl>().TargetVecs)
            {
                GameObject go = Instantiate(GM.Target, v, Quaternion.identity) as GameObject;
                GM.TargetDisplay.Add(go);
            }
        }
    }

    void OnMouseDown()
    {
        SelectTower();
    }

    public void Fire()
    {
        bool hitSome=false;

        foreach(Vector2 v in TargetVecs)
        {
            RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);

            if(hit.collider != null)
            {
                hitSome = true;
            }
            while(hit.collider !=null)
            {
                GM.MSlist.Remove(hit.collider.GetComponent<MinionScript>());
                Destroy(hit.collider.gameObject);
                hit = Physics2D.Raycast(v, Vector2.zero);
            }
        }
        TargetVecs.Clear();
        if (hitSome)
        {
            if (tier != 5)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GM.tierList[tier];
            }
            else
            {
                tier = 0;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GM.tierList[tier];
            }
            tier++;
        }
    }

    public void SelectTower()
    {
        GM.SwitchOffMode();
        GM.CurrSelect=Instantiate(GM.Select, transform.position, Quaternion.identity) as GameObject;
        if (GM.TargetDisplay.Count > 0)
        {
            foreach (GameObject g in GM.TargetDisplay)
                Destroy(g);
            GM.TargetDisplay.Clear();
        }
        TargetVecs.Clear();
        GM.targetMode = true;
        GM.SelectedTower = gameObject;
    }

    public void TryTarget(Vector2 v)
    {
        foreach(PlayerControl p in GM.PClist)
        {
            if (p.transform.position.x == GM.transform.position.x + v.x && p.transform.position.y == GM.transform.position.y + v.y)
                return;
        }
        Vector2 Dir = (Vector2)transform.position - v - (Vector2)GM.transform.position;
        float D = Vector2.Distance(v, transform.position - GM.transform.position);
        if ((Mathf.Abs(Dir.x) <= 5 && Mathf.Abs(Dir.y) <= 5) && D != 0 && TargetVecs.Count<tier)
        {
            GameObject go = Instantiate(GM.Target, (Vector2)GM.transform.position + v, Quaternion.identity) as GameObject;
            TargetVecs.Add((Vector2)GM.transform.position+v);
            GM.TargetDisplay.Add(go);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
