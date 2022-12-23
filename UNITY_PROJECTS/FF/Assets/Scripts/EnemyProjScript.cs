using UnityEngine;
using System.Collections;

public class EnemyProjScript : MonoBehaviour {

    public float force;
    public int Dmg;
    public float lifeTime;
    public int Mode;
    public GameObject Target;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.CompareTag("ship") || Other.gameObject.CompareTag("buildings"))
        {
            HealthScript hs = (HealthScript)Other.gameObject.GetComponent(typeof(HealthScript));
            hs.dealDmg(Dmg);
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start() {
        switch (Mode)
        {
            case 1:
                GetComponent<Rigidbody2D>().AddForce(force * Target.transform.position-transform.position);
                break;
            default: GetComponent<Rigidbody2D>().AddForce(force * new Vector2(Mathf.Cos((90+transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90+transform.eulerAngles.z) * Mathf.Deg2Rad)));
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
	}
}
