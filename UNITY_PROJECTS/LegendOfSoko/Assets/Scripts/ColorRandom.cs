using UnityEngine;
using System.Collections;

public class ColorRandom : MonoBehaviour {

	// Use this for initialization
	void Start () {
        WaveControl wc = GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveControl>();
        GetComponent<SpriteRenderer>().color = new Color(wc.RNG.Next(256) / 255f, wc.RNG.Next(256) / 255f, wc.RNG.Next(256) / 255f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
