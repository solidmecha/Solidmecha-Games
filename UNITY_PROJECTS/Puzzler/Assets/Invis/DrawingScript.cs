using UnityEngine;
using System.Collections.Generic;

public class DrawingScript : MonoBehaviour {

	public GameObject Line;
	public List<Vector3> VectorList=new List<Vector3>{};
	List<Vector3> PrevVectors=new List<Vector3>{};
	public List<Vector3> OffsetList=new List<Vector3>{}; 
	public int iterations;
	public bool clockWise;

	public List<GameObject> Polygons=new List<GameObject>{};

	// Use this for initialization
	void Start () {

		//VectorList.Add(Vector3.zero);
		//VectorList.Add(new Vector3());

		foreach(GameObject GO in Polygons)
		{
			PrevVectors.Clear();
			VectorList.Clear();
			for(int i=0;i<GO.transform.childCount;i++)
				VectorList.Add(GO.transform.GetChild(i).position);
		createPolygon(VectorList);
		spiralPolygon();
		spiralPolygon();
		spiralPolygon();
		spiralPolygon();
		}
	
	}

	void spiralPolygon()
	{
		
			for(int i=0;i<iterations;i++)
		{
			PrevVectors.Clear();
			updateOffsets(i);
			List<Vector3> tempVectorList=new List<Vector3>{};

			for(int j=0;j<VectorList.Count;j++)
			{
				Vector3 v3=VectorList[j]+OffsetList[j];
				PrevVectors.Add(VectorList[j]);
				tempVectorList.Add(v3);
			}

			createPolygon(tempVectorList);

			VectorList.Clear();
			VectorList=tempVectorList;
		}

	}

	void updateOffsets(int i)
	{
		OffsetList.Clear();
		int C=VectorList.Count;
		if(clockWise)
		{
		for(int j=0;j<C;j++)
			{
				Vector3 v3;
				if(j+1==C)
				v3=VectorList[0]-VectorList[j];
				else
				v3=VectorList[j+1]-VectorList[j];

				v3=v3*(1f/(float)iterations);

				OffsetList.Add(v3);
				
			}
		}
	}


	void createPolygon(List<Vector3> vList)
	{
		int C=vList.Count;
		for(int i=0;i<C;i++)
		{
			GameObject go=Instantiate(Line) as GameObject;
			LineAnim LA=(LineAnim) go.GetComponent(typeof(LineAnim));
			LineRenderer lr=go.GetComponent<LineRenderer>();
			Vector3[] points=new Vector3[2];
			if(i+1==C)
			{
				points[0]=vList[i];
				points[1]=vList[0];
			}
			else
			{
				points[0]=vList[i];
				points[1]=vList[i+1];
			}
			if(PrevVectors.Count>0)
			{
				LA.StartPoints[0]=points[0];
				LA.StartPoints[1]=points[1];
				if(i+2==C)
			{
				LA.EndPoints[0]=PrevVectors[i+1];
				LA.EndPoints[1]=PrevVectors[0];
			}
			else if(i+1==C)
			{
				LA.EndPoints[0]=PrevVectors[0];
				LA.EndPoints[1]=PrevVectors[1];
			}
			else
			{
				LA.EndPoints[0]=PrevVectors[i+1];
				LA.EndPoints[1]=PrevVectors[i+2];
			}
			}
			lr.SetPositions(points);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
