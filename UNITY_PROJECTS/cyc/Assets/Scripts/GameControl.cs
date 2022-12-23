using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public List<GameObject> Movable_Entities = new List<GameObject> { };

    public void SnapToGrid()
    {
        foreach(GameObject g in Movable_Entities)
        {
            g.GetComponent<Rigidbody>().velocity = Vector3.zero;
            g.transform.position = new Vector3(Mathf.Round(g.transform.position.x), Mathf.Round(g.transform.position.y), Mathf.Round(g.transform.position.z));
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
