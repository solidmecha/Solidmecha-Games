using UnityEngine;
using System.Collections;

public class CrystalSpawner : MonoBehaviour {

    bool cleared;
    public bool hostile;
    public int ID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!cleared)
            Destroy(collision.gameObject);
    }

    // Use this for initialization
    void Start () {
        Invoke("Grow", .34f);
	}

    void Grow()
    {
        cleared = true;
        CrystalScript cs=(Instantiate(WorldControl.singleton.Crystal, transform.position, Quaternion.identity) as GameObject).GetComponent<CrystalScript>();
        if(hostile)
        {
            cs.hostile = true;
        }
        else
        {
            cs.MinimapIcon = Camera.main.transform.GetChild(2).GetChild(ID).gameObject;
        }
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
