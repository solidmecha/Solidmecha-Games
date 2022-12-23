using UnityEngine;
using System.Collections.Generic;

public class ItemControl : MonoBehaviour {

    public static ItemControl singleton;
    public List<GameObject> Previews;
    public GameObject ItemCanvas;
    public int ItemCount;

    public enum Mod {
        FlatHP,
        FlatPower,
        FlatHeat,
        incHP,
        incPower,
        incHeat,
        FlatDmg,
        incDmg,
        WeaponCDR,
        ShieldCDR,
        DashCDR,
        FlatShields,
        HeatLossPerSec,
        HPperSec,
        LifeOnHit,
        LessHeatOnHit,
        PercentHeatAddedDmg,
        PercentPowerAddedDmg,
        PercentMissingHPaddedDmg,
        Crit,
        moveSpd
    }

    public GameObject DisplayItem(Vector3 pos, ItemScript IS)
    {
        GameObject Go = Instantiate(ItemCanvas) as GameObject;
        Go.transform.position = pos;
        Go.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = IS.ItemType.ToString();
        Go.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "+"+IS.HP.ToString()+" HP";
        if(IS.CD > 0)
            Go.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = IS.CD.ToString() + " CD";
        Go.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text =  IS.PowerReq.ToString() + " Power Req";
        Go.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text =IS.HeatGen.ToString() + " Ht/s";
        for (int i = 0; i < IS.ModDescriptions.Count; i++)
            Go.transform.GetChild(5 + i).GetComponent<UnityEngine.UI.Text>().text = IS.ModDescriptions[i];
        Go.GetComponent<LootButtonScript>().IS = IS;
        Go.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Sell for $" + IS.SellValue.ToString();
        return Go;
    }

    public void LootItem()
    {
        ItemCount--;
        if (ItemCount == 0)
            WaveControl.singleton.ShowStart();
    }

    public void CleanUp()
    {
        foreach (GameObject g in Previews)
            Destroy(g);
        Previews.Clear();
    }

    public void DropLoot()
    {
        int it = GameControl.singleton.RNG.Next(5);
        GameObject go=DisplayItem(GameControl.singleton.startCanvasParent.GetChild(2).position, GenerateItem((GameControl.ItemType)it, GameControl.singleton.CurrentLvl)) as GameObject;
        Destroy(go.transform.GetChild(10).gameObject);
        Previews.Add(go);
        go=DisplayItem(GameControl.singleton.startCanvasParent.GetChild(1).position, PlayerControl.singleton.EquippedItems[it]);
        Destroy(go.transform.GetChild(9).gameObject);
        Destroy(go.transform.GetChild(10).gameObject);
        Destroy(go.transform.GetChild(11).gameObject);
        Destroy(go.transform.GetChild(12).gameObject);
        Previews.Add(go);
        DisplayWeapon(GameControl.singleton.startCanvasParent.GetChild(3).position, GenerateWeapon(GameControl.singleton.CurrentLvl));
        go= DisplayWeapon(GameControl.singleton.startCanvasParent.GetChild(5).position, PlayerControl.singleton.EquippedItems[5]);
        Destroy(go.transform.GetChild(9).gameObject);
        Destroy(go.transform.GetChild(10).gameObject);
        Destroy(go.transform.GetChild(11).gameObject);
        Destroy(go.transform.GetChild(12).gameObject);
        Previews.Add(go);
        go = DisplayWeapon(GameControl.singleton.startCanvasParent.GetChild(4).position, PlayerControl.singleton.EquippedItems[6]);
        Destroy(go.transform.GetChild(9).gameObject);
        Destroy(go.transform.GetChild(10).gameObject);
        Destroy(go.transform.GetChild(11).gameObject);
        Destroy(go.transform.GetChild(12).gameObject);
        ItemCount = 2;
        Previews.Add(go);
    }

    public GameObject DisplayWeapon(Vector3 pos, ItemScript IS)
    {
        GameObject Go = Instantiate(ItemCanvas) as GameObject;
        Go.transform.position = pos;
        Go.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = IS.WeaponType.ToString();
        Go.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = IS.DamageRange[0].ToString() + " - " + IS.DamageRange[1].ToString() + "Dmg";
        Go.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = IS.CD.ToString()+" CD";
        Go.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = IS.PowerReq.ToString() + " Power Req";
        Go.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = IS.HeatGen.ToString() + " Ht";
        for (int i = 0; i < IS.ModDescriptions.Count; i++)
            Go.transform.GetChild(5 + i).GetComponent<UnityEngine.UI.Text>().text = IS.ModDescriptions[i];
        Go.GetComponent<LootButtonScript>().IS = IS;
        Go.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text="Sell for $"+IS.SellValue.ToString();
        return Go;
    }

    public ItemScript GenerateItem(GameControl.ItemType itemType, int lvl)
    {
        ItemScript IS = gameObject.AddComponent<ItemScript>();
        IS.ItemType = itemType;
        IS.Mods = new List<Mod> { };
        IS.ModVals = new List<int> { };
        IS.ModDescriptions = new List<string> { };
        int modCount = GameControl.singleton.RNG.Next(2, 5);
        switch (itemType)
        {
            case GameControl.ItemType.PowerCore:
                IS.HP = GameControl.singleton.RNG.Next(10, 20);
                IS.PowerReq = 0;
                IS.HeatGen = -1;
                IS.SellValue = GameControl.singleton.RNG.Next(1, 100) * 100;
                IS.Mods.Add(Mod.FlatPower);
                int v = GameControl.singleton.RNG.Next(10 + lvl, 21 + lvl * 10);
                IS.ModVals.Add(v);
                IS.ModDescriptions.Add(v.ToString()+" Power");
                modCount--;
                List<Mod> Ms = PossibleMods(GameControl.ItemType.PowerCore);
                while(modCount>0)
                {
                    Mod toAdd = Ms[GameControl.singleton.RNG.Next(Ms.Count)];
                    int val = RollModVal(toAdd);
                    IS.Mods.Add(toAdd);
                    IS.ModVals.Add(val);
                    IS.ModDescriptions.Add(val.ToString() + " " + toAdd.ToString());
                    modCount--;
                }
            break;
            case GameControl.ItemType.Chasis:
                IS.HP = GameControl.singleton.RNG.Next(50, 100);
                IS.PowerReq = GameControl.singleton.RNG.Next(10);
                IS.HeatGen = -1;
                IS.SellValue = GameControl.singleton.RNG.Next(1, 100) * 100;
                Ms = PossibleMods(GameControl.ItemType.Chasis);
                while (modCount > 0)
                {
                    Mod toAdd = Ms[GameControl.singleton.RNG.Next(Ms.Count)];
                    int val = RollModVal(toAdd);
                    IS.Mods.Add(toAdd);
                    IS.ModVals.Add(val);
                    IS.ModDescriptions.Add(val.ToString() + " " + toAdd.ToString());
                    modCount--;
                }
                break;
            case GameControl.ItemType.Thruster:
                IS.HP = GameControl.singleton.RNG.Next(50, 100);
                IS.PowerReq = GameControl.singleton.RNG.Next(10);
                IS.HeatGen = GameControl.singleton.RNG.Next(1, 11);
                IS.SellValue = GameControl.singleton.RNG.Next(1, 100) * 100;
                Ms = PossibleMods(GameControl.ItemType.Thruster);
                IS.CD = GameControl.singleton.RNG.Next(10, 31);
                while (modCount > 0)
                {
                    Mod toAdd = Ms[GameControl.singleton.RNG.Next(Ms.Count)];
                    int val = RollModVal(toAdd);
                    IS.Mods.Add(toAdd);
                    IS.ModVals.Add(val);
                    IS.ModDescriptions.Add(val.ToString() + " " + toAdd.ToString());
                    modCount--;
                }
              break;
            case GameControl.ItemType.Shields:
                IS.HP = GameControl.singleton.RNG.Next(50, 100);
                IS.PowerReq = GameControl.singleton.RNG.Next(10);
                IS.HeatGen = GameControl.singleton.RNG.Next(1,11);
                IS.SellValue = GameControl.singleton.RNG.Next(1, 100) * 100;
                IS.Mods.Add(Mod.FlatShields);
                v = GameControl.singleton.RNG.Next(50, 101);
                IS.ModVals.Add(v);
                IS.ModDescriptions.Add(v.ToString() +" shields!");
                IS.CD = GameControl.singleton.RNG.Next(30, 121);
                modCount--;
                Ms = PossibleMods(GameControl.ItemType.Shields);
                while (modCount > 0)
                {
                    Mod toAdd = Ms[GameControl.singleton.RNG.Next(Ms.Count)];
                    int val = RollModVal(toAdd);
                    IS.Mods.Add(toAdd);
                    IS.ModVals.Add(val);
                    IS.ModDescriptions.Add(val.ToString() + " " + toAdd.ToString());
                    modCount--;
                }
                break;
            case GameControl.ItemType.Hydraulics:
                IS.HP = GameControl.singleton.RNG.Next(50, 100);
                IS.PowerReq = GameControl.singleton.RNG.Next(10);
                IS.HeatGen = -1;
                IS.SellValue = GameControl.singleton.RNG.Next(1, 100) * 100;
                Ms = PossibleMods(GameControl.ItemType.Hydraulics);
                while (modCount > 0)
                {
                    Mod toAdd = Ms[GameControl.singleton.RNG.Next(Ms.Count)];
                    int val = RollModVal(toAdd);
                    IS.Mods.Add(toAdd);
                    IS.ModVals.Add(val);
                    IS.ModDescriptions.Add(val.ToString() + " " + toAdd.ToString());
                    modCount--;
                }
                break;
        }
        IS.Mods.Add(Mod.FlatHP);
        IS.ModVals.Add(IS.HP);
        if (IS.HeatGen < 0)
        {
            IS.Mods.Add(Mod.HeatLossPerSec);
            IS.ModVals.Add(IS.HeatGen);
        }
        for (int i = 0; i < IS.Mods.Count; i++)
            ChangeItemLocalMods(IS.Mods[i], IS.ModVals[i], IS);
        return IS;
    }

    public List<Mod> PossibleMods(GameControl.ItemType item)
    {
        List<Mod> Mods=new List < Mod > {
            Mod.FlatHP,
        Mod.FlatPower,
        Mod.FlatHeat,
        Mod.incHP,
        Mod.FlatShields,
        Mod.HeatLossPerSec,
        Mod.HPperSec,
        Mod.LifeOnHit,
        Mod.LessHeatOnHit,
        Mod.PercentHeatAddedDmg,
        Mod.PercentPowerAddedDmg,
        Mod.PercentMissingHPaddedDmg,
        Mod.Crit,
        };

        switch (item)
        {
            case GameControl.ItemType.Weapon:
                Mods.Add(Mod.WeaponCDR);
                Mods.Add(Mod.FlatDmg);
                Mods.Add(Mod.incDmg);
                break;
            case GameControl.ItemType.Chasis:
                Mods.Add(Mod.incHeat);
                break;
            case GameControl.ItemType.Thruster:
                Mods.Add(Mod.moveSpd);
                Mods.Add(Mod.DashCDR);
                break;
            case GameControl.ItemType.Shields:
                Mods.Add(Mod.ShieldCDR);
                break;
            case GameControl.ItemType.PowerCore:
                Mods.Add(Mod.incPower);
                break;

        default: return Mods;
    }
        return Mods;
    }

    public int RollModVal(Mod M)
    {
        switch(M)
        {
        case Mod.FlatHP:
                return GameControl.singleton.CurrentLvl*5+GameControl.singleton.RNG.Next(30);
        case Mod.FlatPower:
                return GameControl.singleton.CurrentLvl * 10 + GameControl.singleton.RNG.Next(10,50);
            case Mod.FlatHeat:
                return GameControl.singleton.CurrentLvl * 10 + GameControl.singleton.RNG.Next(10)*10;
            case Mod.incHP:
                return  10+GameControl.singleton.RNG.Next(GameControl.singleton.CurrentLvl+10) *10;
            case Mod.incPower:
                return 10 + GameControl.singleton.RNG.Next(GameControl.singleton.CurrentLvl+10) *10;
            case Mod.incHeat:
                return 5 + GameControl.singleton.RNG.Next(30);
            case Mod.FlatDmg:
                return GameControl.singleton.CurrentLvl+ GameControl.singleton.RNG.Next(30);
            case Mod.incDmg:
                return GameControl.singleton.CurrentLvl * 10 + GameControl.singleton.RNG.Next(100)*10;
            case Mod.WeaponCDR:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(10)*5;
            case Mod.ShieldCDR:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(30);
            case Mod.DashCDR:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(30);
            case Mod.FlatShields:
                return GameControl.singleton.RNG.Next(100);
            case Mod.HeatLossPerSec:
                return -1*(1 + GameControl.singleton.RNG.Next(14));
            case Mod.HPperSec:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(30);
            case Mod.LifeOnHit:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(30);
            case Mod.LessHeatOnHit:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(30);
            case Mod.PercentHeatAddedDmg:
                return GameControl.singleton.CurrentLvl * 5 + GameControl.singleton.RNG.Next(10);
            case Mod.PercentPowerAddedDmg:
                return GameControl.singleton.CurrentLvl + GameControl.singleton.RNG.Next(10);
            case Mod.PercentMissingHPaddedDmg:
                return GameControl.singleton.CurrentLvl + GameControl.singleton.RNG.Next(10);
            case Mod.Crit:
                return GameControl.singleton.CurrentLvl + GameControl.singleton.RNG.Next(30);
            case Mod.moveSpd:
                return GameControl.singleton.RNG.Next(1,3);
            default:
                return 0;
        }
    }

    public ItemScript GenerateWeapon(int lvl)
    {
       ItemScript IS= gameObject.AddComponent<ItemScript>();
        IS.ItemType = GameControl.ItemType.Weapon;
        IS.WeaponType = (GameControl.WeaponType)GameControl.singleton.RNG.Next(6);
        IS.Mods = new List<Mod> { };
        IS.ModVals = new List<int> { };
        IS.ModDescriptions = new List<string> { };
        int modCount = GameControl.singleton.RNG.Next(2, 5);
        IS.SellValue = GameControl.singleton.RNG.Next(1, 101) * 100;
        switch (IS.WeaponType)
        {
            case GameControl.WeaponType.Gat:
                IS.DamageRange = new int[2] { lvl + GameControl.singleton.RNG.Next(lvl + 4), 6 + lvl + GameControl.singleton.RNG.Next(lvl + 8) };
                IS.HeatGen = GameControl.singleton.RNG.Next(3, 10);
                IS.PowerReq = GameControl.singleton.RNG.Next(lvl + 15 + GameControl.singleton.RNG.Next(lvl + 5, 2 * lvl + 10));
                IS.CD = GameControl.singleton.RNG.Next(1, 5);
                break;
            case GameControl.WeaponType.Nova:
                IS.DamageRange = new int[2] { lvl + GameControl.singleton.RNG.Next((int)1.5f*lvl + 8), 10 + (int)(1.5f*lvl)+ GameControl.singleton.RNG.Next((int)1.5f*lvl + 10) };
                IS.HeatGen = GameControl.singleton.RNG.Next(8, 16);
                IS.PowerReq = GameControl.singleton.RNG.Next(lvl + 15 + GameControl.singleton.RNG.Next(lvl + 5, 2 * lvl + 10));
                IS.CD = GameControl.singleton.RNG.Next(1, 5);
                break;
            case GameControl.WeaponType.Beam:
                IS.DamageRange = new int[2] { lvl + GameControl.singleton.RNG.Next((int)(1.5f * lvl) + 20), 25 + (int)(1.5f * lvl) + GameControl.singleton.RNG.Next((int)1.5f * lvl + 5) };
                IS.HeatGen = GameControl.singleton.RNG.Next(30, 50+2*lvl);
                IS.PowerReq = GameControl.singleton.RNG.Next(lvl + 15 + GameControl.singleton.RNG.Next(lvl + 5, 2 * lvl + 10));
                IS.CD = GameControl.singleton.RNG.Next(2, 10);
                break;
            case GameControl.WeaponType.OrbitalCannon:
                IS.DamageRange = new int[2] { lvl + GameControl.singleton.RNG.Next((int)(1.5f * lvl) + 10), 10 + (int)(1.5f * lvl) + GameControl.singleton.RNG.Next((int)1.5f * lvl + 12) };
                IS.HeatGen = GameControl.singleton.RNG.Next(16,30+2*lvl);
                IS.PowerReq = GameControl.singleton.RNG.Next(lvl + 15 + GameControl.singleton.RNG.Next(lvl + 5, 2 * lvl + 10));
                IS.CD = GameControl.singleton.RNG.Next(1, 5);
                break;
            case GameControl.WeaponType.Missle:
                IS.DamageRange = new int[2] { lvl + GameControl.singleton.RNG.Next((int)(1.5f * lvl) + 10), 10 + (int)(1.5f * lvl) + GameControl.singleton.RNG.Next((int)1.5f * lvl + 12) };
                IS.HeatGen = GameControl.singleton.RNG.Next(16, 30 + 2 * lvl);
                IS.PowerReq = GameControl.singleton.RNG.Next(lvl + 15 + GameControl.singleton.RNG.Next(lvl + 5, 2 * lvl + 10));
                IS.CD = GameControl.singleton.RNG.Next(1, 5);
                break;
            case GameControl.WeaponType.Burst:
                IS.DamageRange = new int[2] { lvl+GameControl.singleton.RNG.Next(2 * lvl + 15), 20 + 2*lvl + GameControl.singleton.RNG.Next((int)2f * lvl + 6) };
                IS.HeatGen = GameControl.singleton.RNG.Next(16, 30 + 2 * lvl);
                IS.PowerReq = GameControl.singleton.RNG.Next(lvl + 15 + GameControl.singleton.RNG.Next(lvl + 5, 2 * lvl + 10));
                IS.CD = GameControl.singleton.RNG.Next(1, 5);
                break;
        }
        List<Mod> Ms = PossibleMods(GameControl.ItemType.Weapon);
        while (modCount > 0)
        {
            Mod toAdd = Ms[GameControl.singleton.RNG.Next(Ms.Count)];
            int val = RollModVal(toAdd);
            IS.Mods.Add(toAdd);
            IS.ModVals.Add(val);
            IS.ModDescriptions.Add(val.ToString() + " " + toAdd.ToString());
            modCount--;
        }
        return IS;
    }

    public void EquipSecondary(ItemScript IS, GameObject Canvas)
    {
        PlayerControl.singleton.EquippedItems[(int)IS.ItemType+1] = IS;
        PlayerControl.singleton.takeDamage(IS.PowerReq, GameControl.DamageType.energy);
        for (int i = 0; i < IS.Mods.Count; i++)
        {
            PlayerChangeByMod(IS.Mods[i], IS.ModVals[i]);
        }
        if (IS.ItemType == GameControl.ItemType.Weapon)
        {
            PlayerControl.singleton.SecondaryWS.Damage[0] = IS.DamageRange[0];
            PlayerControl.singleton.SecondaryWS.Damage[1] = IS.DamageRange[1];
            PlayerControl.singleton.SecondaryWS.weaponType = IS.WeaponType;
            PlayerControl.singleton.SecondaryWS.CD[1] = IS.CD;
            PlayerControl.singleton.SecondaryWS.HeatGen = IS.HeatGen;
        }
        Destroy(Canvas);
    }

    public void Equip(ItemScript IS, GameObject Canvas)
    {
        PlayerControl.singleton.EquippedItems[(int)IS.ItemType]=IS;
        PlayerControl.singleton.takeDamage(IS.PowerReq, GameControl.DamageType.energy);
        for(int i=0;i<IS.Mods.Count;i++)
        {
            PlayerChangeByMod(IS.Mods[i], IS.ModVals[i]);
        }

        if (IS.ItemType == GameControl.ItemType.Shields)
            PlayerControl.singleton.ShieldCD[1] = IS.CD;
        else if (IS.ItemType == GameControl.ItemType.Thruster)
            PlayerControl.singleton.DashCD[1] = IS.CD;
        else if(IS.ItemType==GameControl.ItemType.Weapon)
        {
            PlayerControl.singleton.PrimaryWS.Damage[0] = IS.DamageRange[0];
            PlayerControl.singleton.PrimaryWS.Damage[1] = IS.DamageRange[1];
            PlayerControl.singleton.PrimaryWS.weaponType = IS.WeaponType;
            PlayerControl.singleton.PrimaryWS.CD[1] = IS.CD;
            PlayerControl.singleton.PrimaryWS.HeatGen = IS.HeatGen;
        }

        Destroy(Canvas);
    }

    public void Unequip(ItemScript IS)
    {
        PlayerControl.singleton.takeDamage(IS.PowerReq * -1, GameControl.DamageType.energy);
        for (int i = 0; i < IS.Mods.Count; i++)
        {
            PlayerChangeByMod(IS.Mods[i], -1*IS.ModVals[i]);
        }
    }

    public void PlayerChangeByMod(Mod M, int val)
    {
        switch (M)
        {
            case Mod.FlatHP:
                PlayerControl.singleton.hp[0] += val;
                PlayerControl.singleton.hp[1] += val;
                PlayerControl.singleton.updateHP();
                break;
        case Mod.FlatPower:
                PlayerControl.singleton.power[0] += val;
                PlayerControl.singleton.power[1] += val;
                PlayerControl.singleton.updatePower();
                break;
            case Mod.FlatHeat:
                PlayerControl.singleton.heat[1] += val;
                PlayerControl.singleton.updateHeat();
                break;
            case Mod.FlatShields:
                PlayerControl.singleton.shieldValue += val;
                break;
            case Mod.HeatLossPerSec:
                PlayerControl.singleton.HeatLoss += val;
                break;
            case Mod.HPperSec:
                PlayerControl.singleton.HPRegen += val;
                break;
            case Mod.LifeOnHit:
                PlayerControl.singleton.LifeOnHit += val;
                break;
            case Mod.LessHeatOnHit:
                PlayerControl.singleton.HeatOnHit += val;
                break;
            case Mod.PercentHeatAddedDmg:
                PlayerControl.singleton.PercentHeatAddedDmg += val;
                break;
            case Mod.PercentPowerAddedDmg:
                PlayerControl.singleton.PercentPowerAddedDmg += val;
                break;
            case Mod.PercentMissingHPaddedDmg:
                PlayerControl.singleton.PercentMissingHPaddedDmg += val;
                break;
            case Mod.Crit:
                PlayerControl.singleton.Crit += val;
                break;
            case Mod.moveSpd:
                PlayerControl.singleton.speed += val;
                break;
        }
    }

    public void ChangeItemLocalMods(Mod M, int val, ItemScript IS)
    {
        switch (M)
        {
            case Mod.incHP:
                IS.HP = Mathf.RoundToInt(IS.HP * val / 100f);
                break;
            case Mod.incPower:
                IS.Power = Mathf.RoundToInt(IS.Power * val / 100f);
                break;
            case Mod.incHeat:
                IS.HeatGen = Mathf.RoundToInt(IS.HeatGen * val / 100f);
                break;
            case Mod.FlatDmg:
                IS.DamageRange[0] += val;
                IS.DamageRange[1] += val;
                break;
            case Mod.incDmg:
                IS.DamageRange[0] = Mathf.RoundToInt(IS.DamageRange[0] * val / 100f);
                IS.DamageRange[1] = Mathf.RoundToInt(IS.DamageRange[1] * val / 100f);
                break;
            case Mod.WeaponCDR:
                IS.CD *= (val / 100f);
                break;
            case Mod.ShieldCDR:
                IS.CD *= (val / 100f);
                break;
            case Mod.DashCDR:
                IS.CD *= (val / 100f);
                break;
            default:
                break;
        }
    }

    public void SellItem(ItemScript IS, GameObject Canvas)
    {
        PlayerControl.singleton.Creds += IS.SellValue;
        PlayerControl.singleton.UpdateCreds();
        Destroy(IS);
        Destroy(Canvas);
    }

   public void RerollItem(ItemScript IS, GameObject Canvas)
    {
        PlayerControl.singleton.Creds -= 1000;
        PlayerControl.singleton.UpdateCreds();
        if (IS.ItemType != GameControl.ItemType.Weapon)
        {
            Destroy(DisplayItem(Canvas.transform.position, GenerateItem(IS.ItemType, GameControl.singleton.CurrentLvl)).transform.GetChild(10).gameObject);
        }
        else
            DisplayWeapon(Canvas.transform.position, GenerateWeapon(GameControl.singleton.CurrentLvl));
        Destroy(IS);
        Destroy(Canvas);
    }


    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
