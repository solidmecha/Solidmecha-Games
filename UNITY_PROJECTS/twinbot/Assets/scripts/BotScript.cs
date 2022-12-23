using UnityEngine;
using System.Collections;

public class BotScript : MonoBehaviour {

    public int ID;
    public int HP;
    public int Armor;
    float counter=2.5f;
    float SRCounter= 0f;
    public bool inImmunity;
    public bool TripleShot;

    public void TakeDamage(int Damage)
    {
        if (Armor == 0 || Damage<0)
        {
            if (HP == 0 && Damage < 0)
            {
                HP = 3;
                SetImmunity(5f);
            }
            else
            {
                HP -= Damage;
                if (HP <= 0)
                {
                    HP = 0;
                }
                else if (Damage > 0)
                {
                    StartCoroutine(BecomeImmune());
                }
            }
            ShowHearts();
        }
        else
        {
            int ArmorLoss = Armor - Damage;
            if (ArmorLoss < 0)
            {
                ChangeArmor(Armor);
                TakeDamage(Damage - Armor);
            }
            else
            {
                ChangeArmor(Damage);
                StartCoroutine(BecomeImmune());
            }
        }
    }

    public void ChangeArmor(int value)
    {
        Armor -= value;
        ShowShields();
    }

    public void tripleShotOn()
    {
        TripleShot = true;
        Invoke("tripleShotOff", 16.5f);
    }

    public void tripleShotOff()
    {
        TripleShot = false;
    }

    void ShowHearts()
    {
        for (int i = 0; i < 3; i++)
            transform.GetChild(i+1).GetComponent<SpriteRenderer>().enabled = HP>i;
    }

    void ShowShields()
    {
        for (int i = 0; i < 3; i++)
            transform.GetChild(i + 4).GetComponent<SpriteRenderer>().enabled = Armor>i;
    }


    public void SetImmunity(float time)
    {
        counter = time;
        StartCoroutine(BecomeImmune());
    }

    IEnumerator BecomeImmune()
    {
        inImmunity = true;
        while (counter>0)
        {
            counter -= Time.deltaTime;

            yield return null;
        }
        inImmunity = false;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        counter = 2.5f;
        SRCounter = 0f;
        
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(inImmunity)
        {
            SRCounter -= Time.deltaTime;
            if(SRCounter<=0)
            {
                SRCounter = .3f;
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = !transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled;
            }
        }
	}
}
