using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    GameObject Blinked;
    Color BlinkedColor;

    // Use this for initialization
    void Start () {
		
	}

    bool CheckRotation(float R)
    {
        Vector3 Rot = transform.eulerAngles + new Vector3(0, 0, R);
        for(int i=0; i<transform.childCount; i++)
        {
            if(transform.GetChild(i).CompareTag("P"))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.root.position+Quaternion.Euler(0, 0, Rot.z) * transform.GetChild(i).localPosition, Vector2.zero);
                if (hit.collider != null && !hit.collider.CompareTag("P"))
                {
                    if(Blinked!=null)
                        turnBack();
                    Blinked = transform.GetChild(i).gameObject;
                    BlinkedColor = Blinked.GetComponent<SpriteRenderer>().color;
                    Invoke("turnRed", 0);
                    Invoke("turnBack", .25f);
                    Invoke("turnRed", .5f);
                    Invoke("turnBack", .75f);
                    Invoke("turnRed", 1f);
                    Invoke("turnBack", 1.25f);
                    return false;
                }
            }

        }
        return true;
    }



    bool CheckMove(Vector2 Dir)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Dir, Vector2.zero);
        if (hit.collider != null && !hit.collider.CompareTag("P"))
            return false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("P"))
            {
                hit = Physics2D.Raycast((Vector2)transform.GetChild(i).position+Dir, Vector2.zero);
                if (hit.collider != null && !hit.collider.CompareTag("P"))
                {
                    if (Blinked != null)
                        turnBack();
                    Blinked = transform.GetChild(i).gameObject;
                    BlinkedColor = Blinked.GetComponent<SpriteRenderer>().color;
                    Invoke("turnRed", 0);
                    Invoke("turnBack", .25f);
                    Invoke("turnRed", .5f);
                    Invoke("turnBack", .75f);
                    Invoke("turnRed", 1f);
                    Invoke("turnBack", 1.25f);
                    return false;
                }
            }

        }
        return true;
    }

    public void ChangePlayer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("P"))
            {
               if(transform.GetChild(i).name.StartsWith("S"))
                {
                    Transform T = transform.GetChild(i);
                    T.SetParent(null);
                    transform.SetParent(T);                    
                    T.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    GetComponent<SpriteRenderer>().color = Color.green;
                    for (int c = 0; c < transform.childCount; c++)
                    {
                        if (transform.GetChild(c).CompareTag("P"))
                        {
                            transform.GetChild(c).SetParent(T);
                            c--;
                        }
                    }
                    transform.SetAsLastSibling();
                    transform.root.gameObject.AddComponent<PlayerControl>();
                    Destroy(this);
                    return;
                }
            }

        }
    }

    void turnRed()
    {
        Blinked.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void turnBack()
    {
        Blinked.GetComponent<SpriteRenderer>().color = BlinkedColor;
    }

    // Update is called once per frame
    void Update () {
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && CheckMove(Vector2.right))
            transform.position = (Vector2)transform.position + Vector2.right;
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && CheckMove(Vector2.left))
            transform.position = (Vector2)transform.position + Vector2.left;
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckMove(Vector2.up))
            transform.position = (Vector2)transform.position + Vector2.up;
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && CheckMove(Vector2.down))
            transform.position = (Vector2)transform.position + Vector2.down;
        else if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Z)) && CheckRotation(90))
            transform.Rotate(0, 0, 90);
        else if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.X)) && CheckRotation(-90))
            transform.Rotate(0, 0, -90);
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.Tab))
            ChangePlayer();
    }
}
