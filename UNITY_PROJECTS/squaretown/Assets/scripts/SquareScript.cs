using UnityEngine;
using System.Collections;

public class SquareScript : MonoBehaviour {

    int credits;
    bool isWorking;
    bool isMoving;
    float MaxMiningDistance;
    float MoveCounter;
    Vector2 Direction;
    GameObject TargetNode;

    private void Move()
    {
        MoveCounter -= Time.deltaTime;
        if (MoveCounter > 0)
            transform.Translate(Direction * Time.deltaTime);
        else
        {
            isMoving = false;
        }
    }

    void FindBuyer()
    {

    }

    void Buy()
    {

    }

    void Mine()
    {
        TargetNode = FindNode();
        if(TargetNode != null)
        {
            Direction = TargetNode.transform.position - transform.position;
            isMoving = true;
            Invoke("Pickup", 1f);
        }
    }

    void PickUp()
    {
        TargetNode.transform.SetParent(transform);
        TargetNode.transform.localPosition = new Vector2(TownControl.singleton.RNG.Next(-5, 6) / 5f, TownControl.singleton.RNG.Next(-5, 6) / 5f);
        TownControl.singleton.RemoveNode(TargetNode);
        isWorking = false;
    }

    GameObject FindNode()
    {
        foreach(GameObject g in TownControl.singleton.MiningNodes)
        {
            if(Vector2.SqrMagnitude(g.transform.position-transform.position)<MaxMiningDistance)
            {
                return g;
            }
        }
        return null;
    }

    void UpdateCredits()
    {
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = credits.ToString();
    }


    void RandomWalk()
    {
        isMoving = true;
        Direction= new Vector2(TownControl.singleton.RNG.Next(-5, 6), TownControl.singleton.RNG.Next(-5, 6));
        Invoke("SetNotWorking", 1f);
    }

    void SetNotWorking()
    {
        isWorking = false;
    }

    void FindWork()
    {
        isWorking = true;
        switch(TownControl.singleton.RNG.Next(10))
        {
            case 0:
                RandomWalk();
                break;
            case 1:
                FindBuyer();
                break;
            default:
                Mine();
                break;
        }
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {	

        if(!isWorking)
        {
            FindWork();
        }
        if(isMoving)
        {
            Move();
        }
	}
}
