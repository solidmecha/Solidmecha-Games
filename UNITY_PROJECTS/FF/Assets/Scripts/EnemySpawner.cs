using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    float counter;
    public float delay;
    public List<GameObject> EnemyShips=new List<GameObject> { };
    int shipCount;
   public GameManager GM;

	// Use this for initialization
	void Start () {
        GM = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
        shipCount = 1;
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter>=delay)
        {
            counter = 0;
            for (int i = 0; i < shipCount; i++)
                Instantiate(EnemyShips[i % 2], (Vector2)transform.position + new Vector2(i*16-32, 0), Quaternion.identity);
            if(shipCount<11)
                shipCount++;
            GM.ShowMessage("ENEMY WAVE INCOMING...");            
        }
	}
}
