using UnityEngine;
using System.Collections;

public class evilOrbFx : MonoBehaviour {

    public GameControl.DamageType damageType;
    public EnemyScript ES;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControl>().takeDamage(ES.dmg, damageType);
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        damageType = GameControl.DamageType.hp;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
