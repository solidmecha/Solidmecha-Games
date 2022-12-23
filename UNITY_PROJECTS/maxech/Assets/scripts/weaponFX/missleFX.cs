using UnityEngine;
using System.Collections;

public class missleFX : MonoBehaviour {

    public GameObject explo;
    public WeaponScript WS;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("floor") || other.CompareTag("enemy"))
        {
            (Instantiate(explo, transform.position, Quaternion.identity) as GameObject).GetComponent<FiringScript>().WS=WS;

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
