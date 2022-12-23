using UnityEngine;
using System.Collections;

public class ProbCell : MonoBehaviour {

    public int probability;
    public bool live;
    public int Value;
    public ProbCell[] Neighbors;

    public void SetLife()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        live = true;
    }

    public void SetDeath()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        live = false;
    }

    public void SetValue(int V, Color one, Color two)
    {
        Value = V;
        GetComponent<SpriteRenderer>().color = Color.Lerp(one, two, V/9f);
        live = V >= 5;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
