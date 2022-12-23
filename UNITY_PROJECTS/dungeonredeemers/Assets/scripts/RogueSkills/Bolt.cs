using UnityEngine;
using System.Collections;

public class Bolt : SkillScript {
    public int DamageIndex;
    public int DamageMulti;
    public override void Activate(Vector2 V)
    {
        transform.position = V+Vector2.up;
        if(Target.CompareTag("Monster"))
            Target.GetComponent<StatScript>().RollBlockAndDodge(GetComponent<StatScript>().Atk[DamageIndex]*DamageMulti, DamageIndex);
        base.Activate(transform.position);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
}
