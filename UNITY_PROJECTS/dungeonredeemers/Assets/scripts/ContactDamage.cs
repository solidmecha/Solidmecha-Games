using UnityEngine;
using System.Collections;

public class ContactDamage : MonoBehaviour {

    public bool EnemyControlled;
    public int Damage;
    public int Type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(EnemyControlled && collision.CompareTag("Player"))
        {
            collision.GetComponent<StatScript>().RollBlockAndDodge(Damage, Type);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
