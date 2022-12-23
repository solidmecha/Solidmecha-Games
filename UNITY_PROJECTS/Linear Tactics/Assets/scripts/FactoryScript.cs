using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class FactoryScript : MonoBehaviour {
	public GameObject unit;
	public GameObject buttons;
	public Text[] textArray = new Text[6];
	GameObject curUnit;
	public float delay;
	public float upgradeDelay;
	public GameObject bullet;
	public GameObject target;
	public float fireRange;
	public float fireRate;
	public float upgradeFireRate;
	public float speed;
	public float upgradeSpeed;
	public int armor;
	public int damage;
	public int hp;
	public int upgradeHp;
	public bool flip;
	bool isBuilding;


	// Use this for initialization
	void Start () {
		if (buttons) {
			for (int i=0; i<6; i++) {
				textArray [i] = (Text)buttons.transform.GetChild (i).GetChild (0).GetComponent<Text> ();
			}
		}
		isBuilding = false;
	}

	IEnumerator buildIt()
	{
		yield return new WaitForSeconds(delay);
		curUnit=Instantiate (unit,transform.position, Quaternion.identity) as GameObject;
		AttackUnit curAU = (AttackUnit)curUnit.GetComponent (typeof(AttackUnit));
		if (flip) 
		{
			curUnit.transform.localEulerAngles = new Vector3 (0, 0, 180f);
			curAU.offsetVec=new Vector3(0.39f,0,0);
		}

		health curHealth=(health)curUnit.GetComponent(typeof(health));
		curHealth.hp=hp;
		curHealth.armor=armor;
		curAU.bullet = bullet;
		curAU.target = target;
		curAU.fireRange = fireRange;
		curAU.fireRate = fireRate;
		curAU.damage = damage;
		curAU.speed = speed;
		curAU.flip = flip;
		isBuilding = false;
	}

	public void upgrades(int i)
	{
		switch (i) {
		case 0:
			damage += 1;
			break;
		case 1:
			fireRate += upgradeFireRate;
			break;
		case 2:
			speed += upgradeSpeed;
			break;
		case 3:
			armor += 1;
			break;
		case 4:
			hp += upgradeHp;
			break;
		case 5:
			delay -= upgradeDelay;
			break;
		}


	}
	
	// Update is called once per frame
	void Update () {

		if (buttons) {
			textArray[0].text="Fire Rate: "+ fireRate;
			textArray[1].text="Speed: "+speed*1000f;
			textArray[2].text="Damage: "+damage;
			textArray[3].text="HP: "+hp;
			textArray[4].text="BuildTime: " + delay;
			textArray[5].text="Armor: " +armor;
		}
	if (!isBuilding) {
			isBuilding=true;
			StartCoroutine(buildIt());
		}
	}
}
