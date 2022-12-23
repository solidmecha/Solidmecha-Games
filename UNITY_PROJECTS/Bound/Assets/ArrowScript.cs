using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {
	public float Rspeed;
	public float minSize;
	public float maxSize;
	public float ScaleChange;
	public float force;
	public float testF;

	Rigidbody2D rb;
	bool isRotating;
	bool isScaling;

	void OnMouseDown()
	{
		if(isRotating)
		{
			isRotating=false;
			isScaling=true;
		}
		else if (isScaling)
		{
			isScaling=false;
		}
	}

	// Use this for initialization
	void Start () {
		rb=gameObject.GetComponent<Rigidbody2D>();
	isRotating=true;
	}

	
	// Update is called once per frame
	void FixedUpdate () {

		if(Input.GetKeyDown("space"))
		{
			rb.AddForce(Vector2.up*testF);
		}
		if(isRotating)
		{
		transform.Rotate(0,0,Time.deltaTime*Rspeed);
		}
		else if(isScaling)
		{
			if(transform.GetChild(0).localScale.x >= maxSize && ScaleChange > 0)
			{
				ScaleChange *= -1;
			}
			else if (transform.GetChild(0).localScale.x <= minSize && ScaleChange < 0)
			{
				ScaleChange *= -1;
			}
			else
			{
				transform.GetChild(0).localScale=new Vector3(ScaleChange*Time.deltaTime,ScaleChange*Time.deltaTime,0)+transform.GetChild(0).localScale;
			}
		}
		else
		{
			Vector2 vf=(Vector2) transform.GetChild(0).transform.position-(Vector2)transform.position;
			rb.AddForce(vf*force);
			isRotating=true;
		}
	
	}
}
