using UnityEngine;
using System.Collections.Generic;
using System;

namespace Nup{
public class ShowMovementScript : MonoBehaviour {
	public static GameObject MovePrefab;
		public static bool canSwipeMove;
		public static bool areThereCircles;
	float Xoff;
	float Yoff;

	void OnMouseOver() {
			if(canSwipeMove && !MoveCircle.makeMove)
			{
			if (areThereCircles) {
								GameManager.stopShowingMovement = true;
						}
		GameManager.SelectedPiece = gameObject;
		GameManager.OffsetVector = new Vector3 (Xoff, Yoff, 0);
			if (!GameManager.stopShowingMovement) {
								showMovement ();
					canSwipeMove=false;
				}
					}
	}

	void OnMouseExit()
		{canSwipeMove = true;
				}



	// Use this for initialization
	void Start () {
			canSwipeMove = true;
		MovePrefab = GameObject.Find ("MovementCircle");
			DontDestroyOnLoad (MovePrefab);
		switch (gameObject.name) 
	{case "HelperPrefab(Clone)":
				Xoff=-0.02f;
				Yoff=-0.03f;
				break;
		case "KittyPrefab(Clone)":
			Xoff=0f;
			Yoff=0f;
			break;
		case "StarPlayerPrefab(Clone)":
			Xoff=0f;
			Yoff=0.02f;
			break;
				}
	}
	
	// Update is called once per frame
	void Update () {

			if (GameManager.UpMoveCircle != null || GameManager.DownMoveCircle != null || GameManager.LeftMoveCircle != null || GameManager.RightMoveCircle != null) {
								areThereCircles = true;
			} else { areThereCircles = false;
						}
			//Debug.Log (areThereCircles);
		if (GameManager.stopShowingMovement) 
			{

				Destroy(GameManager.UpMoveCircle);
				Destroy(GameManager.DownMoveCircle);
				Destroy(GameManager.LeftMoveCircle);
				Destroy(GameManager.RightMoveCircle);
				GameManager.UpMoveCircle=null;
				GameManager.DownMoveCircle=null;
				GameManager.LeftMoveCircle=null;
				GameManager.RightMoveCircle=null;
				GameManager.stopShowingMovement=false;
						}
			switch (gameObject.name) 
		{case "HelperPrefab(Clone)":
					double t = gameObject.transform.position.x + Xoff;
				double c = (t + 4) / 1.25;
				t = gameObject.transform.position.y + Yoff;
				double r = (t - 3) / -1.25;
				GameManager.SidekickLoc[0]=(int)Math.Round(r);
				GameManager.SidekickLoc[1]=(int)Math.Round(c);
				break;
			case "KittyPrefab(Clone)":
				t = gameObject.transform.position.x + Xoff;
				c = (t + 4) / 1.25;
				t = gameObject.transform.position.y + Yoff;
				r = (t - 3) / -1.25;
				GameManager.KittenLoc[0]=(int)Math.Round(r);
				GameManager.KittenLoc[1]=(int)Math.Round(c);
				break;
			case "StarPlayerPrefab(Clone)":
				t = gameObject.transform.position.x + Xoff;
				c = (t + 4) / 1.25;
				t = gameObject.transform.position.y + Yoff;
				r = (t - 3) / -1.25;
				GameManager.PlayerLoc[0]=(int)Math.Round(r);
				GameManager.PlayerLoc[1]=(int)Math.Round(c);
				break;
			}


		}
		
		void showMovement()
		{
			double t = gameObject.transform.position.x + Xoff;
			double c = (t + 4) / 1.25;
			t = gameObject.transform.position.y + Yoff;
			double r = (t - 3) / -1.25;
			canMoveDown((int)Math.Round(r),(int)Math.Round(c));
			canMoveLeft((int)Math.Round(r),(int)Math.Round(c));
			canMoveUp ((int)Math.Round(r), (int)Math.Round(c));
			canMoveRight ((int)Math.Round(r), (int)Math.Round(c));
			
		}



	bool canMoveDown(int r, int c)
	{
		if (GameManager.Board [r, c].name.Equals ("LRCornerPrefab(Clone)")) {
			return false;
		} 
		else if (GameManager.Board [r, c].name.Equals ("LLCornerPrefab(Clone)")) {
			return false;
		} 
		else if (GameManager.Board [r, c].name.Equals ("BottomWallPrefab(Clone)")) {
			return false;
		}
		else {
			int t=0;
			r++;
			for (; r<=5; r++) {
				
				if(GameManager.containsPiece(r, c))
				{
					if(t==0)
					{
						return false;
					}
					else
					{
						r--;
							GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				}
				
				switch (GameManager.Board [r, c].name) {
				case "TopWallPrefab(Clone)":
					
					if(t==0)
					{
						return false;
					}
					else
					{
						r--;
							GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "ULCornerTilePrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						r--;
							GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "URCornerPrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						r--;
							GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "LRCornerPrefab(Clone)":
						GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				case "LLCornerPrefab(Clone)":
						GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				case "BottomWallPrefab(Clone)":
						GameManager.DownMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				default: t++;
					continue;
				}
				
			}
		}		 return false;
		}

	bool canMoveLeft(int r, int c)
	{
	
		if (GameManager.Board [r, c].name.Equals ("ULCornerTilePrefab(Clone)")) {
						return false;
				} else if (GameManager.Board [r, c].name.Equals ("LLCornerPrefab(Clone)")) {
						return false;
				} else if (GameManager.Board [r, c].name.Equals ("LeftWallTilePrefab(Clone)")) {
						return false;
				}
				else {
			int t=0;
			c--;//corner would be hit above if c==0
						for (; c>=0; c--) {

				if(GameManager.containsPiece(r, c))
				{
					if(t==0)
					{
						return false;
					}
					else
					{
						c++;
							GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				}

								switch (GameManager.Board [r, c].name) {
					case "RightWallPrefab(Clone)":

					if(t==0)
					{
						return false;
					}
					else
					{
						c++;
							GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
					case "LRCornerPrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						c++;
							GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
					case "URCornerPrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						c++;
							GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
								case "ULCornerTilePrefab(Clone)":
						GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
										return true;
								case "LLCornerPrefab(Clone)":
						GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
										return true;
								case "LeftWallTilePrefab(Clone)":
						GameManager.LeftMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
										return true;
					default: t++;
										continue;
								}
							
						}
		} return false;


	}

	bool canMoveRight(int r, int c)
	{if (GameManager.Board [r, c].name.Equals ("LRCornerPrefab(Clone)")) {
			return false;
		} else if (GameManager.Board [r, c].name.Equals ("URCornerPrefab(Clone)")) {
			return false;
		} else if (GameManager.Board [r, c].name.Equals ("RightWallPrefab(Clone)")) {
			return false;
		}
		else {
			int t=0;
			c++;
			for (; c<=7; c++) {
				
				if(GameManager.containsPiece(r, c))
				{
					if(t==0)
					{
						return false;
					}
					else
					{
						c--;
							GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				}
				
				switch (GameManager.Board [r, c].name) {
				case "LeftWallTilePrefab(Clone)":
					
					if(t==0)
					{
						return false;
					}
					else
					{
						c--;
							GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "LLCornerPrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						c--;
							GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "ULCornerTilePrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						c--;
							GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "LRCornerPrefab(Clone)":
						GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				case "URCornerPrefab(Clone)":
						GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				case "RightWallPrefab(Clone)":
						GameManager.RightMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				default: t++;
					continue;
				}
				
			}
		} return false;
	}
	bool canMoveUp(int r, int c)
	{
		if (GameManager.Board [r, c].name.Equals ("URCornerPrefab(Clone)")) {
			return false;
		} 
		else if (GameManager.Board [r, c].name.Equals ("ULCornerTilePrefab(Clone)")) {
			return false;
		} 
		else if (GameManager.Board [r, c].name.Equals ("TopWallPrefab(Clone)")) {
			return false;
		}
		else {
			int t=0;
			r--;
			for (; r>=0; r--) {
				
				if(GameManager.containsPiece(r, c))
				{
					if(t==0)
					{
						return false;
					}
					else
					{
						r++;
							GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				}
				
				switch (GameManager.Board [r, c].name) {
				case "LRCornerPrefab(Clone)":
					
					if(t==0)
					{
						return false;
					}
					else
					{
						r++;
							GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "LLCornerPrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						r++;
							GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "BottomWallPrefab(Clone)":
					if(t==0)
					{
						return false;
					}
					else
					{
						r++;
							GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
						return true;
					}
				case "TopWallPrefab(Clone)":
						GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				case "ULCornerTilePrefab(Clone)":
						GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				case "URCornerPrefab(Clone)":
						GameManager.UpMoveCircle = Instantiate (MovePrefab, new Vector3 (-4 + (c * 1.25f), 3 - (r * 1.25f), 0), Quaternion.identity);
					return true;
				default: t++;
					continue;
				}
				
			}
		}		 return false;
		}
}
}