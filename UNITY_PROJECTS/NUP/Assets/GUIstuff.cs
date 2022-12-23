using UnityEngine;
using System.Collections;
namespace Nup{
public class GUIstuff : MonoBehaviour {

	public static GameObject player, kitty, helper;
	public static float seconds;
	public static int moves;
	string needvarName;
	string sNumberOfMoves;
	string sWins;
	string sTimeUsed;
	string sMovesUsed;
	void OnGUI()
	{
			GUI.Label (new Rect ((Screen.width / 2f), 0f, 300f, 500f), "<color=white><size=33>Time: "+needvarName+"</size></color>");
			GUI.Label (new Rect (Screen.width / 4f, 0f, 300f, 50f),  "<color=white><size=33>Moves: "+sNumberOfMoves+  "</size></color>");
			GUI.Label (new Rect (Screen.width / 1.25f, 0f, 300f, 50f),  "<color=white><size=33>Wins: "+sWins  +"</size></color>");
			if(GUI.Button(new Rect(Screen.width-(Screen.width/10f+25),65f,Screen.width/10f+25,Screen.height/15f+25f), "<size=24>Teleport</size>"))
			{
                GameManager.teleportersOn = !GameManager.teleportersOn;
                Application.LoadLevel(0);
            }



	if(GUI.Button(new Rect(Screen.width-(Screen.width/10f+25),165f,Screen.width/10f+25,Screen.height/15f+25f), "<size=26>Collect</size>"))
{
	GameManager.collectMode=!GameManager.collectMode;
                Application.LoadLevel(0);
            }


			//sets a specific timelimit/move limit
		/*	if(!GameManager.withCountDown)
			{
				if(GUI.Button(new Rect(Screen.width-(Screen.width/10f+25),165f,Screen.width/10f+25,Screen.height/15f+25f), "<size=26>Limit</size>"))
			{
				GameManager.secondsPast=GameManager.secondsPast+Time.time;
				GameManager.numberOfMoves=0;
				GameManager.withCountDown=!GameManager.withCountDown;
			}
		}*/

			if(GUI.Button(new Rect(1,0,Screen.width/10f+50,Screen.height/15f+35f), "<size=26>New Level</size>"))
			{
				GameManager.replay=false;
				GameManager.stopShowingMovement=true;
				Application.LoadLevel(0);
			}

			if(GUI.Button (new Rect(1, Screen.height/2,Screen.width/10f+50f, Screen.height/15f+35f), "<size=26>Replay</size>"))
			{
				GameManager.replay=true;
				Application.LoadLevel(0);
			}

			if(GUI.Button(new Rect(1, Screen.height/4,Screen.width/10f+50f, Screen.height/15f+35f), "<size=26>Shuffle</size>"))
			{
				GameManager.stopShowingMovement=true;
                if(Application.loadedLevel==0)
				GameManager.placePieces();
			}
if(GameManager.withCountDown)
{
			if(GUI.Button(new Rect(1, Screen.height/1.5f,Screen.width/10f+50f, Screen.height/15f+35f), "<size=26>+15moves</size>"))
			{

				GameManager.numberOfMoves-=15;
			}

			if(GUI.Button(new Rect(1, Screen.height/1.2f,Screen.width/10f+50f, Screen.height/15f+35f), "<size=26>+60sec</size>"))
			{
				GameManager.secondsPast+=60f;
				GameManager.bonusTime++;
			}
		}

			if(Application.loadedLevel==1)
			{
				GUI.Label(new Rect(Screen.width/4f,Screen.height-30f, 100, 75), "Moves Used: "+sMovesUsed);
				GUI.Label(new Rect(Screen.width/2f,Screen.height-30f, 250, 75), "Time Used: "+sTimeUsed);
			}

		}

	// Use this for initialization
	void Start () {
		
				moves=100;
						
	}
	
	// Update is called once per frame
	void Update () {

			if(GameManager.madeLevel)
			{
				player = GameObject.Find ("StarPlayerPrefab(Clone)");
				kitty = GameObject.Find ("KittyPrefab(Clone)");
				helper = GameObject.Find ("HelperPrefab(Clone)");
			}
			if(!GameManager.withCountDown)
			{
		seconds = Time.time;
		needvarName=(seconds).ToString();
			sNumberOfMoves = GameManager.numberOfMoves.ToString ();
			sWins=GameManager.numberOfWins.ToString ();
			}
			else
			{
				//if(Application.loadedLevel==0)
				//{
					seconds = GameManager.secondsPast-Time.time;
					//}
				needvarName=(seconds).ToString();
				GameManager.movesPast=moves-GameManager.numberOfMoves;
				sNumberOfMoves = (GameManager.movesPast).ToString ();
				sWins=GameManager.numberOfWins.ToString ();
			}

			if(Application.loadedLevel==1)
			{sTimeUsed=GameManager.TimeUsed.ToString();
			 sMovesUsed=GameManager.MovesUsed.ToString();}
	}
}
}

