using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiscPlayerControl :NetworkBehaviour {

    Vector2 orig;
    public const float ForceScale=5.5f;

	// Use this for initialization
	void Start () {
        if(isServer)
        {
            int index = 0;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            {
                g.GetComponent<DiscPlayerControl>().UpdatePlayer(index);
                index++;
            }
        }
    }

    [ClientRpc]
    public void RpcUpdateName(string N, Color C)
    {
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = N;
        GetComponent<SpriteRenderer>().color = C;
    }

    private void Awake()
    {

    }

    void UpdatePlayer(int index)
    {
        NameHolder NH = GameObject.FindGameObjectWithTag("NameHolder").GetComponent<NameHolder>();
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = NH.Names[index];
        RpcUpdateName(NH.Names[index], NH.Colors[index]);
        GetComponent<SpriteRenderer>().color = NH.Colors[index];
    }

    [Command]
    void CmdMove(Vector2 v)
    {
        GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
                orig = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 cur = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cur = (cur - orig);
                if (Vector2.SqrMagnitude(cur) > 1)
                {
                    cur = cur.normalized;
                }
                CmdMove(cur * ForceScale);
                
            }
        }
	}
}
