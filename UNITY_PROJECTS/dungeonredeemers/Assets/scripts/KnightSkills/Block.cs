using UnityEngine;
using System.Collections;

public class Block : SkillScript {

    bool Blocking;
    public int baseBlock;
    public float Duration;

    public override void Activate(Vector2 V)
    {
        Instantiate(Effect, transform.position, Quaternion.identity, transform);
        CooldownCounter = Cooldown;
        OnCooldown = true;
        Blocking = true;
        baseBlock = GetComponent<StatScript>().BlockChance;
        GetComponent<StatScript>().BlockChance = 100;
        Invoke("RemoveBlock", Duration);
    }


    void RemoveBlock()
    {
        Blocking = false;
        GetComponent<StatScript>().BlockChance = baseBlock;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
}
