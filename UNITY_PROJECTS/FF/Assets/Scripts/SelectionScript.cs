using UnityEngine;
using System.Collections;

public class SelectionScript : MonoBehaviour {

    public GameManager GM;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("ship"))
        {
            GM.Select(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("ship"))
        {
            GM.Deselect(other.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
