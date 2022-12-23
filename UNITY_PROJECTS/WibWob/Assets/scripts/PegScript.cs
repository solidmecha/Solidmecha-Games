using UnityEngine;
using System.Collections;

public class PegScript : MonoBehaviour {
    public GameControl GC;
    public bool covered;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Untagged"))
        {
           SpriteRenderer sr= other.GetComponent<SpriteRenderer>();
            if(sr.color.Equals(GetComponent<SpriteRenderer>().color))
            {
                covered = true;
                if(GC != null)
                    GC.CheckWin();
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Untagged"))
            covered = false;
    }

	// Use this for initialization
	void Start () {
        GC = (GameControl)GameObject.Find("GameController").GetComponent(typeof(GameControl));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
