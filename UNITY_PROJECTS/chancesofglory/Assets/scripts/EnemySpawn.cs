using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    public GameObject[] EnemyPrefabs;

	// Use this for initialization
	void Start () {
        EnemyGen();
	}

    public void EnemyGen()
    {
        for (int i = 0; i < 4; i++)
        {
            int r = GameControl.singleton.RNG.Next(EnemyPrefabs.Length);
            GameControl.singleton.EnemyParty.Add(Instantiate(EnemyPrefabs[r], transform.GetChild(i).position, Quaternion.identity) as GameObject);
            if (r >= 5)
                i++;
        }
        for(int i=0;i<GameControl.singleton.WinCount; i++)
        {
            int h = GameControl.singleton.RNG.Next(GameControl.singleton.EnemyParty.Count);
            GameControl.singleton.EnemyParty[h].GetComponent<EnemyScript>().BonusDamage[GameControl.singleton.RNG.Next(6)]++;
            GameControl.singleton.EnemyParty[h].GetComponent<StatScript>().HP[1] += 10;
            GameControl.singleton.EnemyParty[h].GetComponent<StatScript>().UpdateHP(-5);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
