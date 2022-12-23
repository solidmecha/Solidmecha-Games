using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    public static GameStateManager singleton;
    public int CrystalsCaptured;
    public int EnemyCrystalsDestroyed;
    bool startEnemy;

    private void Awake()
    {
        singleton = this;
    }


    public void UpdatePlayerCrystals(int change)
    {
        CrystalsCaptured+=change;
        if (change > 0)
        {
            WorldControl.singleton.ShowMessage("Crystal Attuned.", 2f);
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().HP[1] += 25;
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().TakeDmg(-25);
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().MP[1] += 25;
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().ChangeMana(-25);
        }
        else
        {
            WorldControl.singleton.ShowMessage("A Crystal was lost!", 2f);
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().HP[1]-=25;
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().TakeDmg(25);
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().MP[1] += 25;
            WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().ChangeMana(25);
        }
        if(!startEnemy && CrystalsCaptured==4)
        {
            startEnemy = true;
            WorldControl.singleton.GetComponent<WorldSpawnerScript>().SpawnEnemyCrystals();
        }
        else if(CrystalsCaptured==0)
        {
            CrystalsLost();
        }
    }

    public void UpdateOtherCrystals()
    {
        EnemyCrystalsDestroyed++;
        if (EnemyCrystalsDestroyed < 4)
        {
            string[] num = new string[3] { "Three", "Two", "One" };
            WorldControl.singleton.ShowMessage(num[EnemyCrystalsDestroyed - 1] + " Remaining.", 4 - EnemyCrystalsDestroyed);
        }
        else
        {
            WorldControl.singleton.ShowMessage("Victory!", 42f);
        }
    }

    void CrystalsLost()
    {
        WorldControl.singleton.ShowMessage("Too many Crystals lost. Game Over! Restarting in 3...2..1.", 3f);
        WorldControl.singleton.Invoke("Reset", 3f);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
