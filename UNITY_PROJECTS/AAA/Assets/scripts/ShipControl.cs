using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {

	public GameObject fireButton;
	ButtonScript fireButtonScript;
	public GameObject Torpedo;

	// Use this for initialization
	void Start () {

		fireButtonScript=(ButtonScript)fireButton.GetComponent(typeof(ButtonScript));
		fireButtonScript.skillMethod=Fire;
		fireButtonScript.setUpButton();
	}

	public void Fire()
	{
		Instantiate(Torpedo, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
