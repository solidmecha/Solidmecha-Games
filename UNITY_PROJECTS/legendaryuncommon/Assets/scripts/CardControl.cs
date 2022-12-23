using UnityEngine;
using System.Collections;

public class CardControl : MonoBehaviour {

    public int ID;
    public int Awakening;
    public int[] Stats;
    public string msg;
    public bool PlayerOwned;
    public bool RaidBoss;
    public int[] HP;
    public int[] WeaknessType;
    public DemonSpawner ds;

    private void OnMouseDown()
    {
        GameControl.singleton.CardClick(this);
    }

    private void OnMouseEnter()
    {
        if (GameControl.singleton.ShowPreviews)
        {
            GameControl.singleton.CardPreview(this);
        }
    }

    private void OnMouseExit()
    {
        if (GameControl.singleton.ShowPreviews)
        {
            GameControl.singleton.ShowSelectedCard();
            if(!PlayerOwned)
            {
                GameControl.singleton.PreviewMsg.text = "";
            }
        }
    }

    public void TakeDmg(int D)
    {
        HP[0] -= D;
        if(HP[0]<0)
        {
            if (PlayerOwned)
            {
                HP[0] = -1;
                transform.position = new Vector3(-1000, -1000, -1000);
            }
            else
            {
                if (!PlayerOwned && !RaidBoss)
                {
                    ds.hasSpawned = false;
                }
                else if (RaidBoss)
                {
                    GameControl.singleton.ResolveBossClear(ID);
                }
                Destroy(gameObject);
            }
        }
        else if(HP[0]>HP[1])
        {
            HP[0] = HP[1];
        }
        GameObject fill = transform.GetChild(0).GetChild(0).gameObject;
        fill.transform.localScale = new Vector2(15f * (float)HP[0] / (float)HP[1], 1);
        fill.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, (float)HP[0] / (float)HP[1]);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
