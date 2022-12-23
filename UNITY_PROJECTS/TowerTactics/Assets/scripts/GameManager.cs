using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int height;
    public int width;
    public GameObject tile;
    public GameObject minion;
    public GameObject Castle;
    public GameObject Tower;
    public List<GameObject> CastleList= new List<GameObject> { };
    public System.Random RNG;
    public GameObject MovePip;
    public GameObject CurrentPip;
    public GameObject Target;
    public List<MinionScript> MSlist=new List<MinionScript> { };
    public List<PlayerControl> PClist = new List<PlayerControl> { };
    public List<GameObject> TargetDisplay = new List<GameObject> { };
    public GameObject SelectedTower;
    public GameObject Select;
    public GameObject CurrSelect;
    public bool targetMode;
    public bool towerMode;
    public List<Sprite> tierList = new List<Sprite> { };
    public int turnCount;
    public List<GameObject> pips = new List<GameObject> { };
    public GameObject SelectedCastle;

    public void SwitchOffMode()
    {
        if (CurrSelect != null)
        {
            Destroy(CurrSelect);
            SelectedTower = null;
        }
        if(TargetDisplay.Count>0)
        {
            foreach (GameObject g in TargetDisplay)
                Destroy(g);
            TargetDisplay.Clear();
        }
        if(SelectedCastle != null)
        {
            SelectedCastle.GetComponent<SpriteRenderer>().color = Color.white;
            SelectedCastle = null;
        }
        targetMode = false;
        towerMode = false;
    }


	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        MakeBoard();
	}

    void MakeBoard()
    {
        CastleList.Add(Instantiate(Castle, (Vector2)transform.position + new Vector2(width / 2f - .5f, height / 2f - .5f), Quaternion.identity) as GameObject);
        for (int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Instantiate(tile, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity);
                if (i == 0 || i == width - 1)
                {
                   GameObject go=Instantiate(minion, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                   MinionScript ms= go.GetComponent<MinionScript>();
                    ms.target = CastleList[0];
                    ms.GM = this;
                    MSlist.Add(ms);
                }
                if (j == 0 || j == height - 1)
                {
                   GameObject go= Instantiate(minion, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                    MinionScript ms = go.GetComponent<MinionScript>();
                    ms.target = CastleList[0];
                    ms.GM = this;
                    MSlist.Add(ms);
                }
            }
        }
    }
	

    public void NextTurn()
    {
        turnCount++;
        SwitchOffMode();
        foreach (MinionScript m in MSlist)
        {
            m.Move();
            foreach (PlayerControl p in PClist)
                if (m.transform.position.x==p.transform.position.x && m.transform.position.y==p.transform.position.y)
                {
                    p.TargetVecs.Clear();
                }
        }
        foreach (PlayerControl p in PClist)
            p.Fire();
    }

	// Update is called once per frame
	void Update () {
    if(Input.GetKeyDown(KeyCode.Space))
        {
            if (turnCount < 5)
            {
               foreach(MinionScript m in MSlist)
                {
                    GameObject go = Instantiate(MovePip, (Vector2)m.transform.position + m.FindNextMove(), Quaternion.identity) as GameObject;
                    pips.Add(go);
                }
            }
        }
    else if(Input.GetKeyUp(KeyCode.Space))
        {
            foreach (GameObject g in pips)
                Destroy(g);
            pips.Clear();
        }
	if(Input.GetMouseButtonDown(0))
        {
            if(targetMode)
            {
                Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v -= (Vector2)transform.position;
                v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
                SelectedTower.GetComponent<PlayerControl>().TryTarget(v);
            }
            else if(towerMode)
            {
                Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v -= (Vector2)transform.position;
                v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
                SelectedCastle.GetComponent<TowerSpawn>().TryBuild(v);
            }
        }
	}
}
