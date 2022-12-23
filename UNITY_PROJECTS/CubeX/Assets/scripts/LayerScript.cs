using UnityEngine;
using System.Collections;

public class LayerScript : MonoBehaviour {

    public int index;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("C"))
        {
            ColliderScript cs = (ColliderScript)other.GetComponent(typeof(ColliderScript));
            cs.boolList[index] = true;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
