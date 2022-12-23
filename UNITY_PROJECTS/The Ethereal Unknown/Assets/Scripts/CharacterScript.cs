using UnityEngine;
using System.Collections.Generic;

public class CharacterScript : MonoBehaviour {

	public int[] Stats=new int[15]; 
	/*
	dex,int, str,Loki,Freya, Odin,injury res, curse res, hunger res, secret detect, gold find, item find, item rarity, bonus xp
	*/
	public int[] Problems=new int[3]; //injury/curse/hunger
    public int[] Favor = new int[3]; //loki freya odin
	public List<ItemScript> EquippedItems=new List<ItemScript>{};
    public List<GameObject> curseList = new List<GameObject> { };
    public bool[] injuryArrayBools = new bool[6];
    public bool[] equipBoolArray = new bool[6];
    public bool[] isCursedArray=new bool[6];
	public int Exp;
	public int Lvl;
	public int Gold;
    public GameObject IMObj;
    public ItemManager IM;

	// Use this for initialization
	void Start () {
        IM = (ItemManager)IMObj.GetComponent(typeof(ItemManager));
	}

    public void Pay(int p)
    {
        Gold -= p;
        IM.updateUI();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
