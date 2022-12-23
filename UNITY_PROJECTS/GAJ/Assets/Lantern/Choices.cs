using UnityEngine;
using System.Collections;

public class Choices : MonoBehaviour {

	public int resultIndex;
	public int resultChange;


	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		   {
			other.gameObject.transform.position=new Vector2(0, 4f);
		}
		SystemControl.Stats [resultIndex] += resultChange;
		SystemControl.madeChoice = true;
		}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
