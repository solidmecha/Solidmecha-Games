using UnityEngine;
using System.Collections.Generic;
using System;

public class IceControl : MonoBehaviour {

    public int Turns;
    public GameObject WreckedToken;
    public GameObject TagToken;
    public GameControl GC;
    System.Random RNG = new System.Random();

    //destroy tile TYPE
        public void DestroyTargetTile(int TargetID)
    {
        bool validTarget = false;
        List<List<int>> TargetIndex = new List<List<int>> { };
        for (int i = 0; i < GC.PlayerRig.transform.childCount; i++)
        {
            List<int> nodeIndices = new List<int> { };
            foreach (NodeID n in GC.PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                if (n.ID == TargetID && !n.Destroyed)
                {
                    nodeIndices.Add(n.transform.GetSiblingIndex());
                    validTarget = true;
                }
            }
            TargetIndex.Add(nodeIndices);
        }

        if (validTarget)
        {
            int r;
            do
            {
                r = RNG.Next(TargetIndex.Count);
            }
            while (TargetIndex[r].Count == 0);
            int i = RNG.Next(TargetIndex[r].Count);
            NodeID n = GC.PlayerRig.transform.GetChild(r).GetChild(TargetIndex[r][i]).GetComponent<NodeID>();
            n.Destroyed = true;
           n.DestroyedIcon= Instantiate(WreckedToken, n.transform.position, Quaternion.identity, n.transform) as GameObject;
        }
    }

    public void RemoteAcess()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().UpdateCredit(-1 * RNG.Next(3, 16) * 100);
        int x = RNG.Next(3, 6);
        int y = RNG.Next(3, 6);
        GameObject go = GameObject.FindGameObjectWithTag("GameController").GetComponent<ModuleControl>().CreateModule(x, y, RNG.Next(3, x + y));
        go.transform.position = new Vector2(7,2);
    }

    //destroy random tile
    public void DestroyAnyTile()
    {
        bool validTarget = false;
        List<List<int>> TargetIndex = new List<List<int>> { };
        for (int i = 0; i < GC.PlayerRig.transform.childCount; i++)
        {
            List<int> nodeIndices = new List<int> { };
            foreach (NodeID n in GC.PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                if (!n.Destroyed)
                { 
                    nodeIndices.Add(n.transform.GetSiblingIndex());
                validTarget = true;
                }
        }
            TargetIndex.Add(nodeIndices);
        }

        if (validTarget)
        {
            int r;
            do {
                r = RNG.Next(TargetIndex.Count);
            }
            while (TargetIndex[r].Count == 0);
            int i = RNG.Next(TargetIndex[r].Count);
            NodeID n = GC.PlayerRig.transform.GetChild(r).GetChild(TargetIndex[r][i]).GetComponent<NodeID>();
            n.Destroyed = true;
            n.DestroyedIcon=Instantiate(WreckedToken, n.transform.position, Quaternion.identity, n.transform) as GameObject;
        }
    }



    //discharge tile TYPE
    public void DischargeTargetTile(int TargetID)
    {
        bool validTarget = false;
        List<List<int>> TargetIndex = new List<List<int>> { };
        for (int i = 0; i < GC.PlayerRig.transform.childCount; i++)
        {
            List<int> nodeIndices = new List<int> { };
            foreach (NodeID n in GC.PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                if (n.ID == TargetID && n.Charged)
                {
                    nodeIndices.Add(n.transform.GetSiblingIndex());
                    validTarget = true;
                }
            }
            TargetIndex.Add(nodeIndices);
        }

        if (validTarget)
        {
            int r;
            do
            {
                r = RNG.Next(TargetIndex.Count);
            }
            while (TargetIndex[r].Count == 0);
            int i = RNG.Next(TargetIndex[r].Count);
            NodeID n = GC.PlayerRig.transform.GetChild(r).GetChild(TargetIndex[r][i]).GetComponent<NodeID>();
            n.Discharge();
        }
    }

    //discharge random tile
    public void DischargeAnyTile()
    {
        bool validTarget = false;
        List<List<int>> TargetIndex = new List<List<int>> { };
        for (int i = 0; i < GC.PlayerRig.transform.childCount; i++)
        {
            List<int> nodeIndices = new List<int> { };
            foreach (NodeID n in GC.PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                if (n.Charged)
                {
                    nodeIndices.Add(n.transform.GetSiblingIndex());
                    validTarget = true;
                }
            }
            TargetIndex.Add(nodeIndices);
        }

        if (validTarget)
        {
            int r;
            do
            {
                r = RNG.Next(TargetIndex.Count);
            }
            while (TargetIndex[r].Count == 0);
            int i = RNG.Next(TargetIndex[r].Count);
            NodeID n = GC.PlayerRig.transform.GetChild(r).GetChild(TargetIndex[r][i]).GetComponent<NodeID>();
            n.Discharge();
        }
    }


    //disconnect
    //disconnect IF <type amt
    //prevent recharge


    //change one tile type into another
    //add X turns
    void AddTurns(int T)
    {
        Turns += T;
    }
    //adds cost to break

   public void GetSubID(int id, int val)
    {
        switch(id)
        {
            case 0:
                DestroyTargetTile(val);
                break;
            case 1:
                DestroyAnyTile();
                break;
            case 2:
                AddTurns(val);
                break;
            case 3:
                DischargeTargetTile(val);
                break;
            case 4:
                DischargeAnyTile();
                break;
            case 5:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().UpdateCredit(50);
                break;
            case 12:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().UseClick();
                break;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
