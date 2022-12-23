using UnityEngine;
using System.Collections;

namespace Nup{
public class CustomLevels : MonoBehaviour {

int currentLvl;
bool madeLvl;
GameObject currentTile;
string[] LevelArray=new string[6];
        int tpIndex;
GameObject[] teleporters;
// "17777772"
// "506"
// "37777774"
	// Use this for initialization
	void Start () {
	teleporters=new GameObject[24];
            tpIndex=0;
    
	currentLvl=14;
	madeLvl=false;
	}
	
	// Update is called once per frame
	void Update () {

				if(GameManager.customLvl && !madeLvl)
		{
	madeLvl=true;
	LevelArray=returnCustomLevel(currentLvl);
	makeCustomLevel();
	handlePieces(currentLvl);
	GameManager.actuallyPlacePieces();

      }


	}

void handlePieces(int l) //gamemanager.pieceloc[0]=0 top row, [1]=0 left-most column
{
	switch(l)
	{
	case 10:
	GameManager.PlayerLoc[0]=0;
	GameManager.PlayerLoc[1]=0;

	GameManager.KittenLoc[0]=0;
	GameManager.KittenLoc[1]=1;

	GameManager.SidekickLoc[0]=0;
	GameManager.SidekickLoc[1]=2;

	GameManager.GoalLoc[0]=3;
	GameManager.GoalLoc[1]=4;
	break;

	case 11: //things need moved here
		GameManager.PlayerLoc[0]=0;
	GameManager.PlayerLoc[1]=0;

	GameManager.KittenLoc[0]=0;
	GameManager.KittenLoc[1]=1;

	GameManager.SidekickLoc[0]=0;
	GameManager.SidekickLoc[1]=2;

	GameManager.GoalLoc[0]=3;
	GameManager.GoalLoc[1]=4;
	break;

	case 12:
		GameManager.PlayerLoc[0]=0;
	GameManager.PlayerLoc[1]=6;

	GameManager.KittenLoc[0]=1;
	GameManager.KittenLoc[1]=2;

	GameManager.SidekickLoc[0]=1;
	GameManager.SidekickLoc[1]=0;

	GameManager.GoalLoc[0]=2;
	GameManager.GoalLoc[1]=1;
	break;

	case 13:
		GameManager.PlayerLoc[0]=3;
	GameManager.PlayerLoc[1]=0;

	GameManager.KittenLoc[0]=4;
	GameManager.KittenLoc[1]=2;

	GameManager.SidekickLoc[0]=0;
	GameManager.SidekickLoc[1]=3;

	GameManager.GoalLoc[0]=2;
	GameManager.GoalLoc[1]=1;

	placeTeleporter(new int[2]{0,0}, new int[2]{1,1}, Color.blue);

	break;

	case 14:
	GameManager.PlayerLoc[0]=0;
	GameManager.PlayerLoc[1]=0;

	GameManager.KittenLoc[0]=0;
	GameManager.KittenLoc[1]=1;

	GameManager.SidekickLoc[0]=0;
	GameManager.SidekickLoc[1]=2;

	GameManager.GoalLoc[0]=4;
	GameManager.GoalLoc[1]=7;
	placeDoor(new int[2]{4,4}, new int[2]{3,6});
	break;

	default:
	Debug.Log("Pieces not set!");
	break;


	}
}




public void placeDoor(int[] a, int[] b)
{
	float posX1=-4+(a[1]*1.25f);
	float posY1=3-(a[0]*1.25f);
	GameObject button=Instantiate(GameManager.DoorButton, new Vector2(posX1,posY1),Quaternion.identity)as GameObject;
	DoorScript ds=(DoorScript)button.GetComponent(typeof(DoorScript));
	ds.row=a[0];
	ds.col=a[1];
	ds.doorRow=b[0];
	ds.doorCol=b[1];
	ds.doorTile=LevelArray[b[0]][b[1]];
	currentTile=tileLookup(ds.doorTile);
	ds.doorRotation=getRotation(currentTile.name);

}

public void placeTeleporter(int[] a, int[] b, Color c)
{
	float posX1=-4+(a[1]*1.25f);
	float posY1=3-(a[0]*1.25f);
	float posX2=-4+(b[1]*1.25f);
	float posY2=3-(b[0]*1.25f);
	teleporters[tpIndex]=Instantiate(GameManager.Teleporter, new Vector2(posX1,posY1), Quaternion.identity)as GameObject;
	teleporters[tpIndex+1]=Instantiate(GameManager.Teleporter, new Vector2(posX2,posY2), Quaternion.identity)as GameObject;
	teleportScript ts1= (teleportScript)teleporters[tpIndex].GetComponent(typeof(teleportScript));
	teleportScript ts2= (teleportScript)teleporters[tpIndex+1].GetComponent(typeof(teleportScript));
            SpriteRenderer sr = teleporters[tpIndex].GetComponent<SpriteRenderer>();
            sr.color = c;
            sr = teleporters[tpIndex+1].GetComponent<SpriteRenderer>();
            sr.color = c;
		ts1.otherTele=teleporters[tpIndex+1];
		ts1.otherTS=ts2;
		ts1.teleLoc=new int[2];
		ts1.teleLoc[0]=a[0];
		ts1.teleLoc[1]=a[1];

		ts2.otherTele=teleporters[tpIndex];
		ts2.otherTS=ts1;
		ts2.teleLoc=new int[2];
		ts2.teleLoc[0]=b[0];
		ts2.teleLoc[1]=b[1];
            tpIndex += 2;
		return;

}

void makeCustomLevel()
{
	for(int i=0;i<6;i++)
	{
		for(int j=0;j<8;j++)
		{
				float posX=-4+(j*1.25f);
				float posY=3-(i*1.25f);
			currentTile= tileLookup(LevelArray[i][j]);
			GameManager.Board[i,j]=Instantiate (currentTile,new Vector3(posX,posY,0),getRotation(currentTile.name));
		}
	}

	GameManager.madeLevel=true;

}

	string[] returnCustomLevel(int l)
	{
		string[] sArray=new string[6];
		switch(l)
		{
			case 10:
			sArray[0]="17777772";
			for(int i=1;i<5;i++)
			{sArray[i]="50000006";}
			sArray[5]="38888884";
			break;

			case 11:
			sArray[0]="17777772";
			for(int i=1;i<5;i++)
			{sArray[i]="50000006";}
			sArray[5]="38888884";
			break;

			case 12:
			sArray[0]="17777712";
			sArray[1]="52280406";
			sArray[2]="50304006";
			sArray[3]="50003606";
			sArray[4]="55006006";
			sArray[5]="34888834";
			break;

			case 13:
			sArray[0]="17777172";
			sArray[1]="50020036";
			sArray[2]="50600006";
			sArray[3]="50602046";
			sArray[4]="51200006";
			sArray[5]="33888884";
			break;

			case 14:
			sArray[0]="17777772";
			sArray[1]="50000006";
			sArray[2]="50000006";
			sArray[3]="50000084";
			sArray[4]="50000034";
			sArray[5]="38888884";
			break;


				default:
			Debug.Log("level doesn't exist");
			break;

			
		}
	return sArray;
	}


	public static GameObject tileLookup(char id)
	{
		switch (id)
		{
			case '0': //empty tile
			return GameManager.EmptyTile;
		
			case '1': //upper left corner
			return GameManager.UpperLeftCorner;
		
			case '2': //upper right
			return GameManager.UpperRightCorner;
		
			case '3': //lower left
			return GameManager.LowerLeftCorner;
			
			case '4': //lower right
			return GameManager.LowerRightCorner;
			
			case '5':  //left wall
			return GameManager.LeftWall;
		
			case '6': //right wall
			return GameManager.RightWall;
			
			case '7': //top wall 
			return GameManager.TopWall;
		
			case '8': //bot wall
			return GameManager.BottomWall;
		
			default:
			Debug.Log("Invalid tile ID");
			return GameManager.EmptyTile;
		
		}
	}

	public static Quaternion getRotation(string name)
	{		
			Quaternion URq = Quaternion.LookRotation(new Vector4 (0, 0, 270f,0), Vector3.right); //also top wall
			Quaternion LLq =  Quaternion.LookRotation(new Vector4 (0, 0, 180f,0), Vector3.left); //also bottom wall
			Quaternion LRq =  Quaternion.LookRotation(new Vector4 (0, 0, 180f,0), Vector3.down); //also right wall
		switch (name)
				{
			case "URCornerPrefab": return URq;
			case "LRCornerPrefab": return LRq;
			case "LLCornerPrefab": return LLq;
			case "BottomWallPrefab": return LLq;
			case "RightWallPrefab": return LRq;
			case "TopWallPrefab": return URq;
			default: return Quaternion.identity;
				}
			}
}
}
