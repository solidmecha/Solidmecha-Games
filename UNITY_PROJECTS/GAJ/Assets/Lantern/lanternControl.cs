using UnityEngine;
using System.Collections;

public class lanternControl : MonoBehaviour {


bool rotatedCam;
public float speed;
Animator anim;
	// Use this for initialization
	void Start () {
		anim=GetComponent<Animator>();
		rotatedCam = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.RightArrow))
		{
		
			transform.Translate(new Vector2(speed*Time.deltaTime,0f));
			//Camera.main.transform.Translate(new Vector2(speed*Time.deltaTime,0f));
				transform.localEulerAngles=new Vector3(0,0f,0);
				anim.SetBool("isWalk",true);
			if(rotatedCam)
			{
				Camera.main.transform.RotateAround(transform.position, Vector3.up, 180f);
				rotatedCam=false;
			}
		}
	
	else if(Input.GetKey(KeyCode.LeftArrow))
		{
		
			transform.Translate(new Vector2(speed*Time.deltaTime,0f));
			transform.localEulerAngles=new Vector3(0,-180f,0);
			anim.SetBool("isWalk",true);
			if(!rotatedCam)
			{
				Camera.main.transform.RotateAround(transform.position, Vector3.up, 180f);
				rotatedCam=true;
			}
		}
		else{
			anim.SetBool("isWalk",false);
		}
	}
}
