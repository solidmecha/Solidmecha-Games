using UnityEngine;
using System.Collections;

public class InterceptorScript : MonoBehaviour {
    public GameObject Target;
    Vector2 TargetPos;
    public GameObject Home;
    UnitScript us;
    public int dmg;
    bool isReturning;

	// Use this for initialization
	void Start () {
        us = (UnitScript)GetComponent(typeof(UnitScript));
	}
	
	// Update is called once per frame
	void Update () {
        if (Target != null)
        {
            if (TargetPos.x != Target.transform.position.x || TargetPos.y != Target.transform.position.y)
            {
                TargetPos = Target.transform.position;
                us.move(TargetPos - (Vector2)transform.position);
            }

            if (Vector2.Distance(transform.position, Target.transform.position) < .01f)
            {
                if (!isReturning)
                {
                    HealthScript hs = (HealthScript)Target.GetComponent(typeof(HealthScript));
                    hs.dealDmg(dmg);
                    isReturning = true;
                    Target = Home;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            isReturning = true;
            Target = Home;
            if(Home==null)
            {
                Destroy(gameObject);
            }
        }
    }
	
}
