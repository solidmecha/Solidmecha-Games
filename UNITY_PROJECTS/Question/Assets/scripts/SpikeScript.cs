using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {

    public GameControl GC;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.transform.position = GC.Checkpoint.transform.position;
        }
    }

	// Use this for initialization
	void Start () {
        GC = (GameControl)GameObject.Find("Controller").GetComponent(typeof(GameControl));
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
