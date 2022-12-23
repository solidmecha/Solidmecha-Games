using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	public GameObject Texts;
	Text[] textArray = new Text[3];
	public GameObject bullet;
	GameObject projectile;
	public GameObject target;
	public float fireRange;
	public float fireRate;
	public int damage;
	bool shouldFire;

	public float upgradeRange;
	public float upgradeRate;
	public int upgradeDmg;
	string targetName;
	public bool flip;

	// Use this for initialization
	void Start () {
		if (Texts)
			setUpText ();
		if (transform.parent.name.Equals ("Red turrets")) {
			targetName = "bAtk";
		} else {
			targetName = "rAtk";
		}
		shouldFire = true;
	
	}

	void setUpText()
	{
		for (int i=0; i<3; i++) {
			textArray[i]=(Text)Texts.transform.GetChild(i).gameObject.GetComponent<Text>();
		}
	}

	IEnumerator openFire()
	{
		yield return new WaitForSeconds(fireRate);
		if (target) 
		{
			projectile = Instantiate (bullet, transform.position, Quaternion.identity) as GameObject;
			ProjectileScript projScript = (ProjectileScript)projectile.GetComponent (typeof(ProjectileScript));
			Vector2 direction = new Vector2 (target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
			projScript.targetVec = direction;
			projScript.dmg = damage;
			projScript.target = target;
		}
		shouldFire = true;
	}

	public void upgrades(int i)
	{
		switch (i) {
		case 0: damage+=upgradeDmg; break;
		case 1: fireRange+=upgradeRange; break;
		case 2: fireRate-=upgradeRate; break;
		default: break;
		
		}
	}
	// Update is called once per frame
	void Update () {
		if (Texts) {
			textArray [0].text = "Damage: " + damage.ToString ();
			textArray [1].text = "Delay: " + fireRate.ToString ();
			textArray [2].text = "Range: " + fireRange.ToString ();
		}

		if (target) {
			if (transform.position.x <= target.transform.position.x && !flip)
				transform.localEulerAngles = new Vector3 (0, 0, Vector2.Angle (transform.position, target.transform.position));
			else if(!flip)
				transform.localEulerAngles = new Vector3 (0, 0, -Vector2.Angle (transform.position, target.transform.position));
			if (transform.position.x <= target.transform.position.x && flip)
				transform.localEulerAngles = new Vector3 (0, 0, -Vector2.Angle (transform.position, target.transform.position));
			else if(flip)
				transform.localEulerAngles = new Vector3 (0, 0, Vector2.Angle (transform.position, target.transform.position));
			if (Vector2.Distance (transform.position, target.transform.position) <= fireRange && shouldFire) {
				shouldFire = false;
				StartCoroutine (openFire ());
			} else if (shouldFire) {	
				GameObject[] targetAr = GameObject.FindGameObjectsWithTag (targetName);
				for (int i=0; i<targetAr.Length; i++) {
					if (Vector2.Distance (transform.position, targetAr [i].transform.position) <= fireRange) {
						target = targetAr [i];
					}

				}
			}
		} else {
			GameObject[] targetAr = GameObject.FindGameObjectsWithTag (targetName);
			for (int i=0; i<targetAr.Length; i++) {
				if (Vector2.Distance (transform.position, targetAr [i].transform.position) <= fireRange) {
					target = targetAr [i];
				}
			}
		}
	
	}
}
