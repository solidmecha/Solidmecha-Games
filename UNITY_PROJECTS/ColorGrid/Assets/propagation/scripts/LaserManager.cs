using UnityEngine;
using System.Collections;

public class LaserManager : MonoBehaviour {
	public GameObject laser;
	public GameObject colliderLaser;
	public GameObject resultantLaser;
	public GameObject reflection;
	GameObject resultL;

	// Use this for initialization
	void Start () {
	laserCollisionHandler();
	}
	
	// Update is called once per frame
	void Update () {
		reflection.transform.localEulerAngles=new Vector3(0,0,-laser.transform.localEulerAngles.z);
		float radians=laser.transform.localEulerAngles.z*Mathf.PI/180f;
		reflection.transform.position=laser.transform.position+new Vector3((laser.transform.localScale.x/2f)*Mathf.Cos(radians)+(reflection.transform.localScale.x/2f)*Mathf.Cos(radians),(laser.transform.localScale.x/2f)*Mathf.Sin(radians)-(reflection.transform.localScale.x/2f)*Mathf.Sin(radians),0); 
	}

	void laserCollisionHandler()
	{
		float colLaserRads=colliderLaser.transform.localEulerAngles.z*Mathf.PI/180f;
		float laserRads=laser.transform.localEulerAngles.z*Mathf.PI/180f;
		float colliderLaserOffset=colliderLaser.transform.position.y-Mathf.Sin(colLaserRads)*colliderLaser.transform.position.x/Mathf.Cos(colLaserRads);
		float laserOffset=laser.transform.position.y-Mathf.Sin(laserRads)*laser.transform.position.x/Mathf.Cos(laserRads);
		float offsetDiff=colliderLaserOffset-laserOffset;
		float slopeDiff=Mathf.Sin(laserRads)/Mathf.Cos(laserRads) - Mathf.Sin(colLaserRads)/Mathf.Cos(colLaserRads);
		float resultantX=offsetDiff/slopeDiff;
		float resultantY=Mathf.Sin(laserRads)*resultantX/Mathf.Cos(laserRads) + laserOffset;
		float resultAngle=(laser.transform.localEulerAngles.z+colliderLaser.transform.localEulerAngles.z)/2f;
		float resultRads=resultAngle*Mathf.PI/180f;
		Vector2 resultTransform=new Vector2(resultantX, resultantY)+new Vector2(resultantLaser.transform.localScale.x*Mathf.Cos(resultRads)/2f, resultantLaser.transform.localScale.x*Mathf.Sin(resultRads)/2f);
		resultL=Instantiate(resultantLaser, resultTransform, Quaternion.identity) as GameObject;
		resultL.transform.localEulerAngles=new Vector3(0,0, resultAngle);

	}
}
