using UnityEngine;
using System;

public class SkillScript {

    public string Name;
    public string Description;
    public float DmgMod;
    public int StatusID;
    public Action Action;
    public int TargetCount;
    public bool Ranged;
    public bool Support;
    public int ManaCost;
    public const int SkillCount = 20;

    public SkillScript(string n, string desc, int mana, float dmgMod, int stat, Action A, int targetCount, bool ranged, bool support)
    {
        Name = n;
        Description = desc;
        DmgMod = dmgMod;
        StatusID = stat;
        Action = A;
        TargetCount = targetCount;
        Ranged = ranged;
        Support = support;
        ManaCost = mana;
    }

    public SkillScript() { }

    public SkillScript SkillLookUp(int ID, BehaviourScript B)
    {
        switch(ID)
        {  //string n, string desc, int dmgMod, int stat, Action A, int targetCount, bool ranged, bool support
            case 0:
                return new SkillScript("Atk", "Attack front row",0, 1, 0, B.BasicAttack, 0, false, false);
            case 1:
                return new SkillScript(TypeByID(B.TypeID), "Elemental attack",2, 1, 0, B.BasicTypeAttack, 0, false, false);
            case 2:
                return new SkillScript(3+ TypeByID(B.TypeID), "Ele Attack Front Target and two adjacent lanes for 60% Base Atk.", 6, .6f, 0, B.BasicTypeAttack, 3, false, false);
            case 3:
                return new SkillScript(5+ TypeByID(B.TypeID), "Ele Attack Full front row.", 9, .5f,0, B.BasicTypeAttack, 5, false, false);
            case 4:
                return new SkillScript("Far"+ TypeByID(B.TypeID), "Ele Attack Back Row", 2, 1, 0, B.BasicTypeAttack, 0, true, false);
            case 5:
                return new SkillScript("2" + TypeByID(B.TypeID), "Ele Attack Front and Back Row at 75%", 2, .75f, 0, B.BasicTypeAttack, 2, true, false);
            case 6:
                return new SkillScript("FarShot", "Basic attack back row", 0, 1, 0, B.BasicAttack, 0, true, false);
            case 7:
                return new SkillScript("True Atk", "True damage the front row", 3, 1, 0, B.DealTrueDamage, 0, false, false);
            case 8:
                return new SkillScript("FarTrueDmg", "Attack Back Row ignoring Defense.", 3, 1, 0, B.DealTrueDamage, 0, true, false);
            case 9:
                return new SkillScript("HP+", "Improve Max HP by 5 levels using support's growth rate if it's higher.", 4, 5, 0, B.RaiseStat, 0, false, true);
            case 10:
                return new SkillScript("MP+", "Improve Max MP by 5 levels using support's growth rate if it's higher.", 4, 5, 1, B.RaiseStat, 0, false, true);
            case 11:
                return new SkillScript("ATK+", "Improve Atk by 5 levels using support's growth rate if it's higher.", 4, 5, 2, B.RaiseStat, 0, false, true);
            case 12:
                return new SkillScript("DEF+", "Improve Def by 5 levels using support's growth rate if it's higher.", 4, 5, 3, B.RaiseStat, 0, false, true);
            case 13:
                return new SkillScript("RCV+", "Improve Recovery by 5 levels using support's growth rate if it's higher.", 4, 5, 4, B.RaiseStat, 0, false, true);
            case 14:
                return new SkillScript("MPR+", "Improve MP Regen by 5 levels using support's growth rate if it's higher.", 4, 5, 5, B.RaiseStat, 0, false, true);
            case 15:
                return new SkillScript("SPD+", "Improve Speed by 5 levels using support's growth rate if it's higher.", 4, 5, 6, B.RaiseStat, 0, false, true);
            case 16:
                return new SkillScript("Heal", "Heal your front row a bit.", 3, 1, 0, B.Heal, 0, false, true);
            case 17:
                return new SkillScript("EleHeal", "Heal with an ele modifier (200% if targeting ele weak type, 50% vs strong target)", 5, 1, 0, B.EleHeal, 0, true, false);
            case 18:
                return new SkillScript("Curse", "Set Front Row Target to 100 HP", 16, 1, 0, B.Curse, 0, false, false);
            case 19:
                return new SkillScript("FarCurse", "Set Back Row Target to 100 HP", 16, 1, 0, B.Curse, 0, true, false);
            default:
                return new SkillScript("Pass", "Randomly +1 a stat", 0, 1, 0, B.PassTurn, 0, false, false);
        }
    }

    public static string TypeByID(int T)
    {
        switch (T)
        {
            case 0: return "Fire";
            case 1: return "Water";
            default: return "Plant";
        }
    }

    public void TurnAoe(int targetCount)
    {
        if (targetCount != 0)
        {
            TargetCount = targetCount;
            Name.Insert(0, targetCount.ToString());
        }
    }

    public void TurnElemental()
    {

    }

   
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
