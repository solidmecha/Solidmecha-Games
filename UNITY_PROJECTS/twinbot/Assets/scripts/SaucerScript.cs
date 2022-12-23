using UnityEngine;
using System.Collections;

public class SaucerScript : MonoBehaviour {

    float extendCount=10;
  public  float extendSpeed;
    float spinCount=7;
   public float spinSpeed;
    public GameObject HealthBar;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("B"))
        {
            TakeDamage();
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player") && !col.collider.GetComponent<BotScript>().inImmunity)
        {
            col.collider.GetComponent<BotScript>().TakeDamage(1);
        }
    }


    // Use this for initialization
    void Start () {
        HealthBar = transform.GetChild(2).gameObject;
        HealthBar.transform.SetParent(null);
	}

    void TakeDamage()
    {
        HealthBar.transform.localScale -=new Vector3(0, 1, 0);
        if (HealthBar.transform.localScale.y <= 0)
        {
            Destroy(HealthBar);
            EnemyGen.singleton.PlacePickUp(transform.position);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + Vector2.right);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + -1 * Vector2.right);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + Vector2.up);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + Vector2.down);
            EnemyGen.singleton.BossSpawned = false;
            EnemyGen.singleton.SpawnDelay = 9f;
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {

        extendCount -= Time.deltaTime;
        if(extendCount>=0)
        {
            transform.GetChild(0).localScale = (Vector2)transform.GetChild(0).localScale + new Vector2(0, extendSpeed * Time.deltaTime);
            transform.GetChild(1).localScale = (Vector2)transform.GetChild(1).localScale + new Vector2(0, extendSpeed * Time.deltaTime);
        }
        else
        {
            extendCount = 5;
            extendSpeed *= -1;
        }

        spinCount -= Time.deltaTime;
        if(spinCount >= 0)
        {
            transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
        }
        else
        {
            spinCount = EnemyGen.singleton.RNG.Next(4,16);
            spinSpeed *= -1;
        }

	}
}
