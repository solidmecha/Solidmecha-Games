using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    bool selected;
    public Vector2 Loc;

    void OnMouseDown()
    {
        //selected = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if(!selected)
        {
            Vector2 V = Loc;
            TileScript TS=(TileScript)col.gameObject.GetComponent(typeof(TileScript));
            Loc = TS.Loc;
            TS.Loc = V;
            transform.position = Loc;
        }*/
    }

	// Use this for initialization
	void Start () {
        Loc = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (selected)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
	}
}
