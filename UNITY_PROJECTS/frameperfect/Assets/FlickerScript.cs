using UnityEngine;
using System.Collections;

public class FlickerScript : MonoBehaviour {
    float counter;
    float FrameTime;
    bool Ready;
    int FrameCount;
    public GameObject target;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        bool pushFrame = false;
        counter += Time.deltaTime;
        if (counter >= FrameTime)
        {
            pushFrame = true;
            counter = 0;
        }
        if (pushFrame)
        {
            FrameCount = (FrameCount + 1) % 30;
            if(FrameCount==20)
            {
                transform.position = new Vector2(FrameControl.singleton.RNG.Next(-7, 8), FrameControl.singleton.RNG.Next(-4, 5));
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else if(FrameCount==0)
            {
                target.transform.position = new Vector2(FrameControl.singleton.RNG.Next(-7, 8), FrameControl.singleton.RNG.Next(-4, 5));
                target.GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Ready = true;
            }
            else if(FrameCount==1)
            {
                Ready = false;
                target.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        if(Ready && Input.anyKey)
        {
            FrameControl.singleton.win();
            Destroy(transform.parent.gameObject);
        }
    }
}
