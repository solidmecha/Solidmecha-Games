using UnityEngine;
using System.Collections;

public class SetRocketJumpScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerScript>().RocketJumpTime = .2f;
        }
    }
}
