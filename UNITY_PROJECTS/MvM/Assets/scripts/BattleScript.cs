using UnityEngine;
using System.Collections.Generic;

public class BattleScript:MonoBehaviour{

	public static List<GameObject> team1moveOrderList=new List<GameObject>{};
	public static List<GameObject> team2moveOrderList=new List<GameObject>{};
	public static GameObject currentUnit;
	static bool battleComplete;
	static EntityScript ES;

	public static bool wait;
	
	public static int index;

	public bool startBattle;
	
	bool team1Turn=true;
	bool team2Turn=false;
		

	public static int mana;
	public static int metal;


	void cleanUp()
	{
		//add and remove stuff from move order list
		team1Turn=!team1Turn;
		team2Turn=!team2Turn;
		//Debug.Log(team1Turn);
		index=0;
	}

	
	// Update is called once per frame
	void Update () {

		if (!wait &&startBattle) {
			if(!battleComplete) {
				
				if(team1Turn){
	

		if(index==team1moveOrderList.Count)
		{
				cleanUp();
		}
		else{

					currentUnit = team1moveOrderList [index];
					ES = (EntityScript)team1moveOrderList [index].GetComponent (typeof(EntityScript));
						ES.takeTurn ();
						wait=true;
					}
		
					
				}

				else
				{
					


	if(index==team2moveOrderList.Count)
		{
			cleanUp();
		}
else{
					currentUnit = team2moveOrderList [index];
					ES = (EntityScript)team2moveOrderList [index].GetComponent (typeof(EntityScript));
						ES.takeTurn ();
						wait=true;
					}
					
				}
			}
			}
		}
	
	}

