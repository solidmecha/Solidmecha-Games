using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
    public PlayerControl PC;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "hook(Clone)":
                if (!name.Equals("border") && other.transform.root.CompareTag("Player"))
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
            case "vac(Clone)":
                break;
            default:
                if (!other.CompareTag("noBlock") && PC != null)
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
    // Use this for initialization
    void Start()
    {
        PC = (PlayerControl)GameObject.Find("Star").GetComponent(typeof(PlayerControl));
    }
    // Update is called once per frame
    void Update () {
	
	}
}
