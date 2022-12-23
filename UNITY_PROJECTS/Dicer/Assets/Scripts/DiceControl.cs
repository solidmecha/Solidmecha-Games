using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DiceControl : MonoBehaviour {

	public GameObject[] dice=new GameObject[6];

	public List<DiceValue> ScoringList=new List<DiceValue>{};
	public List<DiceValue> LinkPointers=new List<DiceValue>{};
	bool checkNext;
	int counter;
	public Text RS;
	public Text HS;
	public int highScore;

	public GameObject[][] Board=new GameObject[6][];
	// Use this for initialization
	void Start () {
		for(int i=0;i<6;i++)
		{
			Board[i]=new GameObject[6];
		}
		generateBoard();
	//	scoreBoard();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject Instantiate(GameObject g, Vector2 v)
	{
		return (GameObject)Instantiate(g,v,Quaternion.identity) as GameObject;
	}

	public void reset()
	{
		counter=0;
		for(int i=0;i<6;i++)
		{
			for(int j=0;j<6;j++)
			{
				Destroy(Board[i][j]);
			}
		}
		generateBoard();
	}

	public void generateBoard()
	{
		for(int i=0;i<6;i++)
		{
			for(int j=0;j<6;j++)
			{
				System.Random RNG=new System.Random(ThreadSafeRandom.Next());
				int r=RNG.Next(6);
				Board[i][j]=Instantiate(dice[r],new Vector2(i,j));
				DiceValue dv=(DiceValue)Board[i][j].GetComponent(typeof(DiceValue));
				dv.DC=this;
				dv.Value=r+1;
				dv.Pos[0]=i;
				dv.Pos[1]=j;
			}
		}
	}

	public void reRoll(int[] ia)
	{
		Destroy(Board[ia[0]][ia[1]]);
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		int r=RNG.Next(6);
		Board[ia[0]][ia[1]]=Instantiate(dice[r],new Vector2(ia[0],ia[1]));
		DiceValue dv=(DiceValue)Board[ia[0]][ia[1]].GetComponent(typeof(DiceValue));
				dv.DC=this;
				dv.Value=r+1;
				dv.Pos[0]=ia[0];
				dv.Pos[1]=ia[1];

	}

	public void reRollRow(int r)
	{
		for(int i=0;i<6;i++)
		{
			int[] tempA=new int[2];
			tempA[0]=i;
			tempA[1]=r;
			reRoll(tempA);
		}
	}

	public void reRollCol(int c)
	{
		for(int i=0;i<6;i++)
		{
			int[] tempA=new int[2];
			tempA[0]=c;
			tempA[1]=i;
			reRoll(tempA);
		}
	}
	public void generateLinks()
	{
		for(int i=0;i<6;i++)
		{
			for(int j=0;j<6;j++)
			{
				DiceValue dv=(DiceValue)Board[i][j].GetComponent(typeof(DiceValue));

				if(!dv.Scored)
				{
				for(int a=-1;a<2;a++)
				{
				
					for(int b=-1;b<2;b++)
				 {
				
					if(dv.Pos[0]+a>=0 && dv.Pos[1]+b>=0 && dv.Pos[0]+a<6 && dv.Pos[1]+b<6)
					{
						if(a !=0 || b != 0)
						{
							DiceValue dvCheck=(DiceValue)Board[dv.Pos[0]+a][dv.Pos[1]+b].GetComponent(typeof(DiceValue));
							if(dvCheck.Value==dv.Value)
							{
								
									if(!dvCheck.linked)
								{
								ScoringList.Add(dvCheck);
								dvCheck.linked=true;
								dv.prevPointer[0]=dvCheck.Pos[0];
								dv.prevPointer[1]=dvCheck.Pos[1];
								LinkPointers.Add(dvCheck);
								}
							}
							
						}
					}
					if(a==1 && b==1)
							{

								if(LinkPointers.Count>0)
								{
									dv=LinkPointers[LinkPointers.Count-1];
									a=-1;
									b=-2;
									LinkPointers.Remove(LinkPointers[LinkPointers.Count-1]);
								}

								else
								{
									if(dv.Value==1)
									{
										if(ScoringList.Count==0)
										{dv.gameObject.GetComponent<SpriteRenderer>().color=Color.green;
											counter++;}
										foreach(DiceValue d in ScoringList)
										{
											d.Scored=true;
										}
										dv.Scored=true;
									}
									else{
									foreach(DiceValue d in ScoringList)
									{
										if(ScoringList.Count==dv.Value)
									{
									d.gameObject.GetComponent<SpriteRenderer>().color=Color.green;
									counter++;
									}
									d.Scored=true;
									}
									}
								dv.Scored=true;
								ScoringList.Clear();
								}

							} 
				  
				}
			 }
			}

			}
		}
	}


	public void scoreBoard()
	{
		//link adjacent same #'s
		
		generateLinks();
		RS.text="Round Score: "+counter;
		if(counter>highScore)
		{
			highScore=counter;
			HS.text=("High Score: ")+highScore;
		}
		//print("linked! :)");
		//print(counter);
		counter=0;
		//count links
		//countLinks();
	

	}
}
