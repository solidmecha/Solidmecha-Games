using UnityEngine;
using System.Collections.Generic;

public class CaptureScript : MonoBehaviour {

	public static List<GameObject> CaptureList=new List<GameObject>{};
	public static GameObject WhiteSide;
	public static GameObject BlackSide;
	static GameObject OppositeSide;

	// Use this for initialization
	void Start () {
	WhiteSide=GameObject.Find("White Side");
	BlackSide=GameObject.Find("Black Side");
	}

	public static void CaptureChecker(int a, int b)
	{
		if(GameManager.SelectedPiece.transform.IsChildOf(WhiteSide.transform))
		{
			OppositeSide=BlackSide;
		}
		else if(GameManager.SelectedPiece.transform.IsChildOf(BlackSide.transform))
		{
			OppositeSide=WhiteSide;
		}
		else
		{
			Debug.Log("Check CaptureChecker");
		}

		int c=OppositeSide.transform.childCount;

		for(int i=0; i<c; i++)
		{
			PieceSelect pSelect=(PieceSelect)OppositeSide.transform.GetChild(i).gameObject.GetComponent(typeof(PieceSelect));
			if(!pSelect.thisPiece.isCaptured && pSelect.thisPiece.rank==a && pSelect.thisPiece.file==b)
			{
				PieceSelect aSelect=(PieceSelect)GameManager.SelectedPiece.GetComponent(typeof(PieceSelect));

				if(pSelect.thisPiece.combat<=aSelect.thisPiece.combat)
				{
					CaptureList.Add(GameManager.BoardArrays[a][b]);
				}

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
