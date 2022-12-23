using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordControl : MonoBehaviour {

    public TextAsset WordList;
    public Text inputString;
    public Text containedChars;
    public Text uncontainedChars;
    System.Random RNG;
    int[] LetterCounts=new int[26];
    string letterString = "abcdefghijklmnopqrstuvwxyz";
    List<string> PossibleWords = new List<string> { };
    public Text GuessWord;
    public Text[] LetterExclusions;

    // Use this for initialization
    void Start()
    {
        RNG = new System.Random();
    }

    void printLetterCounts(int index)
    {
        for (int i = 0; i < 26; i++)
            LetterCounts[i] = 0;
        string[] words = WordList.text.Split('\n');
        foreach(string w in words)
        {
            LetterCounts[letterString.IndexOf(w[index])]++;
        }
        for (int i = 0; i < 26; i++)
        {
            transform.GetChild(i).localScale = new Vector2(.5f, LetterCounts[i]/50f);
            print(letterString[i].ToString() + "," + LetterCounts[i].ToString());
        }
    }

    public int[] GatherTotalLetterCounts()
    {
        int[] ia = new int[26];
        foreach (string w in PossibleWords)
        {
            for(int i=0;i<5;i++)
                LetterCounts[letterString.IndexOf(w[i])]++;
        }
        return ia;
    }

    public void PrintPossible()
    {
        string[] wordsLeft = WordList.text.Split('\n');
        PossibleWords.Clear();
        foreach(string word in wordsLeft)
        {
            if (containsAll(word) && doesNotContainAll(word))
            {
                bool addWord = true;
                for(int i=0;i<5;i++)
                {
                    if(inputString.text[i] != '*')
                    {
                        if (inputString.text[i] != word[i])
                            addWord = false;
                    }
                    else
                    {
                        if (CheckExclusion(i, word))
                            addWord = false;
                    }
                }
                if (addWord)
                    PossibleWords.Add(word);   
            }
        }
        print(PossibleWords.Count);
        //foreach (string s in PossibleWords)
           // print(s);
    }

    bool CheckExclusion(int index, string w)
    {
        foreach(char c in LetterExclusions[index].text)
        {
            if (w[index]==c)
                return true;
        }
        return false;
    }

    public void GenerateGuess()
    {
        GuessWord.text = PossibleWords[RNG.Next(PossibleWords.Count)];
    }

    public bool containsAll(string s)
    {
        foreach(char c in containedChars.text)
        {
            if (!s.Contains(c.ToString()))
                return false;
        }
        return true;
    }

    public bool doesNotContainAll(string s)
    {
        foreach (char c in uncontainedChars.text)
        {
            if (s.Contains(c.ToString()))
                return false;
        }
        return true;
    }
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.Return))
        {
            print(inputString.text);
            PrintPossible();
            GenerateGuess();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            printLetterCounts(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            printLetterCounts(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            printLetterCounts(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            printLetterCounts(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            printLetterCounts(4);
    }
}
