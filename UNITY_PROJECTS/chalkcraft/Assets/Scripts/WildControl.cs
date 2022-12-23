using UnityEngine;
using System.Collections.Generic;

public class WildControl : MonoBehaviour {

    public float delay;
    float counter;
    System.Random RNG;

	// Use this for initialization
	void Start () {
        RNG=new System.Random();
	}

    void UpdateWildlings()
    {
        if (!GetComponent<ChalklingControl>().IsAttacking())
        {
            if(GetComponent<ChalklingControl>().currentSpeed==0)
                GetComponent<CommandScript>().Go();
            if (RNG.Next(0, 2) < 1)
            {
                GetComponent<CommandScript>().turn();
            }
            else
            {
                GetComponent<CommandScript>().ccTurn();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if (counter >= delay)
        {
            UpdateWildlings();
            counter = 0;
            delay = RNG.Next(1, 5);
        }
	}
}
