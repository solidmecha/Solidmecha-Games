using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleScript : MonoBehaviour {

    public static BattleScript singleton;
    enum State { Ordering, Resolving};
    State CurrentState;
    public GameObject MenuCanvas;
    public GameObject TargetCanvas;
    public GameObject SwapCanvas;
    GameObject CurrentMenu;
    public List<BehaviourScript> Units;
    public int UnitIndex;
    Vector2 MenuOffset = new Vector2(1.75f, 0);
    public Action CurrentAction;
    public List<BehaviourScript> CurrentTargets=new List<BehaviourScript> { };
    public UnityEngine.UI.Text[] Reports;
    public List<int> FinishedLanes = new List<int> { };
    int PlayerWonLanes;
    int PlayerLostLanes;
    public Vector3[] PlayerFrontRow;
    public Vector3[] PlayerBackRow;
    public Vector3[] OppFrontRow;
    public Vector3[] OppBackRow;
    public GameObject LanesBG;
    public GameObject Unit;
    public bool inBattle;
    public int ActiveSkillIndex;

    public void MakeCanvas()
    {
        if (CurrentMenu != null)
            Destroy(CurrentMenu);
        CurrentMenu = Instantiate(MenuCanvas, (Vector2)Units[UnitIndex].transform.position+MenuOffset, Quaternion.identity) as GameObject;
        CurrentMenu.GetComponent<BattleMenuScript>().BS = Units[UnitIndex];
    }

    public void PlaceUnit(BehaviourScript B)
    {
        if(B.PlayerControlled)
        {
            if (B.isSupport)
                B.transform.position = PlayerBackRow[B.LaneID];
            else
                B.transform.position = PlayerFrontRow[B.LaneID];
        }
        else
        {
            if (B.isSupport)
                B.transform.position = OppBackRow[B.LaneID];
            else
                B.transform.position = OppFrontRow[B.LaneID];
        }
    }

    public bool HasMana(int index)
    {
        return Units[UnitIndex].Skills[index].ManaCost <= Units[UnitIndex].MP[0];
    }


    public void StartBattle()
    {
        Camera.main.transform.SetParent(null);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        inBattle = true;
        CreatOpp();
        PlayerScript.singleton.isMoving = false;
        PlayerScript.singleton.transform.position = new Vector3(-10, -10, -10);
        Instantiate(LanesBG);
        foreach(UnitStats U in PlayerScript.singleton.Team)
        {
            GameObject g = Instantiate(Unit) as GameObject;
            BehaviourScript B = g.GetComponent<BehaviourScript>();
            B.SetStats(U);
            PlaceUnit(B);
            Units.Add(B);
        }
        UnitsBySpeed();
        NextTurn();
    }

    void CreatOpp()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject g=WorldControl.singleton.GenerateUnit();
            g.GetComponent<BehaviourScript>().LaneID = i;
            g.GetComponent<BehaviourScript>().isSupport = true;
            g.GetComponent<BehaviourScript>().LvlUp(WorldControl.singleton.Level);
            PlaceUnit(g.GetComponent<BehaviourScript>());
            g.GetComponent<SpriteRenderer>().flipY = true;
            Units.Add(g.GetComponent<BehaviourScript>());
            g=WorldControl.singleton.GenerateUnit();
            g.GetComponent<BehaviourScript>().LaneID = i;
            g.GetComponent<BehaviourScript>().isSupport = false;
            g.GetComponent<BehaviourScript>().LvlUp(WorldControl.singleton.Level);
            PlaceUnit(g.GetComponent<BehaviourScript>());
            Units.Add(g.GetComponent<BehaviourScript>());
            g.GetComponent<SpriteRenderer>().flipY=true;
        }
    }

    public void StopBattle()
    {
        Camera.main.transform.SetParent(PlayerScript.singleton.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        inBattle = false;
        PlayerScript.singleton.transform.position = Vector3.zero;
        PlayerScript.singleton.isMoving = true;
        Destroy(GameObject.FindGameObjectWithTag("LanesBG"));
        foreach (BehaviourScript b in Units)
            if(b!=null)
                Destroy(b.gameObject);
        Units.Clear();
        PlayerWonLanes = 0;
        PlayerLostLanes = 0;
        FinishedLanes.Clear();
        PlayerScript.singleton.Money += 250;
        WorldControl.singleton.NextLvl();
    }


    public void Swap(int LaneOne, bool backOne, int LaneTwo, bool backTwo)
    {
        BehaviourScript B1 = FindUnit(LaneOne, backOne, true);
        BehaviourScript B2 = FindUnit(LaneTwo, backTwo, true);
        PlayerScript.singleton.Swap(LaneOne,backOne, LaneTwo, backTwo);
        if (B1 != null && !FinishedLanes.Contains(LaneTwo))
        {
            if (B2 == null && LaneOne != LaneTwo && inBattle)
                ResolveKO(B1);
            B1.LaneID = LaneTwo;
            B1.isSupport = backTwo;
            PlaceUnit(B1);
        }
        if (B2 != null && !FinishedLanes.Contains(LaneOne))
        {
            if (B1 == null && LaneOne != LaneTwo && inBattle)
                ResolveKO(B2);
            B2.LaneID = LaneOne;
            B2.isSupport = backOne;
            PlaceUnit(B2);
        }
    }

    public void ResolveKO(BehaviourScript B)
    {
        if (B == null)
            return;
        if(FindUnit(B.LaneID, !B.isSupport, B.PlayerControlled) == null)
        {
            FinishedLanes.Add(B.LaneID);
            if (!B.PlayerControlled)
            {
                PlayerWonLanes++;
                Reports[B.LaneID].text = "P1 win";
                BehaviourScript B1 = FindUnit(B.LaneID, true, true);
                if (B1 != null)
                {
                    PlayerScript.singleton.Team[B1.Index].LevelUp();
                    DestroyImmediate(B1.gameObject);
                }
                B1 = FindUnit(B.LaneID, false, true);
                if (B1 != null)
                {
                    PlayerScript.singleton.Team[B1.Index].LevelUp();
                    DestroyImmediate(B1.gameObject);
                }
            }
            else
            {
                PlayerLostLanes--;
                Reports[B.LaneID].text = "P2 win";
                BehaviourScript B1 = FindUnit(B.LaneID, true, false);
                if (B1 != null)
                {
                    DestroyImmediate(B1.gameObject);
                }
                B1 = FindUnit(B.LaneID, false, false);
                if (B1 != null)
                {
                    DestroyImmediate(B1.gameObject);
                }
            }
            if (PlayerWonLanes >= 3)
            {
                StopBattle();
                foreach (UnityEngine.UI.Text t in Reports)
                    t.text = "";
                print("Player Wins!");
            }
            else if (PlayerLostLanes <= -3)
            {
                foreach (UnityEngine.UI.Text t in Reports)
                    t.text = "GAME OVER";
            }
            

        }
    }

    /*
    public void ChooseTarget()
    {
        if (CurrentMenu != null)
            Destroy(CurrentMenu);
        CurrentMenu = Instantiate(TargetCanvas, (Vector2)Units[UnitIndex].transform.position + MenuOffset, Quaternion.identity) as GameObject;
        for (int i = 0; i < 5; i++)
        {
            //CurrentMenu.transform.GetChild(0).GetChild(i).GetComponent<TargetScript>().BS = OpUnits[i];
        }
    }*/

    public BehaviourScript FindUnit(int Lane, bool BackRow, bool PlayerControlled)
    {
        foreach(BehaviourScript B in Units)
        {
            if(B != null && B.LaneID == Lane && B.isSupport==BackRow && B.PlayerControlled==PlayerControlled)
            {
                return B;
            }
        }
        return null;
    }

    public void NextTurn()
    {
        CurrentTargets.Clear();
        UnitIndex++;
        if (UnitIndex >= Units.Count)
        {
            UnitsBySpeed();
            UnitIndex = -1;
            Instantiate(SwapCanvas);
            foreach (BehaviourScript B in Units)
            {
                if(B!=null)
                    B.RegenMana(B.MPRegen);
            }
            return;
        }
        if (Units[UnitIndex] != null)
        {
            if (Units[UnitIndex].PlayerControlled)
                MakeCanvas();
            else
                BotMove();
        }
        else
            NextTurn();

    }

    void UnitsBySpeed()
    {
        int i, j;
        int N = Units.Count;

        for (j = 1; j < N; j++)
        {
            for (i = j; i > 0 && Units[i].Speed > Units[i - 1].Speed; i--)
            {
                exchange(i, i - 1);
            }
        }
    }

    void exchange(int m, int n)
    {
        BehaviourScript temp = Units[m];
        Units[m] = Units[n];
        Units[n] = temp;
    }

    void BotMove()
    {
        int i= WorldControl.singleton.RNG.Next(3);
        SetTargets(FindTargets(Units[UnitIndex].Skills[i]));
        Units[UnitIndex].Skills[i].Action();
        NextTurn();
    }

    List<BehaviourScript> FindTargets(SkillScript S)
    {
        List<BehaviourScript> Targets=new List<BehaviourScript> { };
        BehaviourScript BS = Units[UnitIndex];
        if (S.TargetCount == 0)
        {
            if (!S.Support)
                Targets.Add(FindUnit(BS.LaneID, S.Ranged, !BS.PlayerControlled));
            else
                Targets.Add(FindUnit(BS.LaneID, S.Ranged, BS.PlayerControlled));
        }
        else if(S.TargetCount==2)
        {
            if (!S.Support)
            {
                Targets.Add(FindUnit(BS.LaneID, S.Ranged, !BS.PlayerControlled));
                Targets.Add(FindUnit(BS.LaneID, !S.Ranged, !BS.PlayerControlled));
            }
            else
            {
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, S.Ranged, BS.PlayerControlled));
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, !S.Ranged, BS.PlayerControlled));
            }
        }
        if (S.TargetCount == 3)
        {
            if (!S.Support)
            {
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, S.Ranged, !BS.PlayerControlled));
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID + 1, S.Ranged, !BS.PlayerControlled));
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID - 1, S.Ranged, !BS.PlayerControlled));
            }
            else
            {
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID, S.Ranged, BS.PlayerControlled));
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID + 1, S.Ranged, BS.PlayerControlled));
                Targets.Add(BattleScript.singleton.FindUnit(BS.LaneID - 1, S.Ranged, BS.PlayerControlled));
            }
        }
        if (S.TargetCount == 5)
        {
            if (!S.Support)
            {
                for (int j = 0; j < 5; j++)
                {
                    Targets.Add(BattleScript.singleton.FindUnit(j, S.Ranged, !BS.PlayerControlled));
                }
            }
            else
            {
                for (int j = 0; j < 5; j++)
                {
                    Targets.Add(BattleScript.singleton.FindUnit(j, S.Ranged, BS.PlayerControlled));
                }
            }
        }
        for (int j = Targets.Count - 1; j > -1; j--)
        {
            if (Targets[j] == null)
                Targets.RemoveAt(j);
        }
        if (Targets.Count == 0)
        {
            if (!S.Support && FindUnit(BS.LaneID, !S.Ranged, !BS.PlayerControlled) != null)
                Targets.Add(FindUnit(BS.LaneID, !S.Ranged, !BS.PlayerControlled));
            else if(FindUnit(BS.LaneID, !S.Ranged, BS.PlayerControlled) !=null)
                Targets.Add(FindUnit(BS.LaneID, !S.Ranged, BS.PlayerControlled));
        }
        return Targets;
    }

    public void SetTargets(List<BehaviourScript> B)
    {
        CurrentTargets.Clear();
        foreach(BehaviourScript b in B)
            CurrentTargets.Add(b);
    }

    void SetUpUnits()
    {
        /*
        foreach(GameObject G in GameObject.FindGameObjectsWithTag("U"))
        {
            BehaviourScript B=G.GetComponent<BehaviourScript>();
            SkillScript S = new SkillScript();
            for(int i=0;i<3;i++)
                B.Skills.Add(S.SkillLookUp(i, B));
        }*/
    }

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        //SetUpUnits();
        //UnitsBySpeed();
        //NextTurn();
        //MakeCanvas();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
