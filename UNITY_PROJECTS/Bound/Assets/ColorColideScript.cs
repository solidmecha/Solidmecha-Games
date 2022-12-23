using UnityEngine;
using System.Collections;

public class ColorColideScript : MonoBehaviour {

    bool hasColor;
    System.Random RNG;

    void OnCollisionEnter2D()
    {
        if(!hasColor)
        {
            int r, g, b;
            do
            {
                r = RNG.Next(2);
                g = RNG.Next(2);
                b = RNG.Next(2);
                GetComponent<SpriteRenderer>().color = new Color(r, g, b);
                hasColor = true;
            }
            while ((r == 0 && g == 0 && b == 0) || (r == 1 && g == 1 && b == 1));
        }
    }

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
