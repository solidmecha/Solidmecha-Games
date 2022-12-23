using UnityEngine;
using System.Collections;

public class SkillGenerator : MonoBehaviour {

    public static SkillGenerator singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void GenericSkills(SkillScript S)
    {
        switch(S.ID)
        {
            case 0: //start
                S.Skill_Name = "Attack";
                S.Damage[0] = GameControl.singleton.RNG.Next(100, 301);
                if(S.Damage[0]>150)
                    S.Damage[1] = S.Damage[0] + GameControl.singleton.RNG.Next(25, 51);
                else
                    S.Damage[1] = S.Damage[0] + GameControl.singleton.RNG.Next(75, 201);
                S.Range = GameControl.singleton.RNG.Next(2, 5);
                S.Skill_AoE = (GameControl.AoE_Type)GameControl.singleton.RNG.Next(4);
                S.KnockBack = GameControl.singleton.RNG.Next(-1, 2);
                if (S.KnockBack == 1)
                    S.Skill_Name = "Push Attack";
                else if(S.KnockBack == -1)
                    S.Skill_Name = "Pull Attack";
                break;
            case 1:
                S.Skill_Name = "Radiance";
                S.Damage[0] = GameControl.singleton.RNG.Next(25, 176);
                S.Damage[1] = S.Damage[0] + GameControl.singleton.RNG.Next(30, 76);
                S.Range = GameControl.singleton.RNG.Next(2, 5);
                S.Skill_AoE = (GameControl.AoE_Type)GameControl.singleton.RNG.Next(4);
                break;
        }
    }

    void ArcherSkills(SkillScript S)
    {
        S.Skill_Name = "Attack";
        S.Damage[0] = GameControl.singleton.RNG.Next(50, 250);
        S.Damage[1] = S.Damage[0]+ GameControl.singleton.RNG.Next(25, 51);
        S.Range = GameControl.singleton.RNG.Next(2, 5);
    }

    void ApothecarySkills(SkillScript S)
    {

    }

    void WizardSkills(SkillScript S)
    {

    }

    void MageSkills(SkillScript S)
    {

    }

    void PugulistSkills(SkillScript S)
    {

    }

    void SwordsmanSkills(SkillScript S)
    {

    }

    void GuardianSkills(SkillScript S)
    {

    }

    void HealerSkills(SkillScript S)
    {

    }

    void RogueSkills(SkillScript S)
    {

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
