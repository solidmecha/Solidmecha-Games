using UnityEngine;
using System.Collections;

public class WaveControl : MonoBehaviour {

    public static WaveControl singleton;
    public int EnemyCount;
    public GameObject[] Enemies;
    public GameObject WaveCanvas;

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
	
	}

    public void ShowStart()
    {
        ItemControl.singleton.Previews.Add(Instantiate(WaveCanvas) as GameObject);
    }

    public void StartWave()
    {
        EnemyCount = GameControl.singleton.CurrentLvl;
        if (EnemyCount > 12)
            EnemyCount = 12;
        for(int i=0; i<EnemyCount;i++)
        {
            Instantiate(Enemies[GameControl.singleton.RNG.Next(Enemies.Length)], transform.GetChild(i).position, Quaternion.identity);
        }
        GameControl.singleton.isPlayingLevel = true;
        ItemControl.singleton.CleanUp();
    }

    public void ReduceEnemyCount()
    {
        EnemyCount--;
        if(EnemyCount==0)
        {
            GameControl.singleton.CurrentLvl++;
            GameControl.singleton.isPlayingLevel = false;
            ItemControl.singleton.DropLoot();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
