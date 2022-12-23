using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BugScript : NetworkBehaviour {
    System.Random RNG;
    bool isMoving;
    float counter;
    Vector2 Dir;

	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        if(isServer)
            NewColor();
	}

    private void Move()
    {
        Vector2[] vec =new Vector2[4] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + vec[i], Vector2.zero);
            if (hit.collider != null && SameColor(hit.collider.GetComponent<SpriteRenderer>().color))
            {
                Dir = vec[i];
                isMoving = true;
            }
        }
        //Dir = new Vector2(RNG.Next(-1,2), RNG.Next(-1,2));

    }

    bool SameColor(Color c)
    {
        Color t = GetComponent<SpriteRenderer>().color;
        return c.r == t.r && c.g == t.g && c.b == t.b;
    }

    void NewColor()
    {
        int r, g, b;
        do
        {
            r = RNG.Next(2);
            g = RNG.Next(2);
            b = RNG.Next(2);
        } while (!(r != g || g != b || b != r));
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        if(isServer)
            RpcSetColor(new Color(r, g, b));
    }

    [ClientRpc]
    public void RpcSetColor(Color C)
    {
        GetComponent<SpriteRenderer>().color = C;
    }

    // Update is called once per frame
    void Update () {
        if (!isMoving)
        {
            counter += Time.deltaTime;
            if (counter > 1f)
            {
                counter = 0;
                Move();
            }
        }
        if (isMoving)
        {
            counter += Time.deltaTime;
            if(counter>=.5f)
            {
                counter = 0;
                    isMoving = false;
                NewColor();
            }
            transform.Translate(Dir*2* Time.deltaTime);
        }
	}
}
