using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour {

    public string Description;
    public string AbilityText;
    public Action Ability;
    public int ActionCost;
    public float ActionCD;
    public int ID; //medic banker Ops Con Para Re Quar Prof
    public int CDCounter;
    public bool OnCD;
    public float SampleProgress;
    public int SampleIndex;
    public int SampleID;
    GameObject SampleBottle;

	// Use this for initialization
	void Start () {

        switch(ID)
        {
            case 0:
                Ability = Medic;
                break;
            case 1:
                Ability = Banker;
                break;
            case 2:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().UpdateActionText(4);
                Ability = Operations;
                break;
            case 3:
                Ability = Construction;
                break;
            case 4:
                Ability = Paratrooper;
                break;
            case 5:
                Ability = Researcher;
                break;
            case 6:
                Ability = Quarantine;
                break;
            case 7:
                Ability = Professor;
                break;
        }
	
	}

    void Medic()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if(gc.ActionCount>=ActionCost)
        {
            gc.UpdateActionText(-1 * ActionCost);
            for (int i = gc.ActivePlayerLocation.Germs.Count-1; i >=0 ; i--)
            {
                Destroy(gc.ActivePlayerLocation.Germs[i]);
                gc.ActivePlayerLocation.Germs.RemoveAt(i);
                gc.ActivePlayerLocation.ActiveGermIndex = 0;
            }
        }
    }

    void Banker()
    {
        //pass
    }

    void Operations()
    {
        //pass
    }

    void Construction()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (gc.HQcount < 3 && gc.ActivePlayerLocation.Germs.Count == 0 && !gc.ActivePlayerLocation.hasHQ)
        {
            if (gc.ActionCount >= ActionCost)
            {
                gc.UpdateActionText(-1 * ActionCost);
                gc.BuildHQ(gc.ActivePlayerLocation);
                gc.HQcount++;
            }
        }
    }

    void Paratrooper()
    {
        //pass
    }

    void Researcher()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (gc.ActionCount >= ActionCost && SampleIndex>0 && SampleIndex<4)
        {
            transform.GetChild(SampleIndex).GetComponent<SpriteRenderer>().color = gc.Colors[SampleID];
            SampleIndex++;
            gc.UpdateActionText(-1 * ActionCost);
        }
    }

    void Quarantine()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
       // ActionCost = gc.ActivePlayerLocation.ConnectedSiblings.Count;
        if (gc.ActionCount >= ActionCost)
        {
            gc.UpdateActionText(-1 * ActionCost);
            foreach(int i in gc.ActivePlayerLocation.ConnectedSiblings)
            {
                if(!gc.transform.GetChild(i).GetComponent<NodeScript>().hasHQ)
                {
                    GameObject go=Instantiate(gc.BioHazard, gc.transform.GetChild(i).position, Quaternion.identity) as GameObject;
                    go.GetComponent<QuaranMarker>().GC = gc;
                    go.GetComponent<QuaranMarker>().Active = gc.ActivePlayerLocation.transform.GetSiblingIndex();
                    go.GetComponent<QuaranMarker>().ProtectionZone = gc.transform.GetChild(i).GetComponent<NodeScript>();


                }
            }
        }
    }

    void Professor()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (gc.ActionCount >= ActionCost)
        {
                if (!gc.ActivePlayerLocation.hasHQ)
                {
                    gc.UpdateActionText(-1 * ActionCost);
                    GameObject go = Instantiate(gc.Book, gc.ActivePlayerLocation.transform.position+Vector3.back, Quaternion.identity) as GameObject;
                    go.GetComponent<Book>().GC = gc;
                    go.GetComponent<Book>().ProtectionZone = gc.ActivePlayerLocation;

                }
          
        }

    }

	// Update is called once per frame
	void Update () {
	    if(SampleProgress>=.25f && SampleIndex<4)
        {
            transform.GetChild(SampleIndex).GetComponent<SpriteRenderer>().color = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().Colors[SampleID];
            SampleIndex += 1;
            SampleProgress = 0;
        }
	}
}
