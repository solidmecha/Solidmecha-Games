using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !col.GetComponent<BotScript>().inImmunity)
        {
            col.GetComponent<BotScript>().TakeDamage(1);
        }
        else if (col.CompareTag("B"))
        {
            transform.parent.GetComponent<SaucerScript>().HealthBar.transform.localScale += new Vector3(0,1, 0);
        }
    }
}
