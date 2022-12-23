using UnityEngine;
using System.Collections.Generic;

public class TileControl : MonoBehaviour {

    void OnMouseDown()
    {
        if(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().SelectedPiece != null && !occupied)
        {
            GameObject sp = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().SelectedPiece;

            if (!sp.CompareTag("Player"))
            {
                Vector2 delta = transform.position - sp.transform.position;
                delta = new Vector2(delta.x * -1, delta.y);
                if (checkMove((Vector2)sp.GetComponent<PieceControl>().Mirror.transform.position + delta * -1))
                {
                    sp.GetComponent<PieceControl>().Mirror.transform.position = (Vector2)sp.GetComponent<PieceControl>().Mirror.transform.position + -1 * delta;
                    sp.transform.position = transform.position + Vector3.back;
                }
            }
            else if (CheckConnectivity((Vector2)sp.transform.position + new Vector2(4, 4), (Vector2)transform.position + new Vector2(4, 4)))
            {
                sp.transform.position = transform.position + Vector3.back;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().CheckVictory();
            }

        }
    }

    bool checkMove(Vector2 pos)
    {
        RaycastHit2D r = Physics2D.Raycast(pos, Vector2.zero);

        return (r.collider != null && r.collider.gameObject.GetComponent<TileControl>() && !r.collider.GetComponent<TileControl>().occupied);
    }

    bool CheckConnectivity(Vector2 v1, Vector2 v2)
    {
        List<Vector2> ToCheckList = new List<Vector2> { };
        ToCheckList.Add(v1);
        Vector2 Target = v2;
        Target.x = Mathf.RoundToInt(Target.x);
        Target.y = Mathf.RoundToInt(Target.y);
        List<Vector2> CheckedList = new List<Vector2> { };
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        while(ToCheckList.Count>0)
        {
            Vector2[] Dirs = new Vector2[4] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            foreach(Vector2 v in Dirs)
            {
                Vector2 c = ToCheckList[0] + v;
                c.x = Mathf.RoundToInt(c.x);
                c.y = Mathf.RoundToInt(c.y);
                if (c.x>=0 && c.y>=0 && c.x<9 && c.y<9 && !GC.Tiles[(int)c.x][(int)c.y].occupied && !GC.Tiles[(int)c.x][(int)c.y].wasChecked)
                {

                    if (c.x == Target.x && c.y == Target.y)
                    {
                        foreach(Vector2 vc in CheckedList)
                        {
                            GC.Tiles[(int)vc.x][(int)vc.y].wasChecked = false;
                        }
                        return true;
                    }
                    else
                    {
                        ToCheckList.Add(c);
                    }
                }
            }
            CheckedList.Add(ToCheckList[0]);
            GC.Tiles[(int)ToCheckList[0].x][(int)ToCheckList[0].y].wasChecked = true;
            ToCheckList.RemoveAt(0);
            if (ToCheckList.Count == 0)
            {
                foreach (Vector2 vc in CheckedList)
                {
                    GC.Tiles[(int)vc.x][(int)vc.y].wasChecked = false;
                }
            }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        occupied = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        occupied = false;
    }

    public bool occupied;
    public bool wasChecked;
        
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
