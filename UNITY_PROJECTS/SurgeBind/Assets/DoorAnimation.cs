using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour {

	Animator animator;
	bool playerNear;

	void OnTriggerStay2D(Collider2D otherCol)
	{

		if(otherCol.gameObject.name.Equals("Player"))
		{
			playerNear=true;
			StartCoroutine(nextlevel());
		}
	}
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	IEnumerator nextlevel()
	{
		yield return new WaitForSeconds(1f);
		if(Application.loadedLevel<2)
		{
 		Application.LoadLevel(Application.loadedLevel+1);
 		GameManager.curlvl++;
 		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerNear) 
		{
				animator.SetBool("openDoor",true);
		}
	}
}
