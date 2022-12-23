using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float Speed;
    public float Dmg;
    private void OnTriggerEnter(Collider other)
    {
        Health hp = other.gameObject.GetComponent<Health>();
        if (hp != null)
        {
            hp.Change(Dmg);
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
	}
}
