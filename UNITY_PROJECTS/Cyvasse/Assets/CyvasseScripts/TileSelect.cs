using UnityEngine;
using System.Collections.Generic;
using System;

public class TileSelect : MonoBehaviour {
	//set by GameManager.MakeBoard();
	public int rank;
	public int file;

	bool isCaptureTile;
	GameObject capturedWhite;
	GameObject capturedBlack;


	void OnMouseDown()
	{
		if(GameManager.SelectedPiece)
		{
		GameManager.SelectedPiece.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-1);
		GameManager.tileLocation = gameObject;
		GameManager.showMovement = false;
		GameManager.stopShowingMovement=true;
		PieceSelect pSelect=(PieceSelect)GameManager.SelectedPiece.GetComponent(typeof(PieceSelect));
	
			this.UpdateMovement(new int[]{pSelect.thisPiece.rank, pSelect.thisPiece.file});
		
		pSelect.thisPiece.rank=rank;
		pSelect.thisPiece.file=file;

		Destroy (GameManager.redOutline);
	
	//determine capture
	pSelect.thisPiece.isCaptured=isCaptureTile;

		//build and destroy fortress
		if (!GameManager.blackKingPlaced && GameManager.SelectedPiece.Equals (GameManager.kingBlack)) 
		{
			GameManager.blackKingPlaced=true;
				}
		if (!GameManager.whiteKingPlaced && GameManager.SelectedPiece.Equals (GameManager.kingWhite)) 
		{
			GameManager.whiteKingPlaced=true;
		}

		if (GameManager.SelectedPiece.transform.position.Equals (GameManager.fortressBlack.transform.position) && GameManager.SelectedPiece.transform.parent.name.Equals ("White Side")) 
		{
		GameManager.fortressBlack.transform.position=new Vector3(20,20,-20);
				}

		if (GameManager.SelectedPiece.transform.position.Equals (GameManager.fortressWhite.transform.position) && GameManager.SelectedPiece.transform.parent.name.Equals ("Black Side")) 
		{
			GameManager.fortressWhite.transform.position=new Vector3(20,20,-20);
		}

			GameManager.SelectedPiece=null;
		}
	}

	// Use this for initialization
	void Start () {
		capturedWhite=GameObject.Find("CapturedWhite");
		capturedBlack=GameObject.Find("CapturedBlack");
		if(transform.IsChildOf(capturedWhite.transform) || transform.IsChildOf(capturedBlack.transform))
		{
			isCaptureTile=true;
		}
		else{isCaptureTile=false;}
	
	}

	 void UpdateMovement(int[] a)
	{
		int index;
		PieceSelect pSelect=(PieceSelect)GameManager.SelectedPiece.GetComponent(typeof(PieceSelect));
		if(!pSelect.thisPiece.isCaptured)
		foreach(int[] ia in MovementScript.OccupiedTiles)
		{
			if(ia[0]==a[0] && ia[1]==a[1])
			{
				index=MovementScript.OccupiedTiles.IndexOf(ia);
				MovementScript.OccupiedTiles.Remove(MovementScript.OccupiedTiles[index]);
				break;
			}
		}
		if(!isCaptureTile)
		{
	MovementScript.OccupiedTiles.Add(new int[]{rank, file});
}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
