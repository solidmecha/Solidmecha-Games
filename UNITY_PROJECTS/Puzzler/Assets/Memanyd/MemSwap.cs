using UnityEngine;
using System.Collections;

public class MemSwap : MonoBehaviour {

    public bool Col;

    private void OnMouseDown()
    {
        if(Col)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MemanydControl>().ColumnSwap(transform.position.x);
        }
        else
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MemanydControl>().RowSwap(transform.position.y);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
