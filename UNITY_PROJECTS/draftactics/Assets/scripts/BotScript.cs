using UnityEngine;
using System.Collections.Generic;

public class BotScript : MonoBehaviour {

    List<int[]> PossibleMoves=new List<int[]> { };
    int MoveIndex;
    int Boost;
    int TargetIndex=-1;

    public void TakeTurn()
    {
        StartMove();
        Invoke("Move", 1f);
    }

    void StartMove()
    {
        GameControl.singleton.ShowMoves();
        GameControl.singleton.CurrentState = GameControl.GameState.Waiting;
        foreach (GameObject g in GameControl.singleton.OutlineTiles)
        {
            g.GetComponent<SpriteRenderer>().color = new Color(.1f, 1, .1f, .65f);
            int[] ia = GameControl.singleton.TileXY_By_World_Position(g.transform.position);
            if (GameControl.singleton.FindCharacterAtPosition(ia) == null)
                PossibleMoves.Add(ia);
        }
    }

    public void Move()
    {
        DetermineMoveList();
        MoveIndex=GameControl.singleton.RNG.Next(PossibleMoves.Count);
        GetComponent<CharacterScript>().Position = PossibleMoves[MoveIndex];
        transform.position=GameControl.singleton.Offset_By_TileXY(PossibleMoves[MoveIndex]);
        GameControl.singleton.OutlineTileCleanUp();
        PossibleMoves.Clear();
        Invoke("ShowAttack", .6f);
    }

    public void DetermineMoveList()
    {
        int TotalRange = GetComponent<CharacterScript>().Movement + GetComponent<SkillScript>().Range;
        List<int> PotentialTargetsIndices = new List<int> { };
        foreach (int i in GameControl.singleton.PCIndices)
        {
            if (TotalRange >= Mathf.Abs(Mathf.Abs(GameControl.singleton.Characters[i].Position[0]) - Mathf.Abs(GetComponent<CharacterScript>().Position[0])) 
                + Mathf.Abs(Mathf.Abs(GameControl.singleton.Characters[i].Position[1]) - Mathf.Abs(GetComponent<CharacterScript>().Position[1])))
            {
                PotentialTargetsIndices.Add(i);
            }
        }
        if(PotentialTargetsIndices.Count>0)
        {
            int lowestHP=10000;
            for(int i=0; i<PotentialTargetsIndices.Count;i++)
            {
                if (GameControl.singleton.Characters[PotentialTargetsIndices[i]].HP[0] < lowestHP)
                {
                    TargetIndex = PotentialTargetsIndices[i];
                    lowestHP = GameControl.singleton.Characters[PotentialTargetsIndices[i]].HP[0];
                }
            }

            for (int i = PossibleMoves.Count-1; i >=0; i--)
            {
                if((GetComponent<SkillScript>().Range < Mathf.Abs(Mathf.Abs(GameControl.singleton.Characters[TargetIndex].Position[0]) - Mathf.Abs(PossibleMoves[i][0]))
                + Mathf.Abs(Mathf.Abs(GameControl.singleton.Characters[TargetIndex].Position[1]) - Mathf.Abs(PossibleMoves[i][1]))))
                {
                    PossibleMoves.RemoveAt(i);
                }
            }

            if(PossibleMoves.Count==0)
            {
                foreach (GameObject g in GameControl.singleton.OutlineTiles)
                {
                    int[] ia = GameControl.singleton.TileXY_By_World_Position(g.transform.position);
                    if(GameControl.singleton.FindCharacterAtPosition(ia) == null)
                        PossibleMoves.Add(ia);
                }
            }

        }
    }

    public void ShowAttack()
    {
        GameControl.singleton.ShowAttack();
        GameControl.singleton.CurrentState = GameControl.GameState.Waiting;
        foreach (GameObject g in GameControl.singleton.OutlineTiles)
        {
            g.GetComponent<SpriteRenderer>().color = new Color(1f, .1f, .1f, .65f);
            PossibleMoves.Add(GameControl.singleton.TileXY_By_World_Position(g.transform.position));
        }

        if (TargetIndex > 0)
        {
            GameControl.singleton.AttackedSquares.Add(GameControl.singleton.Characters[TargetIndex].Position);
            Boost = GameControl.singleton.RNG.Next(1, 3);
        }
        else
        {
            MoveIndex = GameControl.singleton.RNG.Next(PossibleMoves.Count);
            GameControl.singleton.AttackedSquares.Add(PossibleMoves[MoveIndex]);
            Boost = 3;
        }
        for (int i = 0; i < Boost; i++)
            Invoke("showBoost", 1f + .5f * i);
        if (Boost == 0)
        {
            GameControl.singleton.ShowBoost();
            Invoke("ConfirmAttack", .7f);
        }

    }

    public void showBoost()
    {
        Boost--;
        if (Boost == 0)
            Invoke("ConfirmAttack", 1f);
        else
            GameControl.singleton.ChangeBoost(1);
        foreach (GameObject g in GameControl.singleton.OutlineTiles)
            g.GetComponent<SpriteRenderer>().color = new Color(1f, .1f, .1f, .65f);

    }

    void ConfirmAttack()
    {
        GameControl.singleton.ConfirmAttack();
        PossibleMoves.Clear();
        Invoke("EndTurn", .5f);
    }

    void EndTurn()
    {
        MoveIndex = 0;
        TargetIndex = -1;
        Boost = 0;
        GameControl.singleton.EndTurn();
    }

    void TileCleanup()
    {
        GameControl.singleton.OutlineTileCleanUp();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
