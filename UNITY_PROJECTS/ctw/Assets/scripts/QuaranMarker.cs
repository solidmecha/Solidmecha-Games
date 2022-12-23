using UnityEngine;
using System.Collections;

public class QuaranMarker : MonoBehaviour {

    public GameControl GC;
    public int Active;
    public NodeScript ProtectionZone;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (ProtectionZone.Immune != true)
            ProtectionZone.Immune = true;
        if (ProtectionZone.InfectRate > 0)
            ProtectionZone.InfectRate = 0;
        if (GC.ActivePlayerLocation.transform.GetSiblingIndex() != Active)
    {
            ProtectionZone.Immune = false;
            if(ProtectionZone.InfectRate == 0)
                ProtectionZone.InfectRate = GC.InfectRate;
            Destroy(gameObject);
    }


    }
}
