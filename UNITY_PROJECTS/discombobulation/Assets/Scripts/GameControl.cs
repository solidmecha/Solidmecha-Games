using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public List<GameObject> Cards = new List<GameObject> { };
    public System.Random RNG = new System.Random();
    public Color[] ColorIndex;
    public List<Text> CoinText;
    public int[] CoinAmounts; //white, blue, red, Green
    public int TimeChange;
    public bool FlipBack;
    public int LastTimeChange;
    public GameObject CardPos;
    public int CardCount;
    public int ClickCount;
    public Text ClickCountText;
    public bool won;

    public void UpdateClick()
    {
        ClickCount++;
        ClickCountText.text = ClickCount.ToString();
        if(won)
        {
            ClickCountText.text = "Won in " + ClickCount.ToString();
        }
    }

    public void GainCoins(int Index, int Value)
    {
        CoinAmounts[Index] += Value;
        CoinText[Index].text = CoinAmounts[Index].ToString();
    }

	// Use this for initialization
	void Start () {
        //for (int i = 0; i < 3; i++)
            //GenerateCard(CardPos.transform.GetChild(i).position);
	}

    public void GenerateCard(Vector2 Pos)
    {
        int r = RNG.Next(Cards.Count);
        if(CardCount<7 && RNG.Next(5)>2)
        {
            r = 4;
        }
        GameObject go=Instantiate(Cards[r], Pos, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        go.GetComponent<TimeScript>().counter = RNG.Next(5, 25);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
