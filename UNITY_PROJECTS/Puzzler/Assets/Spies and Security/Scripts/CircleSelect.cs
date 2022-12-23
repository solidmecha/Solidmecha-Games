using UnityEngine;
using System.Collections;

public class CircleSelect : MonoBehaviour {

	public GameObject Manager;
	public int transitID; //red, blue, yellow 0 , 1 ,2 for ID
	CryptoManager CM;
	GameObject display;

	void OnMouseDown()
	{
		if(display!=null)
		{Destroy(display);}

		display=(GameObject)Instantiate(CM.SelectedDisplay, transform.position, Quaternion.identity);
		CM.IDnumbers[CM.IDpointer]=transitID;

	}

	// Use this for initialization
	void Start () {
		CM=(CryptoManager)Manager.GetComponent(typeof(CryptoManager));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
