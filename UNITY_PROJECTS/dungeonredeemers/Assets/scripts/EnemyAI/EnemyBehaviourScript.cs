using UnityEngine;
using System.Collections.Generic;

public class EnemyBehaviourScript : MonoBehaviour {
    PlayerScript Target;
    public float offset;
    public Vector2 Destination;
    public bool isMoving;
    public float speed;
    public List<SkillScript> Skills;
    public int ActiveSkillIndex;
    public float SkillDelay;
    public bool isChasing;

    public virtual void SelectTarget()
    {
        Target = PartyControl.singleton.Party[PartyControl.singleton.RNG.Next(PartyControl.singleton.Party.Length)];
    }

    public virtual void MoveToTarget()
    {
        isMoving = true;
        Destination = ((Vector2)Target.transform.position - (Vector2)transform.position);
        float Dist = Destination.magnitude;
        if (Dist > offset)
            Dist -= offset;
        Destination = (Vector2)transform.position+Destination.normalized * Dist;
    }

    public void UseSkill()
    {

        if (!isMoving)
        {
            if (((Vector2)Target.transform.position - (Vector2)transform.position).sqrMagnitude - offset * offset > .1f)
            {
                MoveToTarget();
                return;
            }
            Skills[ActiveSkillIndex].TryActivation(Target.transform.position);
            if (Target.KnockedOut)
            {
                SelectTarget();
                MoveToTarget();
            }
        }
    }

	// Use this for initialization
	void Start () {
        SelectTarget();
        MoveToTarget();
        InvokeRepeating("UseSkill", SkillDelay, SkillDelay);
	}
	
	// Update is called once per frame
	void Update () {
	if(isMoving)
        {
            float distMag = (Destination - (Vector2)transform.position).sqrMagnitude;
            transform.Translate(speed * Time.deltaTime * (Destination - (Vector2)transform.position).normalized);
            if ((Destination - (Vector2)transform.position).sqrMagnitude > distMag)
            {
                transform.position = Destination;
                isMoving = false;
            }
        }
    else if(isChasing)
        {
            if (((Vector2)Target.transform.position - (Vector2)transform.position).sqrMagnitude - offset * offset > .1f)
            {
                MoveToTarget();
                return;
            }
        }
	}
}
