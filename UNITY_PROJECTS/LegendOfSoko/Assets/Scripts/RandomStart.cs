using UnityEngine;
using System.Collections;

public class RandomStart : MonoBehaviour {

    bool placed;

	// Use this for initialization
	void Start () {
       WaveControl wc= GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveControl>();
        while (!placed)
        {
            int x = wc.RNG.Next(-12, 14);
            int y = wc.RNG.Next(-8, 9);
            Vector2 v = new Vector2(x, y);
            if (!Physics2D.Raycast(v, Vector2.zero))
            {
                transform.position = v;
                placed = true;
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
