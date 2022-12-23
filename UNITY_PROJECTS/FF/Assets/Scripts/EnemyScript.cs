using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {

    public GameObject Projectile;
    bool isFiring;
    public float Cooldown;
    float counter;
    public Action Attack;
    public int AttackMode;
    List<GameObject> PotentialTargets=new List<GameObject> { };
    List<Action> AttackList = new List<Action> { };
    GameObject Target;
    System.Random RNG = new System.Random(ThreadSafeRandom.Next());
    public Vector3 TargetLocation;
    public float speed;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.CompareTag("ship") || Other.gameObject.CompareTag("buildings"))
        {
            PotentialTargets.Add(Other.gameObject);
            if (Target != null)
            {
                HealthScript hst = (HealthScript)Target.GetComponent(typeof(HealthScript));
                HealthScript hs = (HealthScript)Other.GetComponent(typeof(HealthScript));
                if (hst.hp > hs.hp)
                    Target = Other.gameObject;
            }
            else
            {
                Target = Other.gameObject;
                Attack();
                isFiring = true;
            }
                }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if ((Other.gameObject.CompareTag("ship") || Other.gameObject.CompareTag("buildings")))
        {
            PotentialTargets.Remove(Other.gameObject);
            if (Other.gameObject.Equals(Target) && PotentialTargets.Count > 0)
            {
                SelectTarget();
            }
            else if (PotentialTargets.Count == 0)
            {
                isFiring = false;
                Target = null;
            }
        }
        }
    void SelectTarget()
    {
        int l=PotentialTargets.Count;
        if (l > 0)
        {
            for (int i = l - 1; i >= 0; i--)
            {
                if (PotentialTargets[i] == null)
                    PotentialTargets.RemoveAt(i);
            }
        }
        if (PotentialTargets.Count > 0)
        {
            Target = PotentialTargets[0];            
            HealthScript hst = (HealthScript)Target.GetComponent(typeof(HealthScript));
            foreach (GameObject T in PotentialTargets)
            {
                if (T != null)
                {
                    HealthScript hs = (HealthScript)T.GetComponent(typeof(HealthScript));
                    if (hs.hp < hst.hp)
                    {
                        hst = hs;
                        Target = T;
                    }
                }
            }
        }
        else
        {
            Target = null;
            isFiring = false;
        }
    }

	// Use this for initialization
	void Start () {
        AttackList.Add(CircleShot);
        AttackList.Add(Gattling);
        Attack = AttackList[AttackMode];
    }

    void CircleShot()
    {
        for (int i = 0; i < 20; i++)
            Instantiate(Projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, i*18)));
    }
	
    void Gattling()
    {
        if(Target!=null)
        {
            GameObject go=Instantiate(Projectile, transform.position, Quaternion.identity) as GameObject;
            go.transform.eulerAngles = RotateTowards(Target.transform.position-transform.position);
        }
    }

    public Vector3 RotateTowards(Vector2 V)
    {
        Vector3 Rot;
        if (V.x >= 0)
            Rot = new Vector3(0, 0, -1 * Vector2.Angle(Vector2.up, V));
        else
            Rot = new Vector3(0, 0, Vector2.Angle(Vector2.up, V));
        return Rot;
    }

    // Update is called once per frame
    void Update () {
        if (isFiring)
        {
            counter += Time.deltaTime;
            if (counter >= Cooldown)
            {
                Attack();
                counter = 0;
                if (Target == null && PotentialTargets.Count > 0)
                {
                    SelectTarget();
                }
            }
        }
        else
        {
            Vector2 v = TargetLocation - transform.position;
            transform.parent.eulerAngles=RotateTowards(v);
            transform.parent.Translate(Vector2.up*speed*Time.deltaTime);
        }
	}
}
