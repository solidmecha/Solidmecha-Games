using UnityEngine;
using System.Collections;

public class StatScript : MonoBehaviour {

    public int[] HP;
    public int[] MP;
    public int HPRegen;
    public int MPRegen;
    public int[] Atk;
    public int[] Def;
    public int DodgeChance;
    public int BlockChance;
    float counter;
    public GameObject[] Bars;

    public void RollBlockAndDodge(int DamageAmount, int DamageIndex)
    {
        if (PartyControl.singleton.RNG.Next(100) < DodgeChance)
        {
            PartyControl.singleton.ShowMessage("Dodged!", transform.position, .5f);
            return;
        }
        if (PartyControl.singleton.RNG.Next(100) < BlockChance)
        {
            PartyControl.singleton.ShowMessage("Blocked!", transform.position, .5f);
            return;
        }
        TakeDamage(DamageAmount, DamageIndex);
    }

    public void TakeDamage(int Amount, int DamageIndex)
    {
        Amount = -1 * Mathf.RoundToInt(Amount * (1 - Def[DamageIndex] / 100f));
        ChangeHP(Amount);
    }

    public void ChangeHP(int value)
    {
        HP[0] += value;
        if (HP[0] <= 0)
        {
            HP[0] = 0;
            KnockedOut();
        }
        if (HP[0] >= HP[1])
        {
            HP[0] = HP[1];
        }
        Bars[0].transform.localScale = new Vector2((float)HP[0] / (float)HP[1], .85f);
    }

    public void ChangeMP(int value)
    {
        MP[0] += value;
        if (MP[0] <= 0)
        {
            MP[0] = 0;
        }
        if (MP[0] >= MP[1])
        {
            MP[0] = MP[1];
        }
        Bars[1].transform.localScale = new Vector2((float)MP[0] / (float)MP[1], 1);
    }

    public void KnockedOut()
    {
        if (CompareTag("Player"))
            GetComponent<PlayerScript>().KnockedOut = true;
        else if (CompareTag("Monster"))
            Destroy(gameObject);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if(counter<=0)
        {
            counter = 1;
            ChangeHP(HPRegen);
            ChangeMP(MPRegen);
        }
	}
}
