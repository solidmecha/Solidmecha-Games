using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DiceControl : MonoBehaviour {
    public static DiceControl singleton;
    public Sprite[] PlayerFaces;
    public Sprite[] EnemyFaces;
    int ReRollCount;
    public GameObject PlayerDiceRef;
    public GameObject DiceObj;
    public GameObject EnemyDiceRef;
    public List<int> SelectedDiceTypes;
    public int[] LastEnemyDice;
    public int[] LastPlayerDice;

    private void Awake()
    {
        singleton = this;
    }

    public void BeginPlayerRoll()
    {
        if(GameControl.singleton.ResourceGeneratingCharacterIndicies[0] != -1)
        {
            foreach (DieScript d in GameControl.singleton.SelectedCharacters[GameControl.singleton.ResourceGeneratingCharacterIndicies[0]].transform.GetChild(3).GetComponentsInChildren<DieScript>())
                d.Rollit();
        }
        foreach (GameObject p in GameControl.singleton.SelectedCharacters)
        {
            StatScript S = p.GetComponent<StatScript>();
            CharacterScript C = p.GetComponent<CharacterScript>();
            if (C.Regen[3] > 0)
            {
                foreach (SkillScript s in p.transform.GetComponentsInChildren<SkillScript>())
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
            else
            {
                S.UpdateHP(-1*C.Regen[0]);
                S.UpdateArmor(C.Regen[1]);
                S.UpdateMP(-1*C.Regen[2]);
            }
        }
        ReRollCount = 2;
        GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.PlayerRoll, GameControl.GameState.PlayerRoll, 999f);
        DiceObj =Instantiate(PlayerDiceRef);
        GameControl.singleton.MessageText.text = "Select Dice to Keep. 2 Rerolls Left";
        DiceObj.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener( delegate { ReRoll(); });
    }

    public void ReRoll()
    {
        ReRollCount--;
        if (ReRollCount > -1)
        {
            foreach (DieScript d in DiceObj.GetComponentsInChildren<DieScript>())
                d.Rollit();
            GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.SkillEffect, GameControl.GameState.PlayerRoll, .75f);
            if (ReRollCount > 1)
                GameControl.singleton.MessageText.text = "Select Dice to Keep. " + ReRollCount.ToString() + " Rerolls Left";
            else
                GameControl.singleton.MessageText.text = "Select Dice to Keep. 1 Reroll Left";
        }
        if(ReRollCount==0)
        {
            DiceObj.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            DiceObj.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { GameControl.singleton.EndPlayerTurn(); });
            DiceObj.transform.GetChild(8).GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "End Turn";
            GameControl.singleton.MessageText.text = "Select Dice to Use.";
            GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.SkillEffect, GameControl.GameState.PlayerSkills, .75f);
            foreach (DieScript d in DiceObj.GetComponentsInChildren<DieScript>())
            {
                if (d.Saved)
                    d.SaveSwap();
            }
        }
    }

    public void UpdateLastPlayerDice()
    {
        for (int i = 0; i < 8; i++)
            LastPlayerDice[i] = DiceObj.transform.GetChild(i).GetComponent<DieScript>().id;
    }

    public void BeginEnemyRoll()
    {
        GameControl.singleton.MessageText.text = "Enemy Turn";
        DiceObj = Instantiate(EnemyDiceRef);
        GameControl.singleton.ChangeStateBackInSeconds(GameControl.GameState.SkillEffect, GameControl.GameState.EnemyRoll, .75f);
        GameControl.singleton.Invoke("HandleDurationEffects", .8f);
    }

    public IEnumerator ProcessEnemyRoll()
    {
        if (GameControl.singleton.ResourceGeneratingCharacterIndicies[1] != -1)
        {
            //Diviner forecast
            int[] count = new int[6];
            foreach (DieScript d in GameControl.singleton.SelectedCharacters[GameControl.singleton.ResourceGeneratingCharacterIndicies[1]].transform.GetChild(3).GetComponentsInChildren<DieScript>())
                count[d.id]++;
            foreach (DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
            {
                if(count[d.id]>0)
                    count[d.id]--;
            }
            int matchedCount = 3;
            foreach (int i in count)
            {
                if (i > 0)
                    matchedCount -= i;
            }

            GameControl.singleton.SelectedCharacters[GameControl.singleton.ResourceGeneratingCharacterIndicies[1]].transform.GetChild(3).GetComponent<SkillScript>().UpdateResourceValue(matchedCount, true);

        }
        int ChangeAttackerIndex=GameControl.singleton.EnemyParty[0].GetComponent<EnemyScript>().DiceCount;
        int AttackerIndex = 0;
        GameControl.singleton.Attacker = GameControl.singleton.EnemyParty[AttackerIndex];
        for(int i=0; i<DiceObj.transform.childCount; i++)
        {
            if(i==ChangeAttackerIndex)
            {
                AttackerIndex++;
                GameControl.singleton.Attacker = GameControl.singleton.EnemyParty[AttackerIndex];
                if (GameControl.singleton.Attacker.GetComponent<EnemyScript>().hasSpecialAtk)
                    GameControl.singleton.Attacker.GetComponent<EnemySkillScript>().Execute();
                ChangeAttackerIndex += GameControl.singleton.EnemyParty[AttackerIndex].GetComponent<EnemyScript>().DiceCount;
            }
            if (GameControl.singleton.Attacker.GetComponent<StatScript>().HP[0] > 0)
            {
                int d = DiceObj.transform.GetChild(i).GetComponent<DieScript>().id;
                DiceObj.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                GameControl.singleton.Target = GetTarget();
                GameControl.singleton.TargetText.text = "Current Target: " + GameControl.singleton.Target.name;
                yield return new WaitForSeconds(1.3f);
                LastEnemyDice[i] = d;
                EnemyDiceBaseEffects(d, GameControl.singleton.Target.GetComponent<StatScript>());
                DiceObj.transform.GetChild(i).position = new Vector3(100, 100, 100);
            }
            else
            {
                yield return null;
                int d = DiceObj.transform.GetChild(i).GetComponent<DieScript>().id;
                LastEnemyDice[i] = d;
                DiceObj.transform.GetChild(i).position = new Vector3(100, 100, 100);
            }
        }

        GameControl.singleton.Invoke("HandleEnemyEoT", 2f);
    }

    void EnemyDiceBaseEffects(int ID, StatScript TargetStats)
    {
        EnemyScript es = GameControl.singleton.Attacker.GetComponent<EnemyScript>();
        switch(ID)
        {
            case 0: //mana burn
                TargetStats.UpdateMP(es.Effectiveness[0]);
                break;
            case 1: //freeze out
                TargetStats.GetComponentsInChildren<SkillScript>()[GameControl.singleton.RNG.Next(TargetStats.GetComponentsInChildren<SkillScript>().Length)].ChangeCD(es.Effectiveness[1]);
                break;
            case 2: //group atk
                foreach (GameObject g in GameControl.singleton.SelectedCharacters)
                {
                    for (int i = 0; i < es.Effectiveness[ID]; i++)
                    {
                        if (g.GetComponent<StatScript>().Armor == 0)
                            g.GetComponent<StatScript>().UpdateHP(1);
                        else
                            g.GetComponent<StatScript>().UpdateArmor(-1);
                    }
                }
                break;
            case 3://poison
                for (int i = 0; i < es.Effectiveness[ID]; i++)
                {
                    TargetStats.UpdateHP(1);
                }
                break;
            case 4: //heal
                GameControl.singleton.Attacker.GetComponent<StatScript>().UpdateHP(-1*GameControl.singleton.RNG.Next(1, 3)* es.Effectiveness[ID]);
                break;
            case 5: //single swipe
                for (int i = 0; i < es.Effectiveness[ID]; i++)
                {
                    if (TargetStats.Armor == 0)
                        TargetStats.UpdateHP(1);
                    else
                        TargetStats.UpdateArmor(-1);
                }
                break;
            default:
                break;
        }
        for (int i = 0; i < es.BonusDamage[ID]; i++)
        {
            if (GameControl.singleton.RNG.Next(3) == 2)
            {
                if (TargetStats.Armor == 0)
                    TargetStats.UpdateHP(1);
                else
                    TargetStats.UpdateArmor(-1);
            }
        }
    }

    GameObject GetTarget()
    {
        if (GameControl.singleton.Taunter == null)
        {
            List<int> TargetIndices=new List<int> { };
            for(int i=0;i<4;i++)
            {
                if (GameControl.singleton.SelectedCharacters[i].GetComponent<StatScript>().HP[0] > 0 && i != GameControl.singleton.ImmunityID)
                    TargetIndices.Add(i);
            }
            if (TargetIndices.Count == 0)
                TargetIndices.Add(GameControl.singleton.RNG.Next(4));
            int r = GameControl.singleton.RNG.Next(TargetIndices.Count);
            return GameControl.singleton.SelectedCharacters[TargetIndices[r]];
        }
        else
            return GameControl.singleton.Taunter;
    }

    // Use this for initialization
    void Start () {
        LastEnemyDice = new int[8];
        for (int i = 0; i < 8; i++)
            LastEnemyDice[i] = GameControl.singleton.RNG.Next(6);
	}

    public bool CheckMatch(int[] IA)
    {
        int[] count = new int[6] { 0, 0, 0, 0, 0, 0 };

        foreach (int i in IA)
        {
            count[i]++;
        }
        foreach(int i in SelectedDiceTypes)
        {
            count[i]--;
        }
        foreach (int i in count)
        {
            if (i != 0)
                return false;
        }
        return true;
    }

    public bool CheckSameMatch(int I)
    {
        if (SelectedDiceTypes.Count == 0)
            return false;
        foreach(int i in SelectedDiceTypes)
        {
            if (i != I)
                return false;
        }
        return true;
    }

    public void DestroyUsedDice()
    {
        SelectedDiceTypes.Clear();
        for(int i= DiceObj.transform.childCount-2; i>=0;i--)
        {
            if (DiceObj.transform.GetChild(i).GetComponent<DieScript>().Saved)
            {
                DiceObj.transform.GetChild(i).GetComponent<DieScript>().Saved = false;
                DiceObj.transform.GetChild(i).localPosition = new Vector3(100, 100, 100);
            }
        }
    }

    public void LoopedWrathEffect()
    {
        for(int i=0;i<DiceObj.transform.childCount; i++)
        {
            DiceObj.transform.GetChild(i).GetComponent<DieScript>().id = LastEnemyDice[i];
            DiceObj.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = EnemyFaces[LastEnemyDice[i]];
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
