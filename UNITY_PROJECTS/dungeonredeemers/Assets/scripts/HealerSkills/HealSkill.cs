using UnityEngine;
using System.Collections;

public class HealSkill : SkillScript {

    public int HealValue;

    public override void Activate(Vector2 V)
    {
        base.Activate(V);
        Target.GetComponent<StatScript>().ChangeHP(HealValue);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
}
