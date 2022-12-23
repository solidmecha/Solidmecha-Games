using UnityEngine;
using System.Collections;

public class Book : MonoBehaviour {

    public GameControl GC;
    public NodeScript ProtectionZone;
    float counter = 15;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ProtectionZone.Immune != true)
            ProtectionZone.Immune = true;
        if (ProtectionZone.InfectRate > 0)
            ProtectionZone.InfectRate = 0;
        counter -= Time.deltaTime;
        if(counter<=0)
        {
            ProtectionZone.Immune = false;
            if(ProtectionZone.InfectRate==0)
                ProtectionZone.InfectRate = GC.InfectRate;
            Destroy(gameObject);
        }


    }
}
