using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {
   public List<SkillScript> Skills;
    public int ActiveSkillIndex;
    public bool isActivePlayer;
    public Vector2 Destination;
    public bool isMoving;
    public float speed;
    public bool KnockedOut;

    // Use this for initialization
    void Start () {
	}

    public void UseSkill(Vector2 V)
    {
        Skills[ActiveSkillIndex].TryActivation(V);
    }

	// Update is called once per frame
	void Update () {
	if(isMoving)
        {
            float distMag = (Destination - (Vector2)transform.position).sqrMagnitude;
            transform.Translate(speed * Time.deltaTime * (Destination - (Vector2)transform.position).normalized);
            if((Destination - (Vector2)transform.position).sqrMagnitude>distMag)
            {
                transform.position = Destination;
                isMoving = false;
            }
        }
	}
}
