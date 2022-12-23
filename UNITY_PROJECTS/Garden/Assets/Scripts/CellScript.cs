using UnityEngine;
using System.Collections.Generic;
using System;

public class CellScript : MonoBehaviour {

    public GameObject CellUIobj;
    GameObject UIwindow;
    //public bool[] hasNeighbor = new bool[8];
    public float counter;
    public float delay;
    public Action<int>[] Actions = new Action<int>[8];
    public int[] ActionsID = new int[8];
    public int[] ActionParams = new int[9];
    public Action<int>[] SupressedActions = new Action<int>[8];
    public int[] SupressedActionParams = new int[8];
    public int[] SupressedActionsID = new int[8];
    public int[] PriorityList = new int[8];
    public List<Action<int>> PossibleActions = new List<Action<int>> { }; 
    bool hasPriority;
    public int index;
    public bool Initialized;
    public int ColorID;
    public GameObject WSobj;
    public WorldScript WS; 

    void OnMouseDown()
    {
        if (WS.CellWindow == null && WS.isPaused)
        {
            WS.CellWindow = Instantiate(CellUIobj, (Vector2)transform.position+WindowOffset(), Quaternion.identity) as GameObject;
            CellUI CU = (CellUI)WS.CellWindow.GetComponent(typeof(CellUI));
            CU.CS = this;
        }
    }
     
	// Use this for initialization
	void Start () {
        PossibleActions.Add(move);
        PossibleActions.Add(destroyTarget);
        PossibleActions.Add(replicate);
        PossibleActions.Add(DoAllTheThings);
        WS = (WorldScript)WSobj.GetComponent(typeof(WorldScript));
        if (!Initialized)
        {
            PossibleActions.Add(move);
            PossibleActions.Add(destroyTarget);
            PossibleActions.Add(replicate);
            PossibleActions.Add(DoAllTheThings);
            for (int i = 0; i < 8; i++)
            {
                PriorityList[i] = i;
                Actions[i] = DoAllTheThings;
                SupressedActions[i] = DoAllTheThings;
                ActionParams[i] = 0;
                ActionsID[i] = 3;
                SupressedActionsID[i] = 3;
                SupressedActionParams[i] = 0;

            }
        }

	}

    public void move(int i)
    {
        if (transform.GetChild(i).position.x - WSobj.transform.position.x >= 0 && transform.GetChild(i).position.x - WSobj.transform.position.x < WS.width && transform.GetChild(i).position.y - WSobj.transform.position.y >= 0 && transform.GetChild(i).position.y - WSobj.transform.position.y < WS.height)
        {
            if (!Physics2D.Raycast((Vector2)transform.GetChild(i).position, Vector2.zero))
            {
                transform.position = (Vector2)transform.position + (Vector2)transform.GetChild(i).localPosition;
            }
        }

    }

    public void destroyTarget(int i)
    {

            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.GetChild(i).position, Vector2.zero);
        if(hit)
            Destroy(hit.collider.gameObject);
    }

    public void replicate(int i)
    {
        if (transform.GetChild(i).position.x - WSobj.transform.position.x >= 0 && transform.GetChild(i).position.x - WSobj.transform.position.x < WS.width && transform.GetChild(i).position.y - WSobj.transform.position.y >=0 && transform.GetChild(i).position.y - WSobj.transform.position.y < WS.height)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.GetChild(i).position, Vector2.zero);
            if (!hit)
            {
                GameObject Go = Instantiate(gameObject, (Vector2)transform.GetChild(i).position, Quaternion.identity) as GameObject;
                CellScript CS = (CellScript)Go.GetComponent(typeof(CellScript));
                CS.PossibleActions.Add(CS.move);
                CS.PossibleActions.Add(CS.destroyTarget);
                CS.PossibleActions.Add(CS.replicate);
                CS.PossibleActions.Add(CS.DoAllTheThings);
                for (int t = 0; t < 8; t++)
                {
                    CS.Actions[t] = CS.PossibleActions[ActionsID[t]];
                    CS.ActionParams[t] = ActionParams[t];
                    CS.ActionsID[t] = ActionsID[t];
                    CS.SupressedActions[t] = CS.PossibleActions[SupressedActionsID[t]];
                    CS.SupressedActionParams[t] = SupressedActionParams[t];
                    CS.SupressedActionsID[t] = SupressedActionsID[t];
                    CS.PriorityList[t] = PriorityList[t];                    
                    CS.counter = 0;
                    CS.index = 0;
                }
                CS.Initialized = true;
            }
        }
    }

    public void DoAllTheThings(int i) { return; }

    Vector2 WindowOffset()
    {
        Vector2 V=Vector2.zero;
        if ((transform.position.x-WSobj.transform.position.x) >= WS.width/2)
            V=new Vector2(-2.5f, V.y);
        else
            V = new Vector2(2.5f, V.y);
        if ((transform.position.y - WSobj.transform.position.y) >= WS.height / 2)
            V = new Vector2(V.x, -2.5f);
        else
            V = new Vector2(V.x, 2.5f);
        return V;
    }
	
	// Update is called once per frame
	void Update () {
        if(counter<delay)
            counter += Time.deltaTime;
        if(delay<=counter)
        {
            if (!WS.isPaused)
            {
                transform.GetChild(9).gameObject.transform.localPosition = transform.GetChild(PriorityList[index]).localPosition;
                transform.GetChild(9).gameObject.transform.eulerAngles =new Vector3(0,0, 45-45*index);
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + (Vector2)transform.GetChild(PriorityList[index]).localPosition, Vector2.zero);
                if (hit)
                {
                    int i = PriorityList[index];
                    if (Actions[i] != null)
                    { Actions[i](ActionParams[i]); }
                }
                else
                {
                    int i = PriorityList[index];
                    if(SupressedActions[i]!=null)
                    { SupressedActions[i](SupressedActionParams[i]); }
                }
                index++;
                if (index == 8)
                    index = 0;
            }
            counter = 0;
        }
	}
}
