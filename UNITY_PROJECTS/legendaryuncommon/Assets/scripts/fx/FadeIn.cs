using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

    public bool fading;

	// Use this for initialization
	void Start () {
	
	}

    public void stop()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 76f / 255f, 1f, 0);
        fading = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (fading)
            GetComponent<SpriteRenderer>().color = new Color(0, 76f / 255f, 1f, Mathf.PingPong(Time.time, 1));
	}
}
