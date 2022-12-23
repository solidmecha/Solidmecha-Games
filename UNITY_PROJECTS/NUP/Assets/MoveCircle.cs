using UnityEngine;
using System.Collections;

namespace Nup{
public class MoveCircle:MonoBehaviour {
	GameObject referencePiece;
	public static bool makeMove;
	bool startFrameCount;
	float frameCount;
	Vector3 Location=new Vector3();
	Vector3 realLocation=new Vector3();
	 void OnMouseOver()
	{
		if(!makeMove && !GameManager.stopShowingMovement)
			{
			referencePiece=gameObject;
			GameManager.numberOfMoves++;
			GameManager.MovesUsed++;
			startFrameCount = true;
			makeMove=true;
			}
		}


	// Use this for initialization
	void Start () {
			makeMove = false;
			startFrameCount = false;
			frameCount=1;
	}
	
	// Update is called once per frame
	void Update () {

			if (startFrameCount) 
			{ frameCount++;
				if(frameCount==31)
				{
					frameCount=1;
					startFrameCount=false;
				}
						}
			if (makeMove && GameManager.SelectedPiece != null && referencePiece != null)
			{
				//interpolates position over 30 frames
				Location=(GameManager.SelectedPiece.transform.position)+(frameCount/30f)*(referencePiece.transform.position-GameManager.SelectedPiece.transform.position);
				//Debug.Log (GameManager.SelectedPiece.transform.position.x);
				//Debug.Log (GameManager.SelectedPiece.transform.position.y);
				//Debug.Log(gameObject.transform.position.x);
				//Debug.Log(gameObject.transform.position.y);
				realLocation=Location;
				GameManager.SelectedPiece.transform.position=realLocation;
				if(Location==gameObject.transform.position)
				{makeMove=false;
					GameManager.stopShowingMovement=true;}
			
						}
	
	}
}
}
