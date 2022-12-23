using UnityEngine;
using System.Collections;

public class OtherObjectScript : MonoBehaviour {

	public float lifeTime;
	public GameObject[] markers=new GameObject[4];
	public float speed;
	public Vector2 direction;


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("torpedo"))
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	System.Random RNG=new System.Random(ThreadSafeRandom.Next());
	int r=RNG.Next(0,4);
	GameObject go=(GameObject)Instantiate(markers[r],transform.position,Quaternion.identity)as GameObject;
	go.transform.SetParent(transform);
	 speed =RNG.Next(0,3);

	int x=RNG.Next(-99,100);
	int y=RNG.Next(-99,100);
	direction=new Vector2(x,y);
	direction=direction.normalized;
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(speed*direction*Time.deltaTime);

		lifeTime=lifeTime-Time.deltaTime;
		if(lifeTime<0)
		{
			Destroy(gameObject);
		}
	
	}
}
