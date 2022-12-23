using UnityEngine;
using System.Collections.Generic;

public class PlayerScript {

    public GameManager GM;
    public string name;
    public List<GameObject> Deck = new List<GameObject> { };
    public GameObject[] Machinations = new GameObject[5];
    public bool[] MachinationOpen = new bool[5];
    public List<GameObject> Hand = new List<GameObject> { };
    public List<GameObject> Graveyard = new List<GameObject> { };
   
    public int[] Resources = new int[4];//Steam, Gears, Life, Gems
   
    public void DrawCards()
    {
        if(Deck.Count>=5)
        {
            for(int i=4;i>-1;i--)
            {
                Deck[i].transform.position = GM.HandView.transform.GetChild(i).position;
                Deck[i].transform.SetParent(GM.HandView.transform);
                Hand.Add(Deck[i]);
                Deck.Remove(Deck[i]);
            }
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
