using UnityEngine;
using System.Collections;

public class InvertMove : MonoBehaviour {

    public float[] Counter;
	
	// Update is called once per frame
	void Update () {
        Counter[0] -= Time.deltaTime;
        if(Counter[0]<=0)
        {
            GetComponent<MoveIt>().move *= -1;
            Counter[0] = Counter[1];
        }
	}
}
