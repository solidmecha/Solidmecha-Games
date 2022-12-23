using UnityEngine;
using System.Collections;

public class RegionControl : MonoBehaviour {

    public int width;
    public int height;
    public int LiveNeighborChange;
    public int DeadNeighborChange;
    public int BaseProb;
    public bool UseValue;
    public int Xstart;
    public int Ystart;
    public int[] NeighborWeights;
    public Color One;
    public Color Two;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
