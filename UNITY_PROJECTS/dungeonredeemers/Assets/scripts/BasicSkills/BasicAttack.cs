using UnityEngine;
using System.Collections;

public class BasicAttack : SkillScript {

    public int DamageIndex;

    public override void Activate(Vector2 V)
    {
        Quaternion Q = Quaternion.AngleAxis(GetAngle(V), Vector3.forward);
        Instantiate(Effect, transform.position, Q);
        if (Target != null)
            Target.GetComponent<StatScript>().RollBlockAndDodge(GetComponent<StatScript>().Atk[DamageIndex], DamageIndex);
        CooldownCounter = Cooldown;
        OnCooldown = true;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
}
