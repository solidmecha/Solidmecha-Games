using UnityEngine;
using System.Collections;

public class BlastScript : MonoBehaviour {

    public float lifetime = 7;
    int Damage = 1;
    public float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!collision.GetComponent<BotScript>().inImmunity)
                collision.GetComponent<BotScript>().TakeDamage(Damage);
            Destroy(gameObject);
        }
        else if(collision.CompareTag("B") || collision.CompareTag("W"))
            Destroy(gameObject);

    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
        transform.Translate(Vector2.up * Time.deltaTime*speed);
	}
}
