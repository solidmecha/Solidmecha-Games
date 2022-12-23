using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HampdomButton : MonoBehaviour {

    public int ID;
    public int Index;
    public int[] Cost;
    public float delay;

    private void OnMouseDown()
    {
        switch(ID)
        {
            case 0:
                {
                   HampdomControl.singleton.LocalPlayer.Workers[Index]++;
                    HampdomControl.singleton.StartCoroutine(HampdomControl.singleton.ShowOptionsAfterDelay(delay));
                    Destroy(transform.root.gameObject);
                }
                break;
            case 1:
                {
                    if (HampdomControl.singleton.LocalPlayer.CanPay(Cost))
                    {
                        HampdomControl.singleton.LocalPlayer.Army[Index]++;
                        HampdomControl.singleton.StartCoroutine(HampdomControl.singleton.ShowOptionsAfterDelay(delay));
                        Destroy(transform.root.gameObject);
                    }
                }
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
