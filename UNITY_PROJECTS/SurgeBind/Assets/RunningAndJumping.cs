using UnityEngine;
using System.Collections;

public class RunningAndJumping : MonoBehaviour {
	public float speed=5f;
	public float antigrav=415f;
	public float wallLashPos;


	float groundDist;
	// Use this for initialization
	void Start () {
		groundDist=GetComponent<Collider2D>().bounds.extents.y; 
	}

	IEnumerator slowDown()
	{
		yield return new WaitForSeconds (2.5f);
		speed = 5f;
	}

	public bool isGrounded()
	{
		return Physics2D.Raycast (transform.position, GetComponent<Rigidbody2D>().gravityScale*Vector3.down, groundDist+.1f); 
		}

	// Update is called once per frame
	void Update () {

	

		if (GameManager.WallLashed) {
			transform.position = new Vector2 (transform.position.x, wallLashPos);
			GetComponent<Rigidbody2D>().gravityScale=0;

				}
		else if(!GameManager.WallLashed)
		{wallLashPos = transform.position.y;}

		if (GameManager.inverted) 
		{
			
			GameObject.Find("Main Camera").transform.localEulerAngles=new Vector3(0,0,180);
			if(!GameManager.WallLashed)
			{GetComponent<Rigidbody2D>().gravityScale=-1;}
		}
		 if( !GameManager.inverted){
			transform.localEulerAngles=new Vector3(0,0,0);
			GameObject.Find("Main Camera").transform.localEulerAngles=new Vector3(0,0,0);
			if(!GameManager.WallLashed)
				{GetComponent<Rigidbody2D>().gravityScale=1;}
		}

	if(Input.GetKeyDown(KeyCode.E))
		{
			GameManager.WallLashed=!GameManager.WallLashed;
			GetComponent<Rigidbody2D>().velocity=Vector3.zero;
		}

	if (Input.GetKeyDown (KeyCode.R)) 
		{GameManager.inverted=!GameManager.inverted;
		
				}

		if (Input.GetKeyDown (KeyCode.T)) 
		{
			GameManager.lashForward=!GameManager.lashForward;
			if (GameManager.lashForward) 
			{
				speed=15f;
				StartCoroutine(slowDown());
			}
		}

	if(Input.GetAxis("Horizontal")>0)
		{
			if(!GameManager.inverted)
			{transform.Translate(new Vector3(speed*Time.deltaTime,0,0));} 
			else{transform.Translate(new Vector3(-speed*Time.deltaTime,0,0));}


		}

		if(Input.GetAxis("Horizontal")<0)
		{
			if(!GameManager.inverted)
			{transform.Translate(new Vector3(-speed*Time.deltaTime,0,0));} 
			else{transform.Translate(new Vector3(speed*Time.deltaTime,0,0));}
	
		}

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) 
		{
			if(isGrounded() && !GameManager.WallLashed)
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,GetComponent<Rigidbody2D>().gravityScale*antigrav));
				}
	}
}
