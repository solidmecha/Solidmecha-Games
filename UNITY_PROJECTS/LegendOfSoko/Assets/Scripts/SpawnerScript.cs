using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public WaveControl wc;
    public GameObject[] Crates;
    public GameObject[] Minions;
    public GameObject tower;
    public float crateDelay;
    public float towerDelay;
    public float MinionDelay;
    public Transform[] SpawnPoints;
    float buffMulti;

	// Use this for initialization
	void Start () {
        wc = GetComponent<WaveControl>();
        StartCoroutine(SpawnTower());
        StartCoroutine(SpawnCrate());
        StartCoroutine(SpawnMinion());
        buffMulti = 1;
    }

    IEnumerator SpawnTower()
    {
        yield return new WaitForSeconds(towerDelay);
        Instantiate(tower, OpenPos(), Quaternion.identity);
        towerDelay += 2;
        StartCoroutine(SpawnTower());
    }

    IEnumerator SpawnCrate()
    {
        yield return new WaitForSeconds(crateDelay);
        Instantiate(Crates[wc.RNG.Next(3)], OpenPos(), Quaternion.identity);
        crateDelay = wc.RNG.Next(3, 13);
        StartCoroutine(SpawnCrate());
    }

    IEnumerator SpawnMinion()
    {
        yield return new WaitForSeconds(MinionDelay);
        GameObject Go=Instantiate(Minions[wc.RNG.Next(3)], SpawnPoints[wc.RNG.Next(4)].transform.position, Quaternion.identity)as GameObject;
        buffMulti += .01f;
        Go.GetComponent<MinionScript>().Buff(buffMulti);
        MinionDelay = wc.RNG.Next(1, 9);
        StartCoroutine(SpawnMinion());
    }

    Vector2 OpenPos()
    {
        bool placed=false;
        Vector2 v=new Vector2(99,99);
        while (!placed)
        {
            int x = wc.RNG.Next(-12, 14);
            int y = wc.RNG.Next(-8, 9);
            v = new Vector2(x, y);
            if (!Physics2D.Raycast(v, Vector2.zero))
            {
                placed = true;      
            }

        }
        return v;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
