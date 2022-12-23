using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelScript : MonoBehaviour {

    public PlayerScript PS;

    void OnBecameVisible()
    {
        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = PS.Resources[2].ToString();
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = PS.Resources[3].ToString();
    }

    void OnMouseDown()
    {
        if (PS.GM.isPlaying)
        {
            PS.Resources[2]--;
            PS.GM.PlayerListObj.transform.position = new Vector3(10, 10, -11);
            PS.GM.HandView.transform.position = Vector3.zero;

            if (PS.Resources[2] == 0)
                PS.GM.Players.Remove(PS);
            PS.GM.UpdateUI();
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
