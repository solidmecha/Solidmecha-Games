using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour
{

    public bool spins;
    public float spinSpeed;
    float counter;
    public float FireDelay;
    public bool Moves;
    // public Vector2[] path;
    // public int pathIndex;
    public Vector2 Dir;
    public float speed;
    bool TripleShot;
    bool inImmunity;
    float SRCounter;
    float iTimer;
    public GameObject Hexa;
    public GameObject HealthBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("B") && !inImmunity)
        {
                TakeDamage();
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage()
    {
        HealthBar.transform.localScale -= new Vector3(0, 1, 0);
        if (HealthBar.transform.localScale.y <= 6)
            TripleShot = true;
        if (HealthBar.transform.localScale.y <= 2)
            SetImmunity(3f);
        if (HealthBar.transform.localScale.y <= 0)
        {
            EnemyGen.singleton.PlacePickUp(transform.position);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + Vector2.right);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + -1 * Vector2.right);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + Vector2.up);
            EnemyGen.singleton.PlacePickUp((Vector2)transform.position + Vector2.down);
            EnemyGen.singleton.BossSpawned = false;
            EnemyGen.singleton.SpawnDelay = 9f;
            Destroy(HealthBar);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("W"))
        {
            Dir = Vector2.Reflect(Dir, col.collider.transform.up);
        }
    }

    public void SetImmunity(float time)
    {
        iTimer = time;
        StartCoroutine(BecomeImmune());
    }

    IEnumerator BecomeImmune()
    {
        inImmunity = true;
        while (iTimer > 0)
        {
            iTimer -= Time.deltaTime;

            yield return null;
        }
        inImmunity = false;
        GetComponent<SpriteRenderer>().enabled = true;
        SRCounter = 0f;
    }

    public void tripleShotOn()
    {
        TripleShot = true;
    }

    void Fire()
    {
        Vector2 offset = Vector2.zero;
        if (EnemyGen.singleton.RNG.Next(3) == 0)
            offset = new Vector2(EnemyGen.singleton.RNG.Next(-2, 3), EnemyGen.singleton.RNG.Next(-2, 3));
        GameObject g = Instantiate(Hexa, transform.position+transform.up, Quaternion.identity) as GameObject;
        g.GetComponent<EnemyScript>().Dir = ((Vector2)BotControl.singleton.Bots[EnemyGen.singleton.RNG.Next(2)].position - (Vector2)transform.position + offset).normalized;
        g.GetComponent<EnemyScript>().Moves = true;
        g.GetComponent<EnemyScript>().speed = speed;

    }

    // Use this for initialization
    void Start()
    {
        HealthBar = transform.GetChild(0).gameObject;
        HealthBar.transform.SetParent(null);
        FireDelay = 13;
        Dir = (EnemyGen.singleton.FindPlace() - (Vector2)transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (spins)
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        counter += Time.deltaTime;
        if (counter >= FireDelay)
        {
            counter = 0;
            Fire();
            if (TripleShot)
            {
                Invoke("Fire", 1f);
                Invoke("Fire", 2f);
            }
        }
        if (Moves)
        {
            transform.position = (Vector2)transform.position + Dir * speed * Time.deltaTime;
        }
        if (inImmunity)
        {
            SRCounter -= Time.deltaTime;
            if (SRCounter <= 0)
            {
                SRCounter = .3f;
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            }
        }
    }
}
