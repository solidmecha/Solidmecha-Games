using UnityEngine;
using System.Collections.Generic;

public class NPCScript : MonoBehaviour {
    public List<UnitScript> NPCunits;
    public float EvalTimer;

	// Use this for initialization
	void Start () {
	}

    void MakeMoves()
    {
        foreach(UnitScript u in NPCunits)
        {
            if(u.HP[0]>0)
            {
                if (u.TargetPoint == null)
                {
                    u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(5)]);
                    if(GameControl.singleton.RNG.Next(10)==4)
                    {
                        u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(GameControl.singleton.Points.Count)]);
                        if(u.TargetPoint.Camp && u.TargetPoint.GetComponent<CampScript>().Mobs.Count==0)
                            u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(5)]);
                    }
                }
                else if(!u.isMoving)
                {
                    if (u.TargetPoint.Camp && u.TargetPoint.GetComponent<CampScript>().Mobs.Count == 0)
                        u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(5)]);
                    else if (!u.TargetPoint.CheckConflict() && !u.TargetPoint.Neutral && !u.TargetPoint.PlayerControlled &&  (u.TargetPoint.Units.Count==0 || !u.TargetPoint.Units[0].PlayerControlled))
                    {
                        u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(5)]);
                        if (GameControl.singleton.RNG.Next(10) == 4)
                        {
                            u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(GameControl.singleton.Points.Count)]);
                            if (u.TargetPoint.Camp && u.TargetPoint.GetComponent<CampScript>().Mobs.Count == 0)
                                u.SetTargetPoint(GameControl.singleton.Points[GameControl.singleton.RNG.Next(5)]);
                        }
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        EvalTimer -= Time.deltaTime;
	if(EvalTimer<=0)
        {
            EvalTimer = 0.5f;
            MakeMoves();
        }
	}
}
