using UnityEngine;
using System.Collections;

public class FarmScript : MonoBehaviour {

    public float delay;
    float counter;
    public GameControl GC;

	// Use this for initialization
	void Start () {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter>=delay)
        {
            GC.ResourceAmount[GC.RNG.Next(2)]++;
            GC.UpdateGUI();
            counter = 0;
        }
	}
}
