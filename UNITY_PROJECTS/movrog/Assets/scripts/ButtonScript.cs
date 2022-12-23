using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public int ID;

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Select(); });
    }

    public void Select()
    {
        GameControl.singleton.SelectUnit(ID);   
    }

    // Update is called once per frame
    void Update () {
	
	}
}
