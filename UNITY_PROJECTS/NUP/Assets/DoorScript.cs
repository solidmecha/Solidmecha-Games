using UnityEngine;
using System.Collections;
namespace Nup{
public class DoorScript : MonoBehaviour {

	public GameObject doorSprite;
	public char doorTile;
	public Quaternion doorRotation;
	public int doorRow;
	public int doorCol;
	public int row;
	public int col;
	bool doorOpen;
	bool check;


	// Use this for initialization
	void Start () {
	if(!name.Equals("DoorButton"))
	check=true;
	}
	
	// Update is called once per frame
	void Update () {
		if(check)
		{
		if(GameManager.containsPiece(row,col) && !doorOpen && !ShowMovementScript.areThereCircles)
		{

			GameObject temp=(GameObject)GameManager.Board[doorRow,doorCol];
			Vector3 pos=temp.transform.position;
			Destroy(temp);
			GameManager.Board[doorRow,doorCol]=new GameObject();
			GameManager.Board[doorRow,doorCol]=Instantiate(GameManager.EmptyTile,pos,Quaternion.identity) as GameObject;
			doorOpen=true;
		}

		if(!GameManager.containsPiece(row,col) && doorOpen)
		{
			GameObject temp=(GameObject)GameManager.Board[doorRow,doorCol];
			Vector3 pos=temp.transform.position;
			Destroy(temp);
			GameManager.Board[doorRow,doorCol]=Instantiate(CustomLevels.tileLookup(doorTile),pos,CustomLevels.getRotation(CustomLevels.tileLookup(doorTile).name))as GameObject;

			doorOpen=false;
		}
	}
	
	}
}
}
