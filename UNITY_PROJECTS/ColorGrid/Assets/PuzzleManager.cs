using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour {

public GameObject Tile;
public GameObject sTile;
public GameObject apple;
public GameObject arrow;
public GameObject[,] Board;
public int rows=10;
public int cols=10;
public int numberOfSnakes=2;
List<Color> LColors=new List<Color>{Color.blue, Color.red, Color.magenta, Color.cyan, Color.green, Color.yellow};
public static List<Snake> ListOfSnakes=new List<Snake>{};
bool moveSnakes;

public enum Direction{up, down, left, right};
public class Snake
{
	public Color color;
	public List<GameObject> Parts;
	public List<GameObject> Arrows;
	public bool onBoard;
	public Vector2 start;
	public GameObject end;
	public Direction curDirection;
}

	// Use this for initialization
	void Start () { 	
		Board=new GameObject[rows, cols];
		makeGrid();
		moveSnakes=true;
	}

	void makeGrid()
	{
		for(int i=0;i<rows;i++)
		{
			for(int j=0;j<cols;j++)
			{
				Board[i,j]=Instantiate(Tile, new Vector2(i,j),Quaternion.identity) as GameObject;
			}
		}

			for(int i=0;i<cols; i++)
			{Board[0,i].GetComponent<SpriteRenderer>().color=Color.black;}
			for(int i=0; i<rows; i++)
			{Board[i,0].GetComponent<SpriteRenderer>().color=Color.black;}
			for(int i=0;i<cols; i++)
			{Board[rows-1,i].GetComponent<SpriteRenderer>().color=Color.black;}
			for(int i=0; i<rows; i++)
			{Board[i,cols-1].GetComponent<SpriteRenderer>().color=Color.black;}

			
			
			//Camera.main.transform.position=new Vector3(rows/2f,cols/2f,-10f);
			//Camera.main.orthographic=true;
			//Camera.main.orthographicSize=.06f*rows*cols;
		//	Camera.main.rect=new Rect(0,0,rows/10f,cols/10f);

	}


	IEnumerator snakeUpdate()
	{
		yield return new WaitForSeconds(1);
		//create new snake if not enough
		if(ListOfSnakes.Count<numberOfSnakes)
		{
			System.Random trip=new System.Random(ThreadSafeRandom.Next());
			Snake S1=new Snake();
				int a=0;
			int b=0;
			int x=0;
			int y=0;
			int d=trip.Next(4);
			switch(d)
			{
				case 0: 
				 a=0;
				 b=trip.Next(cols);
				 x=rows-1;
				 y=trip.Next(cols);
				 S1.curDirection=Direction.right;

					break;
				case 1: 
				a=rows-1;
				 b=trip.Next(cols);
				 x=0;
				 y=trip.Next(cols);
				 S1.curDirection=Direction.left;
				break;
				case 2: 
				b=0;
				a=trip.Next(rows);
				x=trip.Next(rows);
				y=cols-1;
				S1.curDirection=Direction.up;
				break;
				case 3:
				b=cols-1;
				a=trip.Next(rows);
				x=trip.Next(rows);
				y=0; 
				S1.curDirection=Direction.down;
				break;
				default: break;
			}
		
			int c=trip.Next(LColors.Count);
			arrowScript _arscript;
			S1.start=new Vector2(a,b);
			S1.color=LColors[c];
			LColors.Remove(LColors[c]);
			S1.Parts=new List<GameObject>{};
			S1.Arrows=new List<GameObject>{};
			S1.Parts.Add(Instantiate(sTile, S1.start,Quaternion.identity)as GameObject);
			S1.end=Instantiate(apple, new Vector2(x,y),Quaternion.identity) as GameObject;
			S1.Arrows.Add(Instantiate(arrow, new Vector2(a+.5f,b), Quaternion.identity) as GameObject);
			_arscript=(arrowScript)S1.Arrows[0].GetComponent(typeof(arrowScript));
			_arscript.dir=Direction.right;
			S1.Arrows.Add(Instantiate(arrow, new Vector2(a-.5f,b), Quaternion.identity) as GameObject);
			S1.Arrows[1].transform.localEulerAngles=new Vector3(0,0,180f);

			_arscript=(arrowScript)S1.Arrows[1].GetComponent(typeof(arrowScript));
			_arscript.dir=Direction.left;

			S1.Arrows.Add(Instantiate(arrow, new Vector2(a,b+.5f), Quaternion.identity) as GameObject);
			S1.Arrows[2].transform.localEulerAngles=new Vector3(0,0,90f);
			_arscript=(arrowScript)S1.Arrows[2].GetComponent(typeof(arrowScript));
			_arscript.dir=Direction.up;

			S1.Arrows.Add(Instantiate(arrow, new Vector2(a,b-.5f), Quaternion.identity) as GameObject);
			S1.Arrows[3].transform.localEulerAngles=new Vector3(0,0,-90f);
			_arscript=(arrowScript)S1.Arrows[3].GetComponent(typeof(arrowScript));
			_arscript.dir=Direction.down;

			for(int i=0; i<4;i++)
			{	
				_arscript=(arrowScript)S1.Arrows[i].GetComponent(typeof(arrowScript));
				_arscript.snakey=S1;
				S1.Arrows[i].GetComponent<SpriteRenderer>().color=S1.color;
			}

			S1.end.GetComponent<SpriteRenderer>().color=S1.color;
			
			S1.Parts[0].GetComponent<SpriteRenderer>().color=S1.color;
			
			ListOfSnakes.Add(S1);		
		}
	
			int sCount=ListOfSnakes.Count;
			for(int j=0;j<sCount;j++)
			{
				ListOfSnakes[j].Parts.Add(Instantiate(ListOfSnakes[j].Parts[0],ListOfSnakes[j].Parts[0].transform.position, Quaternion.identity)as GameObject);
					foreach(GameObject ar in ListOfSnakes[j].Arrows)
					{
						ar.transform.parent=ListOfSnakes[j].Parts[0].transform;
					}
				switch((int)ListOfSnakes[j].curDirection)
				{

					case 0: //up
					ListOfSnakes[j].Parts[0].transform.position=new Vector2(ListOfSnakes[j].Parts[0].transform.position.x, ListOfSnakes[j].Parts[0].transform.position.y+1f);
					break;
					case 1: //down
					ListOfSnakes[j].Parts[0].transform.position=new Vector2(ListOfSnakes[j].Parts[0].transform.position.x, ListOfSnakes[j].Parts[0].transform.position.y-1f);
					break;
					case 2: //left
					ListOfSnakes[j].Parts[0].transform.position=new Vector2(ListOfSnakes[j].Parts[0].transform.position.x-1f, ListOfSnakes[j].Parts[0].transform.position.y);
					break;
					case 3: //right
					ListOfSnakes[j].Parts[0].transform.position=new Vector2(ListOfSnakes[j].Parts[0].transform.position.x+1f, ListOfSnakes[j].Parts[0].transform.position.y);
					break;

				}

				if(ListOfSnakes[j].Parts[0].transform.position.x==ListOfSnakes[j].end.transform.position.x && ListOfSnakes[j].Parts[0].transform.position.y==ListOfSnakes[j].end.transform.position.y)
				{
					int count=ListOfSnakes[j].Parts.Count;
					for(int i=0;i<count;i++)
					{
						Destroy(ListOfSnakes[j].Parts[i]);
					}
					LColors.Add(ListOfSnakes[j].color);
					Destroy(ListOfSnakes[j].end);
					ListOfSnakes.Remove(ListOfSnakes[j]);
					sCount--;
					j--;
					
				}
				else
				{ListOfSnakes[j].Parts[0].transform.DetachChildren();}
			}

			moveSnakes=true;
	}

	void Update()
	{
		if(moveSnakes)
		{
			StartCoroutine(snakeUpdate());
			moveSnakes=false;
		}
	}
	
}
