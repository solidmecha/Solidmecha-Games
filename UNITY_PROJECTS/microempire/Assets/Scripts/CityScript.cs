using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CityScript : MonoBehaviour {

    public Text InfoText;
    public float[] Resources;
    public float[] ResourceGain;
    public bool inView;
    public bool[] BuildingsBuilt;

    private void OnMouseDown()
    {
        if (!inView)
        {
            GameObject Go = Instantiate(WorldControl.singleton.CityMenu, transform.position, Quaternion.identity) as GameObject;
            inView = true;
            InfoText = Go.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            PlaceBuildingCanvas();
            ShowResources();
        }
    }

    void PlaceBuildingCanvas()
    {
        GameObject Go = Instantiate(WorldControl.singleton.BuildingCanvas, InfoText.transform.parent.parent) as GameObject;
        Go.transform.localPosition = Vector2.zero;
        Go.transform.localScale *= 2;
        UpdateBuildingBuittons(Go);
    }

    void UpdateBuildingBuittons(GameObject Go)
    {
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            if (BuildingsBuilt[i])
            {
                Go.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = "View";
                int t = i;
                Go.transform.GetChild(i).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Go.transform.GetChild(t).GetComponent<BuildingScript>().ViewBuilding(); });
            }
            else
            {
                int t = i;
                Go.transform.GetChild(i).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Go.transform.GetChild(t).GetComponent<BuildingScript>().BuildBuilding(this); });
            }

        }
    }

    void ShowResources()
    {
        InfoText.text = "City Amount:\n" + Mathf.RoundToInt(Resources[0]).ToString() + " Food," +
            Mathf.RoundToInt(Resources[1]).ToString() + " Wood,\n" +
            Mathf.RoundToInt(Resources[2]).ToString() + " Stone," +
            Mathf.RoundToInt(Resources[3]).ToString() + " Iron,\n" +
            Mathf.RoundToInt(Resources[4]).ToString() + " Gold," +
            Mathf.RoundToInt(Resources[5]).ToString() + " Mythril";
    }

    public bool HasEnoughResources(int[] cost)
    {
        return Resources[0] >= cost[0] && Resources[1] >= cost[1] && Resources[2] >= cost[2] && Resources[3] >= cost[3] && Resources[4] >= cost[4] && Resources[5] >= cost[5];
    }

    public void MakePayment(int[] cost)
    {
        for (int i = 0; i < 6; i++)
            Resources[i] -= cost[i];
        if (inView)
            ShowResources();
    }

	// Use this for initialization
	void Start () {
	
	}

    public void AddBuilding()
    {

    }

    public void GenerateResources()
    {
        for (int i = 0; i < Resources.Length; i++)
            Resources[i] += ResourceGain[i];
        if (inView)
            ShowResources();
    }

	
	// Update is called once per frame
	void Update () {
	}
}
