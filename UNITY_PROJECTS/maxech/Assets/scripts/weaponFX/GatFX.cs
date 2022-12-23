using UnityEngine;
using System.Collections;

public class GatFX : MonoBehaviour {
    LineRenderer LR;

    // Use this for initialization
    void Start () {
        LR = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
            if(GameControl.singleton.RNG.Next(2)==1)
                LR.SetPosition(1, Vector3.forward*5);
            else
                LR.SetPosition(1, Vector3.zero);
	
	}
}
