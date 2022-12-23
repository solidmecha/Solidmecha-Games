using UnityEngine;
using System.Collections.Generic;

public class LashableObj : MonoBehaviour {

	bool SeekAndDestroy;
	bool youUnderstandTheGravityOftheSituation;
	bool checkTime;
	GameObject Target;
	GameObject[] TargetList;
	public float varVar;

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.name=="Player" && Input.GetKey(KeyCode.Q)) 
		{
			SeekAndDestroy=true;	}
		
		if (collision.gameObject.name=="Player" && Input.GetKey(KeyCode.W)) 
		{
			youUnderstandTheGravityOftheSituation=!youUnderstandTheGravityOftheSituation;}

	}


	// Use this for initialization
	void Start () {
	checkTime=true;
	}

	bool vector2Equals(Vector2 a, Vector2 b)
	{
		if (a.x==b.x && a.y==b.y)
		{
			return true;
		}



		else{
			return false;}
	}
	
	// Update is called once per frame
	void Update () {
	if(SeekAndDestroy)
	{
		Target=GameObject.Find("Enemy");

//		Vector3 TargetLocation=Target.transform.position;
			if(Target!=null)
			{transform.position=Vector3.Lerp(transform.position,Target.transform.position,.04f);}
			Debug.Log(Target);
		if(Target==null)
			{
				SeekAndDestroy=false;
				Destroy(gameObject);}
	}

	if(youUnderstandTheGravityOftheSituation)
	{
		TargetList=GameObject.FindGameObjectsWithTag("projectile");

		foreach(GameObject g in TargetList)
		{
				if(Vector3.Distance(g.transform.position, transform.position)<=10)
				{
				g.GetComponent<Rigidbody2D>().AddForce(new Vector2((transform.position.x-g.transform.position.x)*varVar, (transform.position.y-g.transform.position.y)*varVar));}
			}
			
		}
		float t=0;
		if(checkTime)
		{t=Time.time;
			checkTime=false;}
		if(t+4f==Time.time)
		{	checkTime=true;
			youUnderstandTheGravityOftheSituation=false;}
	}
}
