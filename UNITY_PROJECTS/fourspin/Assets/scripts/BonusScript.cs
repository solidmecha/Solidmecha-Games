using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusScript : MonoBehaviour {
    public Text[] BonusText;
    string[] BonusStrings = new string[4] { "Clearing\nBonus!", "Prismatic\nBonus!", "Long Chain\nBonus!", "Combo\nBonus!"};
    public int[] ScoreBonus=new int[4] { 0, 0, 0, 0 };
    public bool[] ColorClears=new bool[4];
    float[] BonusTimers=new float[4] {10f, 10f, 10f, 10f}; 

	// Use this for initialization
	void Start () {
	
	}

    public int BonusTotal()
    {
        return ScoreBonus[0] + ScoreBonus[1] + ScoreBonus[2] + ScoreBonus[3];
    }


    public void PrismaticCheck()
    {
        if(ColorClears[0] && ColorClears[1] && ColorClears[2] && ColorClears[3])
        {
            ColorClears[0] = false;
            ColorClears[1] = false;
            ColorClears[2] = false;
            ColorClears[3] = false;
            ShowBonus(1);
        }
    }

   public void ShowBonus(int BonusIndex)
    {
        BonusTimers[BonusIndex] = 10f;
        if (ScoreBonus[BonusIndex] == 0)
        {
            ScoreBonus[BonusIndex] = 100;
            BonusText[BonusIndex].text = BonusStrings[BonusIndex];
        }
        else if(ScoreBonus[BonusIndex]<250)
        {
            ScoreBonus[BonusIndex] += 50;
        }
        else if(ScoreBonus[BonusIndex]==250)
        {
            BonusText[BonusIndex].text = "Maximum\n"+BonusStrings[BonusIndex];
            BonusTimers[BonusIndex] = 15f;
        }
    }

	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; i++)
        {
            BonusTimers[i] -= Time.deltaTime;
            if(ScoreBonus[i]>0 && BonusTimers[i]<=0)
            {
                ScoreBonus[i] = 0;
                BonusText[i].text = "";
            }
        }
    }
}
