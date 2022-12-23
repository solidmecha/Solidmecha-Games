using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public string Name;
    public string description;
    public GameObject Effect;
    public GameObject Target;
    public bool RequiresTarget;
    public float Cooldown;
    public float CooldownCounter;
    public bool OnCooldown;
    public int ManaCost;
    public float MaxRange;

    public virtual void TryActivation(Vector2 V)
    {
        if (!OnCooldown && GetComponent<StatScript>().MP[0] >= ManaCost && (V-(Vector2)transform.position).sqrMagnitude<=MaxRange)
        {
            Activate(V);
        }
    }

    public virtual void Activate(Vector2 V)
    {
        ShowEffect(V);
        CooldownCounter = Cooldown;
        OnCooldown = true;
    }

    public void ShowEffect(Vector2 V)
    {
        Instantiate(Effect, V, Quaternion.identity);
    }

    public float GetAngle(Vector2 V)
    {
        V -= (Vector2)transform.position;
        float a= Vector2.Angle(Vector2.up, V);
        if (V.x > 0)
            a *= -1;
        return a;
        //if(V.x>=0 && )
    }

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    public virtual void Update () {
        if(OnCooldown)
        {
            CooldownCounter -= Time.deltaTime;
            if(CooldownCounter<=0)
            {
                CooldownCounter = 0;
                OnCooldown = false;
            }
        }
	}
}
