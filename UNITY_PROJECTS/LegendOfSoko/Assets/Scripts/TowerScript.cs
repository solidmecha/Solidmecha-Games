using UnityEngine;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour {

    public List<Transform> targetList;
    public Transform CurrentTarget;
    public float fireRate;
    float counter;
    public GameObject Projectile;
    int TargetID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("M"))   
            targetList.Add(other.transform);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("M"))
        {
            targetList.Remove(other.transform);
            if (TargetID==other.GetComponent<MinionScript>().ID)
                CurrentTarget = null;
        }
    }

    void SelectTarget()
    {
            float dist = 99;
            for (int i=0; i<targetList.Count; i++)
            {
            if (targetList[i] == null)
            {
                targetList.RemoveAt(i);
                i--;
            }
            else if (dist > Vector2.Distance(transform.position, targetList[i].position))
            {
                CurrentTarget = targetList[i];
                TargetID=CurrentTarget.GetComponent<MinionScript>().ID;
            }
            }
    }

    void Fire()
    {
        GameObject go=Instantiate(Projectile, transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<ProjectileScript>().target = CurrentTarget;
    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (CurrentTarget != null)
        {
            if((CurrentTarget.position - transform.position).x<0)
                transform.parent.GetChild(1).localEulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.up, CurrentTarget.position - transform.position));
            else
                transform.parent.GetChild(1).localEulerAngles = new Vector3(0, 0, 180+Vector2.Angle(Vector2.down, CurrentTarget.position - transform.position));
            counter += Time.deltaTime;
            if (counter >= fireRate)
            {
                Fire();
                counter = 0;
            }
        }
        else if(targetList.Count>0)
            SelectTarget();
	}
}
