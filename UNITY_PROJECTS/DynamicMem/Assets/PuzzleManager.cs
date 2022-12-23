using UnityEngine;
using System;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour {
	public GameObject puzzle;
	public GameObject[] nArray=new GameObject[9];
	public GameObject[] sArray=new GameObject[3];

	GameObject[] topRow=new GameObject[3];
	GameObject[] midRow=new GameObject[3];
	GameObject[] botRow=new GameObject[3];

	GameObject[] leftCol=new GameObject[3];
	GameObject[] midCol=new GameObject[3];
	GameObject[] rightCol=new GameObject[3];

	static List<GameObject[]> rows=new List<GameObject[]>{};
	static List<GameObject[]> cols=new List<GameObject[]>{};

static int[] shuffleID=new int[3];

	public static List<GameObject> RotatedObjs=new List<GameObject>{};
	public static bool rotateBack;
	public static int puzzlePointer;
	public static List<string> solutionList=new List<string>{};



	// Use this for initialization
	void Start () {
		rotateBack=false;
	puzzlePointer=0;
solutionList.Add("1(Clone)");
solutionList.Add("2(Clone)");
solutionList.Add("3(Clone)");
solutionList.Add("4(Clone)");
solutionList.Add("5(Clone)");
solutionList.Add("6(Clone)");
solutionList.Add("7(Clone)");
solutionList.Add("8(Clone)");
solutionList.Add("9(Clone)");



		setUpCubes ();

		for(int i=0; i<puzzle.transform.childCount;i++)
		{
			if(i < 3)
			{
				topRow[i]=puzzle.transform.GetChild(i).gameObject;
			}
			else if(i<6)
			{
				midRow[i-3]=puzzle.transform.GetChild(i).gameObject;
			}
			else
			{
				botRow[i-6]=puzzle.transform.GetChild(i).gameObject;
			}
		}


		leftCol[0]=topRow[0];
		leftCol[1]=midRow[0];
		leftCol[2]=botRow[0];
		midCol[0]=topRow[1];
		midCol[1]=midRow[1];
		midCol[2]=botRow[1];
		rightCol[0]=topRow[2];
		rightCol[1]=midRow[2];
		rightCol[2]=botRow[2];

		rows.Add(topRow);
		rows.Add(midRow);
		rows.Add(botRow);
		cols.Add(leftCol);
		cols.Add(midCol);
		cols.Add(rightCol);

	}

void setUpCubes()
{
	
		List<int> numbers=new List<int> {0,1,2,3,4,5,6,7,8};
	
		int r;

	int childCount = puzzle.transform.childCount;
	for(int i=0; i<childCount; i++) {
			System.Random rand = new System.Random (ThreadSafeRandom.Next ()); 
			GameObject temp=puzzle.transform.GetChild(i).gameObject;
			 r=rand.Next (numbers.Count);
			GameObject number=Instantiate(nArray[numbers[r]]) as GameObject;
			GameObject shape=Instantiate(sArray[rand.Next(3)]) as GameObject;
			number.transform.SetParent(temp.transform);
			shape.transform.SetParent(temp.transform);
			number.transform.localPosition=new Vector3(0,0,-.5f);
			shape.transform.localPosition=new Vector3(0,0,-.5f);
		temp.transform.localEulerAngles=new Vector3(0,180,0);
			numbers.Remove(numbers[r]);
		}
		numbers.Add(0);
		numbers.Add(1);
		numbers.Add(2);
		for(int i=0; i<3; i++)
		{
			System.Random rand = new System.Random (ThreadSafeRandom.Next ()); 
			r=rand.Next(3-i);
			shuffleID[i]=numbers[r];
			numbers.Remove(numbers[r]);

		}
}



public static void shuffle(int s, GameObject g)
{
	int r=3;
	int c=3;
	for(int i=0; i<3; i++)
	{
		for(int j=0; j<3;j++)
		{
			if(rows[i][j].Equals(g))
			{
				r=i;
				c=j;
				break;
			}
		}
	}

	switch(shuffleID[s])
	{
		case 0:

		pushRow(r);
		break;

		case 1:
		pushCol(c);
		break;

		case 2:
		pushRow(r);
		pushCol(c);
		break;

	}

}

//turn back incorrect
static void pushRow(int r)
{
	GameObject[] tempArray=new GameObject[3];
	for(int i=0; i<3;i++)
	{
	tempArray[i]=rows[r][i];
	}

	rows[r][0]=tempArray[2];
	rows[r][1]=tempArray[0];
	rows[r][2]=tempArray[1];

	Vector3 v=tempArray[2].transform.position;

for(int i=0; i<3;i++)
{
	rows[r][i].transform.position=tempArray[i].transform.position;
}

rows[r][2].transform.position=v;

for(int i=0; i<3;i++)
{
	cols[i][r]=rows[r][i];
}

}

static void pushCol(int r)
{

GameObject[] tempArray=new GameObject[3];
	for(int i=0; i<3;i++)
	{
	tempArray[i]=cols[r][i];
	}

	cols[r][0]=tempArray[2];
	cols[r][1]=tempArray[0];
	cols[r][2]=tempArray[1];

	Vector3 v=tempArray[2].transform.position;

for(int i=0; i<3;i++)
{
	cols[r][i].transform.position=tempArray[i].transform.position;
}

cols[r][2].transform.position=v;

for(int i=0; i<3;i++)
{
	rows[i][r]=cols[r][i];
}


}
	
	// Update is called once per frame
	void Update () {

		if(rotateBack)
		{
			foreach(GameObject temp in RotatedObjs)
			{
				temp.transform.localEulerAngles=new Vector3(0,180,0);
			}

			rotateBack=false;
		}
	
	}
}
