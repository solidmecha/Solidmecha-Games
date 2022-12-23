using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    public int dmg;
    public float speed;
    public GameObject Target;

	// Use this for initialization
	void Start () {
	}

    public void RotateTowards(Vector2 V)
    {
        if (V.x >= 0)
            transform.eulerAngles = new Vector3(0, 0, -1 * Vector2.Angle(Vector2.up, V));
        else
            transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.up, V));
    }

    // Update is called once per frame
    void Update () {
        if (Target != null)
        {
            RotateTowards(Target.transform.position - transform.position);
        transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Target.transform.position)<.09f)
            {
                HealthScript us = (HealthScript)Target.gameObject.GetComponent(typeof(HealthScript));
                us.dealDmg(dmg);
                Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);
	}
}
