using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {
    public static PlayerScript singleton;
    public List<UnitStats> Team=new List<UnitStats> { };
    public bool isMoving;
    public GameObject MainMenu;
    GameObject CurrentMenu;
    public float speed;
    public InteractScript InteractableObject;
    public Vector2 InteractOffset;
    GameObject InteractMenu;
    public int Money;
    public int CurrentLaneID;
    public bool isSupport;


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Interact"))
        {
            InteractableObject = coll.GetComponent<InteractScript>();
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Interact") && InteractableObject == coll.GetComponent<InteractScript>())
        {
            InteractableObject = null;
            if (WorldControl.singleton.CurrentWorldMenu != null)
                Destroy(WorldControl.singleton.CurrentWorldMenu);
        }
    }

    private void Awake()
    {
        singleton = this;
    }

    public int TypeCount(int T)
    {
        int c = 0;
        foreach (UnitStats u in Team)
            if (u.TypeID == T)
                c++;
        return c;
    }

    public void Swap(int LaneOne, bool BackOne, int LaneTwo, bool BackTwo)
    {
        int B1 = UnitIndexByPosition(LaneOne, BackOne);
        int B2 = UnitIndexByPosition(LaneTwo, BackTwo);
        if (B1 > -1)
        {
            Team[B1].LaneID = LaneTwo;
            Team[B1].isSupport = BackTwo;
        }
        if (B2 > -1)
        {
            Team[B2].LaneID = LaneOne;
            Team[B2].isSupport = BackOne;
        }
    }

    public int UnitIndexByPosition(int Lane, bool BackRow)
    {
        foreach (UnitStats u in Team)
            if (u.LaneID == Lane && u.isSupport == BackRow)
                return u.Index;
        return -1;
    }

    public void RemoveUnit(int LaneID, bool BackRow)
    {
        bool removed=false;
        for(int i=0;i<Team.Count;i++)
        {
            if (removed)
                Team[i].Index = i;
            else if (Team[i].LaneID == LaneID && BackRow == Team[i].isSupport)
            {
                Team.RemoveAt(i);
                removed = true;
                i--;
            }

        }
    }

    // Use this for initialization
    void Start () {
	
	}

    void Interact()
    {
        if (WorldControl.singleton.CurrentWorldMenu != null)
            Destroy(WorldControl.singleton.CurrentWorldMenu);

        if (InteractableObject != null)
        {
            WorldControl.singleton.CurrentWorldMenu =
            Instantiate(WorldControl.singleton.InteractableMenus[InteractableObject.MenuIndex], (Vector2)InteractableObject.transform.position + InteractOffset, Quaternion.identity)
            as GameObject;
        }
        else
        {
            WorldControl.singleton.CurrentWorldMenu =
                        Instantiate(WorldControl.singleton.LanePreview, (Vector2)transform.position + InteractOffset, Quaternion.identity)
                        as GameObject;
        }
    }

    void ManageMenu()
    {
        if (CurrentMenu != null)
            Destroy(CurrentMenu);
        else
            Instantiate(MainMenu);
    }

    public void HandleTeamAddition()
    {
      
        for (int i = 0; i < 5; i++)
        {
            if (OpenSpot(i, true))
            {
                Team[Team.Count - 1].LaneID = i;
                Team[Team.Count - 1].isSupport = true;
                Team[Team.Count - 1].PlayerControlled = true;
                Team[Team.Count - 1].Index = Team.Count - 1;
            }
            else if (OpenSpot(i, false))
            {
                Team[Team.Count - 1].LaneID = i;
                Team[Team.Count - 1].isSupport = false;
                Team[Team.Count - 1].PlayerControlled = true;
                Team[Team.Count - 1].Index = Team.Count - 1;
            }
        }
    }

    public bool OpenSpot(int Lane, bool BackRow)
    {
        for(int i=0;i<Team.Count-1;i++)
        {
            if (Team[i].LaneID == Lane && Team[i].isSupport == BackRow)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update () {
	if(isMoving)
        {
            if(Input.GetAxis("Horizontal")<0)
            {
                transform.position= (Vector2)transform.position+Vector2.left * speed * Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                transform.Translate(Vector2.down * speed * Time.deltaTime);
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                foreach (UnitStats u in Team)
                    print(u.name + " " + u.LaneID + " " + u.isSupport);
            }
            if (Input.GetKeyDown(KeyCode.F5))
                Application.LoadLevel(0);
        }
	}
}
