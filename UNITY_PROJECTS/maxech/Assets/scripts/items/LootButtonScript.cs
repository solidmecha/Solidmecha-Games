using UnityEngine;
using System.Collections;

public class LootButtonScript : MonoBehaviour {

    public ItemScript IS;

	// Use this for initialization
	void Start () {
	}

    public void Equip()
    {
        if(PlayerControl.singleton.EquippedItems[(int)IS.ItemType] !=null)
        {
            ItemControl.singleton.Unequip(PlayerControl.singleton.EquippedItems[(int)IS.ItemType]);
        }
        ItemControl.singleton.Equip(IS, transform.root.gameObject);
        ItemControl.singleton.LootItem();
    }

    public void EquipSecondary()
    {
        if (PlayerControl.singleton.EquippedItems[(int)IS.ItemType+1] != null)
        {
            ItemControl.singleton.Unequip(PlayerControl.singleton.EquippedItems[(int)IS.ItemType]);
        }
        ItemControl.singleton.EquipSecondary(IS, transform.gameObject);
        ItemControl.singleton.LootItem();
    }

    public void Sell()
    {
        ItemControl.singleton.SellItem(IS, transform.root.gameObject);
        ItemControl.singleton.LootItem();
    }

    public void Reroll()
    {
        if (PlayerControl.singleton.Creds >= 1000)
            ItemControl.singleton.RerollItem(IS, transform.gameObject);
    }

    public void StartWave()
    {
        WaveControl.singleton.StartWave();
    }
}
