using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public static GameControl singleton;
    public System.Random RNG;
    public int SelectedUnitIndex;
    public List<UnitScript> Units;
    public GameObject PathPreview;
    public GameObject MessageCanvas;
    float PointTimer;
    public float VPTimer;
    public List<PointScript> Points;
    public List<CampScript> Camps;
    public GameObject UnitPrefab;
    public GameObject NPCUnitPrefab;
    public GameObject SelectedUnitOutline;
    public int[] VP;
    public GameObject[] VPText;
    public GameObject Canvas;
    public int Mana;
    public GameObject ManaText;

    private void Awake()
    {
        PointTimer = .3f;
        singleton = this;
        RNG = new System.Random();
        SetupTeams();
    }

    // Use this for initialization
    void Start () {
	
	}


    public void SelectUnit(int index)
    {
        if (Units[index].HP[0] > 0)
        {
            Canvas.transform.GetChild(SelectedUnitIndex).GetComponent<UnityEngine.UI.Image>().color = Color.white;
            SelectedUnitIndex = index;
            Canvas.transform.GetChild(SelectedUnitIndex).GetComponent<UnityEngine.UI.Image>().color = new Color(.5f,1,.5f);
        }

    }

    public void UpdateMana(int c)
    {
        Mana += c;
        ManaText.GetComponent<UnityEngine.UI.Text>().text = Mana.ToString();
    }

    void SetupTeams()
    {
        for(int i=0;i<10;i++)
        {
            Units.Add((Instantiate(UnitPrefab, new Vector2(-8 + 2f * i, -13f), Quaternion.identity) as GameObject).GetComponent<UnitScript>());
            Units[i].UnitIndex = i;
            GetComponent<NPCScript>().NPCunits.Add((Instantiate(NPCUnitPrefab, new Vector2(-8 + 2f * i, 12f), Quaternion.identity) as GameObject).GetComponent<UnitScript>());
        }
        SelectUnit(0);
    }
	
	// Update is called once per frame
	void Update () {
        SelectedUnitOutline.transform.position = Units[SelectedUnitIndex].transform.position;
        PointTimer -= Time.deltaTime;
        VPTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectUnit(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectUnit(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectUnit(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectUnit(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SelectUnit(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SelectUnit(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SelectUnit(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SelectUnit(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SelectUnit(8);
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            SelectUnit(9);
        if (PointTimer<=0)
        {
            PointTimer = .25f;
            foreach (PointScript p in Points)
                p.ResolveUnits();
        }
        if(VPTimer<=0)
        {
            if (VP[0] > 999)
            {
                VPText[0].GetComponent<UnityEngine.UI.Text>().text = "Victory!";
                return;
            }
            else if (VP[1] > 999)
            {
                VPText[1].GetComponent<UnityEngine.UI.Text>().text = "Victory!";
                return;
            }
            VPTimer = 1;
            foreach (PointScript p in Points)
            {
                if(!p.Neutral)
                {
                    if (p.PlayerControlled)
                        VP[0] += 1;
                    else
                        VP[1] += 1;
                }

            }
            VPText[0].GetComponent<UnityEngine.UI.Text>().text = VP[0].ToString();
            VPText[1].GetComponent<UnityEngine.UI.Text>().text = VP[1].ToString();
        }
	}
}
