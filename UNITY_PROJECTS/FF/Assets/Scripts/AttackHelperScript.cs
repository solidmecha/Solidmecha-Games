using UnityEngine;
using System.Collections.Generic;

public class AttackHelperScript : MonoBehaviour {

    UnitScript us;
    public List<GameObject> PotentialTargets = new List<GameObject> { };

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag.Equals("enemy"))
        {
            PotentialTargets.Add(Other.gameObject);
            if (!us.Attacking)
            {
                us.isMoving = false;
                us.Attacking = true;
                us.Target = Other.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.tag.Equals("enemy"))
        {
            PotentialTargets.Remove(Other.gameObject);
        }
            if (Other.gameObject == us.Target)
        {
            us.Target = null;
            SelectTarget();
        }
    }
    void SelectTarget()
    {
        PotentialTargets.Remove(null);
        if (PotentialTargets.Count > 0)
        {
            us.Target = PotentialTargets[0];
        }
        else
        {
            us.Target = null;
            us.Attacking = false;
        }
    }

    // Use this for initialization
    void Start () {
        us = (UnitScript)transform.parent.GetComponent(typeof(UnitScript));
    }
	
	// Update is called once per frame
	void Update () {
        if (us.Target == null && PotentialTargets.Count > 0)
        {
            SelectTarget();
        }
    }
}
