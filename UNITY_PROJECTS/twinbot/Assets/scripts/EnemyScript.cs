using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public bool spins;
    public float spinSpeed;
    public int id;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("B") && ((Vector2)collision.transform.position - (Vector2)transform.position).sqrMagnitude < 1 && !inImmunity)
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage()
    {
        if (id > 0)
        {
            GetComponent<SpriteRenderer>().sprite = EnemyGen.singleton.Efabs[id - 1];
            GetComponent<PolygonCollider2D>().points = EnemyGen.singleton.Colliders[id - 1];
            id--;
        }
        else
        {
            EnemyGen.singleton.PlacePickUp(transform.position);
            EnemyGen.singleton.ScoreUp();
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
        GameObject g = Instantiate(EnemyGen.singleton.blast, transform.position, Quaternion.identity) as GameObject;
        g.transform.up = (Vector2)BotControl.singleton.Bots[EnemyGen.singleton.RNG.Next(2)].position - (Vector2)transform.position + offset;
    }

    // Use this for initialization
    void Start () {
        FireDelay = EnemyGen.singleton.RNG.Next(1, 5);
	}
	
	// Update is called once per frame
	void Update () {
        if (spins)
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        counter += Time.deltaTime;
        if (counter >= FireDelay)
        {
            counter = 0;
            Fire();
            if(TripleShot)
            {
                Invoke("Fire", .2f);
                Invoke("Fire", .4f);
            }
        }
        if(Moves)
        {
            transform.position = (Vector2)transform.position + Dir * speed * Time.deltaTime;
            /*
            Vector2 dir = path[pathIndex]-(Vector2)transform.position;
            if (dir.sqrMagnitude > .1f)
            {
                float dist = (path[pathIndex] - (Vector2)transform.position).sqrMagnitude;
                transform.position = (Vector2)transform.position + dir.normalized * speed * Time.deltaTime;
            }
            else
                pathIndex = (pathIndex + 1) % path.Length;
                */
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
