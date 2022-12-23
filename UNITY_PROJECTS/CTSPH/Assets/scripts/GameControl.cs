using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public GameObject Enemy;
    public GameObject Player;
    public float Delay;
    float counter;

	// Use this for initialization
	void Start () {
	
	}

    Vector2 randV()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        Vector2 V = new Vector2(RNG.Next(-3,4), RNG.Next(-3,4));
        return V;
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;

        if(counter>=Delay)
        {
            //Instantiate(Enemy, (Vector2)Player.transform.position + randV(), Quaternion.identity);
            counter = 0;
        }
	
	}
}
