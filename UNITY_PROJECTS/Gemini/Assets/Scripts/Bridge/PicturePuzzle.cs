using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PicturePuzzle : MonoBehaviour {

    public List<Sprite> Flowers;
    public List<Sprite> Blots;
    public List<Sprite> Weather;
    public List<Sprite> Faces;
    public List<Sprite> Kitties;
    public List<Sprite> Patterns;
    System.Random RNG;
    public bool isPlayer1;

    public GameObject[] Boards;
    public GameObject[] Outlines;
    public GameObject[] SelectedObject;
    public GameObject Strike;
    public Sprite CheckMark;

    GameObject[] CorrectAnswers=new GameObject[6];
    public Button[] Buttons = new Button[6];
    bool[] isChecked = new bool[6];
    public Text Seedtext;
    public Button[] StartButtons = new Button[2];
    // Use this for initialization
    void Start () {
        StartButtons[0].onClick.AddListener(delegate { StartButton(1); });
        StartButtons[1].onClick.AddListener(delegate { StartButton(2); });
        for (int i = 0; i < 6; i++)
        {
            int a = i;
            Buttons[i].onClick.AddListener(delegate { checkAnswer(a); });
            isChecked[a] = false;
        }
        SelectedObject= new GameObject[6];

	}

    void StartButton(int i)
    {
        int s;
        if (int.TryParse(Seedtext.text, out s))
        {
            if (i == 1)
                isPlayer1 = true;
            else
                isPlayer1 = false;
            RNG = new System.Random(s);
            SetUpPuzzles();
            Boards[0].transform.parent.position = Vector3.zero;
            Destroy(StartButtons[0].transform.parent.gameObject);
        }
    }

    void SetUpPuzzles()
    {
        if(isPlayer1)
        {
            for (int i = 0; i < 5; i++)
            {
                int flower = RNG.Next(Flowers.Count);
                Boards[0].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Flowers[flower];
                Boards[0].transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                Flowers.RemoveAt(flower);

                int b = RNG.Next(Blots.Count);
                Boards[1].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Blots[b];
                Boards[1].transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                Blots.RemoveAt(b);

                int w = RNG.Next(Weather.Count);
                Boards[2].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Weather[w];
                Boards[2].transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                Weather.RemoveAt(w);
                int f = RNG.Next(Faces.Count);
                Boards[3].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Faces[f];
                Boards[3].transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                Faces.RemoveAt(f);
                int k = RNG.Next(Kitties.Count);
                Boards[4].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Kitties[k];
                Boards[4].transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                Kitties.RemoveAt(k);
                int p = RNG.Next(Patterns.Count);
                Boards[5].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Patterns[p];
                Boards[5].transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                Patterns.RemoveAt(p);
            }
            int[] correctIndex = new int[6];
            int[] correctP2Index = new int[6];
            for (int i = 0; i < 6; i++)
            {
                correctIndex[i] = RNG.Next(5);
                correctP2Index[i] = RNG.Next(5);
            }

            for(int i=0;i<6;i++)
            {
                CorrectAnswers[i] = Boards[i].transform.GetChild(correctIndex[i]).gameObject; 
            }

        }
        else
        {
            //player2
            int[] B0 = new int[5];
            int[] B1 = new int[5];
            int[] B2 = new int[5];
            int[] B3 = new int[5];
            int[] B4 = new int[5];
            int[] B5 = new int[5];
            for (int i = 0; i < 5; i++)
            {
                B0[i] = RNG.Next(Flowers.Count-i);
                B1[i] = RNG.Next(Blots.Count - i);
                B2[i] = RNG.Next(Weather.Count - i);
                B3[i] = RNG.Next(Faces.Count - i);
                B4[i] = RNG.Next(Kitties.Count - i);
                B5[i] = RNG.Next(Patterns.Count - i);
            }
            Sprite[] correctSprites = new Sprite[6];
            List<int[]> BoardIndexes = new List<int[]> { B0, B1, B2, B3, B4, B5 };
            List<List<Sprite>> ListOfSprites=new List<List<Sprite>> { Flowers, Blots, Weather, Faces, Kitties, Patterns};
            int[] correctIndex = new int[6];
            int[] correctP2Index = new int[6];
            for(int i=0;i<6; i++)
            {
                correctIndex[i] = RNG.Next(5);
                correctP2Index[i] = RNG.Next(5);
            }

            for(int i=0;i<6; i++)
            {
                for(int j=0;j<5;j++)
                {
                    if (j == correctIndex[i])
                    {
                        Sprite S = ListOfSprites[i][BoardIndexes[i][j]];
                        correctSprites[i] = S;
                        ListOfSprites[i].RemoveAt(BoardIndexes[i][j]);
                    }
                    else
                    {
                        ListOfSprites[i].RemoveAt(BoardIndexes[i][j]);
                    }

                }
            }

            for(int i=0;i<6;i++)
            {
                for(int j=0;j<5;j++)
                {
                    if (j == correctP2Index[i])
                    {
                        Boards[i].transform.GetChild(correctP2Index[i]).GetComponent<SpriteRenderer>().sprite = correctSprites[i];
                        Boards[i].transform.GetChild(correctP2Index[i]).gameObject.AddComponent<BoxCollider2D>();
                    }
                    else
                    {
                        int R = RNG.Next(ListOfSprites[i].Count);
                        Boards[i].transform.GetChild(j).GetComponent<SpriteRenderer>().sprite = ListOfSprites[i][R];
                        Boards[i].transform.GetChild(j).gameObject.AddComponent<BoxCollider2D>();
                        ListOfSprites[i].RemoveAt(R);
                    }

                }
            }

            for(int i=0;i<6;i++)
            {              
                CorrectAnswers[i] = Boards[i].transform.GetChild(correctP2Index[i]).gameObject;

            }
        }
    }

    void checkAnswer(int i)
    {
        if (!isChecked[i])
        {
            GameObject go = Instantiate(Strike, Boards[i].transform.position, Quaternion.identity) as GameObject;
            if (SelectedObject[i].Equals(CorrectAnswers[i]))
            {
                go.GetComponent<SpriteRenderer>().sprite = CheckMark;
            }
            isChecked[i] = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
