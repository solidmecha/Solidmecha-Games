using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {
    public float counter;
	// Use this for initialization
	void Start () {
        counter = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter < 0)
            Destroy(gameObject);
	
	}
}
