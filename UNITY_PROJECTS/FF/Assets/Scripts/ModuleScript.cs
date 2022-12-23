using UnityEngine;
using System.Collections.Generic;

public class ModuleScript : MonoBehaviour {

    public int ID;
    public GameObject Target;
    public bool Attacking;
    public GameObject Projectile;
    public float FireDelay;
    float counter;
    public bool isTurret;
    List<GameObject> PotentialTargets = new List<GameObject> { };

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (ID > 0)
        {
            if (Other.tag.Equals("enemy"))
            {
                PotentialTargets.Add(Other.gameObject);
                if (!Attacking)
                {
                    Attacking = true;
                    Target = Other.gameObject;
                }
            }
        }
        else
        {
            if(Other.tag.Equals("ship"))
            {
                PotentialTargets.Add(Other.gameObject);
                if (!Attacking)
                {
                    Attacking = true;
                    Target = Other.gameObject;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (ID > 0)
        {
            if (Other.tag.Equals("enemy"))
            {
                PotentialTargets.Remove(Other.gameObject);
            }
        }
        else
        {
            if (Other.tag.Equals("ship"))
            {
                PotentialTargets.Remove(Other.gameObject);
            }
        }
        if (Other.gameObject==Target)
        {
            Target = null;
            SelectTarget();
        }
    }

    void SelectTarget()
    {
        PotentialTargets.Remove(null);
        if(PotentialTargets.Count>0)
        {
            Target = PotentialTargets[0];
        }
        else
        {
            Target = null;
            Attacking = false;
        }
    }

    public void RotateTowards(Vector2 V)
    {
        if (V.x >= 0)
            transform.eulerAngles = new Vector3(0, 0, -1 * Vector2.Angle(Vector2.up, V));
        else
            transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.up, V));
    }

    void Fire()
    {
        GameObject go = Instantiate(Projectile, transform.position, transform.rotation) as GameObject;
        if (ID != 3)
        {
            ProjectileScript ps = (ProjectileScript)go.GetComponent(typeof(ProjectileScript));
            ps.Target = Target;
        }
        else
        {
            InterceptorScript IS = (InterceptorScript)go.GetComponent(typeof(InterceptorScript));
            IS.Home = gameObject;
            IS.Target = Target;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Attacking && Target != null)
        {
            if (isTurret)
                RotateTowards(Target.transform.position - transform.position);
            counter += Time.deltaTime;
            if (counter >= FireDelay)
            {
                counter = 0;
                Fire();
            }
        }
        if (Target == null && PotentialTargets.Count>0)
        {
            SelectTarget();
        }
    }
}
