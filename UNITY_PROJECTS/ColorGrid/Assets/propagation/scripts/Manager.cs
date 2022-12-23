using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

public GameObject[,] Board;
public GameObject Tile;
public int rows;
public int cols;

	// Use this for initialization
	void Start () {

		Board=new GameObject[rows, cols];
	makeGrid();
	}


	void makeGrid()
	{
		for(int i=0;i<rows;i++)
		{
			for(int j=0;j<cols;j++)
			{
				Board[i,j]=Instantiate(Tile, new Vector2(i,j),Quaternion.identity) as GameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
