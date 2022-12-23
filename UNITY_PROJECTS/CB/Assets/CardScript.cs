using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardScript : MonoBehaviour {

	GameObject control;

	GameControl GC;

	int x;

	int y;

	Text cardX;
	Text cardY;

	void OnMouseDown()
	{
		SpriteRenderer sr;
		if(GC.player.transform.position.x+GC.X>=0 && GC.player.transform.position.x+GC.X<9 && GC.player.transform.position.y+GC.Y>=0 && GC.player.transform.position.y+GC.Y<9)
		{
		sr=GC.Board[(int)GC.player.transform.position.x+GC.X][(int)GC.player.transform.position.y+GC.Y].GetComponent<SpriteRenderer>();
	 	sr.color=new Vector4(1,1,1,1);
	 	}
		if(GC.player.transform.position.x+x>=0 && GC.player.transform.position.x+x<9 && GC.player.transform.position.y+y>=0 && GC.player.transform.position.y+y<9)
		{
	 	
	 	sr=GC.Board[(int)GC.player.transform.position.x+x][(int)GC.player.transform.position.y+y].GetComponent<SpriteRenderer>();
	 	sr.color=new Vector4(.2f,.35f,.2f,1);
		}

	  GC.card=gameObject;
      GC.arrow.transform.position=new Vector2(GC.arrow.transform.position.x, transform.position.y);
      GC.X=x;
      GC.Y=y;

	}

	// Use this for initialization
	void Start () {

		drawCard();
		control=GameObject.Find("Control Object");
		GC=(GameControl)control.GetComponent(typeof(GameControl));
	}


	public void drawCard()
	{
		x=0;
		y=0;
		
		while(x == 0 && y== 0)
		{
		x=randInt();
		y=randInt();
		}
		cardX=transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
		cardY=transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
		cardX.text="X: "+x.ToString();
		cardY.text="Y: "+y.ToString();
	}
	int randInt()
	{
			System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		return RNG.Next(-3,3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
