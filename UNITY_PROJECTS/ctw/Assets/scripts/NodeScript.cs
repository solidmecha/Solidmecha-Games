using UnityEngine;
using System.Collections.Generic;

public class NodeScript : MonoBehaviour {

    public List<int> PossibleSiblingIndex;
    public List<int> ConnectedSiblings;
    public int PlayerIndex;
    public bool hasHQ;
    public float InfectionLevel;
    public List<GameObject> Germs;
    public int ActiveGermIndex;
    public float InfectRate;
    public int[] InfectIDIndex;
    public bool Immune;

    private void OnMouseDown()
    {
        GameControl GC = GetComponentInParent<GameControl>();
        NodeScript n = GC.ActivePlayerLocation;
        //moving
        if (PlayerIndex==-1 && n !=null && (ConnectedSiblings.Contains(n.transform.GetSiblingIndex()) || GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().ID==4
            || (n.hasHQ && hasHQ) ) && (ActiveGermIndex != 4 || CureCheck(GC)))

        {
            
            if(GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().ID != 4 && GetComponentInParent<GameControl>().ActionCount>0)
            {
                if (ActiveGermIndex == 4)
                    ActiveGermIndex--;
                if(GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().ID == 1)
                {
                    if(n.hasHQ)
                    {
                        GC.ActionCD += 1f;
                    }
                    if(hasHQ)
                    {
                        GC.ActionCD -= 1f;
                    }
                }
                GC.UpdateActionText(-1);
                GC.Players[n.PlayerIndex].transform.position = transform.position;
                if(Germs.Count>0 && GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleID != InfectIDIndex[ActiveGermIndex])
                {
                    GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleIndex = 0;
                    GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleProgress = 0;
                    for(int i=0;i<4;i++)
                        GC.Players[n.PlayerIndex].transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.clear;
                    GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleID = InfectIDIndex[ActiveGermIndex];
                }
                PlayerIndex = n.PlayerIndex;
                n.PlayerIndex = -1;
                n.InfectRate=GC.InfectRate;
                GC.Outline.position = transform.position;
                GC.ActivePlayerLocation = this;
                InfectRate = GC.HealRates[InfectIDIndex[ActiveGermIndex]];
                if (hasHQ && GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex==4)
                {
                    GC.ProgressButton.interactable= true;
                }
                else
                    GC.ProgressButton.interactable = false;
                return;
            }
            else if(GetComponentInParent<GameControl>().ActionCount > 1)
            {
                if (ActiveGermIndex == 4)
                    ActiveGermIndex--;
                GC.UpdateActionText(-2);
                GC.Players[n.PlayerIndex].transform.position = transform.position;
                if (Germs.Count > 0 && GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleID != InfectIDIndex[ActiveGermIndex])
                {
                    GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleIndex = 0;
                    GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleProgress = 0;
                    for (int i = 0; i < 4; i++)
                        GC.Players[n.PlayerIndex].transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.clear;
                    GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleID = InfectIDIndex[ActiveGermIndex];
                }
                PlayerIndex = n.PlayerIndex;
                n.PlayerIndex = -1;
                n.InfectRate = GC.InfectRate;
                GC.Outline.position = transform.position;
                GC.ActivePlayerLocation = this;
                InfectRate = GC.HealRates[InfectIDIndex[ActiveGermIndex]];
                if (hasHQ && GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex == 4)
                {
                    GC.ProgressButton.interactable = true;
                }
                else
                    GC.ProgressButton.interactable = false;
                return;
            }
        }
        //selecting

        else if (PlayerIndex != -1)
        {
            //giving
            if (n != null && ConnectedSiblings.Contains(n.transform.GetSiblingIndex()) && GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleIndex > 0 &&
                GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex < 4  &&
                (GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleID==GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleID ||
                (GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex==0 && Germs.Count==0)))
            {
                if (GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex == 0)
                {
                    GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleID = GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleID;
                    GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleProgress = 0;
                        }
                GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex += GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleIndex;
                if (GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex > 4)
                    GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex = 4;
                GC.Players[n.PlayerIndex].GetComponent<PlayerScript>().SampleIndex = 0;
                for (int i = 0; i < 4; i++)
                {
                    GC.Players[n.PlayerIndex].transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.clear;
                    if (GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex > i)
                        GC.Players[PlayerIndex].transform.GetChild(i).GetComponent<SpriteRenderer>().color = GC.Colors[GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleID];
                }
            }
            GC.SetActivePlayer(this, PlayerIndex);
            if (hasHQ && GC.Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex == 4)
            {
                GC.ProgressButton.interactable = true;
            }
            else
                GC.ProgressButton.interactable = false;
        }



    }

    // Use this for initialization
    void Start () {
	
	}

    bool CureCheck(GameControl GC)
    {
        for (int i = 0; i < 4; i++)
        {
            if(!GC.Cures[InfectIDIndex[i]])
                return false;
        }
        return true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Germs.Count > 0 && ActiveGermIndex<4)
        {
                Germs[ActiveGermIndex].transform.localScale = (Vector2)Germs[ActiveGermIndex].transform.localScale + new Vector2(InfectRate, InfectRate) * Time.deltaTime;

            if (PlayerIndex != -1 && InfectRate>-.1f)
                GetComponentInParent<GameControl>().Players[PlayerIndex].GetComponent<PlayerScript>().SampleProgress += InfectRate * -1 * Time.deltaTime;
            if (Germs[ActiveGermIndex].transform.localScale.x >= .5f)
                {
                    Germs[ActiveGermIndex].transform.localScale = new Vector2(.5f, .5f);
                    ActiveGermIndex++;
                    if (ActiveGermIndex < 4 && ActiveGermIndex == Germs.Count)
                    {
                        GetComponentInParent<GameControl>().Infect(this, InfectIDIndex[ActiveGermIndex-1], Germs[ActiveGermIndex - 1].transform.position);
                    }
                    for(int i=0;i< GetComponentInParent<GameControl>().SpreadCount;i++)
                    {
                        GetComponentInParent<GameControl>().Infect(transform.parent.GetChild(ConnectedSiblings[i]).GetComponent<NodeScript>(), InfectIDIndex[ActiveGermIndex-1], Germs[ActiveGermIndex - 1].transform.position);
                    }
                }
            else if(Germs[ActiveGermIndex].transform.localScale.x <= 0f)
                {
                    Destroy(Germs[ActiveGermIndex]);
                    Germs.RemoveAt(ActiveGermIndex);
                if (ActiveGermIndex > 0)
                {
                    ActiveGermIndex--;
                    if(GetComponentInParent<GameControl>().Players[PlayerIndex].GetComponent<PlayerScript>().SampleID != InfectIDIndex[ActiveGermIndex])
                    {
                        InfectRate = GetComponentInParent<GameControl>().HealRates[InfectIDIndex[ActiveGermIndex]];
                        GetComponentInParent<GameControl>().Players[PlayerIndex].GetComponent<PlayerScript>().SampleIndex = 0;
                        GetComponentInParent<GameControl>().Players[PlayerIndex].GetComponent<PlayerScript>().SampleProgress = 0;
                        for (int i = 0; i < 4; i++)
                            GetComponentInParent<GameControl>().Players[PlayerIndex].transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.clear;
                        GetComponentInParent<GameControl>().Players[PlayerIndex].GetComponent<PlayerScript>().SampleID = InfectIDIndex[ActiveGermIndex];
                    }
                }
                }
            }
            else if(ActiveGermIndex==4)
        {
            InfectionLevel += Time.deltaTime;
            if(InfectionLevel>=5)
            {
                InfectionLevel = 0;
                for (int i = 0; i < ConnectedSiblings.Count; i++)
                {
                    GetComponentInParent<GameControl>().Infect(transform.parent.GetChild(ConnectedSiblings[i]).GetComponent<NodeScript>(), InfectIDIndex[ActiveGermIndex - 1], Germs[ActiveGermIndex - 1].transform.position);
                }
            }
        }
        }
    }
