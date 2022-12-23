using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	public GameObject tile;
	public GameObject Button;
	public GameObject[][] Board=new GameObject[9][];
	int[][] Values=new int[9][];

	public GameObject card;

	public GameObject player;

	public GameObject arrow;

	public int X;
	public int Y;

	public GameObject scoreTextObj;
	Text ScoreText;
	public int Score;
	public int numberOfmoves;

	// Use this for initialization
	void Start () {
		X=0;
		Y=0;
		ButtonScript moveScript=(ButtonScript)Button.GetComponent(typeof(ButtonScript));
		moveScript.skillMethod=Move;
		moveScript.setUpButton();
		ScoreText=scoreTextObj.GetComponent<Text>();
		ScoreText.text="Moves:\n0"+"\nScore:\n"+Score.ToString();
for(int i=0; i<9;i++)
{
	Board[i]=new GameObject[9];
	Values[i]=new int[9];

}
		createBoard();
	
	}

	void Move()
	{

		if(player.transform.position.x+X>=0 && player.transform.position.x+X<9 && player.transform.position.y+Y>=0 && player.transform.position.y+Y<9)
		{
			SpriteRenderer sr=Board[(int)player.transform.position.x+X][(int)player.transform.position.y+Y].GetComponent<SpriteRenderer>();
	 	sr.color=new Vector4(1,1,1,1);
		numberOfmoves++;
		player.transform.position=(Vector2)player.transform.position+new Vector2(X,Y);

		Score += Values[(int)player.transform.position.x][(int)player.transform.position.y]*(Mathf.Abs(X)+Mathf.Abs(Y));

		Values[(int)player.transform.position.x][(int)player.transform.position.y]=0;
		Text number=Board[(int)player.transform.position.x][(int)player.transform.position.y].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
		number.text="";
		ScoreText.text="Moves:\n"+numberOfmoves.ToString()+"\nScore:\n"+Score.ToString();
		CardScript CS=(CardScript)card.GetComponent(typeof(CardScript));
		CS.drawCard();
		arrow.transform.position=(Vector2)arrow.transform.position+new Vector2(0,20);
		}
	}

	void createBoard()
	{
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		for(int i=0; i<9;i++)
		{
			for(int j=0; j<9;j++)
			{
			Board[i][j]=(GameObject) Instantiate(tile, new Vector2(i,j), Quaternion.identity) as GameObject;
			Text number=Board[i][j].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
			int r=RNG.Next(10, 100);
			Values[i][j]=r;
			number.text=r.ToString();
			}

		}

		int a=RNG.Next(0,9);
		int b=RNG.Next(0,9);
		player.transform.position=new Vector2(a,b);
	}

	void dealCards()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
