using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    [SyncVar]
    public int HP;

    [SyncVar]
    public int Movement;

    [SyncVar]
    public int hitChance;

    [SyncVar]
    public int CritChance;

    [SyncVar]
    public int Dmg;

    [SyncVar]
    public bool isReady;

    [SyncVar]
    public bool isFiring;

    [SyncVar]
    public int ColorID;

    [SyncVar]
    public int PlayerIndex;

    public GameControl GC;

    [SyncVar]
    public string PlayerName;

    [SyncVar]
    public Vector2 TargetPosition;

    public GameObject SetUpCanvas;
    public GameObject NameCanvas;

    public GameObject MoveDiamond;
    GameObject CurrentMoveDiamond;

    [ClientRpc]
    public void RpcUpdatePosition(Vector2 pos)
    {
        transform.position = pos;
        if(CurrentMoveDiamond != null)
            Destroy(CurrentMoveDiamond);
    }

    public override void OnNetworkDestroy()
    {
        CmdRemovePlayer();
    }

    [Command]
    void CmdRemovePlayer()
    {
        GameControl.singleton.Players.Remove(this);
        GameControl.singleton.PlayerCount = GameControl.singleton.Players.Count;
    }

    [Command]
    public void CmdSetPlayerColor(int ID)
    {
        GameControl.singleton.Players[PlayerIndex].ColorID = ID;
        GameControl.singleton.UpdatePlayerColors();
    }

    [Command]
    public void CmdSyncPlayerColors()
    {
        GameControl.singleton.SyncPlayerColors();
    }

    [ClientRpc]
    public void RpcSetColor(int ID)
    {
        ColorID = ID;
        if (ColorID >= 0)
            GetComponent<SpriteRenderer>().color = GameControl.singleton.Colors[ColorID];
    }


    [Command]
    public void CmdSetPlayerName(string n)
    {
        GameControl.singleton.Players[PlayerIndex].PlayerName = n;
        GameControl.singleton.UpdatePlayerNames();
    }

    [ClientRpc]
    public void RpcSetName(string n)
    {
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = n;
    }

    [ClientRpc]
    public void RpcAddMulti()
    {
        if (!isServer)
        {
            MultiSpacePlayerScript M=gameObject.AddComponent<MultiSpacePlayerScript>();
            M.color = GameControl.singleton.Colors[ColorID];
            M.LocalPlayerID = PlayerIndex;
            M.PlayerName = PlayerName;
            Destroy(this);
        }
    }

    [ClientRpc]
    public void RpcRevealRole(string Role, int textIndex)
    {
        if (isLocalPlayer)
        {
            Transform C = GameObject.FindGameObjectWithTag("MainCanvas").transform;
            C.GetChild(textIndex).GetComponent<Text>().text = Role;
        }
    }

    private void OnMouseOver()
    {
        Transform C = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        C.GetChild(29).GetComponent<Text>().text = "HP: " + HP.ToString();
        C.GetChild(30).GetComponent<Text>().text = "Crit%: " + CritChance.ToString();
        C.GetChild(31).GetComponent<Text>().text = "Dmg: " + Dmg.ToString();
        C.GetChild(32).GetComponent<Text>().text = "Hit%: " + hitChance.ToString();
        C.GetChild(33).GetComponent<Text>().text = "Move: " + Movement.ToString();
    }

    private void OnMouseExit()
    {
        Transform C = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        C.GetChild(29).GetComponent<Text>().text = "HP: ";
        C.GetChild(30).GetComponent<Text>().text = "Crit%: ";
        C.GetChild(31).GetComponent<Text>().text = "Dmg: ";
        C.GetChild(32).GetComponent<Text>().text = "Hit%: ";
        C.GetChild(33).GetComponent<Text>().text = "Move: ";
    }

    /*
    [ClientRpc]
    public void RpcUpdateStats()
    {
        if (isLocalPlayer)
        {
            Transform C = GameObject.FindGameObjectWithTag("MainCanvas").transform;
            C.GetChild(24).GetComponent<UnityEngine.UI.Text>().text = "HP: " + HP.ToString();
            C.GetChild(25).GetComponent<UnityEngine.UI.Text>().text = "Crit%: " + CritChance.ToString();
            C.GetChild(26).GetComponent<UnityEngine.UI.Text>().text = "Dmg: " + Dmg.ToString();
            C.GetChild(27).GetComponent<UnityEngine.UI.Text>().text = "Hit%: " + hitChance.ToString();
            C.GetChild(28).GetComponent<UnityEngine.UI.Text>().text = "Move: " + Movement.ToString();
        }
    }
    */


    [Command]
    void CmdAddPlayer()
    {
        GameControl.singleton.Players.Add(this);
        PlayerIndex = GameControl.singleton.Players.Count - 1;
        GameControl.singleton.PlayerCount++;
        GameControl.singleton.PositionPlayer(PlayerIndex);
    }

    [Command]
    void CmdReadyPlayer(Vector2 pos)
    {
        isReady = true;
        TargetPosition = pos;
        GameControl.singleton.ReadyCount++;
    }


    // Use this for initialization
    void Start() {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (isLocalPlayer)
        {
            CmdAddPlayer();
            ShowSetup();
        }
    }

    void ShowSetup()
    {
        GameObject go=Instantiate(SetUpCanvas);
        CmdSyncPlayerColors();
        for (int i=0;i<12;i++)
        {
            go.transform.GetChild(i).GetComponent<ColorButton>().p = this;
            go.transform.GetChild(i).GetComponent<Button>().enabled = GameControl.singleton.PlayerColorAvailble(i);
        }
    }

    public void ShowNameCanvas()
    {
        GameObject go = Instantiate(NameCanvas);
        go.transform.GetChild(1).GetComponent<NameButton>().p = this;
    }

    bool CheckDistance(Vector2 v)
    {
        v = v - (Vector2)transform.position;
        return Movement * Movement >= v.sqrMagnitude;
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
            return;
        else
        {
            if(!isReady && !isFiring && Input.GetMouseButtonDown(0))
            {
                Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
                if ((Physics2D.Raycast(v, Vector2.zero).collider == null || !Physics2D.Raycast(v, Vector2.zero).collider.CompareTag("Player")) && CheckDistance(v))
                {
                    isReady = true;
                    CurrentMoveDiamond = Instantiate(MoveDiamond, v, Quaternion.identity);
                    CurrentMoveDiamond.GetComponent<SpriteRenderer>().color = GameControl.singleton.Colors[ColorID];
                    TargetPosition = v;
                    CmdReadyPlayer(v);
                }
            }
            else if (!isReady && isFiring && Input.GetMouseButtonDown(0))
            {
                isReady = true;
                Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
                TargetPosition = v;
                CmdReadyPlayer(v);
            }
        }
	}
}
