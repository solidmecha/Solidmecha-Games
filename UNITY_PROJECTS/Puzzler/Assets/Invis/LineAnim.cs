using UnityEngine;
using System.Collections;

public class LineAnim : MonoBehaviour {

	public Vector3[] StartPoints=new Vector3[2];
	public Vector3[] EndPoints=new Vector3[2];
	Vector3[] Points=new Vector3[2];
	LineRenderer LR;
	 float[] speeds=new float[2];
	 float counter;
	 public bool spin;

	// Use this for initialization
	void Start () {
		
		speeds[0]=1/3f;
		speeds[1]=1/3f;
		counter=10;
		LR=GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(spin)
		{
		counter+=Time.deltaTime;
		if(counter<3f)
		{
		Points[0]=Points[0]+(EndPoints[0]-StartPoints[0])*speeds[0]*Time.deltaTime;
		Points[1]=Points[1]+(EndPoints[1]-StartPoints[1])*speeds[1]*Time.deltaTime;
		if(!Points[0].Equals(Points[1]))
		LR.SetPositions(Points);
		}
		else
		{
			Points[0]=StartPoints[0];
		Points[1]=StartPoints[1];
		LR.SetPositions(Points);
		counter=0;
		}
		}
		
		
	
	}
}
