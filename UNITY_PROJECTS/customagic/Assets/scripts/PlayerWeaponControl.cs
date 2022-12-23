using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponControl : MonoBehaviour {

    public List<WeaponScript> Weapons;
    int index;
    bool hasSelected;
    // Use this for initialization
    void Start () {
	
	}

    public void SelectNext()
    {
        if (Weapons.Count > 0)
        {
            if (hasSelected)
            {
                GetComponent<HighlightScript>().StopHiLight();
                index = (index + 1) % Weapons.Count;
            }
            GetComponent<HighlightScript>().BeginHilight(Weapons[index].GetComponent<SpriteRenderer>());
            hasSelected = true;
        }
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SelectNext();
        }
        if(Input.GetMouseButtonDown(1))
        {
            if (hasSelected)
            {
                Weapons[index].targetPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Weapons[index].CancelInvoke();
                Weapons[index].Invoke("SetHolding", 1f);
                Weapons[index].WeapIndex = -1;
                Weapons[index].Debuff();
                Weapons.RemoveAt(index);
                hasSelected = false;
                GetComponent<HighlightScript>().StopHiLight();
                if (index>0 && index == Weapons.Count)
                    index--;
            }
        }
	
	}
}
