using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
    public static GameControl singleton;
    public System.Random RNG;
    public Text MessageText;
    public enum GameState {TeamSelect, PlayerRoll, PlayerSkills, SkillEffect, EoT, EnemyRoll, EnemySkills, EnemyEot};
    public GameState CurrentState;
    GameState NextState;
    float GameStateCounter;
    public GameObject[] PlayerCharacters;
    public GameObject TeamPickRef;
    public GameObject PlayAreaRef;
    List<int> TeamListIndex = new List<int> { };
    int teamSelectIndex=0;
    public List<GameObject> SelectedCharacters;
    public List<GameObject> CharacterOptions;
    public GameObject Target;
    public GameObject Taunter;
    public List<int[]> DurationSkillEffects; //{skillID, Duration}
    public GameObject Attacker;
    public SkillScript MostRecentSkill;
    public GameObject CDCover;
    public int[] ResourceGeneratingCharacterIndicies; //fighter index, diviner index
    public int ImmunityID;
    public GameObject InfoCanvas;
    public List<GameObject> EnemyParty;
    public Text TargetText;
    public int WinCount;
    public GameObject GameOverObj;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        CurrentState = GameState.TeamSelect;
        NextState = GameState.TeamSelect;
        GameStateCounter=999f;
        DurationSkillEffects = new List<int[]> { };
    }

    public void SetCurrentState(GameState g)
    {
        CurrentState=g;
    }

    public void ChangeStateBackInSeconds(GameState g, GameState n, float s)
    {
        CurrentState = g;
        NextState = n;
        GameStateCounter = s;
    }

    // Use this for initialization
    void Start () {
        RandomList(TeamListIndex, PlayerCharacters.Length);
        NextTeamSelect();
    }

    public void HandleDurationEffects()
    {
        foreach (int[] ia in DurationSkillEffects)
        {
            if(ia[0]==12) //Chrono looped wrath
            {
                DiceControl.singleton.LoopedWrathEffect();
            }
            else if(ia[0]==16)//diviner prevention
            {
                int[] ForecastedIDs = new int[3];
                for (int i = 0; i < 3; i++)
                    ForecastedIDs[i] = SelectedCharacters[ResourceGeneratingCharacterIndicies[1]].transform.GetChild(3).GetChild(i).GetComponent<DieScript>().id;
                List<int> EnemyDiceValues=new List<int> { };
                foreach(DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
                {
                    EnemyDiceValues.Add(d.id);
                }
                for (int i=0;i<ia[1];i++)
                {
                    int PreventIndex = RNG.Next(3);
                    if (EnemyDiceValues.Contains(ForecastedIDs[PreventIndex]))
                    {
                        if(RNG.Next(2)==0)
                        {
                            for(int j=0;j< DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>().Length;j++)
                            {
                                if (DiceControl.singleton.DiceObj.transform.GetChild(j).GetComponent<DieScript>().id == ForecastedIDs[PreventIndex])
                                {
                                    Destroy(DiceControl.singleton.DiceObj.transform.GetChild(j).gameObject);
                                    EnemyDiceValues.Remove(ForecastedIDs[PreventIndex]);
                                }
                            }
                        }
                    }
                }
            }
            else if(ia[0]==23) //Mage flare
            {
                Destroy(DiceControl.singleton.DiceObj.transform.GetChild(RNG.Next(DiceControl.singleton.DiceObj.transform.childCount)).gameObject);
            }
        }
        StartCoroutine(DiceControl.singleton.ProcessEnemyRoll());
    }

    public void HandleEnemyEoT()
    {
        Destroy(DiceControl.singleton.DiceObj);
        for(int i=DurationSkillEffects.Count-1;i>-1;i--)
        {
                DurationSkillEffects[i][1]--;
                if (DurationSkillEffects[i][1] == 0)
                    DurationSkillEffects.RemoveAt(i);
        }
        ImmunityID = -1;
        Taunter = null;
        DiceControl.singleton.BeginPlayerRoll();
    }

    public void HandlePlayerEoT()
    {
        if(ResourceGeneratingCharacterIndicies[0] != -1 && !SkillDurationCheck(22))
        {
            //Fighter combo
            int[] count = new int[6];
            foreach (DieScript d in SelectedCharacters[ResourceGeneratingCharacterIndicies[0]].transform.GetChild(3).GetComponentsInChildren<DieScript>())
                count[d.id]++;
            foreach (DieScript d in DiceControl.singleton.DiceObj.GetComponentsInChildren<DieScript>())
                count[d.id]--;
            bool matchedCombo = true;
            foreach(int i in count)
            {
                if (i > 0)
                    matchedCombo = false;
            }
            if(matchedCombo)
            {
                SelectedCharacters[ResourceGeneratingCharacterIndicies[0]].transform.GetChild(3).GetComponent<SkillScript>().UpdateResourceValue(1, true);
            }

        }

        foreach (GameObject c in SelectedCharacters)
            c.GetComponent<StatScript>().Snapshot();
        if(SkillDurationCheck(14))//timeRun
        {
            for(int i=0;i<DurationSkillEffects.Count;i++)
            {
                if (DurationSkillEffects[i][0] == 14)
                    DurationSkillEffects.RemoveAt(i);
            }
            DiceControl.singleton.BeginPlayerRoll();
            MessageText.text = "Here we go again.";
        }
        else
        {
            if(EnemiesAlive())
                DiceControl.singleton.BeginEnemyRoll();
            else
            {
                foreach (GameObject g in EnemyParty)
                    Destroy(g);
                EnemyParty.Clear();
                WinCount++;
                GetComponent<EnemySpawn>().EnemyGen();
                DiceControl.singleton.BeginEnemyRoll();
            }
        }
    }

    bool EnemiesAlive()
    {
        foreach(GameObject g in EnemyParty)
        {
            if(g.GetComponent<StatScript>().HP[0]>0)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckGameOver()
    {
       if(SelectedCharacters[0].GetComponent<StatScript>().HP[0]==0 &&
           SelectedCharacters[1].GetComponent<StatScript>().HP[0] == 0 &&
           SelectedCharacters[2].GetComponent<StatScript>().HP[0] == 0 &&
           SelectedCharacters[3].GetComponent<StatScript>().HP[0] == 0)
        {
            Instantiate(GameOverObj);
        }

    }

    public void Restart()
    {
        Application.LoadLevel(0);
    }

    public void NextTeamSelect()
    {
        foreach (GameObject g in CharacterOptions)
            Destroy(g);
        CharacterOptions.Clear();
        if (teamSelectIndex < PlayerCharacters.Length)
        {
            for (int i = 0; i < 3; i++)
            {
                CharacterOptions.Add(Instantiate(PlayerCharacters[TeamListIndex[teamSelectIndex]], TeamPickRef.transform.GetChild(i).position, Quaternion.identity) as GameObject);
                teamSelectIndex++;
            }
        }
        else
        {
            Target = EnemyParty[0];
            TargetText.text = "Current Target: " + Target.name;
            for(int i=0; i<4;i++)
            {
                SelectedCharacters[i].transform.position = PlayAreaRef.transform.GetChild(i).position;
            }
            DiceControl.singleton.BeginPlayerRoll();
        }
    }

    public void EndPlayerTurn()
    {
        DiceControl.singleton.SelectedDiceTypes.Clear();
        DiceControl.singleton.UpdateLastPlayerDice();
        Destroy(DiceControl.singleton.DiceObj);
        HandlePlayerEoT();
    }

    void RandomList(List<int> ListR, int max)
    {
        List<int> temp = new List<int> { };
        for (int x = 0; x < max; x++)
        {
            temp.Add(x);
        }
        for (int i = 0; i < max; i++)
        {
            int r = RNG.Next(temp.Count);
            ListR.Add(temp[r]);
            temp.RemoveAt(r);
        }
    }

    public bool SkillDurationCheck(int Val)
    {
        foreach(int[] ia in DurationSkillEffects)
        {
            if (ia[0] == Val)
                return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update () {

        GameStateCounter -= Time.deltaTime;
        if(GameStateCounter <= 0)
        {
            SetCurrentState(NextState);
            GameStateCounter = 999f;
        }

	}
}
