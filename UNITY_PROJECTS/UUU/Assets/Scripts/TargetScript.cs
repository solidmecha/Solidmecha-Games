using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetScript : MonoBehaviour {
    public BehaviourScript BS;

    // Use this for initialization
    void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { TakeAction(); });
    }

    void TakeAction()
    {
        BattleScript.singleton.CurrentTargets.Add(BS);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
