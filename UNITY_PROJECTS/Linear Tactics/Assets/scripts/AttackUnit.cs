using UnityEngine;
using System.Collections;

public class AttackUnit : MonoBehaviour {

	public GameObject bullet;
	GameObject projectile;
	public GameObject target;
	public float fireRange;
	public float fireRate;
	public float speed;
	public int damage;
	public Vector3 offsetVec=new Vector3(-.39f,0f,0f);
	bool shouldFire;
	public bool flip;
	// Use this for initialization
	void Start () {
		shouldFire = true;
	}
	

	IEnumerator openFire()
	{
		yield return new WaitForSeconds(fireRate);
		projectile=Instantiate(bullet, transform.position+offsetVec, Quaternion.identity) as GameObject;
		ProjectileScript projScript=(ProjectileScript) projectile.GetComponent(typeof(ProjectileScript));
		Vector2 direction=new Vector2(target.transform.position.x-transform.position.x, target.transform.position.y-transform.position.y);
		projScript.targetVec = direction;
		projScript.dmg = damage;
		projScript.target = target;
		shouldFire = true;
	}
	// Update is called once per frame
	void Update () {
		if(Vector2.Distance(transform.position, target.transform.position)<=fireRange && shouldFire)
		{
			shouldFire=false;
			StartCoroutine(openFire());
		}
		else if(shouldFire)
		{
			Vector2 direction=new Vector2(target.transform.position.x-transform.position.x, target.transform.position.y-transform.position.y);
			if (flip)
				direction=-direction;
			transform.Translate(direction.normalized*speed);
		} 
	}
}