using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("attach"))
        {
            PlayerControl pc = (PlayerControl)transform.root.GetComponent(typeof(PlayerControl));
            if (!pc.move)
            {
                other.gameObject.transform.SetParent(transform.parent);
                if(other.name.Equals("vac(Clone)"))
                {
                    other.offset = new Vector2(0, .65f);
                }
                Destroy(gameObject);
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
