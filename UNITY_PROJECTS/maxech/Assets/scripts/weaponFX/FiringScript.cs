using UnityEngine;
using System.Collections;

public class FiringScript : MonoBehaviour {

    public bool isDot;
    public float[] tick;
    public bool isFiring;
    public WeaponScript WS;

    private void OnTriggerEnter(Collider other)
    {
        if (isFiring && !isDot && other.CompareTag("enemy"))
        {
            WS.DealDamage(other.GetComponent<EnemyScript>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFiring && isDot && other.CompareTag("enemy"))
        {
            WS.DealDamage(other.GetComponent<EnemyScript>());
        }
    }

    // Use this for initialization
    void Start () {
        isFiring = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (isDot)
        {
            tick[0] -= Time.deltaTime;
            if (tick[0] <= 0)
            {
                tick[0] = tick[1];
                isFiring = true;
            }
            else
                isFiring = false;
        }
    }
}
