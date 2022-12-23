using UnityEngine;
using System.Collections.Generic;

public class MovementScript : MonoBehaviour {

	public static List<GameObject> MoveList=new List<GameObject>{};
	public static List<int[]> OccupiedTiles=new List<int[]>{};
	static bool tileAdded;
	static  List<GameObject> DragonList=new List<GameObject>{};
	// Use this for initialization
	void Start () {
	}

//6 cases for 6 edges given boardarray[a][b] the distance d
	public static void findOrthogonalMovement(int a, int b, int d)
	{

	//upleft:
		for (int i=1; i<=d; i++)
		{
			if (a <= 5)
			{
				if (a - i < 0 || b - i < 0) { goto upright; }
				else
				MovementChecker(GameManager.BoardArrays[a - i][b - i]);
				if(!tileAdded){break;}
			}
			else if (a - i < 5)
			{
				if (a - i < 0 || (b - i + a - 5) < 0) { goto upright; }
				else
				MovementChecker(GameManager.BoardArrays[a - i][b - i + a - 5]);
				if(!tileAdded){break;}
			}
			else
			{
				//below mid can move upleft
				MovementChecker(GameManager.BoardArrays[a - i][b]);
				if(!tileAdded){break;}
			}
		}
	upright:
		for (int i=1; i<=d; i++)
		{
			if (a <= 5)
			{
				if (a - i < 0|| b>=GameManager.BoardArrays[a - i].Length) { goto left; }
				else
				MovementChecker(GameManager.BoardArrays[a - i][b]);
				if(!tileAdded){break;}
			}
			else if (a - i < 5)
			{
				if (a - i < 0 || b + a - 5 >= GameManager.BoardArrays[a - i].Length) { goto left; }
				else
				MovementChecker(GameManager.BoardArrays[a - i][b + a - 5]);
				if(!tileAdded){break;}
			}
			else
			{
				if (a - i < 0 || b + i >= GameManager.BoardArrays[a - i].Length) { goto left; }
				else
				MovementChecker(GameManager.BoardArrays[a - i][b + i]);
				if(!tileAdded){break;}
			}
		}
	left:
		for (int i=1; i<=d; i++)
		{
			if (b - i < 0) { goto right; }
			else
			MovementChecker(GameManager.BoardArrays[a][b - i]);
			if(!tileAdded){break;}
		}
	right:
		for (int i=1; i<=d; i++)
		{
			if (b + i >= GameManager.BoardArrays[a].Length) { goto downleft; }
			else
			MovementChecker(GameManager.BoardArrays[a][b + i]);
			if(!tileAdded){break;}
		}
	downleft:
		for (int i=1; i<=d; i++)
		{
			if (a >= 5)
			{
				if (a + i >= GameManager.BoardArrays.Length || b - i < 0) {goto downright; }
				else
				MovementChecker(GameManager.BoardArrays[a + i][b - i]);
				if(!tileAdded){break;}
			}
			else if (a + i > 5)
			{
				if (a + i >= GameManager.BoardArrays.Length || (b - i + 5 - a) < 0) { goto downright; }
				else
				MovementChecker(GameManager.BoardArrays[a + i][b - i + 5 - a]);
				if(!tileAdded){break;}
			}
			
			else
			{
				if (a + i >= GameManager.BoardArrays.Length || b>=GameManager.BoardArrays[a + i].Length) { goto downright; }
				else
				MovementChecker(GameManager.BoardArrays[a + i][b]);
				if(!tileAdded){break;}
			}
		}
		
	downright:
		for (int i=1; i<=d; i++)
		{
			if (a >= 5)
			{
				if (a + i >= GameManager.BoardArrays.Length || b>=GameManager.BoardArrays[a + i].Length) { goto done; }
				else
				MovementChecker(GameManager.BoardArrays[a + i][b]);
				if(!tileAdded){break;}
			}
			else if (a + i > 5)
			{
				if (a + i >= GameManager.BoardArrays.Length || (b + 5 - a) >= GameManager.BoardArrays[a + i].Length) { goto done;  }
				else
				MovementChecker(GameManager.BoardArrays[a + i][b + 5 - a]);
				if(!tileAdded){break;}
			}
			
			else
			{
				if (a + i >= GameManager.BoardArrays.Length || (b + i) >= GameManager.BoardArrays[a + i].Length) { goto done;}
				else
				MovementChecker(GameManager.BoardArrays[a + i][b + i]);
				if(!tileAdded){break;}
			}
		}

	done:;
		
	}

	public static void findContinuousMovement(int a, int b, int d)
	{
		for(int i=1;i<=d;i++)
		{
			if(DragonList.Count==0)
			{findOrthogonalMovement(a,b,1);
				}
			else if(GameManager.SelectedPiece.name.Equals("Dragon") ||GameManager.SelectedPiece.name.Equals("Dragon2"))
			{
				int count=DragonList.Count;
				for(int z=0;z<count;z++)
				{
			
				TileSelect tSelect=(TileSelect)DragonList[z].GetComponent(typeof(TileSelect));
					a=tSelect.rank;
					b=tSelect.file;
				
					findOrthogonalMovement(a,b,1);
					
				}
			}

				else
				{
					int count=MoveList.Count;
				for(int z=0;z<count;z++)
				{
			
				TileSelect tSelect=(TileSelect)MoveList[z].GetComponent(typeof(TileSelect));
				a=tSelect.rank;
					b=tSelect.file;
					findOrthogonalMovement(a,b,1);
				}
				}


			}
			DragonList.Clear();
		}






	//6 cases for 6 edges given boardarray[a][b] the distance d
	public static void findDiagonalMovement(int a, int b, int d)
	{
	//upleft:
		for (int i=1; i<=d; i++)
		{
			if(a<=5)
			{
				if(a-i<0 || b -2*i<0){goto downleft;}
				else
				MovementChecker(GameManager.BoardArrays[a-i][b-(2*i)]);
				if(!tileAdded){break;}
			}
			else if(a-i<5)
			{
				if(a-i<0 || b-(a-5)-(2*(i-(a-5)))<0){goto downleft;}
				else
				MovementChecker(GameManager.BoardArrays[a-i][b-(a-5)-(2*(i-(a-5)))]);
				if(!tileAdded){break;}
			}
			else
			{
				if(a-i<0 || b-i<0){goto downleft;}
				else
				MovementChecker(GameManager.BoardArrays[a-i][b-i]);
				if(!tileAdded){break;}
			}
		}
	downleft:
		for (int i=1; i<=d; i++) 
		{
			if(a>=5)
			{
				if(a+i>=GameManager.BoardArrays.Length || b-2*i<0){goto upright;}
				else
				MovementChecker(GameManager.BoardArrays[a+i][b-(2*i)]);
				if(!tileAdded){break;}
			}
			else if(a+i>5)
			{if(a+i>=GameManager.BoardArrays.Length || b-(5-a)-(2*(i-(5-a)))<0){goto upright;}
				else
				MovementChecker(GameManager.BoardArrays[a+i][b-(5-a)-(2*(i-(5-a)))]);
			if(!tileAdded){break;}}
			else
			{if(a+i>=GameManager.BoardArrays.Length || b-i<0){goto upright;}
				else
				MovementChecker(GameManager.BoardArrays[a+i][b-i]);
			if(!tileAdded){break;}}
				}
	upright:
		for (int i=1; i<=d; i++)
		{
			if(a<=5)
			{
				if(a-i<0 || b+i>=GameManager.BoardArrays[a-i].Length){goto downright;}
				else
				MovementChecker(GameManager.BoardArrays[a-i][b+i]);
				if(!tileAdded){break;}
			}
			else if(a-i<5)
			{
				if(a-i<0 || b+(2*(a-5))+((i-(a-5)))>=GameManager.BoardArrays[a-i].Length){goto downright;}
				else
				MovementChecker(GameManager.BoardArrays[a-i][b+(2*(a-5))+((i-(a-5)))]);
				if(!tileAdded){break;} 
			}
			else
			{
				if(a-i<0 || b+2*i>=GameManager.BoardArrays[a-i].Length){goto downright;}
				else
				MovementChecker(GameManager.BoardArrays[a-i][b+(2*i)]); 
				if(!tileAdded){break;}
			}
		}
	downright:
		for (int i=1; i<=d; i++) 
		{
			if(a>=5)
			{
				if(a+i>=GameManager.BoardArrays.Length || b+i>=GameManager.BoardArrays[a+i].Length){goto up;}
				else
				MovementChecker(GameManager.BoardArrays[a+i][b+i]);
				if(!tileAdded){break;}
			}
			else if(a+i>5)
			{if(a+i>=GameManager.BoardArrays.Length ||b+(2*(5-a))+(i-(5-a))>=GameManager.BoardArrays[a+i].Length){goto up;}
				else
				MovementChecker(GameManager.BoardArrays[a+i][b+(2*(5-a))+(i-(5-a))]);
			if(!tileAdded){break;}}
			else
			{if(a+i>=GameManager.BoardArrays.Length || b+2*i>=GameManager.BoardArrays[a+i].Length){goto up;}
				else
				MovementChecker(GameManager.BoardArrays[a+i][b+(2*i)]);
			if(!tileAdded){break;}}
		}
	up:

		for (int i=1; i<=d; i++)
		{
			if(a<=5)
			{
				if(a-(2*i)<0 || b-i<0 || b-i>=GameManager.BoardArrays[a-(2*i)].Length){goto down;}
				else
				MovementChecker(GameManager.BoardArrays[a-(2*i)][b-i]);
				if(!tileAdded){break;}
			}
			else if(a-(2*i)<5)
			{
				if(a%2!=0)
				{
				int y=b+(a-5)-(5-(a-2*i));
				if(a-(2*i)<0 || y<0 || y>=GameManager.BoardArrays[a-(2*i)].Length){goto down;}
				else
				MovementChecker(GameManager.BoardArrays[a-2*i][y]);
				if(!tileAdded){break;}
				}
				else
				{
					int y=b+(a-6)-(5-(a-2*i));
						if(a==6)
							y+=i;
					if(a-(2*i)<0 || y<0 || y>=GameManager.BoardArrays[a-(2*i)].Length){goto down;}
					else
						MovementChecker(GameManager.BoardArrays[a-2*i][y]);
						if(!tileAdded){break;}
				}

			}
			else
			{
				//always able to move diag up if below mid and not passing mid
				MovementChecker(GameManager.BoardArrays[a-2*i][b+i]);
				if(!tileAdded){break;}
			}
		}

	down:
		for (int i=1; i<=d; i++) 
		{
			if(a>=5)
			{
				if(a+2*i>=GameManager.BoardArrays.Length || b-i<0 || b-i>=GameManager.BoardArrays[a+2*i].Length){goto done;}
				else {
				MovementChecker(GameManager.BoardArrays[a+2*i][b-i]);
				if(!tileAdded){break;}}
				
			}
			else if(a+2*i>5)
			{
				if(a%2!=0)
				{
				int y=b-(5-a)+(2*i-(5-a));
			if(a+2*i>=GameManager.BoardArrays.Length ||y<0 || y>=GameManager.BoardArrays[a+2*i].Length){goto done;}
				else
				MovementChecker(GameManager.BoardArrays[a+2*i][y]);
				if(!tileAdded){break;}
				}
				//else if
				else
				{
					int y=b-i+(5-a); //+1 for a=4, +3 for a=2 
					if(a+2*i>=GameManager.BoardArrays.Length ||y<0 || y>=GameManager.BoardArrays[a+2*i].Length){goto done;}
					else
						MovementChecker(GameManager.BoardArrays[a+2*i][y]);
						if(!tileAdded){break;}
				}
			}
			else
			{//should always resolve
				MovementChecker(GameManager.BoardArrays[a+2*i][b+i]);
			if(!tileAdded){break;}}
		}

	done:;
	}

static void MovementChecker(GameObject g)
{
	int rank;
	int file;
TileSelect tSelect=(TileSelect)g.GetComponent(typeof(TileSelect));
rank=tSelect.rank;
file=tSelect.file;
bool clear=true;
foreach(int[] ia in OccupiedTiles)
{
	if(ia[0]==rank && ia[1]==file)
	{
		clear=false;
		CaptureScript.CaptureChecker(ia[0],ia[1]);
	}
}
if(clear)
{MoveList.Add(g);
	DragonList.Add(g);
	tileAdded=true;
	}
else if(GameManager.SelectedPiece.name.Equals("Dragon") ||GameManager.SelectedPiece.name.Equals("Dragon2"))
{
	clear=true;
	DragonList.Add(g);
	tileAdded=true;
}
else{tileAdded=false;}
}
	
	// Update is called once per frame
	void Update () {
	
	}
}
