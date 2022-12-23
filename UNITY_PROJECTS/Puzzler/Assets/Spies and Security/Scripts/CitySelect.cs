using UnityEngine;
using System.Collections;

public class CitySelect : MonoBehaviour {

	public GameObject Mark;
	public GameObject Manager;
	CryptoManager CM;
	public int[] routeIDs=new int[8];
	void OnMouseDown()
	{
		//Instantiate(Mark,transform.position,Quaternion.identity);
		CM.SelectedPiece.transform.position=transform.position;
	}

	// Use this for initialization
	void Start () {
	Manager=GameObject.Find("Manager");
	CM=(CryptoManager)Manager.GetComponent(typeof(CryptoManager));

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
