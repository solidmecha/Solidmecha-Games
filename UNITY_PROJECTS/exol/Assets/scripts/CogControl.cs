using UnityEngine;
using System.Collections;

public class CogControl : MonoBehaviour {

    public Vector2 TargetPosition;
    public float RotationDelta;
    public bool Moving;
    public bool Rotating;
    public bool Copied;


    public void Activate()
    {
        ArmControl[] Arms = GetComponentsInChildren<ArmControl>();
        foreach(ArmControl a in Arms)
        {
            a.UseArm();
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
