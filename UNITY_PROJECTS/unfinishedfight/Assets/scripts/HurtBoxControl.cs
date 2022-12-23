using UnityEngine;
using System.Collections;

public class HurtBoxControl : MonoBehaviour {

    public int Damage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<FighterScript>().HP -= Damage;
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
