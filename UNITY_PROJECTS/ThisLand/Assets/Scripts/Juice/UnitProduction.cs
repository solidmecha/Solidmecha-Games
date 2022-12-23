using UnityEngine;
using System.Collections;

public class UnitProduction : MonoBehaviour {

    public GameObject Unit;
    public float delay;
    float counter;
    public int maxUnits;
    int unitCount;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter>=delay)
        {
            Instantiate(Unit, transform.position, Quaternion.identity);
            unitCount++;
            if (unitCount >= maxUnits)
                Destroy(gameObject);
            counter = 0;
        }

	}
}
