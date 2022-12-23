using UnityEngine;
using System.Collections;

public class SkillControl : MonoBehaviour {

    public GameObject Hitbox;
    public GameObject HitboxPreview;
    public GameObject ActiveHit;
    public Vector2 Scale;
    public Vector2 Offset;
    public Quaternion rotation;
    public float Delay;
    public float StunLength;
    int Facing;

    public void Unleash(int f)
    {
        Facing = f;
        GetComponent<FighterScript>().ready = false;
        Invoke("unstun", StunLength);
        Invoke("StopHitbox", StunLength);
        ActiveHit = Instantiate(HitboxPreview, (Vector2)GetComponent<FighterScript>().transform.position + Offset * Facing, rotation) as GameObject;
        ActiveHit.transform.localScale = Scale;
        Invoke("ActivateHitbox", Delay);
    }

    public void ActivateHitbox()
    {
        StopHitbox();
        ActiveHit = Instantiate(Hitbox, (Vector2)GetComponent<FighterScript>().transform.position+Offset*Facing, rotation) as GameObject;
        ActiveHit.transform.localScale = Scale;
    }

    public void StopHitbox()
    {
        Destroy(ActiveHit);
    }

    public void unstun()
    {
        GetComponent<FighterScript>().ready = true;
    }


	// Use this for initialization
	void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
