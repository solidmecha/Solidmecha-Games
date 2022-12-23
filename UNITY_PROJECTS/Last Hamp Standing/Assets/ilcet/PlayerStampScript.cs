using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStampScript : NetworkBehaviour {
    
    [SyncVar]
    bool isMoving;
    float counter;
    Vector3 TargetAngle;
    Vector3 TargetPos;
    Vector3 DeltaAngle;
    Vector3 Direction;
    float speed;
    public GameObject Stamp;
    public GameObject PlayerObj;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            System.Random rng = new System.Random();
            foreach (GameObject G in GameObject.FindGameObjectsWithTag("Player"))
            {
                G.transform.position = new Vector3(0, 0, -.5f);
                G.GetComponent<PlayerStampScript>().RpcUpdateTransform(G.transform.position, G.transform.rotation);
                for (int i = 0; i < 6; i++)
                {
                    int r, g, b;
                    do
                    {
                        r = rng.Next(2);
                        g = rng.Next(2);
                        b = rng.Next(2);
                        G.GetComponent<PlayerStampScript>().RpcSetSideColor(i, new Color(r, g, b));
                    } while (!(r != g || g != b || b != r));
                }
            }
        }
	}

    [Command]
    void CmdMove(Vector3 Dir, Vector3 Rot)
    {
        isMoving = true;
        Direction = Dir;
        DeltaAngle = Rot;
        TargetPos = transform.position + Dir;
        TargetAngle = FixAngles(transform.eulerAngles + Rot);
        RpcSetMove(Dir, Rot);
    }

    [ClientRpc]
    void RpcSetMove(Vector3 Dir, Vector3 Rot)
    {
        isMoving = true;
        Direction = Dir;
        DeltaAngle = Rot;
        TargetPos = new Vector3(Mathf.Round(transform.position.x + Dir.x), Mathf.Round(transform.position.y+Dir.y), -.5f);
        TargetAngle = FixAngles(transform.eulerAngles + Rot);
    }

    [Command]
    void CmdColorSwap(Vector2 Dir)
    {

    }

    private Vector3 FixAngles(Vector3 V)
    {
        float x = 0, y = 0, z = 0;
        while (V.x > 360)
            V.x -= 360;
        while (V.x < 0)
            V.x += 360;
        while (V.y > 360)
            V.y -= 360;
        while (V.y < 0)
            V.y += 360;
        while (V.z > 360)
            V.z -= 360;
        while (V.z < 0)
            V.z += 360;

        if (V.x < 45 && V.x >= 315)
            x = 0;
        else if (V.x >= 45 && V.x < 135)
            x = 90;
        else if (V.x >= 135 && V.x < 225)
            x = 180;
        else if (V.x >= 225 && V.x < 315)
            x = 270;

        if (V.y < 45 && V.y >= 315)
            y = 0;
        else if (V.y >= 45 && V.y < 135)
            y = 90;
        else if (V.y >= 135 && V.y < 225)
            y = 180;
        else if (V.y >= 225 && V.y < 315)
            y = 270;

        if (V.z < 45 && V.z >= 315)
            z = 0;
        else if (V.z >= 45 && V.z < 135)
            z = 90;
        else if (V.z >= 135 && V.z < 225)
            z = 180;
        else if (V.z > 225 && V.z < 315)
            z = 270;

        return new Vector3(x, y, z);
    }

    [ClientRpc]
    void RpcUpdateTransform(Vector3 p, Quaternion q)
    {
        transform.SetPositionAndRotation(p, q);
    }

    [ClientRpc]
    void RpcCleanRotation()
    {
        Transform[] T = new Transform[6] { transform.GetChild(0), transform.GetChild(1), transform.GetChild(2), transform.GetChild(3), transform.GetChild(4), transform.GetChild(5) };
        transform.DetachChildren();
        transform.eulerAngles = Vector3.zero;
        foreach (Transform t in T)
            t.SetParent(transform);
    }

    [ClientRpc]
    void RpcSetSideColor(int i, Color C)
    {
        transform.GetChild(i).GetComponent<SpriteRenderer>().color = C;
    }

    [Command]
    void CmdSpawn(Vector2 Position, Color color)
    {
        Stamp.GetComponent<SpriteRenderer>().color = color;
        GameObject Go=Instantiate(Stamp, Position, Quaternion.identity);
        NetworkServer.Spawn(Go);
        Go.GetComponent<ColorSync>().RpcSetColor(color);
    }

    // Update is called once per frame
    void Update () {
	 if(isLocalPlayer && !isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                isMoving = true;
                CmdMove(Vector3.up, new Vector3(90, 0, 0));
                return;
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                isMoving = true;
                CmdMove(Vector3.left, new Vector3(0, 90, 0));
                return;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                isMoving = true;
                CmdMove(Vector3.down, new Vector3(-90, 0, 0));
                return;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                isMoving = true;
                CmdMove(Vector3.right, new Vector3(0, -90, 0));
                return;
            }

        }
     else if(isMoving)
        {
            transform.Rotate(DeltaAngle *3* Time.deltaTime, Space.World);
            transform.position+=Direction *3* Time.deltaTime;
            counter += Time.deltaTime;
           // RpcUpdateTransform(transform.position, transform.rotation);
            if(counter>=.33f)
            {
                transform.eulerAngles = FixAngles(transform.eulerAngles);
                transform.position = TargetPos;
               // RpcUpdateTransform(transform.position, transform.rotation);
                counter = 0;   
                /*Transform[] T = new Transform[6] { transform.GetChild(0), transform.GetChild(1), transform.GetChild(2), transform.GetChild(3), transform.GetChild(4), transform.GetChild(5) };
                transform.DetachChildren();
                transform.eulerAngles = Vector3.zero;
                foreach (Transform t in T)
                    t.SetParent(transform); */
                isMoving = false;
                //RpcCleanRotation();
                if (Physics2D.Raycast((Vector2)transform.position, Vector2.zero).collider != null)
                    Destroy(Physics2D.Raycast((Vector2)transform.position, Vector2.zero).collider.gameObject);
                //GameObject go=Instantiate(Stamp, (Vector2)transform.position, Quaternion.identity);
                RaycastHit hit;
                Physics.Raycast(transform.position, Vector3.forward, out hit);
                //go.GetComponent<SpriteRenderer>().color = ;
                if(isLocalPlayer)
                    CmdSpawn(transform.position, hit.collider.GetComponent<SpriteRenderer>().color);
            }
        }
     if(isServer)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                NetworkManager.singleton.playerPrefab = PlayerObj;
                NetworkManager.singleton.ServerChangeScene("Main");
            }
            else if(Input.GetKeyDown(KeyCode.F7))
                NetworkManager.singleton.ServerChangeScene("ilcet");
        }
	}
}
