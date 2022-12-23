using UnityEngine;
using System.Collections;

public class BubbleScript : MonoBehaviour {

    public bool isIdle;
    public Vector2 deltaScale;
    public float BaseChange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //BubbleScript B = collision.GetComponent<BubbleScript>();
        if(!isIdle)
        {
            deltaScale = deltaScale * -1;    
        }
        else
        {
            isIdle = false;
            deltaScale = Vector2.one * BaseChange;
        }
    }

    private void OnMouseDown()
    {
        if (!isIdle)
        {
            deltaScale = deltaScale * -1;
        }
        else
        {
            isIdle = false;
            deltaScale = Vector2.one * BaseChange;
        }
    }

    // Use this for initialization
    void Start () {
        isIdle = true;
        deltaScale = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {
	 if(!isIdle)
        {
            transform.localScale = (Vector2)transform.localScale + deltaScale*Time.deltaTime;
            if(deltaScale.x<0 && transform.localScale.x<1)
            {
                isIdle = true;
                deltaScale = Vector2.zero;
                transform.localScale = Vector2.one;
            }
        }
	}
}
