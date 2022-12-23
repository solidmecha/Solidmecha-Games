using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleLeader : MonoBehaviour {

	public GameObject Square;
	public GameObject Venn;
	public int width;
	public int length;
	public Color SelectedColor;
	public GameObject selectorObject;
	SpriteRenderer selectSR;
	public GameObject[][] Board;

	public GameObject widthSlider;
	public GameObject lengthSlider;
	public GameObject ModeToggle;
	public GameObject Timer;
	public Text timeText;
	bool isSolved;
	bool maniacalMode;
	float counter;


	// Use this for initialization
	void Start () {

		SelectedColor=Color.black;
		selectSR=selectorObject.GetComponent<SpriteRenderer>();

		if(PlayerPrefs.GetInt("Mode") != 0)
		{
			maniacalMode=true;
		}

		if(PlayerPrefs.GetInt("Width")!=0)
		{
			width=PlayerPrefs.GetInt("Width");
		}

		if(PlayerPrefs.GetInt("Length") !=0)
		{
			length=PlayerPrefs.GetInt("Length");
		}

		lengthSlider.GetComponent<Slider>().value=length;
		widthSlider.GetComponent<Slider>().value=width;
		ModeToggle.GetComponent<Toggle>().isOn=maniacalMode;

		timeText=Timer.GetComponent<Text>();

		widthSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {changeWidth();});
		lengthSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate{changeLength();});
		ModeToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate{changeMode();});


		transform.position=new Vector2(transform.position.x+(6-width)*.2575f, transform.position.y+(6-length)*.4375f);
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		Board=new GameObject[width][];
		for(int i=0;i<width;i++)
		{
			Board[i]=new GameObject[length];
		}


		for(int i=0;i<width;i++)
		{
			for(int j=0;j<length;j++)
			{
				GameObject go=(GameObject) Instantiate(Square, new Vector2(transform.position.x+i,transform.position.y+j), Quaternion.identity) as GameObject;
				SquareScript ss=(SquareScript)go.GetComponent(typeof(SquareScript));
				ss.PL=this;
				ss.SolutionVector=new Vector4(RNG.Next(2),RNG.Next(2),RNG.Next(2), 1);
				Board[i][j]=go;
			}
		}
		
		if(!maniacalMode)		
		setUpDisplay();
		else
		setUpDividedDisplay();

	}

	public void setUpDisplay()
	{
		GameObject go;
		for(int i=0; i<width; i++)
		{
			for(int j=0; j<length;j++)
			{
				go=Board[i][j];
				int[] tempArray=new int[7]{0,0,0,0,0,0,0};
				
				for(int a=-1; a<2;a++)
				{
					for(int b=-1;b<2;b++)
					{
						if(i+a>=0 && i+a<width && j+b>=0 && j+b<length)
						{
							if(a!=0 || b!=0)
							{
								SquareScript ss=(SquareScript) Board[i+a][j+b].GetComponent(typeof(SquareScript));
								
								if(ss.SolutionVector.Equals(new Vector4(1,1,1,1)))
									tempArray[0]++;
								else if(ss.SolutionVector.Equals( new Vector4(1,1,0,1)))
									tempArray[1]++;
							
									else if(ss.SolutionVector.Equals(new Vector4(1,0,1,1)))
									tempArray[2]++;
									
									else if(ss.SolutionVector.Equals( new Vector4(0,1,1,1)))
									tempArray[3]++;
								
									else if(ss.SolutionVector.Equals( new Vector4(1,0,0,1)))
									tempArray[4]++;
									
									else if(ss.SolutionVector.Equals( new Vector4(0,1,0,1)))
									tempArray[5]++;
									
									else if(ss.SolutionVector.Equals( new Vector4(0,0,1,1)))
									tempArray[6]++;
									
								}
							}

						}

					}

				
				
				GameObject display=(GameObject) Instantiate(Venn, go.transform.position, Quaternion.identity);

				for(int q=0;q<7;q++)
				{
					display.transform.GetChild(0).GetChild(q).gameObject.GetComponent<Text>().text=tempArray[q].ToString();
				}
			}

			}


		}

void changeMode()
{
	if(PlayerPrefs.GetInt("Mode") ==0)
	{
		PlayerPrefs.SetInt("Mode", 1);
	}
	else
	{
		PlayerPrefs.SetInt("Mode", 0);
	}
	PlayerPrefs.Save();
	Application.LoadLevel(0);
}
		
void changeLength()
{
	int l=(int)lengthSlider.GetComponent<Slider>().value;
	PlayerPrefs.SetInt("Length", l);
	PlayerPrefs.Save();
	Application.LoadLevel(0);
}

void changeWidth()
{
	int w=(int)widthSlider.GetComponent<Slider>().value;
	PlayerPrefs.SetInt("Width", w);
	PlayerPrefs.Save();
	Application.LoadLevel(0);
}


public void checkSolution()
	{
		int s=0;
		for(int i=0;i<width;i++)
		{
			for(int j=0;j<length;j++)
			{
				SquareScript ss=(SquareScript)Board[i][j].GetComponent(typeof(SquareScript));
				Vector4 v= ss.SR.color;
				if(v.Equals(ss.SolutionVector))
				{
					s++;
				}
				if(s==width*length)
				{
					isSolved=true;
				}
			}
		}
	}
	

	public void setUpDividedDisplay()
	{
		GameObject go;
		for(int i=0; i<width; i++)
		{
			for(int j=0; j<length;j++)
			{
				System.Random RNG=new System.Random(ThreadSafeRandom.Next());
				go=Board[i][j];

				if(true)
				{
				int[] tempArray=new int[7]{0,0,0,0,0,0,0};
				int[] primeColorTotals= new int[3] {0,0,0};
				for(int a=-1; a<2;a++)
				{
					for(int b=-1;b<2;b++)
					{
						if(i+a>=0 && i+a<width && j+b>=0 && j+b<length)
						{
							if(a!=0 || b!=0)
							{
								SquareScript ss=(SquareScript) Board[i+a][j+b].GetComponent(typeof(SquareScript));
								primeColorTotals[0]+=(int)ss.SolutionVector.x;
								primeColorTotals[1]+=(int)ss.SolutionVector.y;
								primeColorTotals[2]+=(int)ss.SolutionVector.z;
							}

						}

					}

				}

				if(primeColorTotals[0] > 0 && primeColorTotals[1] > 0 && primeColorTotals[2] >0) //white
				{
					int r=RNG.Next(returnLowest(primeColorTotals));
					if(RNG.Next(3)>0)
					{
					for(int z=0;z<3;z++)
					{
						primeColorTotals[z]-=r;
					}
						tempArray[0]=r;
					}
				}

				if(primeColorTotals[0] > 0 && primeColorTotals [1] > 0) //yellow
				{
					if(primeColorTotals[0]<primeColorTotals[1])
					{
						if(RNG.Next(2)==1)
						{
						int r=RNG.Next(primeColorTotals[0]);
						primeColorTotals[0]-=r;
						primeColorTotals[1]-=r;
						tempArray[1]=r;
						}
					}
					else
					{
						if(RNG.Next(2)==1)
						{
						int r=RNG.Next(primeColorTotals[1]);
						primeColorTotals[0]-=r;
						primeColorTotals[1]-=r;
						tempArray[1]=r;
						}

					}
				}

				if(primeColorTotals[0] > 0 && primeColorTotals [2] > 0)  //violet
				{
					if(primeColorTotals[0]<primeColorTotals[2])
					{
						if(RNG.Next(2)==1)
						{
						int r=RNG.Next(primeColorTotals[0]);
						primeColorTotals[0]-=r;
						primeColorTotals[2]-=r;
						tempArray[2]=r;
						}
					}
					else
					{
						if(RNG.Next(2)==1)
						{
						int r=RNG.Next(primeColorTotals[2]);
						primeColorTotals[0]-=r;
						primeColorTotals[2]-=r;
						tempArray[2]=r;
						}

					}
				}

				if(primeColorTotals[2] > 0 && primeColorTotals [1] > 0) //cyan
				{
					if(primeColorTotals[2]<primeColorTotals[1])
					{
						if(RNG.Next(2)==1)
						{
						int r=RNG.Next(primeColorTotals[2]);
						primeColorTotals[2]-=r;
						primeColorTotals[1]-=r;
						tempArray[3]=r;
						}
					}
					else
					{
						if(RNG.Next(2)==1)
						{
						int r=RNG.Next(primeColorTotals[1]);
						primeColorTotals[2]-=r;
						primeColorTotals[1]-=r;
						tempArray[3]=r;
						}

					}
				}

				for(int n=0;n<3;n++)
				{tempArray[4+n]=primeColorTotals[n];}

				GameObject display=(GameObject) Instantiate(Venn, go.transform.position, Quaternion.identity);

				for(int q=0;q<7;q++)
				{
					display.transform.GetChild(0).GetChild(q).gameObject.GetComponent<Text>().text=tempArray[q].ToString();
				}

				}
			}
		}
	}
	
	int returnLowest(int[] a)
	{
		int l=a[0];
		for(int i=1; i<a.Length;i++)
		{
			if(a[i]<l)
			{
				l=a[i];
			}
		}
		return l;
	}
	// Update is called once per frame
	void FixedUpdate () {

		selectSR.color=SelectedColor;
		if(!isSolved)
		{
			counter+=Time.deltaTime;
			timeText.text="Time: "+counter.ToString();
		}
		else
		{
			timeText.text="Solved in "+counter.ToString();
		}
	
	}
}
