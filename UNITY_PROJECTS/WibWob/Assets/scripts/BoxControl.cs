using UnityEngine;
using System.Collections;

public class BoxControl : MonoBehaviour {

    public PlayerControl PC;
    bool hooked;
    bool pushed;
    bool pulled;
    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "bolt(Clone)":
                if (!hooked)
                    Destroy(gameObject);
                else
                    hooked = false;
                break;
            case "hook(Clone)":
                if (other.transform.root.name.Equals("Star"))
                {

                    Vector2 v = transform.position + transform.position - PC.transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);
                    if (!hit || hit.collider.CompareTag("noBlock"))
                    {
                        if (PC.transform.childCount > 1)
                        {
                            PC.transform.GetChild(1).transform.position = PC.FindPos(PC.transform.position - PC.GC.transform.position);
                            PC.transform.GetChild(1).SetParent(null);
                        }
                        PC.transform.position = v;
                        if (PC.pos.x == transform.position.x && PC.pos.y == transform.position.y)
                            PC.pos = PC.FindPos(PC.transform.position - PC.GC.transform.position);
                        hooked = true;
                    }
                    else
                    {
                        PC.speed = 3;
                        transform.SetParent(null);
                        transform.position = PC.FindPos(transform.position - PC.GC.transform.position);
                        PC.pos = PC.FindPos(PC.transform.position - PC.GC.transform.position);
                        PC.transform.position = PC.FindPos(PC.transform.position - PC.GC.transform.position);
                    }
                }
                break;
            case "push(Clone)":
                if (!hooked && other.transform.root.name.Equals("Star"))
                {
                    PC.speed = 2.5f;
                    transform.SetParent(PC.transform);
                    pushed = true;
                }
                else
                    hooked = false;
                    break;
            case "vac(Clone)":
                if (!pulled && !hooked && other.transform.root.CompareTag("Player"))
                {
                    transform.SetParent(PC.transform);
                    pulled = true;
                    PC.speed = 2.5f;
                }
                else if(hooked)
                    hooked = false;
                break;
            default:
                if (!other.CompareTag("noBlock"))
                {
                    PC.speed = 3;
                    transform.SetParent(null);
                    transform.position = PC.FindPos(transform.position - PC.GC.transform.position);
                    PC.pos = PC.FindPos(PC.transform.position - PC.GC.transform.position);
                    PC.transform.position = PC.FindPos(PC.transform.position - PC.GC.transform.position);
                }
                break;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.name.Equals("vac(Clone)") && pulled)
        {
            PC.speed = 3;
            pulled = false;
            transform.SetParent(null);
            transform.position = PC.FindPos(transform.position - PC.GC.transform.position);
        }
    }
	// Use this for initialization
	void Start () {
        PC = (PlayerControl)GameObject.Find("Star").GetComponent(typeof(PlayerControl));
	}

    void checkPull()
    {
        Vector2 v = transform.position - PC.transform.position;
        Vector2 p = PC.pos-(Vector2)PC.transform.position;;
        if ((p.x * v.x >= 0 && p.y == v.y) || (p.y * v.y >= 0 && p.x == v.x))
        {
            PC.pos = PC.FindPos((PC.transform.position - PC.GC.transform.position));
        }
        if (!((p.x * v.x < 0 && p.y == v.y) || (p.y * v.y < 0 && p.x == v.x)))
        {
            PC.speed = 3;
            pulled = false;
            transform.SetParent(null);
            transform.position = PC.FindPos(transform.position - PC.GC.transform.position);
        }
    }
	
	// Update is called once per frame
	void Update () {
    if(pushed && !PC.move)
        {
            transform.SetParent(null);
            transform.position = PC.FindPos(transform.position - PC.GC.transform.position);
            pushed = false;
            PC.speed = 3;
        }
    else if(pulled && PC.move)
        {
            checkPull();
        }
	}
}
