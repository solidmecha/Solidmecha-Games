using UnityEngine;
using System.Collections.Generic;

public class NodeScript : MonoBehaviour {

    public WirePuzzle WP;
    public List<WirePuzzle.Connection> connectedNodes= new List<WirePuzzle.Connection> { };
    public List<NodeScript> Neighbors=new List<NodeScript> { };
    public int ID;

    void OnMouseDown()
    {
         if (WP.selected)
          {
            if (Vector2.Distance(transform.localPosition, WP.SelectedNode.transform.localPosition) < .2f && WP.SelectedNode.ID !=ID)
            {
                WP.placeWire(this);
                WP.SelectedNode = this;
            }
            else
            {
                print(WP.checkFullSurround(this));
                WP.SelectedNode = this; }
      }
        else
       {
            WP.selected = true;
            WP.SelectedNode = this;
        }

        
       

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
