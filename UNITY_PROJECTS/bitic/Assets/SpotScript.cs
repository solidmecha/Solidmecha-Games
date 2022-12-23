using UnityEngine;
using System.Collections;

public class SpotScript : MonoBehaviour {
    public bool Taken;
    public void SetTaken(bool B)
    {
        Taken = B;
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = !Taken;
    }
}
