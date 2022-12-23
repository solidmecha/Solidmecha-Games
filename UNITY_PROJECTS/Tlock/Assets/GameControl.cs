using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
    public List<BlockScript> Blocks=new List<BlockScript> { };
    public PlayerControl PC;
    public int MoveIndex;
    public int MoveCount;

    public void MoveBlocks()
    {
        if (MoveIndex == -1)
            MoveIndex = MoveCount - 1;
        else if (MoveIndex == MoveCount)
        {
            MoveIndex = 0;
            PC.transform.position = PC.Start_Pos;
            PC.CurrentState = PlayerControl.MoveState.None;
            foreach (BlockScript b in Blocks)
                b.transform.position = b.Start_Pos;
        }
        else
        {
            foreach (BlockScript b in Blocks)
            {
                b.CurrentState = b.MoveList[MoveIndex];
            }
        }
    }

    public void SnapBlocks()
    {
        foreach (BlockScript b in Blocks)
        {
            b.transform.position = new Vector3(Mathf.Round(b.transform.position.x), Mathf.Round(b.transform.position.y));
            b.CurrentState = BlockScript.MoveState.None;
            b.timeSet = false;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
