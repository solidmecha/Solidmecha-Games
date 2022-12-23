using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubroutineSelect : MonoBehaviour {

    public int BreakID;
    public int BreakCount;
    public bool Broken;
    public int ActionID;
    public int ActionValue;
    public int ActionCount;
    public bool isReadable;
    public int Bounty;

    private void OnMouseDown()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameController");
        go.GetComponent<GameControl>().TargetSubRoutine=this;
        go.GetComponent<PlayerControl>().BreakID = BreakID;
        go.GetComponent<PlayerControl>().BreakCount = BreakCount;
        go.GetComponent<GameControl>().Reticle.transform.position = (Vector2)transform.position+2*Vector2.right;
    }

     string GenerateToolTip()
    {
        if (isReadable)
        {
            switch (ActionID)
            {
                case 0: return BreakCount.ToString() + "      " + "Destroy " + ActionCount.ToString() + " " + IconID();
                case 1: return BreakCount.ToString() + "      " + "Destroy " + ActionCount.ToString() + " at random";
                case 2: return BreakCount.ToString() + "      " + "Add " + ActionValue.ToString() + " Turns";
                case 3: return BreakCount.ToString() + "      " + "Discharge " + ActionCount.ToString()+" "+ IconID();
                case 4: return BreakCount.ToString() + "      " + "Discharge " + ActionCount.ToString() + " at random";
                case 5: return BreakCount.ToString() + "      " + "Lose " + (ActionCount*50).ToString() + " Credits";
                case 12:
                    if(ActionCount>1)
                        return BreakCount.ToString() + "      " + "Lose " + ActionCount.ToString()+ " clicks";
                    else
                        return BreakCount.ToString() + "      " + "Lose " + ActionCount.ToString() + " click";
                case 100: return BreakCount.ToString() + "      " + transform.root.GetComponent<IceControl>().Turns.ToString() + " turns remain.";

                default: return "";
            }
        }
        else
            return BreakCount.ToString() + "      " + " ????";
            
    }

    public void SetReadable()
    {
        isReadable = true;
        SetToolTip();
    }

    public void SetToolTip()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GenerateToolTip();
    }

    public string IconID()
    {
        switch(ActionValue)
        {
            case 0:return "eye";
            case 1:return "virus";
            case 2: return "sword";
            case 3: return "wrench";
            case 4: return "power";
            case 5: return "key";
            case 6: return "heart";
            case 7: return "money";

            default: return "";
        }
    }

    // Use this for initialization
    void Start () {
       SetToolTip();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
