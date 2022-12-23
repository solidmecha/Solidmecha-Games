using UnityEngine;
using System.Collections;
using System;

public class WorldMenuButton : MonoBehaviour {

    public string ActionName;
	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Click(); });
	}

    void Click()
    {
        WorldControl.singleton.Invoke(ActionName, 0);
        Destroy(WorldControl.singleton.CurrentWorldMenu);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
