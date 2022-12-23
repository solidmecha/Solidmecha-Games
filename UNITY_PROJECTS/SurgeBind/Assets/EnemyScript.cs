using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public GameObject fireball;
	public float fireFrameDelay;
	public float MovementDelay;

	public float YlowerBound;
	public float YupperBound;
	public float XupperBound;
	public float XlowerBound;
	public float Yadjust;
	public float Xadjust;
	int frameCount;

void OnTriggerEnter2D(Collider2D OtherOne)
{	
	if(OtherOne.gameObject.name.Equals("LashObj"))
	Destroy(gameObject);}

	// Use this for initialization
	void Start () {
		StartCoroutine(FireFireball());
		StartCoroutine(UpdatePos());
	}

		IEnumerator FireFireball() {
		yield return new WaitForSeconds(fireFrameDelay);
		Instantiate (fireball, gameObject.transform.position,Quaternion.identity);
		StartCoroutine(FireFireball());
	}

	IEnumerator UpdatePos()
	{
		yield return new WaitForSeconds(MovementDelay);
		transform.position=new Vector2(MageXpos(), MageYpos());
		StartCoroutine(UpdatePos());
	}

	float MageYpos()
	{
		if(Yadjust>0)
		{

		if(transform.position.y <= YupperBound)
		{return transform.position.y+Yadjust;}
		
		else if(transform.position.y>=YupperBound)
		{Yadjust*=-1;
		return transform.position.y+Yadjust;}
		}
		else
		{
		 if(transform.position.y>=YlowerBound)
		{return transform.position.y+Yadjust;}
		if(transform.position.y<=YlowerBound)
		{Yadjust*=-1;
		return transform.position.y+Yadjust;}}
		return transform.position.y;
	}

	float MageXpos()
	{
		if(Xadjust>0)
		{

		if(transform.position.x <= XupperBound)
		{return transform.position.x+Xadjust;}
		
		else if(transform.position.x>=XupperBound)
		{Xadjust*=-1;
		return transform.position.x+Xadjust;}
		}
		else
		{
		 if(transform.position.x>=XlowerBound)
		{return transform.position.x+Xadjust;}
		if(transform.position.x<=XlowerBound)
		{Xadjust*=-1;
		return transform.position.x+Xadjust;}}
		return transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {

		
	
	}
}
