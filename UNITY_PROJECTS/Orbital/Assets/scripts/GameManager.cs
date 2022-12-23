using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Circle;
    public float delay;
    public float counter;
    System.Random RNG;

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
	}
	
	// Update is called once per frame
	void Update () {

        counter += Time.deltaTime;

        if(counter>delay)
        {
            Vector2 v = new Vector2((float)RNG.NextDouble(), (float)RNG.NextDouble());
            v = v.normalized;
           GameObject go= Instantiate(Circle) as GameObject;
            float r = (float)RNG.NextDouble() / 2 + .1f;
            go.GetComponent<SpriteRenderer>().color = new Color((float)RNG.NextDouble(), (float)RNG.NextDouble(), (float)RNG.NextDouble());
            go.transform.localScale=new Vector2(r, r);
            CircleScript cs = (CircleScript)go.GetComponent(typeof(CircleScript));
            cs.direction = v;
            cs.speed = (float)RNG.NextDouble() * 4;   
        }

	}
}
