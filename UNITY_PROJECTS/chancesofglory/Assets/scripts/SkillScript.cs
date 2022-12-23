using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public string Description;
    public int ID;
    public bool XSkill;
    public bool Matches;
    public bool SingleTarget;
    public int[] SkillMatch; //sword, mana, time, shield, scroll, heart
    public int ManaCost;
    public int DamageValue;
    public int DamageMultiplier;
    public int CDValue;
    public int CDMultiplier;
    public int CD;
    public GameObject CDObj;
    public int ResourceCost;
    int ResourceValue;
    GameObject SkillEffectObject;

    private void OnMouseEnter()
    {
        if(GameControl.singleton.CurrentState != GameControl.GameState.SkillEffect)
            GameControl.singleton.MessageText.text = Description;
    }

    private void OnMouseDown()
    {
        if (GameControl.singleton.CurrentState == GameControl.GameState.PlayerSkills)
        {
            if(GetComponentInParent<StatScript>().HP[0]==0)
            {
                GameControl.singleton.MessageText.text = "Must be Revived.";
                return;
            }
            if (CD > 0 && DiceControl.singleton.CheckSameMatch(2))
            {
                CD -= DiceControl.singleton.SelectedDiceTypes.Count;
                if (CD <= 0)
                {
                    CD = 0;

                    Destroy(CDObj);
                }
                else
                    CDObj.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = CD.ToString();
                DiceControl.singleton.DestroyUsedDice();
                return;
            }
            else if (CD > 0)
            {
                GameControl.singleton.MessageText.text = "Skill on Cooldown.";
                return;
            }
            if (Matches)
            {
                if (ManaCost <= transform.parent.GetComponent<StatScript>().MP[0])
                {
                    if (XSkill && DiceControl.singleton.CheckSameMatch(SkillMatch[0]))
                    {
                        DamageValue = DiceControl.singleton.SelectedDiceTypes.Count;
                        CDValue = DiceControl.singleton.SelectedDiceTypes.Count;
                        int OriginalCostTemp = ResourceCost;
                        if(ResourceCost== -1)
                        {
                            ResourceCost= DiceControl.singleton.SelectedDiceTypes.Count;
                        }
                        if (transform.parent.GetChild(3).GetComponent<SkillScript>().ResourceValue < ResourceCost)
                        {
                            GameControl.singleton.MessageText.text = "Not enough resources.";
                            ResourceCost = OriginalCostTemp;
                            return;
                        }
                        OriginalCostTemp = ManaCost;
                        if (ManaCost < 0)
                            ManaCost = -1 * ManaCost * DiceControl.singleton.SelectedDiceTypes.Count;
                        if (ManaCost > transform.parent.GetComponent<StatScript>().MP[0])
                        {
                            GameControl.singleton.MessageText.text = "Not enough Mana.";
                            ManaCost = OriginalCostTemp;
                            return;
                        }
                        transform.parent.GetComponent<StatScript>().UpdateMP(ManaCost);
                        ManaCost = OriginalCostTemp;

                    }
                    else if (DiceControl.singleton.CheckMatch(SkillMatch))
                    {
                        if (transform.parent.GetChild(3).GetComponent<SkillScript>().ResourceValue < ResourceCost)
                        {
                            GameControl.singleton.MessageText.text = "Prepare better next time.";
                            return;
                        }
                        transform.parent.GetComponent<StatScript>().UpdateMP(ManaCost);
                    }
                    else
                    {
                        GameControl.singleton.MessageText.text = "Incorrect Dice Type Selected.";
                        return;
                    }

                    SkillEffectByID();
                    Damage(DamageValue * DamageMultiplier);
                    ChangeCD(CDValue * CDMultiplier);
                    GameControl.singleton.MostRecentSkill = this;
                    DiceControl.singleton.DestroyUsedDice();
                }
                else
                {
                    GameControl.singleton.MessageText.text = "Not enough Mana.";
                    return;
                }
            }
            else
            {
                SkillEffectByID();
            }
        }
        else if(GameControl.singleton.CurrentState==GameControl.GameState.SkillEffect && GameControl.singleton.SkillDurationCheck(13))
        {
            GameControl.singleton.MostRecentSkill.DelayOfPain(this);
        }
    }

    void Damage(int val)
    {
        if (SingleTarget)
        {
            if (GameControl.singleton.Target != null)
            {
                if(val>0 && GameControl.singleton.Target.CompareTag("Player"))
                {
                    GameControl.singleton.Target = GameControl.singleton.EnemyParty[GameControl.singleton.RNG.Next(GameControl.singleton.EnemyParty.Count)];
                    GameControl.singleton.TargetText.text = "Current Target: "+GameControl.singleton.Target.name;
                }
                GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP(val);
            }
        }
        else
        {
            if (val > 0)
            {
                foreach (GameObject g in GameControl.singleton.EnemyParty)
                    g.GetComponent<StatScript>().UpdateHP(val);
            }
            else
            {
                foreach (GameObject g in GameControl.singleton.SelectedCharacters)
                    g.GetComponent<StatScript>().UpdateHP(val);
            }
        }
    }

    public void UpdateResourceValue(int Change, bool hasText)
    {
        ResourceValue += Change;
        if(hasText)
            transform.parent.GetChild(2).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = ResourceValue.ToString();
    }

    void SkillEffectByID()
    {
        switch (ID)
        {
            case 1: //Ranger poison Arrow
                if (GameControl.singleton.RNG.Next(2) == 1)
                {
                    DamageValue = 2;
                    Description = "Deal " + DamageValue.ToString() + " Damage. 50% chance Damage is set to to 2.";
                    GameControl.singleton.MessageText.text = Description;
                }
                break;
            case 2: //Ranger Way of the hunter
                transform.parent.GetChild(2).GetComponent<SkillScript>().DamageValue *= 2;
                transform.parent.GetChild(2).GetComponent<SkillScript>().Description = "Deal " + transform.parent.GetChild(2).GetComponent<SkillScript>().DamageValue.ToString() + " Damage. 50% chance Damage is set to to 2.";
                break;
            case 3: //Knight Block
                transform.parent.GetComponent<StatScript>().UpdateArmor(2*CDValue);
                break;
            case 4://Knight Engage;
                GameControl.singleton.Taunter = transform.parent.gameObject;
                transform.parent.GetComponent<StatScript>().UpdateArmor(2);
                break;
            case 5: //Knight Retribution
                {
                    foreach (GameObject g5 in GameControl.singleton.SelectedCharacters)
                        g5.GetComponent<StatScript>().UpdateArmor(DamageValue);
                    GameControl.singleton.DurationSkillEffects.Add(new int[] { 5, DamageValue }); //resolved in StatScript
                }
                break;
            case 6: //Healer Healing Winds
                {
                    foreach (GameObject g6 in GameControl.singleton.SelectedCharacters)
                    {
                        g6.GetComponent<StatScript>().UpdateHP(-1*DamageValue);
                        if (DamageValue > 2)
                        {
                            foreach (SkillScript s in g6.transform.GetComponentsInChildren<SkillScript>())
                            {
                                if (s.CD > 0)
                                {
                                    s.CD--;
                                    s.CDObj.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = s.CD.ToString();
                                    if (s.CD == 0)
                                        Destroy(s.CDObj);
                                }
                            }
                        }
                    }
                }
                break;
            case 7://Healer Divine Intervention
                    foreach (DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
                    {
                        if (d.id < 3)
                            return;
                    }
                    GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP(-1*GameControl.singleton.Target.GetComponent<StatScript>().HP[1]);
                GameControl.singleton.Target.GetComponent<StatScript>().UpdateMP(-1*GameControl.singleton.Target.GetComponent<StatScript>().MP[1]);
                ChangeCD(CDValue);
                break;
            case 8: //Dragon Ice Breath
                transform.parent.GetComponent<StatScript>().UpdateArmor(DamageValue);
                break;
            case 9://Dragon Hoard
                transform.parent.GetComponent<StatScript>().UpdateHP(-4);
                transform.parent.GetComponent<StatScript>().UpdateMP(-4);
                transform.parent.GetComponent<StatScript>().UpdateArmor(4);
                foreach (SkillScript s in transform.parent.GetComponentsInChildren<SkillScript>())
                {
                    if (s.CD > 0)
                    {
                        s.CD -= 4;
                        s.CDObj.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = s.CD.ToString();
                        if (s.CD <= 0)
                        {
                            s.CD = 0;
                            Destroy(s.CDObj);
                        }
                    }
                }
                break;
            case 10://Dragon Unleashed Fury
                SkillEffectObject = Instantiate(DiceControl.singleton.PlayerDiceRef, new Vector2(1.5f, -6.29f), Quaternion.identity) as GameObject;
                GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.SkillEffect, GameControl.GameState.PlayerSkills, 2f);
                Destroy(SkillEffectObject.transform.GetChild(8).gameObject);
                Destroy(SkillEffectObject.transform.GetChild(7).gameObject);
                StartCoroutine(UnleashFury());
                GameControl.singleton.MessageText.text = "";
                break;
            case 11: //Chrono blinkback
                StatScript S = GameControl.singleton.Target.GetComponent<StatScript>();
                S.UpdateHP(S.HP[0] - S.LastTurnStats[0]);
                S.UpdateArmor(S.LastTurnStats[1] - S.Armor);
                S.UpdateMP(S.MP[0] - S.LastTurnStats[2]);
                break;
            case 12://Chrono Looped Wrath
                GameControl.singleton.DurationSkillEffects.Add(new int[2] { 12, 1 });
                break;
            case 13: //chrono pain of delay
                GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.SkillEffect, GameControl.GameState.SkillEffect, 999f);
                GameControl.singleton.DurationSkillEffects.Add(new int[2] { 13, 0 });
                GameControl.singleton.MessageText.text = "Select Target Skill.";
                break;
            case 14://Chrono Time Run
                GameControl.singleton.DurationSkillEffects.Add(new int[2] { 14, 1 }); //resolved in HandlePlayerEot
                break;
            case 15: //Diviner Forecast
                foreach (DieScript d in GetComponentsInChildren<DieScript>())
                    d.Rollit();
                break;
            case 16://Diviner prevention
                GameControl.singleton.DurationSkillEffects.Add(new int[2] {16, 1});
                break;
            case -16: //Diviner Mindbreak
                transform.parent.GetChild(3).GetComponent<SkillScript>().UpdateResourceValue(-1 * ResourceCost, true);
                break;
            case 17://Druid Human
                DamageValue = DiceControl.singleton.SelectedDiceTypes.Count;
                if (GameControl.singleton.Target.CompareTag("Player"))
                    Damage(DamageValue * -2);
                else
                    Damage(2*DamageValue);
                DiceControl.singleton.DestroyUsedDice();
                ChangeCD(DamageValue);
                transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetComponentInParent<SpriteHolder>().sprites[0];
                transform.parent.GetChild(0).localScale = new Vector2(1, 1);
                break;
            case 18://Druid Scorpion
               // GameControl.singleton.DurationSkillEffects.Add(new int[2] { 18, 1 });
                transform.parent.GetChild(0).localScale = new Vector2(.5f, .5f);
                transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetComponentInParent<SpriteHolder>().sprites[1];
                int i18 = 0;
                foreach (DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
                {
                    if (d.id == 0 || d.id == 4)
                    {
                        i18++;
                    }
                }
                for(int i=0;i<8;i++)
                {
                    if (DiceControl.singleton.LastEnemyDice[i] == 5)
                        i18 += 3;
                }
                Damage(i18);
                break;
            case 19://Druid spider
               // GameControl.singleton.DurationSkillEffects.Add(new int[2] { 19, 1 });
                transform.parent.GetChild(0).localScale = new Vector2(.5f, .5f);
                transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetComponentInParent<SpriteHolder>().sprites[2];
                int i19 = 0;
                foreach (DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
                {
                    if (d.id == 2 || d.id == 4)
                    {
                        i19++;
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    if (DiceControl.singleton.LastEnemyDice[i] == 3)
                        i19 += 3;
                }
                Damage(i19);
                break;
            case 20: //druid wolf
               // GameControl.singleton.DurationSkillEffects.Add(new int[2] { 20, 1 });
                transform.parent.GetChild(0).localScale = new Vector2(.5f, .5f);
                transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetComponentInParent<SpriteHolder>().sprites[3];
                int i20 = 0;
                foreach (DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
                {
                    if (d.id == 0 || d.id == 5)
                    {
                        i20++;
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    if (DiceControl.singleton.LastEnemyDice[i] == 4)
                        GetComponentInParent<StatScript>().UpdateHP(-2);
                }
                Damage(i20);
                break;
            case 21://druid Nature's will
                GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP(-4*DamageValue);
                GameControl.singleton.Target.GetComponent<StatScript>().UpdateArmor(4 * DamageValue);
                break;
            case 22://Fighter Limit Break
                GameControl.singleton.DurationSkillEffects.Add(new int[2] { 22, 1 });
                transform.parent.GetChild(3).GetComponent<SkillScript>().UpdateResourceValue(-1 * ResourceCost, true);
                break;
            case 23://Mage flare
                GameControl.singleton.DurationSkillEffects.Add(new int[2] { 23, 1});
                DamageValue = GameControl.singleton.RNG.Next(5, 16);
                break;
            case 24://Mage Ultima
                GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.SkillEffect, GameControl.GameState.PlayerSkills, 2*DamageValue);
                SkillEffectObject = Instantiate(DiceControl.singleton.PlayerDiceRef, new Vector2(.78f, -6.29f), Quaternion.identity) as GameObject;
                Destroy(SkillEffectObject.transform.GetChild(8).gameObject);
                StartCoroutine(Ultima());
                GameControl.singleton.MessageText.text = "";
                break;
            case 25://Rogue preparation
                if(ResourceValue==0)
                    UpdateResourceValue(1, false);
                GameControl.singleton.MessageText.text = "Toolkits prepared.";
                break;
            case -25://Rogue Backstab
                CDValue = 1;
                break;
            case 26://Rogue Smoke bomb
                GameControl.singleton.DurationSkillEffects.Add(new int[2] { 26, 1});
                GameControl.singleton.ImmunityID = GameControl.singleton.Target.GetComponent<CharacterScript>().index;
                transform.parent.GetChild(3).GetComponent<SkillScript>().UpdateResourceValue(-1 * ResourceCost, false);
                break;
            case -26: //other Rogue Specials
                transform.parent.GetChild(3).GetComponent<SkillScript>().UpdateResourceValue(-1 * ResourceCost, false);
                break;
            case 27://templar mana infusion
                GameControl.singleton.Target.GetComponent<StatScript>().UpdateMP(-2 * DamageValue);
                break;
            case 28://Templar bolster
                GameControl.singleton.Target.GetComponent<StatScript>().UpdateArmor(2 * DamageValue);
                break;
            case 29://Templar fortify
                GameControl.singleton.Taunter = transform.parent.gameObject;
                GetComponentInParent<StatScript>().UpdateArmor(DamageValue);
                break;
            case 30://Templar Shield Shatter
                int i30 = 0;
                foreach(GameObject g30 in GameControl.singleton.SelectedCharacters)
                {
                    int a = g30.GetComponent<StatScript>().Armor;
                    i30 +=a;
                    g30.GetComponent<StatScript>().UpdateArmor(-1*a);
                }
                Damage(DamageMultiplier*i30);
                break;
            case 31: //Jack something?
                if (DiceControl.singleton.SelectedDiceTypes.Count == 2)
                {
                    if (GameControl.singleton.Target.CompareTag("Player"))
                    {
                        if (GameControl.singleton.RNG.Next(3) == 0)
                            GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP(GameControl.singleton.RNG.Next(-4, 0));
                        if (GameControl.singleton.RNG.Next(3) == 1)
                            GameControl.singleton.Target.GetComponent<StatScript>().UpdateMP(GameControl.singleton.RNG.Next(-4, 0));
                        if (GameControl.singleton.RNG.Next(3) == 0)
                            GameControl.singleton.Target.GetComponent<StatScript>().UpdateArmor(GameControl.singleton.RNG.Next(5));
                    }
                    else
                    {
                        if (GameControl.singleton.RNG.Next(2) == 0)
                        {
                            GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP(GameControl.singleton.RNG.Next(-4, 9));
                        }
                        else
                        {
                            Damage(GameControl.singleton.RNG.Next(-2, 6));
                        }
                    }
                    ChangeCD(GameControl.singleton.RNG.Next(1, 5));
                    DiceControl.singleton.DestroyUsedDice();
                }
                else
                {
                    GameControl.singleton.MessageText.text = "Select 2 Dice.";
                }
                break;
            case 32://Jack something better?
                if (DiceControl.singleton.SelectedDiceTypes.Count > 0)
                {
                    DamageValue = DiceControl.singleton.SelectedDiceTypes.Count;
                    if (transform.parent.GetComponent<StatScript>().MP[0] < DamageValue)
                    {
                        GameControl.singleton.MessageText.text = "Not enough Mana.";
                        return;
                    }
                    else
                    {
                        transform.parent.GetComponent<StatScript>().UpdateMP(DamageValue);
                        if (GameControl.singleton.Target.CompareTag("Player"))
                        {
                            if (GameControl.singleton.RNG.Next(2) == 0)
                                GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP(GameControl.singleton.RNG.Next(-3-DamageValue, -1));
                            if (GameControl.singleton.RNG.Next(2) == 1)
                                GameControl.singleton.Target.GetComponent<StatScript>().UpdateMP(GameControl.singleton.RNG.Next(-3-DamageValue, -1));
                            if (GameControl.singleton.RNG.Next(2) == 0)
                                Damage(GameControl.singleton.RNG.Next(1, 4+DamageValue));
                        }
                        else
                        {
                            if (GameControl.singleton.RNG.Next(2) == 0)
                            {
                                GameControl.singleton.Target.GetComponent<StatScript>().UpdateHP((GameControl.singleton.RNG.Next(2, 8) * DamageValue));
                            }
                            else
                            {
                                Damage(GameControl.singleton.RNG.Next(-1, 5) * DamageValue);
                            }
                        }
                        ChangeCD(GameControl.singleton.RNG.Next(2, 7));
                        DiceControl.singleton.DestroyUsedDice();
                    }
                }
                break;
            default:
                break;
        }
    }

    IEnumerator UnleashFury()
    {
        yield return new WaitForSeconds(1f);
        int count = 0;
        foreach(DieScript D in SkillEffectObject.GetComponentsInChildren<DieScript>())
        {
            if (D.id == 4 || D.id == 0)
            {
                D.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                count++;
            }
        }
        yield return new WaitForSeconds(1f);
        if (count == 0)
            Damage(777);
        else
            Damage(count * 7);
        Destroy(SkillEffectObject);
    }

    IEnumerator Ultima()
    {
        yield return new WaitForSeconds(1f);
        int count = 0;
        foreach (DieScript D in SkillEffectObject.GetComponentsInChildren<DieScript>())
        {
            if (D.id == 4 || D.id == 1)
            {
                D.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.magenta;
                count++;
            }
        }
        yield return new WaitForSeconds(1f);
        Damage(3 * count);
        DamageValue--;
        Destroy(SkillEffectObject);
        if (DamageValue > 0)
        {
            SkillEffectObject = Instantiate(DiceControl.singleton.PlayerDiceRef, new Vector2(.78f, -6.29f), Quaternion.identity) as GameObject;
            Destroy(SkillEffectObject.transform.GetChild(8).gameObject);
            StartCoroutine(Ultima());
        }
    }

    public void DelayOfPain(SkillScript S)
    {
        if(S.CD>0)
        {
            if (S.CD > DamageValue)
                Damage(4*DamageValue);
            else
                Damage(4*S.CD);
            S.CD -= DamageValue;
            S.CDObj.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = S.CD.ToString();
            if (S.CD <= 0)
            {
                S.CD = 0;
                Destroy(S.CDObj);
            }
        }
        GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.PlayerSkills, GameControl.GameState.PlayerSkills, 999f);
        GameControl.singleton.MessageText.text = "No patience for this!";
    }

    public void ChangeCD(int val)
    {
        if (CD == 0 && val > 0)
        {
            CDObj = Instantiate(GameControl.singleton.CDCover, (Vector2)transform.position+GetComponent<BoxCollider2D>().offset, Quaternion.identity) as GameObject;
            CDObj.transform.GetChild(1).localScale = new Vector2(transform.localScale.x*GetComponent<BoxCollider2D>().size.x*100, transform.localScale.y*GetComponent<BoxCollider2D>().size.y*100);
        }
        CD += val;
        if (CD > 0)
            CDObj.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = CD.ToString();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
