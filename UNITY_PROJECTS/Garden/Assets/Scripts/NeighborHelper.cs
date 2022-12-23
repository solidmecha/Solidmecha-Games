using UnityEngine;
using System.Collections;

public class NeighborHelper : MonoBehaviour {

    public int index;
    public GameObject CommandUIobj;
    public Sprite sprite;
    public Vector2 Loc;
    GameObject ComWindow;
    float offset=-1;

    void OnMouseDown()
    {
        CellUI cellui = (CellUI)transform.parent.parent.GetComponent(typeof(CellUI));
        if (transform.position.y - cellui.CS.WSobj.transform.position.y > cellui.CS.WS.height / 2)
            offset = 1;
        if (cellui.CS.WS.CommandWindow == null)
        {
            buildComWindow();
            cellui.CS.WS.CommandWindow = ComWindow;
        }
       
    }

    void buildComWindow()
    {
        ComWindow = Instantiate(CommandUIobj, new Vector3(transform.position.x, transform.position.y-2f*offset,-2),Quaternion.identity) as GameObject;
        CommandUI cu = (CommandUI)ComWindow.GetComponent(typeof(CommandUI));
        cu.NH = this;
        cu.cellui = (CellUI)transform.parent.parent.gameObject.GetComponent(typeof(CellUI));
    
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
