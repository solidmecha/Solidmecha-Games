using UnityEngine;
using System.Collections.Generic;

public class TownControl : MonoBehaviour {
    public static TownControl singleton;
    public List<GameObject> MiningNodes;
    public List<GameObject> Buyers;
    public System.Random RNG;

    private void Awake()
    {
        RNG = new System.Random();
        singleton = this;
    }

    public void RemoveNode(GameObject g)
    {
        MiningNodes.Remove(g);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
