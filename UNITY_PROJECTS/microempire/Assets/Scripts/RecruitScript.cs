using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecruitScript : MonoBehaviour {

    public Text UnitText;
    public int[] cost;
    public CityScript LocalCity;
    public int UnitCount;

    public void RecruitUnit()
    {
        if (LocalCity.HasEnoughResources(cost))
        {
            LocalCity.MakePayment(cost);
            UnitCount++;
            UpdateUnitText();
        }
    }


    void UpdateUnitText()
    {
        UnitText.text = UnitCount.ToString();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
