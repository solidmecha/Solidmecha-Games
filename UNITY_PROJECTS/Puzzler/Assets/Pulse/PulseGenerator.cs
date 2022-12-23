using UnityEngine;
using System.Collections;

public class PulseGenerator : MonoBehaviour {
	public GameObject Pulse;
	GameObject varGO;
	public float delay;
	public float incSet;
	public Color color;

	// Use this for initialization
	void Start () {
		StartCoroutine(startPulse());
	}

	IEnumerator startPulse()
	{
		yield return new WaitForSeconds(delay);
		varGO = Instantiate (Pulse, transform.position, Quaternion.identity) as GameObject;
		PulseControl pc = (PulseControl)varGO.GetComponent (typeof(PulseControl));
		pc.inc = incSet;
		SpriteRenderer sr = varGO.GetComponent<SpriteRenderer> ();
		sr.color = color;
		StartCoroutine(startPulse());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
