using UnityEngine;
using System.Collections;

public class BasketScript : MonoBehaviour {

	public float speed;

	public float Xmax;

	bool moveRight;

	// Use this for initialization
	void Start () {
		moveRight=true;
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(Vector3.right*speed*Time.deltaTime);

		if(transform.position.x <= Xmax)
		{
			//yay it works. don't ask me why
		}
		else if(speed>0)
		{
			speed *= -1;
		}

		if(transform.position.x <= -1*Xmax)
		{
			speed*=-1;
		}


	
	}
}
