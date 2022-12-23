using UnityEngine;
using System.Collections;

namespace Nup{
public class teleportScript : MonoBehaviour {


public GameObject otherTele;
public teleportScript otherTS;
public int[] teleLoc; 
public bool pieceTeleported;
GameObject targetPiece;

	// Use this for initialization
	void Start () {
           
        }

	bool readyTeleport()
	{
		
for(int i=0; i<GameManager.PieceLocations.Count;i++) 
		{
			if(GameManager.PieceLocations[i][0]==teleLoc[0] && GameManager.PieceLocations[i][1]==teleLoc[1])
			{
                    
				if(i==0)
				{
					targetPiece=(GameObject)GameManager.Player;
				}
				else if(i==1)
				{
					targetPiece=(GameObject)GameManager.Kitten;
				}
				else
				{
					targetPiece=(GameObject)GameManager.Sidekick;
				}
				return true;
			}
			else
				{
				continue;
				}
			}

			return false;
		}

				

	

	
	// Update is called once per frame
	void Update () {
           
            if (GameManager.madeLevel && otherTS != null)
		{

                if (!pieceTeleported && !otherTS.pieceTeleported && !ShowMovementScript.areThereCircles )
	{
		if(readyTeleport())
		{
			if(targetPiece != null)
			targetPiece.transform.position=otherTele.transform.position;
			pieceTeleported=true;
		} 
	}

	if(pieceTeleported)
	{
		pieceTeleported=GameManager.containsPiece(teleLoc[0],teleLoc[1]) || GameManager.containsPiece(otherTS.teleLoc[0],otherTS.teleLoc[1]);
	}


	}
}
}
}
