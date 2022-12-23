using UnityEngine;
using System.Collections;

public class Blink : SkillScript {

    public override void Activate(Vector2 V)
    {
        base.Activate(V);
        Instantiate(Effect, transform.position, Quaternion.identity);
        GetComponent<PlayerScript>().isMoving = false;
        transform.position = V;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
}
