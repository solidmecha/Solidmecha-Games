using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	public GameObject HexTile;
	public static GameObject SelectedPiece;
	float x,y;
	//int makesSenseToMe;


	public static GameObject[][] BoardArrays=new GameObject[11][];
	public GameObject highlight;
	public static bool highlighted;
	public static GameObject redOutline;

	public static bool showMovement;
	public static GameObject tileLocation;
	public GameObject moveTile;
	public GameObject captureTile;

	//bool isplayer1setup;
	//bool isplayer2setup;
	public static bool blackToMove;
	public static bool whiteToMove;
	public GameObject whiteSide;
	public GameObject blackSide;
	public GameObject capturedWhite;
	public GameObject capturedBlack;


	public static GameObject kingWhite;
	public static GameObject kingBlack;
	public static GameObject fortressWhite;
	public static GameObject fortressBlack;
	bool builtFortressWhite;
	bool builtFortressBlack;
	public static bool blackKingPlaced;
	public static bool whiteKingPlaced;
	public static GameObject rulesObj;
	public static bool stopShowingMovement;
	List<GameObject> moveTiles=new List<GameObject>{};


	// Use this for initialization
	void Start () {
		rulesObj = gameObject;
		GameRules Rules = (GameRules)rulesObj.GetComponent (typeof(GameRules)); 
		kingWhite = GameObject.Find ("King");
		kingBlack = GameObject.Find ("King2");
		fortressBlack = GameObject.Find ("Fortress2");
		fortressWhite = GameObject.Find ("Fortress");
		builtFortressBlack = false;
		builtFortressWhite = false;
		blackKingPlaced = false;
		whiteKingPlaced = false;
		for(int i=0;i<11;i++)
		{
			if(i<6)
			{BoardArrays[i]=new GameObject[i+6];}
			else
			{
				BoardArrays[i]=new GameObject[16-i];
			}
		}
		makeBoard ();
		player1setup();
		player2setup();
	}

	public void makeBoard()
	{for (int r=0; r<6; r++) {
				for (int i=11-r; i>0; i--) {

				x=1.26f*i+(.63f*r);
				y=r;


				if(y==0)
				{BoardArrays[r+5][i-1]=Instantiate(HexTile, new Vector2(x,y), Quaternion.identity) as GameObject;
					TileSelect tSelect=(TileSelect)BoardArrays[r+5][i-1].GetComponent(typeof(TileSelect));
					tSelect.rank=r+5;
					tSelect.file=i-1;
				}
				else
				{
				BoardArrays[5-r][i-1]=Instantiate(HexTile, new Vector2(x,y), Quaternion.identity) as GameObject;
				BoardArrays[r+5][i-1]=Instantiate(HexTile, new Vector2(x,-y), Quaternion.identity) as GameObject;
				TileSelect tSelect=(TileSelect)BoardArrays[r+5][i-1].GetComponent(typeof(TileSelect));
					tSelect.rank=r+5;
					tSelect.file=i-1;
					TileSelect bSelect=(TileSelect)BoardArrays[5-r][i-1].GetComponent(typeof(TileSelect));
					bSelect.rank=5-r;
					bSelect.file=i-1;
				}

//				if(i-1==0)
//				{
//					Debug.Log (BoardArrays[5-r][i-1].transform.position.x);
//					Debug.Log (BoardArrays[5-r][i-1].transform.position.y);
//				}
						}
				}

//			{for (int r=0; r<6; r++) {
//						for (int i=11; i-r>5; i--) {
//		if(r%2==0)
//						x=1.26f*i;
//				else x=.63f*i;
//						y=r;
//					Instantiate(HexTile, new Vector2(x,y), Quaternion.identity);
//				Instantiate(HexTile, new Vector2(x,-y), Quaternion.identity);
//						}
//					}
		}


	//set up for player1
	void player1setup()
	{
		Transform t;
		for (int i=0; i<21; i++) 
		{
			t=whiteSide.transform.GetChild(i);
				t.position=new Vector3(capturedWhite.transform.GetChild(i).position.x, capturedWhite.transform.GetChild(i).position.y, -1);
			capturedWhite.transform.GetChild(i).localScale=new Vector2(t.GetComponent<BoxCollider2D>().size.x*t.transform.localScale.x,t.GetComponent<BoxCollider2D>().size.y*t.transform.localScale.y);
//		
			//placed pieces in the middle of the board
			//if(i<10)
//			{
//			 t=whiteSide.transform.GetChild(i);
//				t.position=new Vector3(BoardArrays[4][i].transform.position.x,BoardArrays[4][i].transform.position.y,-1);
//			}
//			else 
//			{
//				t=whiteSide.transform.GetChild(i);
//				t.position=new Vector3(BoardArrays[5][i-10].transform.position.x,BoardArrays[5][i-10].transform.position.y,-1);
//			}
//
				}
		}

	void player2setup()
	{
				Transform t;
				for (int i=0; i<21; i++) {
						t = blackSide.transform.GetChild (i);
						t.position = new Vector3 (capturedBlack.transform.GetChild (i).position.x, capturedBlack.transform.GetChild (i).position.y, -1);
			capturedBlack.transform.GetChild(i).localScale=new Vector2(t.GetComponent<BoxCollider2D>().size.x*t.transform.localScale.x,t.GetComponent<BoxCollider2D>().size.y*t.transform.localScale.y);
				}
		}



	// Update is called once per frame
	void Update () {
	

		if (!builtFortressBlack && blackKingPlaced) 
		{
			fortressBlack.transform.position=kingBlack.transform.position;
			builtFortressBlack=true;
				}
		if (!builtFortressWhite && whiteKingPlaced) 
		{
			fortressWhite.transform.position=kingWhite.transform.position;

			builtFortressWhite=true;
				}

		if (!highlighted && SelectedPiece) 
		{
			 redOutline=Instantiate(highlight, SelectedPiece.transform.position, Quaternion.identity)as GameObject;
			redOutline.transform.localScale=new Vector2(SelectedPiece.GetComponent<BoxCollider2D>().size.x*SelectedPiece.transform.localScale.x,SelectedPiece.GetComponent<BoxCollider2D>().size.y*SelectedPiece.transform.localScale.y);
			highlighted=true;
				}

		if(stopShowingMovement)
	{

		foreach(GameObject g in moveTiles)
		{
			Destroy(g);
		}
		moveTiles.Clear();
		stopShowingMovement=false;
	}

		if (showMovement) 
		{
			PieceSelect pSelect=(PieceSelect)SelectedPiece.GetComponent(typeof(PieceSelect));

			int a=pSelect.thisPiece.rank;
			int b=pSelect.thisPiece.file;
	/*		for(int i=0; i<11;i++)
			{

				if(Array.IndexOf(BoardArrays[i],tileLocation)>=0)//fun bug >
				   {
					a=i;
					b= Array.IndexOf(BoardArrays[i], tileLocation);
					break;
				} 

			} */
			MovementScript.findOrthogonalMovement(a,b,pSelect.thisPiece.orthogonalMovement);
			MovementScript.findDiagonalMovement(a,b,pSelect.thisPiece.diagonalMovement);

			MovementScript.findContinuousMovement(a,b,pSelect.thisPiece.continuousMovement);
			foreach(GameObject g in MovementScript.MoveList)
			{
				moveTiles.Add(Instantiate(moveTile,new Vector3(g.transform.position.x,g.transform.position.y,g.transform.position.z-1),Quaternion.identity)as GameObject);
			}

			foreach(GameObject g in CaptureScript.CaptureList)
			{
				moveTiles.Add(Instantiate(captureTile,new Vector3(g.transform.position.x,g.transform.position.y,g.transform.position.z-0.5f),Quaternion.identity)as GameObject);
			}
			MovementScript.MoveList.Clear();
			CaptureScript.CaptureList.Clear();
			showMovement=false;
				}



	if(Input.GetKeyDown(KeyCode.R))
		   {
			Application.LoadLevel(0);
		}
	}
}
