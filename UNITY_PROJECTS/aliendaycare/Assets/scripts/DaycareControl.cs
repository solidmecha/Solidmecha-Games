using UnityEngine;
using System.Collections;

public class DaycareControl : MonoBehaviour {

    public static DaycareControl singleton;
    public GameObject baby;
    public Sprite[] BabySprites;
    public Sprite[] ObjectSprites;
    public Color[] ClothesColors;
    public System.Random RNG;
    bool won;
    int babyCount;
    float counter;
    public int MaxBabies;
    public Sprite[] Faces;
    public int Score;
    public UnityEngine.UI.Text sText;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
    }

    void BabyArrives()
    {
       GameObject g=Instantiate(baby) as GameObject;
        bool Placed=false;
        while(!Placed)
        {
            int r = RNG.Next(11);
            if (transform.GetChild(0).GetChild(r).GetComponent<ObjectScript>().index != -1)
            {
                BabyScript b = g.GetComponent<BabyScript>();
                b.wants = new int[DaycareControl.singleton.RNG.Next(4, 9)];
                b.addWants();
                transform.GetChild(0).GetChild(r).GetComponent<ObjectScript>().SetBaby(b);
                Placed = true;
            }

        }
    }

    // Use this for initialization
    void Start () {
	
	}

    void updateScore()
    {
        sText.text="Final Score: "+Score.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if(babyCount<MaxBabies)
            counter -= Time.deltaTime;
        if(counter<=0)
        {
            counter = 5;
            babyCount++;
            BabyArrives();
        }
        if(MaxBabies==0 && !won)
        {
            updateScore();
            won = true;
        }
	}
}
