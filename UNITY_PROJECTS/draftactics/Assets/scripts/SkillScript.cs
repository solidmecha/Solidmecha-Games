using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public string Skill_Name;
    public string Description;
    public int ID;
    public int FX_ID;
    public int[] Damage;
    public int Range;
    public int Cooldown;
    public int Cost;
    public GameControl.AoE_Type Skill_AoE;
    public int KnockBack;

    public int DamageRoll()
    {
        return GameControl.singleton.RNG.Next(Damage[0], Damage[1] + 1);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
