using UnityEngine;
using System.Collections;

public class Sprint : SkillScript{

    bool Sprinting;
    public float baseSpeed;
    public float Duration;

    public override void Activate(Vector2 V)
    {
        Instantiate(Effect, transform.position, Quaternion.identity, transform);
        CooldownCounter = Cooldown;
        OnCooldown = true;
       Sprinting  = true;
        baseSpeed = GetComponent<PlayerScript>().speed;
        GetComponent<PlayerScript>().speed *= 3f;
        Invoke("RemoveSprint", Duration);
    }


    void RemoveSprint()
    {
        Sprinting = false;
        GetComponent<PlayerScript>().speed = baseSpeed;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
