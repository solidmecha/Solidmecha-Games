using UnityEngine;
using System.Collections;

public class ColorScript : MonoBehaviour {
    float counter;
    float FrameTime;
    int r;
    int g;
    int b;
    int TargetR=0;
    int TargetB=255;
    int TargetG=233;
    SpriteRenderer SR;

	// Use this for initialization
	void Start () {
        FrameTime = 1f/FrameControl.singleton.FPS;
        SR = GetComponent<SpriteRenderer>();
        r = TargetR;
        g = TargetG;
        b = TargetB;
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
            b = (b + 8) % 256;
            g = (g + 8) % 256;
            SR.color = new Color(0f, g/255f, b/255f);
        }
        if (b == TargetB && Input.anyKeyDown)
        {
            FrameControl.singleton.win();
            Destroy(transform.parent.gameObject);
        }
    }
}
