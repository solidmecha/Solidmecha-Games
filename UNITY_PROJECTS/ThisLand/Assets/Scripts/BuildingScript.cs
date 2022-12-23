using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {

    public float delay;
    float counter;
    public GameObject Resource;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter>=delay)
        {
            Instantiate(Resource, transform.position, Quaternion.identity);
            counter = 0;
        }
	}
}
