using UnityEngine;
using System;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour {
    public int ID;

	// Use this for initialization
	void Start () {

        GetComponent<Slider>().onValueChanged.AddListener(delegate { GameObject.FindGameObjectWithTag("GameController").GetComponent<ProbLifeControl>().HandleSliderChange((int)GetComponent<Slider>().value, ID); });
        if (ID >= 10)
        {
            GetComponent<Slider>().onValueChanged.AddListener(delegate { SetValueText(); });
        }
    }



    public void SetValueText()
    {
        transform.GetChild(transform.childCount - 1).GetComponent<Text>().text = GetComponent<Slider>().value.ToString();
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
