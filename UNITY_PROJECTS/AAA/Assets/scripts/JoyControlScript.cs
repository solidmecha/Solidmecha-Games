using UnityEngine;
using System.Collections;

public class JoyControlScript : MonoBehaviour {

	public Vector2 origMousePos;
	public bool move;
	public float yMax=.36f;
	public float xMax=.36f;
	public GameObject Ship;
	public float speed=3;
	public GameObject go;

	void OnMouseDown()
	{
		Vector3 v3 = Input.mousePosition;
 			v3.z = 10.0f;
 			v3 = Camera.main.ScreenToWorldPoint(v3);
		
 		origMousePos=(Vector2) v3;
 		move=true;
	}

	void OnMouseUp()
	{
		move=false;
		transform.localPosition=Vector2.zero;
	}
	// Use this for initialization
	void Start () {
	move=false;
	}
	
	// Update is called once per frame
	void Update () {

		if(move)
		{
			Vector3 v3 = Input.mousePosition;
 			v3.z = 10.0f;
 			v3 = Camera.main.ScreenToWorldPoint(v3);
 			Vector2 mv=(Vector2)v3-origMousePos;
 			if(mv.x<=xMax && mv.x>=(-xMax))
			{transform.localPosition=new Vector2(mv.x,transform.localPosition.y);
				}
			if(mv.y<=yMax && mv.y>=(-yMax))
			{transform.localPosition=new Vector2(transform.localPosition.x,mv.y);}

			if(mv.x<=0)
			{
			float r=Vector2.Angle(Vector2.up, mv);
			Ship.transform.eulerAngles=new Vector3(0,0,r);
			}
			else
			{
				float r=Vector2.Angle(Vector2.up, mv);
			Ship.transform.eulerAngles=new Vector3(0,0,-r);
			}
			float a=Mathf.Abs(mv.x)/xMax;
			float b=Mathf.Abs(mv.y)/yMax;
			float c=(a+b)/2;
			if(c>1)
			{c=1;}
			Ship.transform.Translate(c*Vector2.up*speed*Time.deltaTime);
			Transform t=go.transform;
			t.position=origMousePos;
			t.transform.Translate(new Vector2(Ship.transform.position.x-Camera.main.transform.position.x,Ship.transform.position.y-Camera.main.transform.position.y)*2f*Time.deltaTime);
			origMousePos=t.position;
		}
	
	}
}
