using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Nup{
public class GameManager : MonoBehaviour {

	public static GameObject UpperLeftCorner;
	public static  GameObject UpperRightCorner;
	public static GameObject LowerLeftCorner;
	public static GameObject LowerRightCorner;
	public static GameObject LeftWall;
	public static GameObject RightWall;
	public static GameObject TopWall;
	public static GameObject BottomWall;
	public static GameObject LowHiReflector;
	public static GameObject HiLowReflector;
	public static GameObject UpperLeftWalledReflector;
	public static GameObject LowerLeftWalledReflector;
	public static GameObject UpperRightWalledReflector;
	public static GameObject LowerRightWalledReflector;
	public static GameObject EmptyTile;
	public static GameObject Star;
	public static GameObject Kitty;
	public static GameObject Helper;
	public static GameObject Exit;
	public static GameObject PsudoRandomPiece;
	public static GameObject Teleporter;
	public static GameObject DoorButton;

	public static List<GameObject> DontDestroyList;
	public static List<GameObject>PsudoRandomList=new List<GameObject>{};
	public static List<GameObject> TopWalls;
	public static List<GameObject> LeftWalls;
	public static List<GameObject> RightWalls;
	public static List<GameObject> BotWalls;
	public static GameObject scriptThingy;
	public static GameObject SelectedPiece;
	public static Vector3 OffsetVector;
	public static UnityEngine.Object LeftMoveCircle;
	public static UnityEngine.Object RightMoveCircle;
	public static UnityEngine.Object DownMoveCircle;
	public static UnityEngine.Object UpMoveCircle;

	public static int bonusTime;
	public static int numberOfMoves;
	public static float secondsPast;
	public static int movesPast;
	static float posZ, posX, posY, qZ,qX,qY;
	static float playerX, playerY, playerZ;
	static float kittenX, kittenY, kittenZ;
	static float goalX, goalY, goalZ;
	static float sidekickX, sidekickY, sidekickZ;
	public static UnityEngine.Object[,] Board =new UnityEngine.Object[6,8];
	public static List<int[]> PieceLocations=new List<int[]>{};
	public static UnityEngine.Object Player;
	public static UnityEngine.Object Goal;
	public static UnityEngine.Object Kitten;
	public static UnityEngine.Object Sidekick;
	
	public static int[] PlayerLoc = new int[2];
	public static int[] KittenLoc = new int[2];
	public static int[] SidekickLoc=new int[2];
	public static int[] GoalLoc = new int[2];

    public static List<int[]> teleportLocs=new List<int[]> { };

		public static Vector3 PlayerStart;
		public static Vector3 KittenStart;
		public static Vector3 HelperStart;
		public static Vector3 GoalStart;

	public static float[] PlayerOffset = new float[2];
	public static float[] KittenOffset = new float[2];
	public static float[] SidekickOffset = new float[2];
	public static bool stopShowingMovement;
		public static int numberOfWins;

	public static int[] psudoRandomIntArray=new int[2]; //not acutually needed
	public static GameObject[,] RecordPieceArray=new GameObject[6,8]; //records last board 

	public static float TimeUsed;
	public static int MovesUsed;

	public static bool replay;
	public static bool collectMode;
    public static bool teleportersOn;
        public List<Color> teleColor = new List<Color> {new Color(.2367f,.01f,.99f,1), new Color(.6875f, .76757f, .867f,1), new Color(.09f, .83f, .2f), new Color(.37f, .28f, 0, 1) };


    public static bool withCountDown;

	public static bool madeLevel;

	public static bool customLvl;

	public GameObject collectionStar;
	
		// Use this for initialization
	void Start () {
          
		//	withCountDown = false;
	//	customLvl=true;
			secondsPast = 300 + bonusTime*60;

			TimeUsed=0f;
			MovesUsed=0;

		stopShowingMovement = false;
		madeLevel = false;
			
			DoorButton=GameObject.Find("DoorButton");
			Teleporter=GameObject.Find("Teleport");
			UpperLeftCorner=GameObject.Find("ULCornerTilePrefab");
			UpperRightCorner=GameObject.Find("URCornerPrefab");
			LowerLeftCorner=GameObject.Find ("LLCornerPrefab");
			LowerRightCorner=GameObject.Find("LRCornerPrefab");
			LeftWall=GameObject.Find("LeftWallTilePrefab");
			RightWall=GameObject.Find ("RightWallPrefab");
			TopWall=GameObject.Find ("TopWallPrefab");
			BottomWall=GameObject.Find ("BottomWallPrefab");
			EmptyTile=GameObject.Find ("EmptyTilePrefab");
			Star=GameObject.Find ("StarPlayerPrefab");
			Kitty=GameObject.Find("KittyPrefab");
			Helper= GameObject.Find("HelperPrefab");
			Exit=GameObject.Find ("RedSquarePrefab");
			scriptThingy = GameObject.Find ("ScriptRunner");
			DontDestroyList= new List<GameObject>{UpperLeftCorner,UpperRightCorner, LowerLeftCorner, LowerRightCorner,LeftWall, RightWall, TopWall, BottomWall, EmptyTile, Star, Kitty, Helper, Exit};

			foreach (GameObject g in DontDestroyList) 
			{DontDestroyOnLoad(g);
						}

			TopWalls = new List<GameObject>{UpperLeftCorner, UpperRightCorner, TopWall};
			LeftWalls = new List<GameObject>{UpperLeftCorner,LeftWall,LowerLeftCorner};
			RightWalls = new List<GameObject>{UpperRightCorner,RightWall, LowerRightCorner};
			BotWalls=new List<GameObject>{ LowerLeftCorner,LowerRightCorner,BottomWall};

			SidekickOffset[0]=-0.02f;
			SidekickOffset[1]=-0.03f;
			KittenOffset[0]=0f;
			KittenOffset[1]=-0.1987f;
			PlayerOffset[0]=0f;
			PlayerOffset[1]=0.02f;
	if(!customLvl)
{
			for (int i=0; i<20; i++) 
			{
				PsudoRandomList.Add(EmptyTile);
				if(i<3)
				{
					PsudoRandomList.Add (TopWalls[i]);
					PsudoRandomList.Add (LeftWalls[i]);
					PsudoRandomList.Add (RightWalls[i]);
					PsudoRandomList.Add (BotWalls[i]);
				}
						}
			for (int i=0; i<18; i++) 
			{
				TopWalls.Add(TopWall);
				BotWalls.Add(BottomWall);
				LeftWalls.Add(LeftWall);
				RightWalls.Add(RightWall);
			}
if(replay)
{replayLastLevel();}
else
{	   createLevel ();
       placeTeleporters();
       placePieces();
       
   }
			madeLevel = true;
		}

		else //custom level
		{

		}
	}

	 void createLevel()
	{
		
		for(int r=0; r<6; r++)
		{
			for(int c=0;c<8;c++)
			{
				posZ=0;
				posX=-4+(c*1.25f);
				posY=3-(r*1.25f);
				qX=0;
				qY=0;
				qZ=0;
					setPsudoRandomPiece(r, c);
					RecordPieceArray[r,c]=PsudoRandomPiece;
					Board[r,c]=Instantiate(PsudoRandomPiece, new Vector3(posX, posY, 0), getRotation());
					if(collectMode)
					Instantiate(collectionStar,new Vector3(posX, posY, 0), Quaternion.identity);
				}
					
		}

        }
    

    void placeTeleporters()
        {
            if (teleportersOn)
            {
                GameManager.teleportLocs.Clear();
                System.Random RNG = new System.Random(ThreadSafeRandom.Next());
                int n = RNG.Next(2, 13);

                for (int i = 0; i < n; i++)
                {
                    bool placed = false;
                    while (!placed)
                    {
                        bool buildTele = false;
                        int[] a = new int[2] { RNG.Next(5), RNG.Next(7) };
                        int[] b = new int[2] { RNG.Next(5), RNG.Next(7) };
                        int z = GameManager.teleportLocs.Count;
                        if (z > 0)
                        {
                            for (int j = 0; j < z; j++)
                            {
                                if (GameManager.teleportLocs[j][0] != a[0] || GameManager.teleportLocs[j][1] != a[1])
                                {
                                    if (GameManager.teleportLocs[j][0] != b[0] || GameManager.teleportLocs[j][1] != b[1])
                                    {
                                        if (a[0] != b[0] || a[1] != b[1])
                                        {
                                            if (j + 1 == z)
                                            {
                                               // if ((SidekickLoc[0] != a[0] || SidekickLoc[1] != a[1]) && (KittenLoc[0] != a[0] || KittenLoc[1] != a[1]) && (PlayerLoc[0] != a[0] || PlayerLoc[1] != a[1]))
                                                //{
                                                  //  if ((SidekickLoc[0] != b[0] || SidekickLoc[1] != b[1]) && (KittenLoc[0] != b[0] || KittenLoc[1] != b[1]) && (PlayerLoc[0] != b[0] || PlayerLoc[1] != b[1]))
                                                        buildTele = true;
                                                //}
                                            }

                                        }
                                        else
                                            break;
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (a[0] != b[0] || a[1] != b[1])
                                buildTele = true;
                        }

                        if (buildTele)
                        {
                            CustomLevels cl = (CustomLevels)GetComponent(typeof(CustomLevels));
                            cl.placeTeleporter(a, b, teleColor[i]);
                            placed = true;
                            int[] c = new int[2] { a[0], a[1] };
                            int[] d = new int[2] { b[0], b[1] };
                            GameManager.teleportLocs.Add(c);
                            GameManager.teleportLocs.Add(d);
                        }
                    }
                }
            }
        }
    bool teleLocsContain(int[] A)
        {
            bool doesContain=false;
          
            foreach(int[] loc in teleportLocs)
            {
                if(loc[0]==A[0] && loc[1]==A[1])
                {
                    doesContain = true;
                }
            }
            return doesContain;
        }
	public static void placePieces() //randomly
	{
		psudoRandomIntArray[0]=ThreadSafeRandom.Next ();
		System.Random squirell = new System.Random (psudoRandomIntArray[0]);
        GameManager gm = (GameManager)scriptThingy.GetComponent(typeof(GameManager));

            if (collectMode)
{float f=-4+(PlayerLoc[1]*1.25f);
float g=3-(PlayerLoc[0]*1.25f);
               
Instantiate(gm.collectionStar,new Vector3(f, g, 0), Quaternion.identity);}

            for (int i=0;i<2;i++)
			{
				PlayerLoc[i]=0;
				KittenLoc[i]=0;
				SidekickLoc[i]=0;
				GoalLoc[i]=0;
			}
            do
            {
                PlayerLoc[0] = squirell.Next(0, 5);
                PlayerLoc[1] = squirell.Next(0, 7);
            }
            while (gm.teleLocsContain(PlayerLoc));
		do
			{
		KittenLoc[0]=squirell.Next (0,5);
		KittenLoc[1]=squirell.Next (0,7);
			}
					while(KittenLoc[0]==PlayerLoc[0] && PlayerLoc[1]==KittenLoc[1] || gm.teleLocsContain(KittenLoc));

	do{
		SidekickLoc[0]=squirell.Next (0,5);
		SidekickLoc[1]=squirell.Next (0,7);
			}
			while(((SidekickLoc[0]==PlayerLoc[0] && PlayerLoc[1]==SidekickLoc[1]) || (SidekickLoc[0]==KittenLoc[0] && KittenLoc[1]==SidekickLoc[1])) || gm.teleLocsContain(SidekickLoc));

		do{
		if(numberOfWins<2)
		{
		GoalLoc[0]=squirell.Next (0,5);
		GoalLoc[1]=squirell.Next (0,7);
		}
		else
		{
			int a, b;
			do
			{
				 a=squirell.Next (0,5);
				 b=squirell.Next (0,7);
			}
			while(!Board[a,b].name.Equals("EmptyTilePrefab(Clone)"));

			GoalLoc[0]=a;
			GoalLoc[1]=b;
		}
			}
			while(GoalLoc[0]==PlayerLoc[0] && PlayerLoc[1]==GoalLoc[1]);

		actuallyPlacePieces();
		
	}

	public static void actuallyPlacePieces()
	{
			playerX=-4+ PlayerLoc[1]*1.25f;
		playerY=3-PlayerLoc[0]*1.25f;
		playerZ=0;
		goalX=-4+ GoalLoc[1]*1.25f;
		goalY=3-GoalLoc[0]*1.25f;
		goalZ=0;
		kittenX=-4+ KittenLoc[1]*1.25f;
		kittenY=3-KittenLoc[0]*1.25f;
		kittenZ=0;
		sidekickX=-4+ SidekickLoc[1]*1.25f;
		sidekickY=3-SidekickLoc[0]*1.25f;
		sidekickZ=0;
			PlayerStart = new Vector3(playerX,playerY, playerZ);
			KittenStart = new Vector3(kittenX, kittenY, kittenZ);
			HelperStart = new Vector3(sidekickX,sidekickY,sidekickZ);
			GoalStart=new Vector3(goalX,goalY,0);

		if (Player != null) 
			{
				Destroy(Player);
				Destroy(Goal);
				Destroy (Kitten);
				//Kitten is a variable name for a unity engine object that should no longer be in the scene not an actual kitten.
				//I would never destroy an actual kitten, I love them, feed them, pet them, and play with them.
				Destroy (Sidekick);
						}
		Player=Instantiate(Star, PlayerStart, Quaternion.identity);
		Goal=Instantiate(Exit, new Vector3(goalX, goalY, goalZ), Quaternion.identity);
		Kitten=Instantiate(Kitty, KittenStart, Quaternion.identity);
		Sidekick=Instantiate(Helper, HelperStart, Quaternion.identity);

	}

	public void replayLastLevel()
	{
			Player=Instantiate(Star);
			Goal=Instantiate(Exit);
			Kitten=Instantiate(Kitty);
			Sidekick=Instantiate(Helper);
		GameObject player = GameObject.Find ("StarPlayerPrefab(Clone)");
				GameObject kitty = GameObject.Find ("KittyPrefab(Clone)");
				GameObject helper = GameObject.Find ("HelperPrefab(Clone)");

				GameObject goal= GameObject.Find ("RedSquarePrefab(Clone)");

				player.transform.position=GameManager.PlayerStart;
				kitty.transform.position=GameManager.KittenStart;
				helper.transform.position=GameManager.HelperStart;
				goal.transform.position=GameManager.GoalStart;
				GameManager.stopShowingMovement=true;


				for(int r=0; r<6; r++)
		{
			for(int c=0;c<8;c++)
			{
				posZ=0;
				posX=-4+(c*1.25f);
				posY=3-(r*1.25f);
				qX=0;
				qY=0;
				qZ=0;
				PsudoRandomPiece=RecordPieceArray[r,c];
					//Debug.Log(RecordPieceArray[r,c].name);
					Board[r,c]=Instantiate(PsudoRandomPiece, new Vector3(posX, posY, 0), getRotation());
						if(collectMode)
					Instantiate(collectionStar,new Vector3(posX, posY, 0), Quaternion.identity);
					
				}
					
		}

                for(int i=0;i<GameManager.teleportLocs.Count;i++)
            {
                CustomLevels cl = (CustomLevels)GetComponent(typeof(CustomLevels));
                cl.placeTeleporter(GameManager.teleportLocs[i], GameManager.teleportLocs[i + 1], teleColor[i / 2]);
                i++;
            }
	}

	public static bool containsPiece(int r, int c)
	{
		foreach (int[] i in PieceLocations) 
		{
			if(i[0]==r && i[1]==c)
			{
				return true;
			}
				}
		return false;
		}

	
	public static void setPsudoRandomPiece(int r, int c)
	{
		psudoRandomIntArray[1]=ThreadSafeRandom.Next();
				System.Random eroasgh = new System.Random (psudoRandomIntArray[1]);
			int tempR;
				switch (r) {
				case 0:
						switch (c) {
						case 0:
								PsudoRandomPiece=UpperLeftCorner;
								break;
						case 7:
						PsudoRandomPiece=UpperRightCorner;
								break;
						default:
					tempR=eroasgh.Next (0, TopWalls.Count);
					PsudoRandomPiece=TopWalls[tempR];
					break;
						}
						break;
				case 5:
						switch (c) {
						case 0:
								PsudoRandomPiece=LowerLeftCorner;
								break;
						case 7:
					PsudoRandomPiece =LowerRightCorner;
								break;
						default:
					tempR=eroasgh.Next (0, BotWalls.Count);
					PsudoRandomPiece=BotWalls[tempR];
					break;
						}
						break;
				default:
				switch (c) {
				case 0:
					tempR=eroasgh.Next (0, LeftWalls.Count);
					PsudoRandomPiece=LeftWalls[tempR];
					break;
				case 7:
					tempR=eroasgh.Next (0,RightWalls.Count);
					PsudoRandomPiece=RightWalls[tempR];
					break;
				default:
					tempR=eroasgh.Next (0,PsudoRandomList.Count);
					PsudoRandomPiece=PsudoRandomList[tempR];
					break;
				}
				
				break;
				
			}
		}

	public Quaternion getRotation()
	{		
			Quaternion URq = Quaternion.LookRotation(new Vector4 (qX, qY, 270f,0), Vector3.right); //also top wall
			Quaternion LLq =  Quaternion.LookRotation(new Vector4 (qX, qY, 180f,0), Vector3.left); //also bottom wall
			Quaternion LRq =  Quaternion.LookRotation(new Vector4 (qX, qY, 180f,0), Vector3.down); //also right wall
		switch (PsudoRandomPiece.name)
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
	
	// Update is called once per frame
	void Update () {
	PieceLocations.Clear ();
	PieceLocations.Add (PlayerLoc);
	PieceLocations.Add (KittenLoc);
	PieceLocations.Add (SidekickLoc);

	TimeUsed=Time.timeSinceLevelLoad;


				if(madeLevel && !ShowMovementScript.areThereCircles && PieceLocations[0][0]==GoalLoc[0] && PieceLocations[0][1]==GoalLoc[1])
				{
					replay=false;
					Application.LoadLevel(1);
					numberOfWins++;
				}

			if(withCountDown && (GUIstuff.seconds<=0f || movesPast<0))
			   {
				Application.LoadLevel(2);
				}

						
	}
}
}
