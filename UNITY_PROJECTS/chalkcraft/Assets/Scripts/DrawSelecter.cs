using UnityEngine;
using System.Collections;

public class DrawSelecter : MonoBehaviour {

    public int index;

    private void OnMouseDown()
    {
        if (index >= 0)
        {
            DrawScript ds = GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>();
            ds.ChalkIndex = index;
            ds.drawingChalking = true;
            ds.OrderGiven = true;
            ds.SelectBox.position = transform.position;
        }

        else
        {
            DrawScript ds = GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>();
            ds.drawingChalking = false;
            ds.OrderGiven = true;
            ds.SelectBox.position = transform.position;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
