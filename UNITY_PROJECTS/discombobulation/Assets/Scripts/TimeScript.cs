using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScript : MonoBehaviour {
    public float counter;
    public Text TimeText;
	// Use this for initialization
	void Start () {
	
	}
	

	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            GetComponent<CardControl>().SwapCard();
        }
        else
            TimeText.text = Mathf.RoundToInt(counter).ToString();
	}
}
