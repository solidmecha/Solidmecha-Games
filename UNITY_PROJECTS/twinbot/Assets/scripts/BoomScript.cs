using UnityEngine;
using System.Collections;

public class BoomScript : MonoBehaviour {
    float lifeTime;
    public bool hostile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hostile)
        {
            if (!collision.GetComponent<BotScript>().inImmunity)
                collision.GetComponent<BotScript>().TakeDamage(1);
        }
        else if (collision.CompareTag("E"))
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage();
        }
    }
    // Use this for initialization
    void Start () {
        lifeTime = 5f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = (Vector2)transform.localScale + new Vector2(3, 3)*Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
	}
}
