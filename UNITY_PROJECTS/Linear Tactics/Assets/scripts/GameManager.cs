using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject commandCenter;
	health ccHealth;
	public GameObject enemyCC;
	health eccHealth;
	public GameObject econBuilding;
	EconBuildingScript econScript;
	public GameObject hp;
	public GameObject eHp;
	public GameObject gold;
	public GameObject fuel;
	public GameObject vp;

	public static GameObject SelectedObject;

	public GameObject[] buttons = new GameObject[3];
	int bint;

	Text ehpT;
	Text hpT;
	Text goldT;
	Text fuelT;
	Text vpT;
	// Use this for initialization
	void Start () {
		bint = 0;
		ehpT = (Text)eHp.GetComponent<Text> ();
		hpT = (Text)hp.GetComponent<Text> ();
		goldT=(Text)gold.GetComponent<Text> ();
		fuelT=(Text)fuel.GetComponent<Text> ();
		vpT=(Text)vp.GetComponent<Text> ();
		eccHealth = (health)enemyCC.GetComponent (typeof(health));
		ccHealth = (health)commandCenter.GetComponent (typeof(health));
		econScript=(EconBuildingScript)econBuilding.GetComponent(typeof(EconBuildingScript));
	}

	public void buttonDisplay(int b)
	{	buttons [bint].transform.position = new Vector3 (0, 50, 1);
		buttons[b].transform.position=Vector3.zero;
		bint = b;
	}
	
	// Update is called once per frame
	void Update () {
		if (econScript.VP >= 150 || !enemyCC) 
		{
			Application.LoadLevel(1);
		}
		ehpT.text = "Enemy Base HP: " + eccHealth.hp.ToString ();
	hpT.text="Base HP: "+ ccHealth.hp.ToString();
		goldT.text = "Gold: " + econScript.Gold.ToString ();
		fuelT.text = "Fuel: " + econScript.Fuel.ToString ();
		vpT.text = "Victory Points: " + econScript.VP.ToString ();
	}
}
