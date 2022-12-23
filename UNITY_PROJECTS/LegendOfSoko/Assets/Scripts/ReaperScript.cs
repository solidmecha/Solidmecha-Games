using UnityEngine;
using System.Collections;

public class ReaperScript : MonoBehaviour {

    float delay;
    float counter;
    WaveControl wc;
	// Use this for initialization
	void Start () {
        delay = 6;
        wc = GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveControl>();
	}

    void ChangeWeakness()
    {
        int r = wc.RNG.Next(1,3);
        GetComponent<MinionScript>().WeaknessID = r;
        GetComponent<SpriteRenderer>().color = wc.Colors[r];
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(delay<=counter)
        {
            counter = 0;
            ChangeWeakness();
        }
	}
}
