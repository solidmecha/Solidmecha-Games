using UnityEngine;
using System.Collections.Generic;

public class ItemScript : MonoBehaviour {

    public int InventoryIndex;
    public int[] DamageRange;
    public float CD;
    public int HeatGen;
    public int PowerReq;
    public int SellValue;
    public int HP;
    public int Power;
    public GameControl.ItemType ItemType;
    public GameControl.WeaponType WeaponType;
    public List<ItemControl.Mod> Mods;
    public List<int> ModVals;
    public List<string> ModDescriptions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
