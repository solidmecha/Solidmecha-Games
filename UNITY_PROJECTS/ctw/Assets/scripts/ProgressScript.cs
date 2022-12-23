using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressScript : MonoBehaviour {

    public SpriteRenderer[] Rends;


	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { FindCure(); });
	}

    void FindCure()
    {
        GameControl GC=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        int i=GC.Players[GC.ActivePlayerLocation.PlayerIndex].GetComponent<PlayerScript>().SampleID;
        if (!GC.Cures[i])
        {
            GC.HealRates[i] = -.5f;
            GC.Cures[i] = true;
            Rends[i].color = GC.Colors[i];
            GC.Players[GC.ActivePlayerLocation.PlayerIndex].GetComponent<PlayerScript>().SampleIndex = 0;
            GC.Players[GC.ActivePlayerLocation.PlayerIndex].GetComponent<PlayerScript>().SampleProgress = 0;
            for (int t = 0; t < 4; t++)
                GC.Players[GC.ActivePlayerLocation.PlayerIndex].transform.GetChild(t).GetComponent<SpriteRenderer>().color = Color.clear;
            foreach(NodeScript n in GC.GetComponentsInChildren<NodeScript>())
            {
                if (n.PlayerIndex != -1 && n.InfectIDIndex[n.ActiveGermIndex] == i)
                    n.InfectRate = -.5f;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
