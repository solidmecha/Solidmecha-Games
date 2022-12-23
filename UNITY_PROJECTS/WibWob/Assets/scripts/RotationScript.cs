using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {
    public float direction;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name=="Star")
        {
            PlayerControl pc = (PlayerControl)other.GetComponent(typeof(PlayerControl));
            pc.rotate = true;
            pc.TempPos = pc.pos;
            pc.pos = pc.FindPos(transform.position - pc.GC.transform.position);
            pc.RotDir = direction;           
            if(direction<0)
            {
                pc.RotationTarget = pc.transform.GetChild(0).localEulerAngles.z + 270;
            }
            else
                pc.RotationTarget = pc.transform.GetChild(0).localEulerAngles.z + 90;
            while (pc.RotationTarget > 360)
                pc.RotationTarget -= 360;
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
