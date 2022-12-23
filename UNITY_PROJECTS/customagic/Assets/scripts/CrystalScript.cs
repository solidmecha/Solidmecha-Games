using UnityEngine;
using System.Collections.Generic;

public class CrystalScript : MonoBehaviour {

    bool captured;
    bool isCapturing;
    float CaptureRate = .25f;
    public Color TargetC;
    public List<WeaponScript> Attackers;
    public List<int> AttackerTargets;
    bool attacked;
    public bool hostile;
    public GameObject MinimapIcon;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hostile && collision.collider.CompareTag("Player") && !captured)
        {
            isCapturing = true;
            MinimapIcon.transform.localPosition = new Vector2(1.4f * transform.position.x / 17.14f, transform.position.y * .87f / 9.87f);
            if (MinimapIcon.transform.localPosition.x > 4.16f)
                MinimapIcon.transform.localPosition = new Vector2(4.16f, MinimapIcon.transform.localPosition.y);
            MinimapIcon.GetComponent<SpriteRenderer>().enabled = true;
            MinimapIcon.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(captured && collision.CompareTag("W"))
        {
            WeaponScript ws = collision.GetComponent<WeaponScript>();
            if (ws.hostile != hostile && ws.WeapIndex == -1)
            {
                ws.WeapIndex = Attackers.Count;
                int TargetIndex = FindNextTargetIndex();
                Attackers.Add(ws);
                if (TargetIndex < 5)
                    ws.transform.position = (Vector2)transform.GetChild(TargetIndex).position + Vector2.left;
                else
                    ws.transform.position = (Vector2)transform.GetChild(TargetIndex).position + Vector2.right;
                ws.Engage(transform.GetChild(TargetIndex).position);
                AttackerTargets.Add(TargetIndex);
                ws.interactableObj = gameObject;
                if(!hostile && !attacked)
                {
                    MinimapIcon.GetComponent<HighlightScript>().BeginHilight(MinimapIcon.GetComponent<SpriteRenderer>());
                }
                attacked = true;
            }
        }
        else if(hostile && collision.CompareTag("Proj"))
        {
            transform.GetChild(0).GetChild(0).localScale = new Vector2(transform.GetChild(0).GetChild(0).localScale.x - collision.GetComponent<ProjScript>().Dmg*.01f, 1);
            CheckStatus();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            isCapturing = false;
    }

    int FindNextTargetIndex()
    {
        List<int> Indicies=new List<int> {1,2,3,4,5,6,7,8};
        foreach(WeaponScript w in Attackers)
        {
            Indicies.Remove(AttackerTargets[w.WeapIndex]);
        }
        return Indicies[0];
    }
    // Use this for initialization
    void Start () {
	if(hostile)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetChild(0).GetChild(0).localScale = Vector2.one;
            captured = true;
        }
	}

    public void RemoveAttacker(int index)
    {
        Attackers.RemoveAt(index);
        AttackerTargets.RemoveAt(index);
        for (int i = index; i < Attackers.Count; i++)
        {
            Attackers[i].WeapIndex--;
        }
        if(Attackers.Count==0)
        {
            attacked = false;
            MinimapIcon.GetComponent<HighlightScript>().StopHiLight();
        }
    }

    void CheckStatus()
    {
        if (transform.GetChild(0).GetChild(0).localScale.x <= 0)
        {
            transform.GetChild(0).GetChild(0).localScale = new Vector2(0, 1);
            if(!hostile)
                MinimapIcon.transform.GetChild(0).GetChild(0).localScale = new Vector2(0, 1);
            if (hostile)
            {
                foreach (WeaponScript w in Attackers)
                {
                    w.SetHolding();
                }
                GameStateManager.singleton.UpdateOtherCrystals();
                WorldControl.singleton.ClearCrystal(transform.position);
                Destroy(gameObject);
            }
            else
            {
                CaptureRate *= .5f;
                GameStateManager.singleton.UpdatePlayerCrystals(-1);
                captured = false;
                attacked = false;
                foreach (WeaponScript w in Attackers)
                {
                    w.WeapIndex=-1;
                    Destroy(w.gameObject);
                }
                MinimapIcon.GetComponent<HighlightScript>().StopHiLight();
                Attackers.Clear();
                AttackerTargets.Clear();
                
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	if(isCapturing)
        {
            transform.GetChild(0).GetChild(0).localScale = new Vector2(transform.GetChild(0).GetChild(0).localScale.x+CaptureRate*Time.deltaTime, 1);
            MinimapIcon.transform.GetChild(0).GetChild(0).localScale= new Vector2(transform.GetChild(0).GetChild(0).localScale.x + CaptureRate * Time.deltaTime, 1);
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, TargetC, transform.GetChild(0).GetChild(0).localScale.x);
            if(transform.GetChild(0).GetChild(0).localScale.x>=1)
            {
                transform.GetChild(0).GetChild(0).localScale = new Vector2(1,1);
                MinimapIcon.transform.GetChild(0).GetChild(0).localScale = new Vector2(1, 1);
                captured = true;
                GameStateManager.singleton.UpdatePlayerCrystals(1);
                isCapturing = false;
            }
        }
    else if(attacked)
        {
            Vector2 v= new Vector2(transform.GetChild(0).GetChild(0).localScale.x - Attackers.Count * CaptureRate * CaptureRate * CaptureRate * Time.deltaTime, 1);
            transform.GetChild(0).GetChild(0).localScale = v;
            if (!hostile)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, TargetC, transform.GetChild(0).GetChild(0).localScale.x);
                MinimapIcon.transform.GetChild(0).GetChild(0).localScale = v;
            }
            CheckStatus();
        }
	}
}
