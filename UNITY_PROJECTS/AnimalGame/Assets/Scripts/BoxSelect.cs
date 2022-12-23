using UnityEngine;
using System.Collections;

public class BoxSelect : MonoBehaviour {
    public GameControl GC;
    public int id;

    void OnMouseDown()
    {
        if (GC.CurSelect[Mathf.Abs(id - 1)].Count == 0 || GC.CurSelect[Mathf.Abs(id - 1)].Count >= 4)
        {
            GC.csIndex = id;
            GC.SelectPos = (Vector2)transform.position + 4 * Vector2.down;
            GC.Outline.transform.position = transform.position;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
