using UnityEngine;
using System.Collections.Generic;

public class WorldControl : MonoBehaviour {

    List<string> prefixes = new List<string> { "circ", "diamond", "para", "tri" };
    List<string> suffixes = new List<string> { "_ol", "_fill" };
    System.Random RNG;

    // Use this for initialization
    void Start () {
        RNG = new System.Random();
        Create(RNG.Next(5), RNG.Next(2,10), RNG.Next(2, 10));
	}

    void Create(int MoveBehavior, int scaleFactor, int rotationFactor)
    {
        for (int i = 0; i < 199; i++)
        {
            GameObject go = Instantiate(Resources.Load(GetName(), typeof(GameObject))) as GameObject;
            go.transform.position = new Vector2(RNG.Next(-7000, 7001) / 1000f, RNG.Next(-4500, 4501) / 1000f);
            if (RNG.Next(scaleFactor) == 0)
            {
                float s = RNG.Next(45, 251) / 100f;
                go.transform.localScale = new Vector2(s, s);
            }
            if (RNG.Next(rotationFactor) == 0)
                go.transform.localEulerAngles = new Vector3(0, 0, RNG.Next(361));

            ColorScript cs = go.GetComponent<ColorScript>();
            
            switch (MoveBehavior)
            {
                case 0:
                case 1:
                    cs.Move_ID = MoveBehavior;
                    break;
                case 3:
                    cs.Move_ID = RNG.Next(3);
                    cs.dir = new Vector2(RNG.Next(-1, 2), RNG.Next(-1, 2));
                    cs.speed = RNG.Next(100, 501) / 100;
                    break;
                default:
                    cs.Move_ID = 2;
                    cs.dir = new Vector2(RNG.Next(-1, 2), RNG.Next(-1, 2));
                    cs.speed = RNG.Next(100, 501) / 100;
                    break;
            }
        }
    }

    string GetName()
    {
        return prefixes[RNG.Next(prefixes.Count)] + suffixes[RNG.Next(suffixes.Count)];

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject[] GOarray=GameObject.FindGameObjectsWithTag("S");
            int r = RNG.Next(GOarray.Length);
            if (r > 0)
            {
                GOarray[r].GetComponent<ColorScript>().Click();
                GOarray[r].tag = "Untagged";
            }
        }
	
	}
}
