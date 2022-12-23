using UnityEngine;
using System.Collections;

public class AmmoScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerScript>().AddAmmo(6);
            Destroy(gameObject);
        }
    }
}
