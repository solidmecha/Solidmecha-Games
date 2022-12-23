using UnityEngine;
using System.Collections;

public class GuessTest : MonoBehaviour {

    public int teamCount;
    public int OtherCount;
    int OtherDir = 0;
    System.Random RNG;
    public int number;

    int[] Wins = new int[3];
	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
	}


	
	// Update is called once per frame
	void Update () {
        number = RNG.Next(1, 1001);
        OtherCount = RNG.Next(1, 1001);
        OtherDir = RNG.Next(-1, 1);
        if (number >= 501)
        {
            teamCount = number - 501;
        }
        else
        {
            teamCount = 500 - number;
        }

        if (number > OtherCount && OtherDir == 0)
        {
            OtherCount = number - OtherCount;
        }
        else if (OtherCount < number && OtherDir == 0)
        {
            OtherCount = 1000 - OtherCount + number;
        }
        else if (OtherCount > number && OtherDir < 0)
        {
            OtherCount = OtherCount + (1000 - number);
        }
        else if (OtherCount < number && OtherDir < 0)
        {
            OtherCount = OtherCount - number;
        }
        else if (OtherCount == number)
            OtherCount = 0;

        if (teamCount < OtherCount)
        {
            Wins[0]++;
        }
        else if (OtherCount < teamCount)
            Wins[1]++;
        else
            Wins[2]++;

        print(Wins[0].ToString() + " , " + Wins[1].ToString() + " , " + Wins[2].ToString());

	 
	}
}
