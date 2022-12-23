using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BindControl : MonoBehaviour {

    public DrawScript DS;
    public Text TimerText;
    public float Counter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Counter -= Time.deltaTime;
        TimerText.text = Mathf.RoundToInt(Counter).ToString();
        if (Counter <= 0)
        {
            Counter = 60;
            DS.BindCount++;
            transform.GetChild(DS.BindCount - 1).GetComponent<SpriteRenderer>().enabled = true;
        }
	}
}
