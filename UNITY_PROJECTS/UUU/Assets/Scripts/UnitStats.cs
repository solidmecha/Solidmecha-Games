using UnityEngine;
using System.Collections.Generic;

public class UnitStats {

    public string name;
    public List<SkillScript> Skills = new List<SkillScript> { };
    public int[] HP;
    public int[] MP;
    public int MPRegen;
    public int[] XP;
    public int Atk;
    public int Def;
    public int Speed;
    public int Recovery;
    public List<int> StatusIDs;
    public int TypeID;
    public int WeakTypeID;
    public int[] GrowthPerLevel = new int[7]; //HP, MP, Atk, Def, RCV, MPR, Speed
    public bool PlayerControlled;
    public int LaneID;
    public bool isSupport;
    public int Lvl;
    public int Index;

    public void LevelUp()
    {
        //LEVEL UP
        HP[0] += GrowthPerLevel[0];
        HP[1] += GrowthPerLevel[0];
        MP[0] += GrowthPerLevel[1];
        MP[1] += GrowthPerLevel[1];
        Atk += GrowthPerLevel[2];
        Def += GrowthPerLevel[3];
        Recovery -= GrowthPerLevel[4];
        MPRegen += GrowthPerLevel[5];
        Speed += GrowthPerLevel[6];
        Lvl++;
    }

    public UnitStats (BehaviourScript U)
    {
        name = U.name;
        HP = U.HP;
        MP = U.MP;
        MPRegen = U.MPRegen;
        XP = U.XP;
        Atk = U.Atk;
        Def = U.Def;
        Speed = U.Speed;
        Recovery = U.Recovery;
        StatusIDs = U.StatusIDs;
        TypeID = U.TypeID;
        WeakTypeID = U.WeakTypeID;
        GrowthPerLevel = U.GrowthPerLevel; //HP, MP, Atk, Def, RCV, MPR, Speed
        PlayerControlled = U.PlayerControlled;
        LaneID = U.LaneID;
        isSupport = U.isSupport;
        Lvl = U.Lvl;
        Index = U.Index;
        TypeID = U.TypeID;
        WeakTypeID = U.WeakTypeID;
        foreach (SkillScript s in U.Skills)
            Skills.Add(s);
    }
	
}
