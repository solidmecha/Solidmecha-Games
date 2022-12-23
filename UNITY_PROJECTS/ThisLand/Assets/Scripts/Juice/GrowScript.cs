using UnityEngine;
using System.Collections;

public class GrowScript : MonoBehaviour {
    public Sprite Grain;
    public Sprite Empty;
    bool withGrain;
    public float delay;
    float counter;
    GameControl GC;

	// Use this for initialization
	void Start () {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
    }

    public void Harvest()
    {
        if (withGrain)
        {
            GC.GrainTime += 12;
            GetComponent<SpriteRenderer>().sprite = Empty;
            withGrain = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(!withGrain)
            counter += Time.deltaTime;
        if(counter>=delay && !withGrain)
        {
            withGrain = true;
            GetComponent<SpriteRenderer>().sprite = Grain;
            counter = 0;
        }
	}
}
