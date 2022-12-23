using UnityEngine;
using System.Collections;

public class FXscript : MonoBehaviour {
    public float bSpeed;
    public float rSpeed;
    public float bTime;
    public float rTime;
    public float rCounter;
    public float bCounter;
    public Vector2 dir;
	// Use this for initialization
	void Start () {
        dir = Vector2.up;
        rCounter = rTime/2;
        bCounter = bTime/2;
	}
	
	// Update is called once per frame
	void Update () {
        rCounter -= Time.deltaTime;
        bCounter -= Time.deltaTime;
        if(rCounter<=0)
        {
            rCounter = rTime;
            rSpeed *= -1;
        }
        if(bCounter<=0)
        {
            bCounter = bTime;
            bSpeed *= -1;
        }
        transform.Rotate(new Vector3(0, 0, rSpeed) * Time.deltaTime);
        transform.Translate(dir * bSpeed * Time.deltaTime);
	
	}
}
