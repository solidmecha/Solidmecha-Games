using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
	public static float delay;
	public GameObject[] turrets=new GameObject[3];
	public GameObject Factory;
	FactoryScript fScript;
	Tower[] tScripts=new Tower[3];
	bool upgrade;



	// Use this for initialization
	void Start () {
		upgrade = true;
	 fScript = (FactoryScript)Factory.GetComponent (typeof(FactoryScript));
		for (int i=0; i<3; i++) 
		{
			tScripts[i]=(Tower) turrets[i].GetComponent(typeof(Tower));
		}

	}
	IEnumerator AIupgrade()
	{
		yield return new WaitForSeconds(delay);
		System.Random albequerque = new System.Random (ThreadSafeRandom.Next ());
		int r=albequerque.Next (4);
		Debug.Log ("upgrade " + r);
		switch (r) {

		case 3: 
			fScript.upgrades(albequerque.Next(6));
			break;
		default:
			tScripts[r].upgrades(albequerque.Next(3));
			break;
		}
		upgrade = true;
	}
	// Update is called once per frame
	void Update () {
		if (upgrade) {
			upgrade=!upgrade;
			StartCoroutine(AIupgrade());
		}
	
	}
}
