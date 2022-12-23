using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
    Vector2 dir;
	// Use this for initialization
	void Start () {
        dir = new Vector2(GameControl.singleton.RNG.Next(-3, 4), GameControl.singleton.RNG.Next(-4, 5));
        dir = dir.normalized;

    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(dir.normalized * Time.deltaTime);
	}
}
