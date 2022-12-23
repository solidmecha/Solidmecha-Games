using UnityEngine;
using System.Collections;

public class PassivesScript : MonoBehaviour {

    public int PassiveID;
    public string Description;
    public bool SingleUse;
    public string Name;

	// Use this for initialization
	void Start () {
	
	}

    public void SetPassive()
    {
        switch(PassiveID)
        {
            case 0:
                Name = "Final Stand.";
                Description = "Remains at 1 HP the first time taking fatal damage.";
                break;
            case 1:
                Name = "Hearty.";
                Description = "Additional 250 HP.";
                GetComponent<CharacterScript>().HP[1] += 250;
                if (GetComponent<CharacterScript>().HP[1] > 999)
                    GetComponent<CharacterScript>().HP[1] = 999;
                GetComponent<CharacterScript>().HP[0] = GetComponent<CharacterScript>().HP[1];
                break;
            case 2:
                Name = "Boosting.";
                Description = "Additional 2 BP.";
                GetComponent<CharacterScript>().BP[0] += 2;
                GetComponent<CharacterScript>().BP[1] += 2;
                break;
            case 3:
                Name = "Quick.";
                Description = "1 Additional movement range.";
                GetComponent<CharacterScript>().Movement += 1;
                break;
            case 4:
                Name = "Lucky Damage.";
                Description = "Damage is rolled twice.";
                break;
            case 5:
                Name = "Armored.";
                Description = "Additional 5 armor.";
                GetComponent<CharacterScript>().Armor += 5;
                break;
            case 6:
                Name = "Crit Chance.";
                Description = "10% chance to deal double damage.";
                GetComponent<CharacterScript>().Armor += 5;
                break;
            case 7:
                Name = "Far Shot.";
                Description = "1 addition skill range.";
                GetComponent<SkillScript>().Range += 1;
                break;
            case 8:
                Name = "Evasive.";
                Description = "Additional 3 evasion.";
                GetComponent<CharacterScript>().Evasion += 3;
                break;
            case 9:
                Name = "Fire Aura";
                Description = "Deals damage to adjacent characters at end of turn.";
                break;
            default:
                Description = "None.";
                break;
        }
    }

    
	
	// Update is called once per frame
	void Update () {
	
	}
}
