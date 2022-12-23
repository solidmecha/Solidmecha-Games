using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

	public List<GameObject> Armor;
	public List<GameObject> Boots;
	public List<GameObject> Helms;
	public List<GameObject> Shields;
	public List<GameObject> Weapons;
	public List<GameObject> Amulets;
    public List<GameObject> Food;
    public GameObject DisplayBox;
	public GameObject ItemBox;
	public GameObject Character;
    public GameObject InjuryMark;
    public GameObject[] InjuryMarkArray =new  GameObject[6];
	public CharacterScript CS;
    public ScenarioManager SM;
	public bool canEquip;
	public Vector2[] EquipLocs=new Vector2[6];
    public Vector2[] InjuryLocs = new Vector2[6];
    public bool[] inventoryArrayBool = new bool[9]; //false if nothing's there 
    public int invLocPoint;//points to where next inventory item is going in InventoryLocs
    public Vector2[] InventoryLocs = new Vector2[9];
    public int CurseIndex;
    public GameObject curseHolder;
    public Vector2[] CurseLocs = new Vector2[6];
    public List<GameObject> curseObjs = new List<GameObject> { };
    public ItemScript IS1;
    public GameObject[] favorBars=new GameObject[3];
    public GameObject hungerBar;
    public GameObject[] statDisplays= new GameObject[4];
    public bool justFound;
    public bool isSelling;
    public bool healing;
    public int TurnNumber=0;

    public void updateUI()
    {
        for(int i=0;i<3;i++)
        { favorBars[i].transform.localScale=new Vector2(((float)CS.Favor[i]/100f)*58.7f, favorBars[i].transform.localScale.y);
            statDisplays[i].GetComponent<Text>().text = CS.Stats[i].ToString();
        }
        statDisplays[3].GetComponent<Text>().text = CS.Gold.ToString();
        hungerBar.transform.localScale = new Vector2((CS.Problems[2] / 100f) * 20.22f, hungerBar.transform.localScale.y);
    }

    public void initializeChar()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        CS.Problems[2] = 100;//set hunger to max
        for (int i = 0; i < 3; i++)
        {
            CS.Favor[i] = RNG.Next(5, 100);
        }
        updateUI();
    }

    public void Injury()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        int r = CS.Problems[0];
        Vector2 v= Vector2.zero;
        while (r == CS.Problems[0])
        {
            int p = RNG.Next(6);
            if(!CS.injuryArrayBools[p])
            {
                if(CS.equipBoolArray[r])
                {
                    foreach(ItemScript I in CS.EquippedItems)
                    {
                        if(I.baseID==r)
                        {
                            foreach(ItemScript.Affix A in I.AffixList)
                            {
                                A.UnEquipEffect();
                            }
                            I.equipped = false;
                        }
                    }
                }
                CS.Problems[0]++;
                if(CS.Problems[0]==6)
                {
                    cursesFoiledAgain();
                }
                v = InjuryLocs[p];
                CS.injuryArrayBools[p] = true;
                InjuryMarkArray[p]=(GameObject)Instantiate(InjuryMark, v, Quaternion.identity) as GameObject;
            }
        }
       
    }

    public void healInjury(int r)
    {
        if (CS.injuryArrayBools[r])
        {
            Destroy(InjuryMarkArray[r]);
            CS.injuryArrayBools[r] = false;
            CS.Problems[0]--;

            if (CS.equipBoolArray[r])
            {
                foreach (ItemScript I in CS.EquippedItems)
                {
                    if (I.baseID == r)
                    {
                        foreach (ItemScript.Affix A in I.AffixList)
                        {
                            A.EquipEffect();
                        }
                        I.equipped = true;
                    }
                }
            }
        }
    }

    public void Curse()
    {

        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        int r = RNG.Next(3);
        ItemScript.Affix curse = new ItemScript.Affix(-20, -5, 0, r, IS1);
        GameObject go=(GameObject)Instantiate(curseObjs[r], CurseLocs[CurseIndex], Quaternion.identity) as GameObject;
        curseScript curses = (curseScript)go.GetComponent(typeof(curseScript));
        curses.IM = this;
        curses.Index = CurseIndex;
        curse.IS.CS = CS;
        curses.curse = curse;
        curse.EquipEffect();
        CS.isCursedArray[CurseIndex] = true;
        for(int i=0;i<6;i++)
        {
            if(!CS.isCursedArray[i])
            { CurseIndex = i;
                break;
            }
            if(i==5 && CS.isCursedArray[i])
            {
                cursesFoiledAgain();
            }
        }
        CS.curseList.Add(go);
        updateUI();
    }

    public void removeCurse(int p)
    {
        healing = false;
        curseScript curse = (curseScript)CS.curseList[p].GetComponent(typeof(curseScript));
        curse.curse.UnEquipEffect();
        CurseIndex = curse.Index;
        CS.isCursedArray[CurseIndex] = false;
        Destroy(CS.curseList[p]);
        updateUI();
    }

    public void timePasses()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());

        for(int i=0;i< CS.EquippedItems.Count; i++)
        {
            CS.EquippedItems[i].Duration--; //duration drops
            if(CS.EquippedItems[i].Duration==0)
            {
                GameObject go = CS.EquippedItems[i].gameObject;
                CS.EquippedItems[i].unEquip();
                Destroy(go);
            }
        }
        int r = (int)CS.Stats[2] / 3 + CS.Stats[8];
        if(RNG.Next(100)>r)
        {
            CS.Problems[2]--;//food gauge decreases
            if(CS.Problems[2]<=0)
            {
                cursesFoiledAgain();
            }
        }
        TurnNumber++;
        updateUI();

    }

    public GameObject cookFood(Vector2 v)
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        GameObject go = (GameObject)Instantiate(Food[RNG.Next(Food.Count)], v, Quaternion.identity) as GameObject;
        FoodScript fs = (FoodScript)go.GetComponent(typeof(FoodScript));
        fs.IM = this;
        fs.Value = RNG.Next(200, 2000);
        fs.Nom = RNG.Next(5, 15);
        fs.inShop = true;
        return go;
    }

    public GameObject createRandomItem(Vector2 v)
	{
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		GameObject item;
		string baseType;
		int r=RNG.Next(6);
		switch(r)
		{
			case 0:
			item=Armor[RNG.Next(Armor.Count)];
			baseType=" Armor";
			break;
			case 1:
			item=Boots[RNG.Next(Boots.Count)];
			baseType=" Boots";
			break;
			case 2:
			item=Helms[RNG.Next(Helms.Count)];
			baseType=" Helm";
			break;
			case 3:
			item=Shields[RNG.Next(Shields.Count)];
			baseType=" Shield";
			break;
			case 4:
			item=Weapons[RNG.Next(Weapons.Count)];
			baseType=" Weapon";
			break;
			default:
			item=Amulets[RNG.Next(Amulets.Count)];
			baseType=" Amulet";
			break;
		}

		item=(GameObject) Instantiate(item, v, Quaternion.identity) as GameObject;
		ItemScript IS=(ItemScript)item.GetComponent(typeof(ItemScript));
        IS.justFound = true;
		IS.EquipLocation=EquipLocs[r];
        IS.baseID = r;
		int affixNumber=RNG.Next(6);
        if(affixNumber!=5)
        {
            if(RNG.Next(100)<CS.Stats[12])
            {
                affixNumber++;
            }
        }
		IS.Name="+"+affixNumber.ToString()+baseType;
		IS.CS=CS;
		IS.IM=this;
		IS.ItemBox=ItemBox;
		IS.Duration=RNG.Next(2,21);
		IS.Value=RNG.Next(100,5000);
		//generate affixes watch min and max
		for(int i=0;i<affixNumber;i++)
		{
			int statPoint=RNG.Next(15);
			int affixid=0;
			if(statPoint==14) //if affixid is increased duration
			{
				affixid=1;
			}
			ItemScript.Affix affix=new ItemScript.Affix(10,100, affixid,statPoint, IS);
			IS.AffixList.Add(affix);
		}

		canEquip=true;
        return item;
	}

	// Use this for initialization
	void Start () {
		CS=(CharacterScript)Character.GetComponent(typeof(CharacterScript));
        initializeChar();

        SM = (ScenarioManager)GetComponent(typeof(ScenarioManager));

        for(int i=0;i<6;i++)
        {
            CurseLocs[i]=curseHolder.transform.GetChild(i).position;
        }

        IS1 =(ItemScript)GetComponent(typeof(ItemScript));
	}
	
    void cursesFoiledAgain()
    {
        Application.LoadLevel(1);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
