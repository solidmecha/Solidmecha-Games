using UnityEngine;
using System.Collections.Generic;

public class BehaviourScript : MonoBehaviour {

    public List<SkillScript> Skills=new List<SkillScript> { };
    public int[] HP=new int[2];
    public int[] MP = new int[2];
    public int MPRegen;
    public int[] XP = new int[2];
    public int Atk;
    public int Def;
    public int Speed;
    public int Recovery;
    public List<int> StatusIDs;
    public GameObject Target;
    public int TypeID;
    public int WeakTypeID;
    public int[] GrowthPerLevel=new int[7]; //HP, MP, Atk, Def, RCV, MPR, Speed
    public bool PlayerControlled;
    public int LaneID;
    public bool isSupport;
    public int Lvl;
    public int Index;
    public int Cost;

    private void OnMouseEnter()
    {
        ShowStats();
    }

    private void OnMouseExit()
    {
        StopStats();
    }

    public void ShowStats()
    {
        if (WorldControl.singleton.LiveStatScreen != null)
            StopStats();
        GameObject go=Instantiate(WorldControl.singleton.StatScreen) as GameObject;
        go.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = name + " Lvl " + Lvl ;
        go.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = TypeString();
        go.transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.UI.Text>().text = "HP: " + HP[0] + "/" + HP[1] + "(+" + GrowthPerLevel[0] + ")";
        go.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Text>().text = "MP: " + MP[0] + "/" + MP[1] + "(+" + GrowthPerLevel[1] + ")";
        go.transform.GetChild(0).GetChild(4).GetComponent<UnityEngine.UI.Text>().text = "Atk: " + Atk + "(+" + GrowthPerLevel[2] + ")";
        go.transform.GetChild(0).GetChild(5).GetComponent<UnityEngine.UI.Text>().text = "Def: " + Def + "(+" + GrowthPerLevel[3] + ")";
        go.transform.GetChild(0).GetChild(6).GetComponent<UnityEngine.UI.Text>().text = "Rcv: " + Recovery + "(+" + GrowthPerLevel[4]*-1 + ")";
        go.transform.GetChild(0).GetChild(7).GetComponent<UnityEngine.UI.Text>().text = "MPR: " + MPRegen + "(+" + GrowthPerLevel[5] + ")";
        go.transform.GetChild(0).GetChild(8).GetComponent<UnityEngine.UI.Text>().text = "Spd: " + Speed + "(+" + GrowthPerLevel[6] + ")";
        go.transform.GetChild(0).GetChild(9).GetComponent<UnityEngine.UI.Text>().text = Skills[0].Name;
        go.transform.GetChild(0).GetChild(10).GetComponent<UnityEngine.UI.Text>().text = Skills[1].Name;
        go.transform.GetChild(0).GetChild(11).GetComponent<UnityEngine.UI.Text>().text = Skills[2].Name;
        go.transform.position = WorldControl.singleton.StatsLoc.position;
        WorldControl.singleton.LiveStatScreen = go;
    }

    public void StopStats()
    {
        Destroy(WorldControl.singleton.LiveStatScreen);
    }

    string TypeString()
    {
        switch(TypeID)
        {
            case 0: return "Fire Type";
            case 1: return "Water Type";
            default: return "Plant Type";
        }
    }

    public void RegenMana(int Regen)
    {
        MP[0] = Mathf.Clamp(MP[0] + Regen,0, MP[1]);
        UpdateUI();
    }

    public void LvlUp(int L)
    {
        Lvl += L;
        for(int i=0;i<L;i++)
        {
            HP[0] += GrowthPerLevel[0];
            HP[1] += GrowthPerLevel[0];
            MP[0] += GrowthPerLevel[1];
            MP[1] += GrowthPerLevel[1];
            Atk += GrowthPerLevel[2];
            Def += GrowthPerLevel[3];
            Recovery -= GrowthPerLevel[4];
            MPRegen += GrowthPerLevel[5];
            Speed += GrowthPerLevel[6];
        }
        UpdateUI();
    }

    public void Damage(int d)
    {
        HP[0] = Mathf.Clamp(HP[0]-d, 0, HP[1]);
        if (HP[0] == 0)
        {
            BattleScript.singleton.ResolveKO(this);
            DestroyImmediate(gameObject);
        }
        else
            transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = HP[0] + "/" + HP[1];
    }

    public void DealTrueDamage()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
            B.Damage(Mathf.RoundToInt(Atk * Skills[BattleScript.singleton.ActiveSkillIndex].DmgMod));
    }

    public void BasicAttack()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
        {
            int D = Mathf.RoundToInt((float)Atk * (100-(float)B.Def) / 100f);
            B.Damage(Mathf.RoundToInt(D * Skills[BattleScript.singleton.ActiveSkillIndex].DmgMod));
        }
    }

    public void Heal()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
        {
            B.Damage(Mathf.RoundToInt(Recovery * Skills[BattleScript.singleton.ActiveSkillIndex].DmgMod));
        }
    }

    public void EleHeal()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
        {
            int D = Recovery;
            if (TypeID == B.WeakTypeID)
                D *= 2;
            else if (WeakTypeID == B.TypeID)
                D /= 2;
            B.Damage(Mathf.RoundToInt(D * Skills[BattleScript.singleton.ActiveSkillIndex].DmgMod));
        }
    }


    public void BasicTypeAttack()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
        {
            int D = Mathf.RoundToInt((float)Atk * (100f - (float)B.Def) / 100f);
            if (TypeID == B.WeakTypeID)
                D *= 2;
            else if (WeakTypeID == B.TypeID)
                D /= 2;
            B.Damage(Mathf.RoundToInt(D*Skills[BattleScript.singleton.ActiveSkillIndex].DmgMod));
        }
    }

    public void Curse()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
        {
            B.HP[0] = 100;
            B.HP[1] = 100;
            B.UpdateUI();
        }
    }

   public void PayMana(int cost)
    {
        MP[0] -= cost;
        UpdateUI();
    }

    public void RaiseStat()
    {
        foreach (BehaviourScript B in BattleScript.singleton.CurrentTargets)
        {
            int val = GrowthPerLevel[Skills[BattleScript.singleton.ActiveSkillIndex].StatusID];
            if (B.GrowthPerLevel[Skills[BattleScript.singleton.ActiveSkillIndex].StatusID] > val)
                val = B.GrowthPerLevel[Skills[BattleScript.singleton.ActiveSkillIndex].StatusID];
            B.GrowStat(Skills[BattleScript.singleton.ActiveSkillIndex].StatusID, 5*val);
            B.UpdateUI();
        }
    }

   public void GrowStat(int I, int Val)
    {
        switch(I)
        {
            case 0:
                HP[0] += Val;
                HP[1] += Val;
                break;
            case 1:
                MP[0] += Val;
                MP[1] += Val;
                break;
            case 2:
                Atk += Val;
                break;
            case 3:
                Def += Val;
                break;
            case 4:
                Recovery -= Val;
                break;
            case 5:
                MPRegen += Val;
                break;
            case 6:
                Speed += Val;
                break;
        }
    }

    public void PassTurn()
    {
        GrowStat(WorldControl.singleton.RNG.Next(7), 1);
    }

    public void SetStats(UnitStats U)
    {
        name = U.name;
    HP=U.HP;
    MP=U.MP;
     MPRegen=U.MPRegen;
     XP=U.XP;
    Atk=U.Atk;
    Def=U.Def;
    Speed=U.Speed;
    Recovery=U.Recovery;
    StatusIDs=U.StatusIDs;
    TypeID=U.TypeID;
    WeakTypeID=U.WeakTypeID;
    GrowthPerLevel = U.GrowthPerLevel; //HP, MP, Atk, Def, RCV, MPR, Speed
    PlayerControlled=U.PlayerControlled;
    LaneID=U.LaneID;
    isSupport=U.isSupport;
    Lvl=U.Lvl;
    Index=U.Index;
       foreach(SkillScript s in U.Skills)
        {
            Skills.Add(s);
        }
        UpdateUI();
    }

    // Use this for initialization
    void Start () {
    }

    public void SpawnStats()
    {
        name = WorldControl.singleton.RandomName();
        HP[0] = WorldControl.singleton.RNG.Next(300,600);
        HP[1] = HP[0];
        MP[0] = WorldControl.singleton.RNG.Next(15,51);
        MP[1] = MP[0];
        Atk = WorldControl.singleton.RNG.Next(50,260);
        Def = WorldControl.singleton.RNG.Next(50);
        Speed = WorldControl.singleton.RNG.Next(160);
        MPRegen = WorldControl.singleton.RNG.Next(3);
        Recovery = WorldControl.singleton.RNG.Next(-120, -2);
        for (int i = 0; i < GrowthPerLevel.Length; i++)
            GrowthPerLevel[i] = WorldControl.singleton.RNG.Next(5);
        TypeID = WorldControl.singleton.RNG.Next(3);
        SkillScript s = new SkillScript();
        int a = WorldControl.singleton.RNG.Next(10);
        int b = WorldControl.singleton.RNG.Next(SkillScript.SkillCount);
        int c = WorldControl.singleton.RNG.Next(SkillScript.SkillCount);
        while((a==b || a==c || b==c))
        {
           a = WorldControl.singleton.RNG.Next(SkillScript.SkillCount);
           b = WorldControl.singleton.RNG.Next(SkillScript.SkillCount);
           c = WorldControl.singleton.RNG.Next(SkillScript.SkillCount);
        }
        Skills.Add(s.SkillLookUp(a, this));
        Skills.Add(s.SkillLookUp(b, this));
        Skills.Add(s.SkillLookUp(c, this));
        Skills.Add(s.SkillLookUp(-1, this)); //Pass
        SetWeakTypeID();
        UpdateUI();
        Cost = WorldControl.singleton.RNG.Next(5, 45);
    }

    public void SetWeakTypeID()
    {
        switch(TypeID)
        {
            case 0:
                WeakTypeID = 1;
                break;
            case 1:
                WeakTypeID = 2;
                break;
            case 2:
                WeakTypeID = 0;
                break;
        }
    }
    public void ShowTypeID()
    {
        switch (TypeID)
        {
            case 0:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
        }
    }

    public void UpdateUI()
    {
        ShowTypeID();
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = name;
        transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = HP[0] + "/" + HP[1];
        transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.UI.Text>().text = MP[0] + "/" + MP[1];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
