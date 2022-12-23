using UnityEngine;
using System.Collections;

public class lifeTime : MonoBehaviour {
    public float counter;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter <= 0)
            Destroy(gameObject);
	}
}
