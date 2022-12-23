using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TerminalScript : MonoBehaviour {

    public int Seed;
    public string[] Solutions = new string[5]; //
    public Text HintScreen;
    System.Random RNG;

	// Use this for initialization
	void Start () {
        RNG = new System.Random(Seed);
        showWirePuzzleHint();
	}

    char[][] createWirePuzzle()
    {
        List<char> PossibleChars = new List<char> { 'X', 'B', 'L','R', 'Y', 'S'};
        for(int i=0;i<9;i++)
        {
            PossibleChars.Add(i.ToString()[0]);
        }

        char[][] PuzzleHint=new char[8][];

        for(int i=0;i<8;i++)
        {
            PuzzleHint[i] = new char[8];
            for(int j=0; j<8;j++)
            {

                int R = RNG.Next(4 * PossibleChars.Count);
                if (R >= PossibleChars.Count)
                    R = 0;
                PuzzleHint[i][j] = PossibleChars[R]; 
            }
        }

        return PuzzleHint;
    }

    void showWirePuzzleHint()
    {
        char[][] hint= createWirePuzzle();
        for(int i=0;i<8;i++)
        {
            HintScreen.text += "\n" + new string(hint[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
