using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlockControl : MonoBehaviour {

	int Lines;
	public GameObject LineTextObj;
	Text lineText;
	public GameObject timerObj;
	Text timeText;
	float timeInSec;

	public GameObject blackBlock;
	public GameObject blueBlock;
	public GameObject yellowBlock;
	public GameObject greyBlock;
	public GameObject greenBlock;
	public GameObject purpleBlock;
	public GameObject redBlock;
	public GameObject orangeBlock;

	public GameObject rightArrow;
	public GameObject leftArrow;
	public GameObject downArrow;
	public GameObject rotateClock;
	public GameObject rotateCounter;
	public GameObject rotate180;
	public GameObject hold;

	Tetrimino pieceSix;
	Tetrimino pieceSeven;

	Tetrimino holdPiece=new Tetrimino();

	int[][] collisionVectors=new int[24][];
	GameObject[][] collisionObjects=new GameObject[24][];

	GameObject[] ghostPieces=new GameObject[4];

	bool gameStarted;
	bool callHarddrop;
	bool pieceHeld;

	float timeCounter;


	int SandZcounter;
	int Icounter;
	bool firstSeven;

	//0-3 previews, 4 play, 5 hold 
	Tetrimino[] PieceArray=new Tetrimino[6];

	Tetrimino I=new Tetrimino();
	Tetrimino J=new Tetrimino();
	Tetrimino L=new Tetrimino();
	Tetrimino S=new Tetrimino();
	Tetrimino Z=new Tetrimino();
	Tetrimino T=new Tetrimino();
	Tetrimino O=new Tetrimino();


	GameObject Instantiate(GameObject g, Vector2 v)
	{
		return (GameObject) Instantiate(g, v, Quaternion.identity) as GameObject;
	} 

	class Tetrimino
	{
		public Tetrimino()
		{
			 Intialize();
		}
		public char name;

		public GameObject[] blocks=new GameObject[4];

		public GameObject baseBlock;

		//0=preview 1=entryPoint 2=hold
		public Vector2[][] pieceCriticalLocations=new Vector2[4][];

		public Vector2[][] rotateVectorArrays=new Vector2[4][];

		//0=initial, 1=clockwise, 2=180, 3=counterclockwise
		public int rotationIndex;

		void Intialize()
	{
		for(int i=0; i<4; i++)
		{
			pieceCriticalLocations[i]=new Vector2[4];
			rotateVectorArrays[i]=new Vector2[4];
		}
	}

		
	}

	
	// Use this for initialization
	void Start () {
		timeText=timerObj.GetComponent<Text>();
		lineText=LineTextObj.GetComponent<Text>();
		Lines=0;
		ButtonScript raScript=(ButtonScript) rightArrow.GetComponent(typeof(ButtonScript));
		ButtonScript laScript=(ButtonScript) leftArrow.GetComponent(typeof(ButtonScript));
		ButtonScript daScript=(ButtonScript) downArrow.GetComponent(typeof(ButtonScript));
		ButtonScript rclScript=(ButtonScript) rotateClock.GetComponent(typeof(ButtonScript));
		ButtonScript rccScript=(ButtonScript) rotateCounter.GetComponent(typeof(ButtonScript));
		ButtonScript roScript=(ButtonScript) rotate180.GetComponent(typeof(ButtonScript));
		ButtonScript holdScript=(ButtonScript) hold.GetComponent(typeof(ButtonScript));

		raScript.skillMethod=moveRight;
		raScript.setUpButton();

		laScript.skillMethod=moveLeft;
		laScript.setUpButton();

		rclScript.skillMethod=rotateClockwise;
		rclScript.setUpButton();

		rccScript.skillMethod=rotateCounterClockwise;
		rccScript.setUpButton();

		roScript.skillMethod=rotateDouble;
		roScript.setUpButton();

		daScript.skillMethod=hardDrop;
		daScript.setUpButton();

		holdScript.skillMethod=activateHold;
		holdScript.setUpButton();

		startCollisionVectors();
		setUpTetriminos();
		createWell();

		firstSeven=true;
		pieceHeld=false;

		for(int i=0; i<6;i++)
		{
			PieceArray[i]=new Tetrimino();
		}
		managePieces();
		gameStarted=true;
	
	}


	void setUpTetriminos()
	{
		//I
		I.name='I';
		I.baseBlock=blueBlock;
		I.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(12.35f, 6.5f), new Vector2(12.35f, 5.5f), new Vector2(12.35f, 4.5f), new Vector2(12.35f, 3.5f)};
		I.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(5f,23f), new Vector2(5f, 22f), new Vector2(5f, 21f), new Vector2(5f, 20f)};
		I.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-3.28f, 22.5f), new Vector2(-3.28f, 21.5f), new Vector2(-3.28f, 20.5f), new Vector2(-3.28f, 19.5f)}; 
		I.rotateVectorArrays[0]=new Vector2[4]{new Vector2(1,-1), Vector2.zero, new Vector2(-1,1), new Vector2(-2, 2)};
		I.rotateVectorArrays[1]=new Vector2[4]{new Vector2(-1,1), Vector2.zero, new Vector2(1,-1), new Vector2(2, -2)};
		I.rotateVectorArrays[2]=new Vector2[4]{new Vector2(1,-1), Vector2.zero, new Vector2(-1,1), new Vector2(-2, 2)};
		I.rotateVectorArrays[3]=new Vector2[4]{new Vector2(-1,1), Vector2.zero, new Vector2(1,-1), new Vector2(2, -2)};

		//J
		J.name='J';
		J.baseBlock=greenBlock;
		J.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(12.35f, 6f), new Vector2(12.35f, 5f), new Vector2(12.35f, 4f), new Vector2(11.35f, 4f)};
		J.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(5f,23f), new Vector2(5f, 22f), new Vector2(5f, 21f), new Vector2(4f, 21f)};
		J.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-3.28f, 22f), new Vector2(-3.28f, 21f), new Vector2(-3.28f, 20f), new Vector2(-4.28f, 20f)}; 
		J.rotateVectorArrays[0]=new Vector2[4]{new Vector2(0,-2), new Vector2(-1,-1), new Vector2(-2,0), new Vector2(-1, 1)};
		J.rotateVectorArrays[1]=new Vector2[4]{new Vector2(-2,0),new Vector2(-1,1), new Vector2(0,2), new Vector2(1, 1)};
		J.rotateVectorArrays[2]=new Vector2[4]{new Vector2(0,2),new Vector2(1,1), new Vector2(2,0), new Vector2(1, -1)};
		J.rotateVectorArrays[3]=new Vector2[4]{new Vector2(2,0),new Vector2(1,-1), new Vector2(0,-2), new Vector2(-1, -1)};
		//L
		L.name='L';
		L.baseBlock=purpleBlock;
		L.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(12.35f, 6f), new Vector2(12.35f, 5f), new Vector2(12.35f, 4f), new Vector2(13.35f, 4f)};
		L.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(4f,23f), new Vector2(4f, 22f), new Vector2(4f, 21f), new Vector2(5f, 21f)};
		L.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-3.28f, 22f), new Vector2(-3.28f, 21f), new Vector2(-3.28f, 20f), new Vector2(-2.28f, 20f)}; 
		L.rotateVectorArrays[0]=new Vector2[4]{new Vector2(2,0), new Vector2(1,1), new Vector2(0,2), new Vector2(-1, 1)};
		L.rotateVectorArrays[1]=new Vector2[4]{new Vector2(0,-2),new Vector2(1,-1), new Vector2(2,0), new Vector2(1, 1)};
		L.rotateVectorArrays[2]=new Vector2[4]{new Vector2(-2,0),new Vector2(-1,-1), new Vector2(0,-2), new Vector2(1, -1)};
		L.rotateVectorArrays[3]=new Vector2[4]{new Vector2(0,2),new Vector2(-1,1), new Vector2(-2,0), new Vector2(-1, -1)};
		
		//S
		S.name='S';
		S.baseBlock=redBlock;
		S.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(11.85f, 6f), new Vector2(11.85f, 5f), new Vector2(12.85f, 5f), new Vector2(12.85f, 4f)};
		S.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(4f,23f), new Vector2(4f, 22f), new Vector2(5f, 22f), new Vector2(5f, 21f)};
		S.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-3.78f, 22f), new Vector2(-3.78f, 21f), new Vector2(-2.78f, 21f), new Vector2(-2.78f, 20f)}; 
		S.rotateVectorArrays[0]=new Vector2[4]{new Vector2(1,-1), Vector2.zero, new Vector2(-1,-1), new Vector2(-2, 0)};
		S.rotateVectorArrays[1]=new Vector2[4]{new Vector2(-1,1),new  Vector2(0,0), new Vector2(1,1), new Vector2(2, 0)};
		S.rotateVectorArrays[2]=new Vector2[4]{new Vector2(1,-1), Vector2.zero, new Vector2(-1,-1), new Vector2(-2, 0)};
		S.rotateVectorArrays[3]=new Vector2[4]{new Vector2(-1,1),new  Vector2(0,0), new Vector2(1,1), new Vector2(2, 0)};

		//Z
		Z.name='Z';
		Z.baseBlock=orangeBlock;
		Z.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(12.85f, 6f), new Vector2(12.85f, 5f), new Vector2(11.85f, 5f), new Vector2(11.85f, 4f)};
		Z.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(5f,23f), new Vector2(5f, 22f), new Vector2(4f, 22f), new Vector2(4f, 21f)};
		Z.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-2.78f, 22f), new Vector2(-2.78f, 21f), new Vector2(-3.78f, 21f), new Vector2(-3.78f, 20f)}; 
		Z.rotateVectorArrays[0]=new Vector2[4]{new Vector2(0,-2),new  Vector2(-1,-1), new Vector2(0,0), new Vector2(-1, 1)};
		Z.rotateVectorArrays[1]=new Vector2[4]{new Vector2(0,2),new  Vector2(1,1), new Vector2(0,0), new Vector2(1, -1)};
		Z.rotateVectorArrays[2]=new Vector2[4]{new Vector2(0,-2),new  Vector2(-1,-1), new Vector2(0,0), new Vector2(-1, 1)};
		Z.rotateVectorArrays[3]=new Vector2[4]{new Vector2(0,2),new  Vector2(1,1), new Vector2(0,0), new Vector2(1, -1)};

		//T
		T.name='T';
		T.baseBlock=greyBlock;
		T.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(12.35f, 6f), new Vector2(12.35f, 5f), new Vector2(12.35f, 4f), new Vector2(13.35f, 5f)};
		T.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(4f,23f), new Vector2(4f, 22f), new Vector2(4f, 21f), new Vector2(5f, 22f)};
		T.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-3.28f, 22f), new Vector2(-3.28f, 21f), new Vector2(-3.28f, 20f), new Vector2(-2.28f, 21f)}; 
		T.rotateVectorArrays[0]=new Vector2[4]{new Vector2(2,0),  new Vector2(1,1), new Vector2(0,2), Vector2.zero};
		T.rotateVectorArrays[1]=new Vector2[4]{new Vector2(0,-2),new Vector2(1,-1), new Vector2(2,0), new Vector2(0, 0)};
		T.rotateVectorArrays[2]=new Vector2[4]{new Vector2(-2,0),new Vector2(-1,-1), new Vector2(0,-2), new Vector2(0, 0)};
		T.rotateVectorArrays[3]=new Vector2[4]{new Vector2(0,2),new Vector2(-1,1), new Vector2(-2,0), new Vector2(0, 0)};

		//O
		O.name='O';
		O.baseBlock=yellowBlock;
		O.pieceCriticalLocations[0]=new Vector2[4]{new Vector2(12.85f, 5.5f), new Vector2(12.85f, 4.5f), new Vector2(11.85f, 5.5f), new Vector2(11.85f, 4.5f)};
		O.pieceCriticalLocations[1]=new Vector2[4]{new Vector2(5f,23f), new Vector2(5f, 22f), new Vector2(4f, 23f), new Vector2(4f, 22f)};
		O.pieceCriticalLocations[2]=new Vector2[4]{new Vector2(-2.78f, 21.5f), new Vector2(-2.78f, 20.5f), new Vector2(-3.78f, 21.5f), new Vector2(-3.78f, 20.5f)}; 
		O.rotateVectorArrays[0]=new Vector2[4]{ Vector2.zero,  Vector2.zero, Vector2.zero, Vector2.zero};
		O.rotateVectorArrays[1]=new Vector2[4]{ Vector2.zero,  Vector2.zero, Vector2.zero, Vector2.zero};
		O.rotateVectorArrays[2]=new Vector2[4]{ Vector2.zero,  Vector2.zero, Vector2.zero, Vector2.zero};
		O.rotateVectorArrays[3]=new Vector2[4]{ Vector2.zero,  Vector2.zero, Vector2.zero, Vector2.zero};


	}


    void startCollisionVectors()
    {
    	for(int i=0; i<24;i++)
    	{
    		collisionVectors[i]=new int[10]{0,0,0,0,0,0,0,0,0,0};
    		collisionObjects[i]=new GameObject[10];
    	}
 		
    }

    void moveLeft()
    {
    	if(canMoveLeft())
    	{
    	for(int i=0;i<4;i++)
    	{
    		PieceArray[4].blocks[i].transform.position=new Vector2(PieceArray[4].blocks[i].transform.position.x-1,PieceArray[4].blocks[i].transform.position.y);
    	}
    	}
    }

    void moveRight()
    {
    	if(canMoveRight())
    	{
    	for(int i=0;i<4;i++)
    	{
    		PieceArray[4].blocks[i].transform.position=new Vector2(PieceArray[4].blocks[i].transform.position.x+1,PieceArray[4].blocks[i].transform.position.y);
    	}
    	}
    }

    void rotateClockwise()
    {
    	int index=PieceArray[4].rotationIndex % 4;
    	if(canRotateClockwise())
    	{
    	for(int i=0;i<4;i++)
    	{
    		
    		PieceArray[4].blocks[i].transform.position=(Vector2)PieceArray[4].blocks[i].transform.position+PieceArray[4].rotateVectorArrays[index][i];
    	}
    	PieceArray[4].rotationIndex++;

   		 }
    }

    bool canRotateClockwise()
    {
    	int index=PieceArray[4].rotationIndex % 4;
    	for(int i=0;i<4;i++)
    	{
    		Vector2 v=(Vector2)PieceArray[4].blocks[i].transform.position+PieceArray[4].rotateVectorArrays[index][i];
    		if(v.x<0 || v.x>9 || v.y<0 || collisionVectors[(int)v.y][(int)v.x]==1)
    		{
    		return false;
    		}
    	}

    	return true;

    }

    void rotateCounterClockwise()
    {
    	int index=(PieceArray[4].rotationIndex % 4)-1;
    	if(index<0)
    	{
    		index=3;
    	}
    	if(canRotateCounter())
    	{
    	for(int i=0;i<4;i++)
    	{
    		
    		PieceArray[4].blocks[i].transform.position=(Vector2)PieceArray[4].blocks[i].transform.position-PieceArray[4].rotateVectorArrays[index][i];
    	}
    	PieceArray[4].rotationIndex--;
    	if(PieceArray[4].rotationIndex<0)
    	{
    		PieceArray[4].rotationIndex=3;
    	}

   		 }
    }

    bool canRotateCounter()
    {
    	int index=(PieceArray[4].rotationIndex % 4)-1;
    	if(index<0)
    	{
    		index=3;
    	}
    	for(int i=0;i<4;i++)
    	{
    		Vector2 v=(Vector2)PieceArray[4].blocks[i].transform.position-PieceArray[4].rotateVectorArrays[index][i];
    			if(v.x<0 || v.x>9 || v.y<0 || collisionVectors[(int)v.y][(int)v.x]==1)
    		{
    		return false;
    		}
    		
    	}
    	return true;
    }

    void rotateDouble()
    {
    	rotateClockwise();
    	rotateClockwise();
    }

    void activateHold()
    { 
    	if(pieceHeld)
    	{
    		Tetrimino tet4=new Tetrimino();
    		Tetrimino tet5=new Tetrimino();
    	for(int i=0;i<4;i++)
    	{
    		
    		PieceArray[4].blocks[i].transform.position=PieceArray[4].pieceCriticalLocations[2][i];
    		holdPiece.blocks[i].transform.position=holdPiece.pieceCriticalLocations[1][i];
    		tet4.name=PieceArray[4].name;
    		tet4.blocks[i]=PieceArray[4].blocks[i];
    		tet5.blocks[i]=holdPiece.blocks[i];

    		for(int j=0; j<4; j++)
    		{
    		tet4.pieceCriticalLocations[i][j]=PieceArray[4].pieceCriticalLocations[i][j];
    		tet4.rotateVectorArrays[i][j]=PieceArray[4].rotateVectorArrays[i][j];
    		tet5.pieceCriticalLocations[i][j]=holdPiece.pieceCriticalLocations[i][j];
    		tet5.rotateVectorArrays[i][j]=holdPiece.rotateVectorArrays[i][j];
    		}


    	}

    	for(int i=0;i<4;i++)
    	{
    		PieceArray[4].name=tet5.name;
    		PieceArray[4].blocks[i]=tet5.blocks[i];
    		holdPiece.name=tet4.name;
    		holdPiece.blocks[i]=tet4.blocks[i];
    		for(int j=0;j<4;j++)
    		{
    			PieceArray[4].rotateVectorArrays[i][j]=tet5.rotateVectorArrays[i][j];
    			PieceArray[4].pieceCriticalLocations[i][j]=tet5.pieceCriticalLocations[i][j];
    			holdPiece.rotateVectorArrays[i][j]=tet4.rotateVectorArrays[i][j];
    			holdPiece.pieceCriticalLocations[i][j]=tet4.pieceCriticalLocations[i][j];
    		}
    	}

    	PieceArray[4]=tet5;
    	PieceArray[4].rotationIndex=0;
    	holdPiece=tet4;
    	holdPiece.rotationIndex=0;
    	}
    	else
    	{
    		PieceArray[5]=PieceArray[4];
    			for(int i=0;i<4;i++)
    	{
    		PieceArray[5].blocks[i].transform.position=PieceArray[5].pieceCriticalLocations[2][i];
    	}

    	for(int i=0;i<4;i++)
    	{
    		holdPiece.name=PieceArray[5].name;
    		holdPiece.blocks[i]=PieceArray[5].blocks[i];
    		for(int j=0;j<4;j++)
    		{
    		holdPiece.pieceCriticalLocations[i][j]=PieceArray[5].pieceCriticalLocations[i][j];
    		holdPiece.rotateVectorArrays[i][j]=PieceArray[5].rotateVectorArrays[i][j];
    		}
    	}
    	holdPiece.rotationIndex=0;
    	pieceHeld=true;
    	moveUpPreview();
    	managePieces();
    	}
    }

    void hardDrop()
    {
    	callHarddrop=true;
    	timeCounter=.351f;
    }

    void moveUpPreview()
    {
    	for(int i=3; i>=0;i--)
    	{
    		for(int j=0; j<4;j++)
    		{
    			if(PieceArray[i].blocks[j] != null)
    			{

    				PieceArray[i+1].name=PieceArray[i].name;
    			PieceArray[i+1].blocks[j]=PieceArray[i].blocks[j];
    		PieceArray[i+1].blocks[j].transform.position=new Vector2(PieceArray[i+1].blocks[j].transform.position.x, PieceArray[i+1].blocks[j].transform.position.y+5);    		
    		}
    	

    		}
    	}

    	if(PieceArray[4].blocks[0] != null)
    	{
    	switch(PieceArray[4].name)
    	{
    		case 'I':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=I.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=I.rotateVectorArrays[t][j];
					}
					}
    		break;

    		case 'J':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=J.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=J.rotateVectorArrays[t][j];
					}
					}
    		break;

    		case 'L':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=L.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=L.rotateVectorArrays[t][j];
					}
					}
    		break;

    		case 'S':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=S.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=S.rotateVectorArrays[t][j];
					}
					}
    		break;

    		case 'Z':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=Z.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=Z.rotateVectorArrays[t][j];
					}
					}
    		break;

    		case 'T':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=T.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=T.rotateVectorArrays[t][j];
					}
					}
    		break;

    		case 'O':
    			for(int t=0;t<4;t++)
					{
					for(int j=0;j<4;j++)
					{
					PieceArray[4].pieceCriticalLocations[t][j]=O.pieceCriticalLocations[t][j];
					PieceArray[4].rotateVectorArrays[t][j]=O.rotateVectorArrays[t][j];
					}
					}
    		break;

    	}

    	for(int a=0;a<4;a++)
    	{
    		if(PieceArray[4].blocks[a] != null)
    		PieceArray[4].blocks[a].transform.position=PieceArray[4].pieceCriticalLocations[1][a];
    	}

    	PieceArray[4].rotationIndex=0;

    	}
    	}
    

	void managePieces()
	{
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		if(firstSeven)
		{
			List<int> selected=new List<int>{0,1,2,3,4,5,6};

			for(int i=0; i<5; i++)
			{
				bool pieceSelected=false;
			moveUpPreview();

			do
			{
			int r=RNG.Next(7);

			switch(r)
			{
				case 0:
				if(selected.Remove(0))
				{
					PieceArray[0].name='I';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(I.baseBlock, I.pieceCriticalLocations[0][j]);
					}

					pieceSelected=true;

				}
				break;

				case 1:
				if(selected.Remove(1))
				{
					PieceArray[0].name='J';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(J.baseBlock, J.pieceCriticalLocations[0][j]);
					}

					
					pieceSelected=true;

				}

				break;

				case 2:

				if(selected.Remove(2))
				{
					PieceArray[0].name='L';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(L.baseBlock, L.pieceCriticalLocations[0][j]);
					}
						
					pieceSelected=true;

				}
				break;

				case 3:
				if(selected.Remove(3))
				{
					PieceArray[0].name='S';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(S.baseBlock, S.pieceCriticalLocations[0][j]);
					}
						
					pieceSelected=true;

				}
				break;

				case 4:
				if(selected.Remove(4))
				{
					PieceArray[0].name='Z';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(Z.baseBlock, Z.pieceCriticalLocations[0][j]);
					}
						
					pieceSelected=true;

				}
				break;

				case 5:
				if(selected.Remove(5))
				{
					PieceArray[0].name='T';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(T.baseBlock, T.pieceCriticalLocations[0][j]);
					}
						
					pieceSelected=true;

				}
				break;

				case 6:
				if(selected.Remove(6))
				{
					PieceArray[0].name='O';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(O.baseBlock, O.pieceCriticalLocations[0][j]);
					}
						
					pieceSelected=true;

				}
				break;
			}
		}
			while(!pieceSelected);

		}

			firstSeven=false;
		}

		else
		{
			
			int r=RNG.Next(7);

			if(Icounter==13)
			{
				r=0;
			}

			switch(r)
			{
				case 0:
			
					PieceArray[0].name='I';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(I.baseBlock, I.pieceCriticalLocations[0][j]);
					}

					Icounter=0;
				
				break;

				case 1:
			
					PieceArray[0].name='J';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(J.baseBlock, J.pieceCriticalLocations[0][j]);
					}

				Icounter++;
				break;
				case 2:

					PieceArray[0].name='L';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(L.baseBlock, L.pieceCriticalLocations[0][j]);
					}
						
					Icounter++;
				break;

				case 3:
			
					PieceArray[0].name='S';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(S.baseBlock, S.pieceCriticalLocations[0][j]);
					}
						
					Icounter++;
				
				break;

				case 4:
			
					PieceArray[0].name='Z';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(Z.baseBlock, Z.pieceCriticalLocations[0][j]);
					}
						Icounter++;	
				break;

				case 5:
				
					PieceArray[0].name='T';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(T.baseBlock, T.pieceCriticalLocations[0][j]);
					}
						Icounter++;
				break;

				case 6:
			
					PieceArray[0].name='O';
					for(int j=0;j<4;j++)
					{
					PieceArray[0].blocks[j]=Instantiate(O.baseBlock, O.pieceCriticalLocations[0][j]);
					}
						Icounter++;
				break;
			}
			}

			showGhost();
		
		

	}

	void showGhost()
	{
		for(int i=0;i<4;i++)
		{
			if(ghostPieces[i] != null)
			{
				Destroy(ghostPieces[i]);
			}

		}
		float offset=0;

		for(int i=23;i>-1;i--)
		{
			for(int j=0;j<4;j++)
			{
			 if(collisionVectors[i][(int)PieceArray[4].blocks[j].transform.position.x]==1)
			 {
			 	if(offset > PieceArray[4].blocks[j].transform.position.y-i-1 || offset==0)
			 	{
			 		offset=PieceArray[4].blocks[j].transform.position.y-i-1;
			 	}
			 }
			}
		}
		if(offset==0)
		{
	for(int i=0;i<4;i++)
	{
		if(PieceArray[4].blocks[i].transform.position.y<offset || offset==0)
		{
			offset=PieceArray[4].blocks[i].transform.position.y;
		}
	}}
		for(int i=0; i<4;i++)
		{ 
			ghostPieces[i]=Instantiate(PieceArray[4].blocks[i], new Vector2(PieceArray[4].blocks[i].transform.position.x, PieceArray[4].blocks[i].transform.position.y-offset));
			SpriteRenderer sr=ghostPieces[i].GetComponent<SpriteRenderer>();
			sr.color=new Vector4(1,1,1,.6f);
		}
		print(offset);

	}

	void createWell()
	{
		Color bg;
		for(int i=0; i<10; i++)
		{
			
			for(int j=0; j<24; j++)
			{
				if(i % 2 == j % 2)
             {
             	bg=new Vector4(0,0,0,1);

             }
             else
             {
             	bg=new Vector4(.1f,.1f,.1f,1);
             }
				GameObject go=Instantiate(blackBlock, new Vector2(i,j));
				SpriteRenderer sr=go.GetComponent<SpriteRenderer>();
				sr.color=bg;
			}
		}
	}

	bool canMoveDown()
	{
				for(int i=0;i<4;i++)
				{
					if(PieceArray[4].blocks[i].transform.position.y<=0 || collisionVectors[(int)PieceArray[4].blocks[i].transform.position.y-1][(int)PieceArray[4].blocks[i].transform.position.x]==1)
					return false;
				}

				return true;
	}

	bool canMoveLeft()
	{
		for(int i=0;i<4;i++)
				{
					if(PieceArray[4].blocks[i].transform.position.x<=0 || collisionVectors[(int)PieceArray[4].blocks[i].transform.position.y][(int)PieceArray[4].blocks[i].transform.position.x-1]==1)
					return false;
				}

				return true;

	}

	bool canMoveRight()
	{
		for(int i=0;i<4;i++)
				{
					if(PieceArray[4].blocks[i].transform.position.x==9 || collisionVectors[(int)PieceArray[4].blocks[i].transform.position.y][(int)PieceArray[4].blocks[i].transform.position.x+1]==1)
					return false;
				}

				return true;
	}

	void checkforlines()
	{
		for(int i=23; i>-1;i--)
		{
			
				if(collisionVectors[i][0]==1 && collisionVectors[i][1]==1 && collisionVectors[i][2]==1 && collisionVectors[i][3]==1 && collisionVectors[i][4]==1 && collisionVectors[i][5]==1 && collisionVectors[i][6]==1 && collisionVectors[i][7]==1 && collisionVectors[i][8]==1 && collisionVectors[i][9]==1)
					{
						Lines++;
						lineText.text="Level:\n"+Lines.ToString();
						for(int j=0;j<10;j++)
						{
						collisionVectors[i][j]=0;
						Destroy(collisionObjects[i][j]);
						}

						for(int k=i+1; k<24;k++)
						{
							for(int l=0;l<10;l++)
							{
								if(collisionVectors[k][l]==1)
								{
								collisionObjects[k][l].transform.position=(Vector2)collisionObjects[k][l].transform.position+new Vector2(0,-1);
								collisionVectors[k][l]=0;
								collisionVectors[k-1][l]=1;
								collisionObjects[k-1][l]=collisionObjects[k][l];
								collisionObjects[k][l]=null;

								}
							}
						}
				    }
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(callHarddrop)
		{
			hardDrop();
		}

		if(Lines<40)
		{
			timeInSec =Time.time;
			timeText.text=timeInSec.ToString();
		}

		if(canMoveDown() && gameStarted)
		{
			showGhost();
			timeCounter+=Time.deltaTime;
			if(timeCounter>=.350)
			{
				for(int i=0;i<4;i++)
				{
				PieceArray[4].blocks[i].transform.position=(Vector2)PieceArray[4].blocks[i].transform.position+new Vector2(0,-1);
				}
				timeCounter=0;


			}


		}
		else
		{
			callHarddrop=false;
			timeCounter+=Time.deltaTime;
			if(timeCounter>=.350)
			{
			for(int i=0;i<4;i++)
			{
				collisionVectors[(int)PieceArray[4].blocks[i].transform.position.y][(int)PieceArray[4].blocks[i].transform.position.x]=1;
				collisionObjects[(int)PieceArray[4].blocks[i].transform.position.y][(int)PieceArray[4].blocks[i].transform.position.x]=PieceArray[4].blocks[i];
			}
			checkforlines();
			moveUpPreview();
			managePieces();
			timeCounter=0;
			}
		}
	
	}
}
