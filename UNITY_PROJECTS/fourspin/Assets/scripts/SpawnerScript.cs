using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public float counter;
    public float MoveDelay;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if(counter<=0)
        {
            counter = MoveDelay;
            transform.position += Vector3.right;
            if (transform.position.x > 6)
                transform.position = new Vector2(-6, 7);
        }
	}
}
