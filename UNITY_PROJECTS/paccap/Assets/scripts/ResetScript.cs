using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerScript>().ResetPlayer();
        }
    }
}
