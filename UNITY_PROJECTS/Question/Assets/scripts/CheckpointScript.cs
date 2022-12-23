using UnityEngine;
using System.Collections;

public class CheckpointScript : MonoBehaviour {

   public GameControl GC;

    void OnTriggerEnter2D(Collider2D other)
    { 
            if (other.gameObject.tag.Equals("Player"))
            {
            if (!GC.Checkpoint.Equals(gameObject))
            {
                GC.Checkpoint.GetComponent<SpriteRenderer>().color = new Color(.905f, 1, .33f);
                GC.Checkpoint = gameObject;
                GetComponent<SpriteRenderer>().color = Color.green;
            }
            }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
