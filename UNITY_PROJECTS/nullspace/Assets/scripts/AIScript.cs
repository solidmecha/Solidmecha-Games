using UnityEngine;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {


    public List<Transform> Ships;
    public List<int> ShipsToCheck;
    public List<Transform> Players;
    Vector2 ShotLocation;
    Vector2 MoveLocation;
    Vector2 FinalLocation;
    int ShipIndex;
    bool Moving;
    float counter;
    System.Random RNG=new System.Random();
  
    public void BotMove()
    {
        SetShipsToCheck();
        print(ShipsToCheck.Count);
        if (ShipsToCheck.Count>0)
        {
            //RandomShot();
            PlayerShot();
        }
        else
        {
            //gameloss
            GetComponent<GameControl>().Victory();
        }
        
    }

    public void SetShipsToCheck()
    {
        GameControl gc = GetComponent<GameControl>();
        ShipsToCheck.Clear();
        for (int i = 0; i < Ships.Count; i++)
        {
            if (gc.PossibleMovement((int)Ships[i].position.x, (int)Ships[i].position.y).Count > 0)
                ShipsToCheck.Add(i);
        }
    }

    void RandomShot()
    {
        GameControl gc = GetComponent<GameControl>();
        ShipIndex = ShipsToCheck[RNG.Next(ShipsToCheck.Count)];
        var P = gc.PossibleMovement((int)Ships[ShipIndex].position.x, (int)Ships[ShipIndex].position.y);

            gc.SelectedShip = Ships[ShipIndex];
            FinalLocation=P[RNG.Next(P.Count)];
            MoveLocation = FinalLocation - (Vector2)Ships[ShipIndex].transform.position;
            Moving = true;
            gc.UpdateWorld(Ships[ShipIndex].transform.position, FinalLocation);
            P = gc.PossibleMovement((int)FinalLocation.x, (int)FinalLocation.y);
            ShotLocation=P[RNG.Next(P.Count)];

    }

    void PlayerShot()
    {
        GameControl gc = GetComponent<GameControl>();

        //FIX: handle variable player ship count and check for win
        var V0 = gc.PossibleMovement((int)Players[0].position.x, (int)Players[0].position.y);
        var V1 = gc.PossibleMovement((int)Players[1].position.x, (int)Players[1].position.y);
        var V2 = gc.PossibleMovement((int)Players[2].position.x, (int)Players[2].position.y);
        var V3 = gc.PossibleMovement((int)Players[3].position.x, (int)Players[3].position.y);
        foreach (Vector2 v in V1)
            V0.Add(v);
        foreach (Vector2 v in V2)
            V0.Add(v);
        foreach (Vector2 v in V3)
            V0.Add(v);
        if(V0.Count==0)
        {
            RandomShot();
            gc.Invoke("Defeat", .5f);
            return;
        }
        Vector2 ShotLoc = V0[RNG.Next(V0.Count)];
        var P = gc.PossibleMovement((int)ShotLoc.x, (int)ShotLoc.y);
        List<Vector2> CurrentPositions = new List<Vector2> { };
        List<List<Vector2>> ReachablePositions = new List<List<Vector2>> { };
        List<int> PossibleIndex = new List<int> { };
        for(int i=0;i<Ships.Count;i++)
        {
            CurrentPositions.Add(Ships[i].position);
            ReachablePositions.Add(new List<Vector2> { });
            int t = i;
            PossibleIndex.Add(t);
        }

        foreach(Vector2 v in P)
        {
            RaycastHit2D hit=Physics2D.Raycast(v, Vector2.up);
            if(hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.down);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.left);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.right);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.right+Vector2.up);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.right + Vector2.down);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.left + Vector2.down);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
            hit = Physics2D.Raycast(v, Vector2.left + Vector2.up);
            if (hit.collider != null && hit.collider.CompareTag("E"))
            {
                ReachablePositions[hit.transform.GetSiblingIndex()].Add(v);
            }
        }
        for (int i = ReachablePositions.Count-1; i >=0; i--)
        {
            if (ReachablePositions[i].Count == 0)
            {
                PossibleIndex.RemoveAt(i);
            }
        }
        if (PossibleIndex.Count == 0)
        {
            //maybe try again?
            RandomShot();
        }
        else
        {
            ShipIndex = PossibleIndex[RNG.Next(PossibleIndex.Count)];
            gc.SelectedShip = Ships[ShipIndex];
            //gc.Move(ReachablePositions[S][RNG.Next(ReachablePositions[S].Count)]);
            //gc.Move(ShotLoc);
            ShotLocation = ShotLoc;
            FinalLocation = ReachablePositions[ShipIndex][RNG.Next(ReachablePositions[ShipIndex].Count)];
            MoveLocation = FinalLocation-(Vector2)Ships[ShipIndex].transform.position;
            gc.UpdateWorld(Ships[ShipIndex].transform.position, FinalLocation);
            Moving = true;
        }
    }

    void StepShot()
    {

    }

    void AdjacentShot()
    {

    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	 if(Moving)
        {
            Ships[ShipIndex].transform.Translate(MoveLocation * Time.deltaTime * -2);
            counter += Time.deltaTime;
            if(counter>=.5f)
            {
                counter = 0;
                Ships[ShipIndex].transform.position=FinalLocation;
                GetComponent<GameControl>().Move(ShotLocation);
                Moving = false;
            }
        }
	}
}
