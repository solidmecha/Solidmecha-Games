using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletScript : NetworkBehaviour
{
    [SyncVar]
    public Vector2 Target;
    [SyncVar]
    public Color TargetColor;
    float counter;

	// Use this for initialization
	void Start () {
        Target = (Target - (Vector2)transform.position)/1.5f;	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Target * Time.deltaTime);
        counter += Time.deltaTime;
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, TargetColor, counter);
	}
}
