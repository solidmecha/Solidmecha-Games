using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class DragonScript : MonoBehaviour {
    public int HP;
    public int DPS;
    public int elementID; //air, earth, fire, water
    public int StrongEleID;
    public int WeakEleID;
    public float StrongMultiplier;
    public float WeakMultiplier;
    public Action Ability;
    public int playerID;
    public int enemyID;
    public int AbilityID;
    public float[] PreVarArray=new float[5];
    public bool isBanned; 

    public GameControl GC;

    void OnMouseDown()
    {
        if (GC.inDraftMode && !isBanned)
        {
            if (GC.roundNumber >= 7)
            {
                transform.position = GC.PlayerPoints[GC.ActivePlayerID][GC.Dragons[GC.ActivePlayerID].Count];
                GC.Dragons[GC.ActivePlayerID].Add(this);
                isBanned = true;
            }
            else
            {
                isBanned = true;
                GameObject g=Instantiate(GC.BanSymbol, transform.position, Quaternion.identity)as GameObject;
                if(transform.localScale.x==1.6f)
                { g.transform.localScale = new Vector2(.625f, .625f); }
                g.transform.SetParent(transform);
            }
            GC.roundNumber++;
            GC.SetDraftState();
        }
    }

    void OnMouseEnter()
    {
        GC.CurrentStatWindow=Instantiate(GC.StatWindowPrefab, new Vector2(5, 0), Quaternion.identity) as GameObject;
        GC.CurrentStatWindow.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = HP.ToString() + " HP";
        GC.CurrentStatWindow.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = DPS.ToString() + " DPS";
        GC.CurrentStatWindow.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "x" + StrongMultiplier.ToString() + " Damage vs";
        GC.CurrentStatWindow.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = "x" + WeakMultiplier.ToString() + " Damage Taken vs";
        GC.CurrentStatWindow.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = GC.Sprites[StrongEleID];
        GC.CurrentStatWindow.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = GC.Sprites[WeakEleID];
    }

    void OnMouseExit()
    {
        Destroy(GC.CurrentStatWindow);
    }

    string AbilityDescription(int i)
    {
        switch(i)
        {
            case 0: return "Boost allies HP by weakness multiplier";
            case 1: return "Zero all weakness multipliers.";
            default: return "Ability Not Found!";
        }
    }
    //1
    void HPBoostByWeak()
    {
        if (!GC.isEoT)
        {
            for (int i = 0; i < 5; i++)
            {
                PreVarArray[i] = GC.Dragons[playerID][i].HP;
               float f = GC.Dragons[playerID][i].WeakMultiplier * GC.Dragons[playerID][i].HP;
                GC.Dragons[playerID][i].HP=(int)f;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GC.Dragons[playerID][i].HP = (int)PreVarArray[i];
            }
        }
    }
    //2
    void HPBoostByStrong()
    {
        if (!GC.isEoT)
        {
            for (int i = 0; i < 5; i++)
            {
                PreVarArray[i] = GC.Dragons[playerID][i].HP;
                float f = GC.Dragons[playerID][i].StrongMultiplier * GC.Dragons[playerID][i].HP;
                GC.Dragons[playerID][i].HP = (int)f;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GC.Dragons[playerID][i].HP = (int)PreVarArray[i];
            }
        }
    }
    //3
    void ZeroWeakness()
    {
        if (!GC.isEoT)
        {
            for (int i = 0; i < 5; i++)
            {
                PreVarArray[i] = GC.Dragons[playerID][i].WeakMultiplier;
                GC.Dragons[playerID][i].WeakMultiplier = 1;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                  GC.Dragons[playerID][i].WeakMultiplier= PreVarArray[i];
            }
        }
    }
    //4
    void ZeroEnemyStrength()
    {
        if (!GC.isEoT)
        {
            for (int i = 0; i < 5; i++)
            {
                PreVarArray[i] = GC.Dragons[enemyID][i].StrongMultiplier;
                GC.Dragons[enemyID][i].StrongMultiplier = 1;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GC.Dragons[enemyID][i].StrongMultiplier = PreVarArray[i];
            }
        }

    }
    //5
    void SwitchStrongWeak()
    {
        if (!GC.isEoT)
        {
            for (int i = 0; i < 5; i++)
            {
                PreVarArray[i] = GC.Dragons[playerID][i].StrongEleID;
                GC.Dragons[playerID][i].StrongEleID = GC.Dragons[playerID][i].WeakEleID;
                GC.Dragons[playerID][i].WeakEleID = (int)PreVarArray[i];
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GC.Dragons[playerID][i].WeakEleID = GC.Dragons[playerID][i].StrongEleID;
                GC.Dragons[playerID][i].StrongEleID = (int)PreVarArray[i];
            }
        }
    }
    //6
    void Balance()
    {
    }
    //7
    void RoundBoost()  
    {
        if(!GC.isEoT)
        {
            float f = DPS * (1 + .1f * GC.roundNumber);
            DPS=(int)f;
        }
    }
    //8
    void StrongVsAll()
    {
    }
    //9
    void CounterSpell()
    {

    }
    //10
    void Pacifist()
    {

    }
    //11
    void ReverseResult()
    {

    }
    //12
    void ReverseTime()
    {

    }
    //13
    void CopyCat()
    { }
    //14
    void TeamAverage()
    { }
    //15
    void DoublePointsOnSweep()
    { }
    //16
    void EleSynergy()
    { }
    //17
    void EleDiversity()
    { }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
