using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public GameObject Diamond;
	public GameObject Ship;
	System.Random RNG;
	public float objectCounter;

	void Start()
	{
		RNG=new System.Random(ThreadSafeRandom.Next());
	}

	void Update()
	{

		if(RNG.Next(0,60)==4)
		{
			Vector2 v=new Vector2(Ship.transform.position.x-RNG.Next(-5, 6),Ship.transform.position.y-RNG.Next(-5, 6));
			GameObject go=(GameObject)Instantiate(Diamond, v, Quaternion.identity) as GameObject;

			SpriteRenderer sr=go.GetComponent<SpriteRenderer>();
			sr.color=new Vector4((float)RNG.Next(10,100)/100f,(float)RNG.Next(10,100)/100f,(float)RNG.Next(10,100)/100f,.95f);
		}

	}
}
