using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour { 
	public int[] cost=new int[3];
	int number;
	public GameObject Econ;
	public GameObject Fact;
	EconBuildingScript econScript;
	FactoryScript factScript;
	// Use this for initialization
	void Start () {
		if(Fact)
			factScript = (FactoryScript)Fact.GetComponent(typeof(FactoryScript));
		if(Econ)
			econScript=(EconBuildingScript)Econ.GetComponent(typeof(EconBuildingScript));
		setUpButton();
	}

	void setUpButton()
	{
		switch (name) {
		case "VPButton":
				number=2;
				GetComponent<Button>().onClick.AddListener(delegate{changeIncome(number);});
				break;
		case "FuelButton":
				number=1;
				GetComponent<Button>().onClick.AddListener(delegate{changeIncome(number);});
				break;
		case "GoldButton":
				number=0;
				GetComponent<Button>().onClick.AddListener(delegate{changeIncome(number);});
				break;
		case "AtkDmg":
			number=0;
			GetComponent<Button>().onClick.AddListener(delegate{factUpgrade(number);});
			break;
		case "AtkFireRate":
			number=1;
			GetComponent<Button>().onClick.AddListener(delegate{factUpgrade(number);});
			break;
		case "Speed":
			number=2;
			GetComponent<Button>().onClick.AddListener(delegate{factUpgrade(number);});
			break;
		case "Armor":
			number=3;
			GetComponent<Button>().onClick.AddListener(delegate{factUpgrade(number);});
			break;
		case "AtkHp":
			number=4;
			GetComponent<Button>().onClick.AddListener(delegate{factUpgrade(number);});
			break;
		case "BuildTime":
			number=5;
			GetComponent<Button>().onClick.AddListener(delegate{factUpgrade(number);});
			break;
		case "TurretDmg":
			number=0;
			GetComponent<Button>().onClick.AddListener(delegate{turretUpgrade(number);});
			break;
		case "TurretFireRange":
			number=1;
			GetComponent<Button>().onClick.AddListener(delegate{turretUpgrade(number);});
			break;
		case "TurretFireRate":
			number=2;
			GetComponent<Button>().onClick.AddListener(delegate{turretUpgrade(number);});
			break;
		default:
			Debug.Log(name);
			break;
		}
	}
	bool canPay()
	{
		if (econScript.Gold >= cost [0] && econScript.Fuel >= cost [1]) 
			{return true;}
			else{return false;}
	}
	void changeIncome(int i)
	{
			econScript.changeIncome (i);
	}

	void turretUpgrade(int i)
	{
		if (canPay ()) {
			econScript.Gold-=cost[0];
			econScript.Fuel-=cost[1];
			Tower tower = (Tower)GameManager.SelectedObject.GetComponent (typeof(Tower));
			tower.upgrades (i);
		}
	}
	void factUpgrade(int i)
	{
		if (canPay ()) {
			econScript.Gold-=cost[0];
			econScript.Fuel-=cost[1];
			factScript.upgrades (i);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
