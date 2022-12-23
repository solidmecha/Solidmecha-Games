using UnityEngine;
using System.Collections;

public class SizeScript : MonoBehaviour {

    float FrameTime;
    float counter;
    int size;
    int change=1;
    int maxSize=120;
    int minSize=0;
    int TargetSize = 100;
    public Transform Target;

	// Use this for initialization
	void Start () {
        FrameTime = 1f/FrameControl.singleton.FPS;
    }
	
	// Update is called once per frame
	void Update () {
        bool pushFrame = false;
        counter += Time.deltaTime;
        if(counter>=FrameTime)
        {
            pushFrame = true;
            counter = 0;
        }
        if(pushFrame)
        {
            size += change;
            if (size > maxSize)
            {
                size = maxSize;
                change *= -1;
            }
            else if(size < minSize)
            {
                size = minSize;
                change *= -1;
            }
            if(size>TargetSize)
            {
                transform.position = Vector3.zero;
                Target.position = new Vector3(0, 0, -1);
            }
            else if(size<TargetSize)
            {
                transform.position = new Vector3(0, 0, -1);
                Target.position = Vector3.zero;
            }
            transform.localScale = new Vector2(size / 100f, size / 100f);
        }
        if(size==TargetSize && Input.anyKeyDown)
        {
            FrameControl.singleton.win();
            Destroy(transform.parent.gameObject);
        }
	}
}
