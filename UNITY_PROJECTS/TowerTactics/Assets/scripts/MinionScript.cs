using UnityEngine;
using System.Collections.Generic;

public class MinionScript : MonoBehaviour {

    public GameManager GM;
    public GameObject target;
    public int hp;
    public List<Vector2> MoveOrder=new List<Vector2> {};
    public int moveIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("m"))
        {

        }
        else
        {
            if (other.GetComponent<PlayerControl>() != null)
            {
                other.GetComponent<PlayerControl>().ts.tierBuilt[other.GetComponent<PlayerControl>().boolIndex] = false;
                GM.PClist.Remove(other.GetComponent<PlayerControl>());
            }
            Destroy(other.gameObject);
        }
    }

    void OnMouseOver()
    {
        if (GM.turnCount < 5)
        {
            if (GM.CurrentPip != null)
                Destroy(GM.CurrentPip);
            GM.CurrentPip = Instantiate(GM.MovePip, (Vector2)transform.position + FindNextMove(), Quaternion.identity) as GameObject;
            GM.CurrentPip.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        }
    }

    void OnMouseExit()
    {
        if (GM.CurrentPip != null)
            Destroy(GM.CurrentPip);
    }

        // Use this for initialization
        void Start () {

        //need to set target or movelist or both
        MoveOrder = FunMoveLists(GM.RNG.Next(3));
       // int R = GM.RNG.Next(1,4);
       // if (R < 2)
          //  SetTargetBehavior(R);
	
	}

    public void Move()
    {
        transform.position = (Vector2)transform.position+FindNextMove();
    }

    List<Vector2> FunMoveLists(int M)
    {
        List<Vector2> funList = new List<Vector2> { };
        switch(M)
        {
            //zig zag
            case 0:
                int r = GM.RNG.Next(1,4);
                funList.Add(new Vector2(r, r));
                funList.Add(new Vector2(-r, r));
                funList.Add(new Vector2(r, -r));
                funList.Add(new Vector2(-r, -r));
                break;

            //variant knight
            case 1:
                r = GM.RNG.Next(1, 4);
                int s = GM.RNG.Next(1, 4);
                funList.Add(new Vector2(r, s));
                funList.Add(new Vector2(-r, s));
                funList.Add(new Vector2(r, -s));
                funList.Add(new Vector2(-r, -s));
                funList.Add(new Vector2(s, r));
                funList.Add(new Vector2(r, -s));
                funList.Add(new Vector2(-r, s));
                funList.Add(new Vector2(-r, -s));
                break;

            //charger
            case 2:
                r = GM.RNG.Next(1, 4);
                s = GM.RNG.Next(1, 4);
                funList.Add(new Vector2(0, r));
                funList.Add(new Vector2(0, s));
                funList.Add(new Vector2(0, -r));
                funList.Add(new Vector2(0, -s));
                funList.Add(new Vector2(r, 0));
                funList.Add(new Vector2(s, 0));
                funList.Add(new Vector2(-r, 0));
                funList.Add(new Vector2(-s, 0));
                break;

            default:
                break;
        }

        return funList;
    }

    void SetTargetBehavior(int T)
    {
        switch(T)
        {
            //closest castle
            case 0:
                    float d = Mathf.Infinity;
                    foreach (GameObject g in GM.CastleList)
                    {
                        if (Vector2.Distance(g.transform.position, transform.position) < d)
                        {
                            d = Vector2.Distance(g.transform.position, transform.position);
                            target = g;
                        }
                    }
                break;
            //closet tower
            case 2:
                 d = Mathf.Infinity;
                foreach (PlayerControl p in GM.PClist)
                {
                    if (Vector2.Distance((Vector2)p.transform.position, (Vector2)transform.position) < d)
                    {
                        d = Vector2.Distance((Vector2)p.transform.position, (Vector2)transform.position);
                        target = p.gameObject;
                    }
                }
                break;
            //random castle
            case 1:
                target = GM.CastleList[GM.RNG.Next(GM.CastleList.Count)];
                break;

            //random tower
            case 3:
                if (GM.PClist.Count > 0)
                    target = GM.PClist[GM.RNG.Next(GM.PClist.Count)].gameObject;
                break;

            default:
                break;
        }
    }

    void SetMoveIndex()
    {
        Vector2 v = (Vector2)transform.position + MoveOrder[moveIndex]-(Vector2)GM.transform.position;
        if (v.x>=0 && v.x < GM.width && v.y>=0 && v.y < GM.height)
        {
        }
        else
        {
            while(!(v.x >= 0 && v.x < GM.width && v.y >= 0 && v.y < GM.height))
            {
                moveIndex++;
                if (moveIndex == MoveOrder.Count)
                    moveIndex = 0;
                v = (Vector2)transform.position + MoveOrder[moveIndex] - (Vector2)GM.transform.position;
            }
        }
    }

    public Vector2 FindNextMove()
    {
        Vector2 Move = Vector2.zero;
        if (MoveOrder.Count > 0)
        {
            if(target == null)
            {
                SetMoveIndex();
                Move = MoveOrder[moveIndex];
            }
            else
            {
                float d = Mathf.Infinity;
                foreach(Vector2 v in MoveOrder)
                {
                    if(Vector2.Distance((Vector2)transform.position+v, target.transform.position)<d )
                    {
                        d = Vector2.Distance((Vector2)transform.position + v, target.transform.position);
                        Move = v;
                    }
                }
            }
        }
        else
        {
            //diagonal rush
            Vector2 Dir = target.transform.position - transform.position;            
            if (Dir.x < 0)
                Move += Vector2.left;
            else if (Dir.x > 0)
                Move += Vector2.right;

            if (Dir.y < 0)
                Move += Vector2.down;
            else if (Dir.y > 0)
                Move += Vector2.up;
        }
        return Move;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
