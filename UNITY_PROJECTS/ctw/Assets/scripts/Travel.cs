using UnityEngine;
using System.Collections;

public class Travel : MonoBehaviour {

    public Vector2 Dir;
    public Vector2 Dest;
    float Counter;
    public bool immuned=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Dir * Time.deltaTime * 2);
        Counter += Time.deltaTime;
        if (Counter >= .5f)
        {
            transform.position = Dest;
            if (!immuned)
                Destroy(this);
            else { Destroy(gameObject); }
        }
	}
}
