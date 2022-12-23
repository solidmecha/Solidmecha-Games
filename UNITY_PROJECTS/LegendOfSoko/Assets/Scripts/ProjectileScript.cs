using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

   public Transform target;
    float speed = 5;
    public float damage=1;
    public int EleID;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("M"))
        {
            if (collision.GetComponent<MinionScript>().WeaknessID == EleID)
                damage = 5;
            else
                damage = 1;
            collision.GetComponent<MinionScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
            Destroy(gameObject);
        else
            transform.Translate((target.transform.position - transform.position) * speed * Time.deltaTime);
	}
}
