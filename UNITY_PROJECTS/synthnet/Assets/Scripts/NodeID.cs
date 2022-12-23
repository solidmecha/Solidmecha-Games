using UnityEngine;
using System.Collections.Generic;

public class NodeID : MonoBehaviour {

    public int ID;
    public bool Charged;
    public bool Destroyed;
    public GameObject DestroyedIcon;
    public bool Displaced;


    public void Discharge()
    {
        GetComponent<SpriteRenderer>().color = new Color(.4f, .4f, .4f, .4f);
        Charged = false;
    }

    public void Recharge(Vector2 v)
    {

       RaycastHit2D hit= Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero);
        if(hit.collider!=null)
        {
            foreach(NodeID n in hit.collider.transform.parent.GetComponentsInChildren<NodeID>())
                {
                if(n.ID != 4 && n.ID !=3 && !n.Destroyed && !n.Charged)
                {
                    n.Charged = true;
                    n.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    public void Repair(Vector2 v)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero);
        if (hit.collider != null)
        {
            foreach (NodeID n in hit.collider.transform.parent.GetComponentsInChildren<NodeID>())
            {
                if (n.ID != 3 && n.Destroyed)
                {
                    n.Destroyed = false;
                   Destroy(n.DestroyedIcon);
                }
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
