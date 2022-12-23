using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HampdomControl : MonoBehaviour {
    public static HampdomControl singleton;
    public HampdomPlayer LocalPlayer;
    public GameObject Options;

    private void Awake()
    {
        singleton = this;
    }
    // Use this for initialization
    void Start () {
		
	}

    public IEnumerator ShowOptionsAfterDelay (float T)
    {
        yield return new WaitForSeconds(T);
        Instantiate(Options);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
