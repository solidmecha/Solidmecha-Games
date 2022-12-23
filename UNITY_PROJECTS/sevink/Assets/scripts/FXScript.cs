using UnityEngine;
using System.Collections;

public class FXScript : MonoBehaviour {
    float count = .5f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject.GetComponent<BoxCollider2D>());
	}
	
	// Update is called once per frame
	void Update () {
        count -= Time.deltaTime;
        if (count <= 0)
            Destroy(gameObject);
        transform.Rotate(new Vector3(0, 0, 720) * Time.deltaTime);
        transform.localScale -= Vector3.one*2* Time.deltaTime;
	
	}
}
