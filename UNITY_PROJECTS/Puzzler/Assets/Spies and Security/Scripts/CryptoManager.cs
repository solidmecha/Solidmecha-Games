using UnityEngine;
using System.Collections.Generic;

public class CryptoManager : MonoBehaviour {

	public GameObject city;
	public GameObject diagonalLine;
	public GameObject horizontalLine;
	
	public GameObject Spy;
	public GameObject Security;
	public GameObject Agent;
	public GameObject Detective;

	public int[] IDnumbers=new int[3]; //Security, Agent, Detective, 0,1,2

	public GameObject SelectedPiece;
	public GameObject SelectedDisplay;
	public int IDpointer;

	public Vector2 SpyLocation;

	public GameObject Scanner;
	List<Color> Colors=new List<Color>{Color.red, Color.blue, Color.yellow, Color.green, Color.cyan}; //red, blue, yellow 0 , 1 ,2 for ID
	public int mapHeight=8;
	public int mapWidth=8;

	public bool confirmed;

	public GameObject[][] horizontalRoutes=new GameObject[8][];
	public GameObject[][] verticalRoutes=new GameObject[8][];
	public GameObject[][] nwRoutes=new GameObject[8][];
	public GameObject[][] neRoutes=new GameObject[8][];
	public GameObject[] SpyRoute=new GameObject[3];

	// Use this for initialization
	void Start () {
	setUpArea();
	relocateSpy(0);
	relocateSpy(1);
	relocateSpy(2);
	}

	void setUpArea()
	{
		for(int i=0;i<8;i++)
		{
			horizontalRoutes[i]=new GameObject[8];
			verticalRoutes[i]=new GameObject[8];
			nwRoutes[i]=new GameObject[8];
			neRoutes[i]=new GameObject[8];
		}

		for(int j=0;j<mapHeight;j++)
		{
		for(int i=0;i<mapWidth;i++)
			{
				Instantiate(city, new Vector2(i,j), Quaternion.identity);

				if(i!=mapWidth-1)
				horizontalRoutes[i][j]=cInstantiate(horizontalLine, new Vector2(i+.5f, j), Quaternion.identity);
				if(j!=mapHeight-1)
				verticalRoutes[i][j]=cInstantiate(horizontalLine, new Vector2(i,j+.5f),Quaternion.Euler(0,0,90));
				if(j!=mapHeight-1 && i!=mapWidth-1)
					neRoutes[i][j]=cInstantiate(diagonalLine, new Vector2(i+.5f,j+.5f),Quaternion.Euler(0,0,90));
				if(i!=0 && j!=mapHeight-1)
					nwRoutes[i][j]=cInstantiate(diagonalLine, new Vector2(i-.5f,j+.5f),Quaternion.identity);

			}
		}

		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		
		Spy.transform.position=new Vector2(RNG.Next(1,7), RNG.Next(1,7));

		do
		{
			Security.transform.position=new Vector2(RNG.Next(8), RNG.Next(8));
		}
		while(Security.transform.position.Equals(Spy.transform.position));

			do
		{
			Agent.transform.position=new Vector2(RNG.Next(8), RNG.Next(8));
		}
		while(Agent.transform.position.Equals(Spy.transform.position) || Agent.transform.position.Equals(Security.transform.position));

			do
		{
			Detective.transform.position=new Vector2(RNG.Next(8), RNG.Next(8));
		}
		while(Detective.transform.position.Equals(Spy.transform.position)|| Detective.transform.position.Equals(Security.transform.position) ||Detective.transform.position.Equals(Agent.transform.position));

	}

	public void relocateSpy(int z)
	{
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		int Rx=0;
		int Ry=0;
		int sx=(int)Spy.transform.position.x;
		int sy=(int)Spy.transform.position.y;
		Vector2 sV=new Vector2(sx, sy);
		do
		{
			Rx=RNG.Next(-1,2);
			Ry=RNG.Next(-1,2);
			if(Spy.transform.position.x+Rx<=7 && Spy.transform.position.x+Rx>0 && Spy.transform.position.x+Ry<=7 && Spy.transform.position.x+Ry>0)
			{
				if(Rx != 0 || Ry != 0)
				Spy.transform.position=new Vector2(Spy.transform.position.x+Rx, Spy.transform.position.y+Ry);
			}
		}
		while(Spy.transform.position.x==sV.x && Spy.transform.position.y==sV.y);

		GameObject Route=Spy;
		//sV is earlier pos of spy
		switch(Rx)
		{
			case 0:
			if(Ry==0)
			{print("0 bug");}
			else if(Ry==1)
			{Route=verticalRoutes[(int)sV.x][(int)sV.y];}
			else
			{Route=verticalRoutes[(int)Spy.transform.position.x][(int)Spy.transform.position.y];}
			break;
			case 1:
				if(Ry==0)
			{Route=horizontalRoutes[(int)sV.x][(int)sV.y];}
			else if(Ry==1)
			{Route=neRoutes[(int)sV.x][(int)sV.y];}
			else
			{Route=nwRoutes[(int)Spy.transform.position.x][(int)Spy.transform.position.y];}
			break;
			case -1:
			if(Ry==0)
			{Route=horizontalRoutes[(int)Spy.transform.position.x][(int)Spy.transform.position.y];}
			else if(Ry==1)
			{Route=nwRoutes[(int)sV.x][(int)sV.y];}
			else
			{Route=neRoutes[(int)Spy.transform.position.x][(int)Spy.transform.position.y];}
			break;

			default:
			print("Rx not set");
			break;
		}

		SpriteRenderer sr=Route.GetComponent<SpriteRenderer>();	

		SpyRoute[z].GetComponent<SpriteRenderer>().color=sr.color;
		print(sV.x.ToString() + " , " + sV.y.ToString());

	}
	public GameObject cInstantiate (GameObject g, Vector2 v, Quaternion q)
	{
		GameObject returnObj = (GameObject)Instantiate (g, v, q)as GameObject;
		if (!g.Equals (city)) {
			System.Random RNG=new System.Random(ThreadSafeRandom.Next());
			SpriteRenderer sr=returnObj.GetComponent<SpriteRenderer>();
			sr.color=Colors[RNG.Next (Colors.Count)];
		}
		return returnObj;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
