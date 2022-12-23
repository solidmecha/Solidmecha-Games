using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

    bool shouldHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shouldHit)
            GetComponent<SpriteRenderer>().color = Color.green;
        else
            GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (shouldHit)
            GetComponent<SpriteRenderer>().color = Color.red;
        else
            GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void Awake()
    {
        shouldHit=GameControl.singleton.RNG.Next(2) == 0;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
