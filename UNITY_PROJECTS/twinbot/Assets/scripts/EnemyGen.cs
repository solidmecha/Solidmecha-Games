using UnityEngine;
using System.Collections.Generic;

public class EnemyGen : MonoBehaviour {

    public static EnemyGen singleton;
    public System.Random RNG;
    public GameObject blast;
    public int eList;
    public Sprite[] Efabs;
    public Vector2[][] Colliders;
    public GameObject[] EnemyPrefabs;
    public GameObject Walls;
    public float counter;
    public float SpawnDelay;
    public GameObject Pickup;
    public Sprite[] Psprites;
    float pcounter = 11f;
    public int Score;
    public GameObject[] Bosses;
    public bool BossSpawned;
    public GameObject spinner;
    public GameObject BoomBlast;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        Colliders = new Vector2[4][]
        {
            new Vector2[3] {new Vector2(.5f, -.5f), new Vector2(0,.5f), new Vector2(-.5f, -.5f)},
            new Vector2[4] {new Vector2(.5f, 0f),new Vector2(0f, .5f), new Vector2(-0.5f, 0f), new Vector2(0f, -.5f)},
            new Vector2[5] {new Vector2(0.3130355f, -.5f), new Vector2(.5f, .125f), new Vector2(0,.5f), new Vector2(-.5f, .125f), new Vector2(-0.3130355f, -.5f) },
            new Vector2[6] {new Vector2(0,.5f), new Vector2(-.5f, .25f), new Vector2(-.5f, -.27f), new Vector2(0,-.51f), new Vector2(.5f, -.26f), new Vector2(.5f, .25f) }
        };
    }

    public void BuildWalls()
    {

    }

    public Vector2 FindPlace()
    {
        return new Vector2(RNG.Next(-11,12), RNG.Next(-6,7));
    }

    public void PlaceEnemy()
    {
        SpawnDelay++;
        int r = RNG.Next(EnemyPrefabs.Length);
        GameObject g = Instantiate(EnemyPrefabs[r], FindPlace(), Quaternion.identity) as GameObject;
        if(r<EnemyPrefabs.Length)
        {
            r = RNG.Next(Efabs.Length);
            g.GetComponent<SpriteRenderer>().sprite = Efabs[r];
            g.GetComponent<PolygonCollider2D>().points = Colliders[r];
            EnemyScript es = g.GetComponent<EnemyScript>();
            es.id = r;
            es.speed = RNG.Next(1, 5);
            es.Dir = (FindPlace() - (Vector2)es.transform.position).normalized;
        }
    }

    public void ScoreUp()
    {
        Score++;
        if(Score%7==0 && !BossSpawned)
        {
            BotControl.singleton.Bots[0].GetComponent<BotScript>().SetImmunity(3.5f);
            BotControl.singleton.Bots[1].GetComponent<BotScript>().SetImmunity(3.5f);
            if (RNG.Next(5) == 4)
            {
                Vector2 a = FindPlace();
                while (a.sqrMagnitude < 8.5f)
                    a = FindPlace();
                BotControl.singleton.Bots[0].position = a;
                a = FindPlace();
                while (a.sqrMagnitude < 8.5f)
                    a = FindPlace();
                BotControl.singleton.Bots[1].position = a;
            }
            Instantiate(Bosses[RNG.Next(2)], Vector2.zero, Quaternion.identity);
            BossSpawned = true;
            SpawnDelay = 18f;
        }
    }

    public void PlacePickUp(Vector2 Loc)
    {
        GameObject g = Instantiate(Pickup, Loc, Quaternion.identity) as GameObject;
        PickUpScript ps = g.GetComponent<PickUpScript>();
        int r = RNG.Next(Psprites.Length);
        ps.GetComponent<SpriteRenderer>().sprite = Psprites[r];
        ps.ID = r;
    }



    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        counter -= Time.deltaTime;
        pcounter -= Time.deltaTime;
        if(counter<=0)
        {
            counter = SpawnDelay;
            PlaceEnemy();
        }
        if(pcounter<0)
        {
            pcounter = 11f;
            PlacePickUp(FindPlace());
        }
	}
}
