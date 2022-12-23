using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
    System.Random RNG=new System.Random();
    public GameObject Disc;
    public GameObject Disc2;
    public GameObject Player;
    float SpawnTimer;
	// Use this for initialization
	void Start () {
	}

    void SpawnDisc()
    {
        GameObject go;
        if (RNG.Next(4)==2)
            go = Instantiate(Disc2, new Vector2(RNG.Next(-6, 7), RNG.Next(-4, 5)), Quaternion.identity) as GameObject;
        else
            go = Instantiate(Disc, new Vector2(RNG.Next(-6, 7), RNG.Next(-4, 5)), Quaternion.identity) as GameObject;
        if(RNG.Next(6)==4)
        {
            go.AddComponent<HazardScript>();
            go.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (RNG.Next(5) == 4)
        {
            go.transform.localScale *= 2;
            go.GetComponent<Rigidbody2D>().mass *= 2;
        }
        if (RNG.Next(5) == 4)
        {
            go.transform.localScale *= .5f;
            go.GetComponent<Rigidbody2D>().mass *= .5f;
        }
    }
    void SpawnPlayer()
    {
        GameObject go=Instantiate(Player, new Vector2(RNG.Next(-6, 7), RNG.Next(-4, 5)), Quaternion.identity) as GameObject;
        if (RNG.Next(5) == 4)
        {
            go.transform.localScale *= 2;
            go.GetComponent<Rigidbody2D>().mass *= 2f;
        }
        if (RNG.Next(5) == 4)
        {
            go.transform.localScale *= .5f;
            go.GetComponent<Rigidbody2D>().mass *= .5f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        SpawnTimer += Time.deltaTime;
        if(SpawnTimer>=2.7)
        {
            SpawnTimer = 0;
            if (RNG.Next(7) == 4 || GameObject.FindGameObjectWithTag("Player") ==null)
                SpawnPlayer(); 
            else
                SpawnDisc();
        }
	}
}
