using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public int levelID;
    public Levels LS;
    public GameObject player;
    public List<GameObject> WorldObjects; // 0 blank,1 red,2 green,3 blue blocks, 4 walls
    public List<GameObject> Mods; //0 push, 1 vac, 2 hook, 3 bolt,4 CCW rotation, 5 CW rotation, 6 red,7 green,8 blue pegs
    public List<PegScript> Pegs=new List<PegScript> { };
    public string[] CurLevel;
    public List<Vector4> CurMods = new List<Vector4> { };
    public List<GameObject> CurObjects = new List<GameObject> { };
    public Button ResetButton;
    public Button NewButton;
    public GameObject WibWobMachine;
    public GameObject Victory;

    public void CheckWin()
    {
        foreach(PegScript p in Pegs)
        {
            if (!p.covered)
                return;
        }
        GameObject go = Instantiate(Victory) as GameObject;
        CurObjects.Add(go);
    }

	// Use this for initialization
	void Start () {
        //string[] Level = LS.LevelLookUp(levelID);
        ResetButton.onClick.AddListener(delegate { Reset(); });
        NewButton.onClick.AddListener(delegate { NewLevel(); });
        LS.GenerateRandomLevel();
	}
void NewLevel()
    {
        CurMods.Clear();
        foreach (GameObject g in CurObjects)
        {
            if (g != null)
                Destroy(g);
        }
        player.transform.position = new Vector2(10, 10);
        LS.GenerateRandomLevel();
    }
void Reset()
{
    foreach(GameObject g in CurObjects)
        {
            if (g != null)
                Destroy(g);
        }
    player.transform.position = new Vector2(10, 10);
    CreateLevel();
}

public void CreateLevel()
{
   Pegs.Clear();
   GameObject wwm = (GameObject)Instantiate(WibWobMachine, player.transform.position, Quaternion.identity) as GameObject;
   wwm.transform.SetParent(player.transform);
   CurObjects.Add(wwm);
    for (int i = 0; i < 7; i++)
    {

        for (int j = 0; j < 7; j++)
        {
                //print(CurLevel[i][j]);
            int woID = int.Parse(CurLevel[j][i].ToString());
            if (woID < 6 && woID != 0)
            {
                GameObject go = Instantiate(WorldObjects[woID], (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                CurObjects.Add(go);
            }
            else if (woID == 6)
                player.transform.position = (Vector2)transform.position + new Vector2(i, j);

        }
    }
    foreach (Vector4 v in CurMods)
    {
        GameObject go = Instantiate(Mods[(int)v.w], (Vector2)transform.position + new Vector2(v.x, v.y), Quaternion.Euler(0, 0, v.z)) as GameObject;
        CurObjects.Add(go);
        if (v.w < 4)
        {
            switch ((int)v.z)
            {
                case 0:
                    go.transform.position = (Vector2)go.transform.position + new Vector2(0, .25f);
                    break;
                case 90:
                    go.transform.position = (Vector2)go.transform.position + new Vector2(-.25f, 0);
                    break;
                case 180:
                    go.transform.position = (Vector2)go.transform.position + new Vector2(0, -.25f);
                    break;
                case 270:
                    go.transform.position = (Vector2)go.transform.position + new Vector2(.25f, 0);
                    break;
            }
        }
        else if ((int)v.w == 6 || (int)v.w == 7 || (int)v.w == 8)
        {
            PegScript ps = (PegScript)go.GetComponent(typeof(PegScript));
            Pegs.Add(ps);
        }
        else if ((int)v.w == 4)
            go.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        PlayerControl pc = (PlayerControl)player.GetComponent(typeof(PlayerControl));
        pc.pos = pc.FindPos(player.transform.position - transform.position);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
