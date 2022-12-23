using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    public int[] IDs;
    public bool marked;
    public Vector2 destination;
    bool Move;
    float c;
    bool Locked;
    public GameObject lockLine;

    private void OnMouseDown()
    {
        if (!Input.GetMouseButton(1) && !Locked)
        {
            GameControl.singleton.isMatching = false;
            GameControl.singleton.ActiveTile = this;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        if(!Move && !GameControl.singleton.isMatching && GameControl.singleton.ActiveTile !=null && !Locked)
        {
            destination=GameControl.singleton.ActiveTile.destination;
            GameControl.singleton.ActiveTile.destination = transform.position;
            Move = true;
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButton(1) && !Input.GetMouseButton(0) && GameControl.singleton.isMatching)
        {
            if (!marked)
            {
                GameControl.singleton.PotentialMatches.Add(this);
                marked = true;
                GameControl.singleton.Outlines.Add(Instantiate(GameControl.singleton.Outline, transform.position, Quaternion.identity) as GameObject);
            }
        }
        else if(!Input.GetMouseButton(1) && !Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.N))
        {
            Locked = !Locked;
            if(Locked)
            {
                lockLine=Instantiate(GameControl.singleton.Outline, transform.position, Quaternion.identity) as GameObject;
                lockLine.GetComponent<SpriteRenderer>().color = Color.red;
                lockLine.transform.parent = transform;
            }
            else
            {
                Destroy(lockLine);
            }

        }
    }

    // Use this for initialization
    void Start()
    {
        destination = transform.position;
        c = .5f;
        GetComponent<SpriteRenderer>().sprite = GameControl.singleton.Shapes[IDs[0]];
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameControl.singleton.Patterns[IDs[1]];
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = GameControl.singleton.Numbers[IDs[2]];
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = GameControl.singleton.Colors[IDs[3]];
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameControl.singleton.Colors[IDs[3]];
    }

        // Update is called once per frame
        void Update () {
        if(Move)
        {
            c -= Time.deltaTime;
            transform.Translate((destination - (Vector2)transform.position).normalized * Time.deltaTime * 4);
            if(c<=0)
            {
                transform.position = destination;
                c = .5f;
                Move = false;
            }
        }
	
	}
}
