using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {

    public float counter;
    UnityEngine.UI.Text msg;

	// Use this for initialization
	void Start () {
        msg = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        msg.text = ((int)counter).ToString();
	}
}
