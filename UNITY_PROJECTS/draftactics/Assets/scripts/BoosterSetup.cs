using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoosterSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { GameControl.singleton.UpBoost(); });
        transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { GameControl.singleton.DownBoost(); });
        transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { GameControl.singleton.ConfirmAttack(); });
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { GameControl.singleton.CancelBoost(); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
