using UnityEngine;
using System.Collections;

public class CellBehavior : MonoBehaviour {

	public float speed;
	public Vector2 moveVector;
	public bool move;
	public bool attack;
	public bool circle;
	public GameObject target;
	public int teamID;



	void OnCollisionEnter2D(Collision2D other)
	{
		CellBehavior cb=(CellBehavior)other.gameObject.GetComponent(typeof(CellBehavior));
		if(cb.teamID != teamID)
		{
			target=other.gameObject;
			move=false;
			attack=true;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(move)
		{
			transform.Translate(moveVector*(speed)*Time.deltaTime);
		}

		if(attack)
		{
			System.Random rng=new System.Random(ThreadSafeRandom.Next());
			if(rng.Next(0,2)==1)
			{
				Destroy(target);
				attack=false;
			}
		}

		if(circle)
		{
			moveVector=new Vector2(transform.position.y-target.transform.position.y, target.transform.position.x-transform.position.x);
			moveVector=moveVector.normalized;
			transform.Translate(moveVector*speed*Time.deltaTime);
		}
	
	}
}
