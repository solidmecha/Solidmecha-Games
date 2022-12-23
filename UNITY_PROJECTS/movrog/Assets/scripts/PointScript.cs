using UnityEngine;
using System.Collections.Generic;

public class PointScript : MonoBehaviour {

    public int ID;
    public bool Camp;
    public List<UnitScript> Units;
    public bool Neutral;
    public bool PlayerControlled;
    public float captureAmt;

	// Use this for initialization
	void Start () {
	
	}

    private void OnMouseDown()
    {
        if(GameControl.singleton.Units[GameControl.singleton.SelectedUnitIndex].HP[0]>0 && !GameControl.singleton.Units[GameControl.singleton.SelectedUnitIndex].isMoving)
            GameControl.singleton.Units[GameControl.singleton.SelectedUnitIndex].SetTargetPoint(this);
    }

    public bool CheckConflict()
    {
        if(Units.Count>=2)
        {
            bool hasPlayer=false;
            bool hasNPC=false;
            foreach(UnitScript u in Units)
            {
                if (u.PlayerControlled)
                    hasPlayer = true;
                else
                    hasNPC = true;
                if (hasPlayer && hasNPC)
                    return true;
            }
            return false;
        }
        return false;
    }

    public bool CheckCapture()
    {
        return !Camp && Units.Count > 0 && (Units[0].PlayerControlled != PlayerControlled || Neutral);
    }

    public void ResolveUnits()
    {
        if (CheckConflict())
        {
            List<int> PlayerIndex = new List<int> { };
            List<int> NPCIndex = new List<int> { };
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i].PlayerControlled)
                    PlayerIndex.Add(i);
                else
                    NPCIndex.Add(i);
            }
            foreach (UnitScript u in Units)
            {
                if (u.PlayerControlled)
                {
                    UnitScript targetA = Units[NPCIndex[GameControl.singleton.RNG.Next(NPCIndex.Count)]];
                    UnitScript targetB = Units[NPCIndex[GameControl.singleton.RNG.Next(NPCIndex.Count)]];
                    if (targetA.HP[0] <= 0 || targetA.HP[0] >= targetB.HP[0])
                        u.Attack(targetB);
                    else
                        u.Attack(targetA);
                }
                else
                    u.Attack(Units[PlayerIndex[GameControl.singleton.RNG.Next(PlayerIndex.Count)]]);
            }
        }
        else if(CheckCapture())
        {
            foreach(UnitScript u in Units)
                captureAmt += (u.speed/2f);
            UpdateLines();
            if(captureAmt>=100)
            {
                captureAmt = 0;
                PlayerControlled = Units[0].PlayerControlled;
                transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, new Vector3(-2.5f, 2.3f, -0.05f));
                transform.GetChild(1).GetComponent<LineRenderer>().SetPosition(1, new Vector3(2.3f, 2.5f, -0.05f));
                transform.GetChild(2).GetComponent<LineRenderer>().SetPosition(1, new Vector3(2.5f, -2.3f, -0.05f));
                transform.GetChild(3).GetComponent<LineRenderer>().SetPosition(1, new Vector3(-2.3f, -2.5f, -0.05f));
                if (PlayerControlled)
                    GetComponent<SpriteRenderer>().color = Color.blue;
                else
                {
                    if(!Neutral)
                    {
                        GameControl.singleton.VP[1] += 20;
                        GameControl.singleton.VPText[1].GetComponent<UnityEngine.UI.Text>().text = GameControl.singleton.VP[1].ToString();
                    }
                    GetComponent<SpriteRenderer>().color = Color.red;
                }
                Neutral = false;
            }
        }
        else if (Camp)
        {
            if(GetComponent<CampScript>().Mobs.Count>0 && Units.Count>0)
            {
                foreach (UnitScript m in GetComponent<CampScript>().Mobs)
                {
                    if (Units.Count > 0)
                    {
                        if(Units[0].PlayerControlled)
                            m.Attack(Units[GameControl.singleton.RNG.Next(Units.Count)]);
                    }
                }
                foreach (UnitScript u in Units)
                {
                    if(GetComponent<CampScript>().Mobs.Count > 0)
                        u.Attack(GetComponent<CampScript>().Mobs[GameControl.singleton.RNG.Next(GetComponent<CampScript>().Mobs.Count)]);
                }
            }
        }
    }

    public void UpdateLines()
    {
        if(Units[0].PlayerControlled)
        {
            if(captureAmt<=25)
            {
                transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, new Vector3(-2.5f+(captureAmt * 5f/ 25f), 2.3f, -0.05f));
            }
            else if(captureAmt<=50)
            {
                transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, new Vector3(2.5f, 2.3f, -0.05f));
                transform.GetChild(1).GetComponent<LineRenderer>().SetPosition(1, new Vector3(2.3f, 2.5f+((captureAmt - 25) * -4.66f / 25f), -0.05f));
            }
            else if(captureAmt<=75)
            {
                transform.GetChild(1).GetComponent<LineRenderer>().SetPosition(1, new Vector3(2.3f, -2.5f, -0.05f));
                transform.GetChild(2).GetComponent<LineRenderer>().SetPosition(1, new Vector3(2.5f+((captureAmt-50f) * -5f / 25f), -2.3f, -0.05f));
            }
            else
            {
                transform.GetChild(2).GetComponent<LineRenderer>().SetPosition(1, new Vector3(-2.5f, -2.3f, -0.05f));
                transform.GetChild(3).GetComponent<LineRenderer>().SetPosition(1, new Vector3(-2.3f, -2.5f+((captureAmt - 75f) * 4.6f / 25f), -0.05f));
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
