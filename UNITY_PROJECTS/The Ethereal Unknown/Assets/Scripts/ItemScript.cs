using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ItemScript : MonoBehaviour {


    public bool equipped; //if affixes were added on equip due to not being injured
    public bool stashed;
    public bool justFound;
    public bool inShop;
	public CharacterScript CS;
	public ItemManager IM;
	public string Name;
	public List<Affix> AffixList=new List<Affix>{};
	public int Duration;
	public int Value;
	public GameObject ItemBox;
    public GameObject shopBox;
	public Vector2 iBoxV2=new Vector2(0.93f,0.27f);
	public Vector2 EquipLocation;
	public int baseID; //armor, boots, helm, shield, weapon, amulet
    public int invLoc = 10;


	public void OnMouseDown()
	{
        if(IM.DisplayBox!=null)
        { Destroy(IM.DisplayBox); }

        if (!inShop)
        {
            GameObject itemBox = (GameObject)Instantiate(ItemBox, iBoxV2, Quaternion.identity) as GameObject;
            IM.DisplayBox = itemBox;
            itemBox.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = Name;
            int count = AffixList.Count;
            for (int i = 0; i < count; i++)
            {
                itemBox.transform.GetChild(0).GetChild(i + 1).gameObject.GetComponent<Text>().text = AffixList[i].Description;
            }
            itemBox.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Text>().text = Value.ToString();
            itemBox.transform.GetChild(0).GetChild(7).gameObject.GetComponent<Text>().text = Duration.ToString();
            GameObject go0 = itemBox.transform.GetChild(0).GetChild(8).gameObject;
            //make exit button work
            go0.GetComponent<Button>().onClick.AddListener(delegate { Destroy(go0.transform.parent.parent.gameObject); });

            GameObject go1 = itemBox.transform.GetChild(0).GetChild(9).gameObject;
            if (!equipped && IM.canEquip)
            {
                go1.GetComponent<Button>().onClick.AddListener(delegate { OnEquip(); });
                go1.GetComponent<Button>().onClick.AddListener(delegate { checkJustFound(); });
                go1.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Equip";
            }

            GameObject go2 = itemBox.transform.GetChild(0).GetChild(10).gameObject;
            go2.GetComponent<Button>().onClick.AddListener(delegate { unEquip(); });
            go2.GetComponent<Button>().onClick.AddListener(delegate { checkJustFound(); });

            for (int i = 0; i < 3; i++)
            {
                GameObject go3 = itemBox.transform.GetChild(0).GetChild(11 + i).gameObject;
                ButtonScript Bscript = (ButtonScript)go3.GetComponent(typeof(ButtonScript));
                Bscript.id = i;
                go3.GetComponent<Button>().onClick.AddListener(delegate { sacrifice(Bscript.id); });
                go3.GetComponent<Button>().onClick.AddListener(delegate { checkJustFound(); });
                go3.GetComponent<Button>().onClick.AddListener(delegate { Destroy(go3.transform.parent.parent.gameObject); });
            }
        }
        else
        {
            //shopping popup
            GameObject go = (GameObject)Instantiate(shopBox, iBoxV2, Quaternion.identity) as GameObject;
            IM.DisplayBox = go;
            go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = Name;
            int count = AffixList.Count;
            for (int i = 0; i < count; i++)
            {
                go.transform.GetChild(0).GetChild(i + 1).gameObject.GetComponent<Text>().text = AffixList[i].Description;
            }
            go.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Text>().text = Value.ToString();
            go.transform.GetChild(0).GetChild(7).gameObject.GetComponent<Text>().text = Duration.ToString();
            GameObject go0 = go.transform.GetChild(0).GetChild(8).gameObject;
            //make exit button work
            go0.GetComponent<Button>().onClick.AddListener(delegate { Destroy(go0.transform.parent.parent.gameObject); });
            if (CS.Gold >= Value)
            {
               
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { IM.SM.shopInventory.Remove(gameObject); });
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { unEquip(); });
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { CS.Pay(Value); });
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { IM.SM.cleanUp(); });
            }
        }
    }

    public void checkJustFound()
    {
        if(justFound)
        {
            justFound = false;
            IM.justFound = false;
        }
    }
    public void sacrifice(int i)
    {
       unEquip();
        if(stashed)
        {
            IM.inventoryArrayBool[invLoc] = false;
            IM.invLocPoint = invLoc;
            stashed = false;
        } 
        int v = (int)Value / 1000;
        if (CS.Favor[i] + v <= 100)
        {
            CS.Favor[i] += v;
        }
        else
        { CS.Favor[i] = 100; }
        IM.updateUI();
        Destroy(gameObject);
    }

	public class Affix
	{
		public string Description;
		public int Modifier;
		public int StatPointer;
		public Action EquipEffect;
		public Action UnEquipEffect;
		public ItemScript IS;

		public Affix(int min, int max, int affixID, int statPointer, ItemScript its)
		{
			System.Random RNG=new System.Random(ThreadSafeRandom.Next());
			IS=its;
			Modifier=RNG.Next(min, max);
			StatPointer=statPointer;

			switch(statPointer)
			{
				case 0:
				Description= Modifier.ToString() + " Dex";
				break;
				case 1:
				Description= Modifier.ToString() + " Int";
				break;
				case 2:
				Description= Modifier.ToString() + " Str";
				break;
				case 3:
				Description= Modifier.ToString() + " Loki Favor";
				break;
				case 4:
				Description= Modifier.ToString() + " Freya Favor";
				break;
				case 5:
				Description= Modifier.ToString() + " Odin Favor";
				break;
				case 6:
				Description= Modifier.ToString() + "% Injury Resist";
				break;
				case 7:
				Description= Modifier.ToString() + "% Curse Resist";
				break;
				case 8:
				Description= Modifier.ToString() + " Hunger Resist";
				break;
				case 9:
				Description= Modifier.ToString() + " Secret Detect";
				break;
				case 10:
				Description= Modifier.ToString() + "% Gold Find";
				break;
				case 11:
				Description= Modifier.ToString() + "% Item Find";
				break;
				case 12:
				Description= Modifier.ToString() + "% Item Rarity";
				break;
				case 13:
				Description= Modifier.ToString() + "% Bonus XP";
				break;
				case 14:
				Description="+"+ Modifier.ToString() + " Duration";
				break;
			}

			switch(affixID)
			{
				case 0: //change stat
				EquipEffect=IncStat;
				UnEquipEffect=DecStat;
				break;
				case 1: //increase duration
				IS.Duration+=Modifier;
				EquipEffect=voidIt;
                UnEquipEffect = voidIt;
				break;
				default:
				break;
			}
		}


		public void voidIt(){}
		public void IncStat()
		{
			IS.CS.Stats[StatPointer]+=Modifier;
		}

		public void  DecStat()
		{
			IS.CS.Stats[StatPointer]-=Modifier;
		}

		public void Void()
		{}
		
	}


	public void OnEquip()
	{
        if (!CS.equipBoolArray[baseID])
        {
            if (!CS.injuryArrayBools[baseID])
            {
                foreach (Affix A in AffixList)
                {
                    A.EquipEffect();
                }
                IM.updateUI();
                equipped = true;
            }
            if (stashed)
            {
                IM.invLocPoint = invLoc;
                stashed = false;
            }
            transform.position = EquipLocation;
            CS.EquippedItems.Add(this);
            CS.equipBoolArray[baseID] = true;
          
        }
      
	}

    public void unEquip()
    {
        inShop = false;
        if (equipped)
        {
            foreach (Affix A in AffixList)
            {
                A.UnEquipEffect();
            }          
            equipped = false;
            IM.updateUI();
        }
        if (Duration > 0)
        {
            if (IM.invLocPoint < 9 && !stashed)
            {
                transform.position = IM.InventoryLocs[IM.invLocPoint];
                invLoc = IM.invLocPoint;
                IM.inventoryArrayBool[invLoc] = true;
               
                for (int i = 0; i < 9; i++)
                {
                    if (!IM.inventoryArrayBool[i])
                    {
                        IM.invLocPoint = i;
                        break;
                    }
                    if (i == 8 && IM.inventoryArrayBool[8])
                    {
                        IM.invLocPoint = 10;
                    }
                }
            }
            stashed = true;
        }
        if (CS.EquippedItems.Contains(this))
        { CS.EquippedItems.Remove(this);
            CS.equipBoolArray[baseID] = false;
        }
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
