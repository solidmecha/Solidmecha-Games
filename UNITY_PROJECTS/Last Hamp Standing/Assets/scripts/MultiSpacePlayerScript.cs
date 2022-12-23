using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiSpacePlayerScript : NetworkBehaviour {

    public int LocalPlayerID;
    public Color color;
    public string PlayerName;
    public MultiSpaceControl MC;
    public bool PlayingMultiSpace;

    // Use this for initialization
    void Start () {
    }

    [ClientRpc]
    public void RpcDisplayShot(int x, int y)
    {
        if (isLocalPlayer)
        {
            var List = MC.PossibleMovement(x, y);
            foreach (Vector2 v in List)
                Instantiate(MC.ShotOutline, v, Quaternion.identity);
        }
    }

    [Command]
    public void CmdMove(Vector2 v)
    {
        if (MC.ShipMove)
        {
            MC.RpcClearClientMovement();
            MC.World[(int)MC.SelectedShip.transform.position.x + 6][(int)MC.SelectedShip.transform.position.y + 6] = true;
            MC.World[(int)v.x + 6][(int)v.y + 6] = false;
            MC.RpcUpdateWorld((Vector2)MC.SelectedShip.transform.position + new Vector2(6, 6), true);
            MC.RpcUpdateWorld(v + new Vector2(6, 6), false);
            RpcDisplayShot((int)v.x, (int)v.y);
            MC.SelectedShip.transform.position = v;
            MC.SelectedShip.GetComponent<MultiSpaceShipControl>().RpcUpdatePosition(v);
            MC.ShipMove = false;
        }
        else
        {
            MC.RpcClearClientMovement();
            MC.World[(int)v.x + 6][(int)v.y + 6] = false;
            MC.RpcUpdateWorld(v + new Vector2(6, 6), false);
            GameObject go = Instantiate(MC.Static, v, Quaternion.identity);
            go.GetComponent<SpriteRenderer>().color = color;
            go.GetComponent<StaticScript>().ColorID = GetComponent<PlayerControl>().ColorID;
            NetworkServer.Spawn(go);
            MC.ShipMove = true;
            MC.PlayerTurn++;
            if (MC.PlayerTurn == MC.Players.Count)
                MC.PlayerTurn = 0;
            int PlayerOutCount = 0;
            bool PlayerReady = false;
            do
            {
                foreach (Transform t in MC.ShipPositions[MC.PlayerTurn])
                {
                    if (MC.PossibleMovement((int)t.position.x, (int)t.position.y).Count > 0)
                    {
                        PlayerReady = true;
                        break;
                    }
                }
                    if (!PlayerReady)
                    {
                        PlayerOutCount++;
                        MC.PlayerTurn++;
                        if (MC.PlayerTurn == MC.Players.Count)
                            MC.PlayerTurn = 0;
                    }

                if (PlayerOutCount == MC.Players.Count - 1)
                    break;
            } while (!PlayerReady);
            if (PlayerReady)
                MC.RpcMessage("Active Player: " + MC.Players[MC.PlayerTurn].PlayerName, 24);
            else
                MC.RpcMessage(MC.Players[MC.PlayerTurn].PlayerName + " wins!", 24);
        }
    }

    [Command]
    public void CmdSetSelectedShip(int Index)
    {
        MC.SelectedShip = MC.ShipPositions[MC.PlayerTurn][Index].gameObject;
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        if (isLocalPlayer)
        {
            MC = GameObject.FindGameObjectWithTag("MultiControl").GetComponent<MultiSpaceControl>();
            PlayerControl p = GetComponent<PlayerControl>();
            color = GameControl.singleton.Colors[p.ColorID];
            LocalPlayerID = p.PlayerIndex;
            PlayerName = p.PlayerName;
            PlayingMultiSpace = true;
        }

    }
    // Update is called once per frame
    void Update () {
        if(PlayingMultiSpace && MC == null)
            MC = GameObject.FindGameObjectWithTag("MultiControl").GetComponent<MultiSpaceControl>();
        if (!isLocalPlayer || !PlayingMultiSpace)
            return;
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D col = Physics2D.Raycast(pos, Vector2.zero).collider;
                if (col != null && col.CompareTag("S"))
                {
                    if (col.GetComponent<MultiSpaceShipControl>().SelectShip(LocalPlayerID))
                        CmdSetSelectedShip(col.GetComponent<MultiSpaceShipControl>().Index);
                }
                else if(col != null && col.CompareTag("M"))
                {
                   CmdMove(col.transform.position);
                }
            }
        }
	}
}
