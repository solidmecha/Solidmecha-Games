using UnityEngine;
using System.Collections;

public class TreeControl : MonoBehaviour {

    public float MaxScale;
    bool isGrowing=true;
    public float GrowthRate;
    bool ChoppedDown;
    Vector3 ChopVector;
    float counter;
    public float[] SeasonsGrowth;
    public Vector3 IdealLocation;
    int SunlightPerSecond=10;
    public int NeighborCount;
    public float Penalty;
    public bool[] Varieties;
    bool[] NeighborsChecked=new bool[2] {false, false};
    float NewRate=4;
    public int ID;
    public int SeasonIndex;

	// Use this for initialization
	void Start () {
	    
	}

    public void ChopDown(Vector3 v)
    {
        if (!ChoppedDown)
        {
            counter = 0;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerControl>().NeighborLoss(transform.position, ID);
            isGrowing = false;
            ChoppedDown = true;
            ChopVector = v;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if(isGrowing)
        {
            transform.localScale += new Vector3(GrowthRate, GrowthRate, GrowthRate)* SeasonsGrowth[SeasonIndex] * Time.deltaTime;
            if(transform.localScale.x>=MaxScale)
            {
                transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
                isGrowing = false;
            }
            else if(transform.localScale.x >= MaxScale*.33f && !NeighborsChecked[0])
            {
                NeighborsChecked[0] = true;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerControl>().NeighborCheck(this, NeighborCount);

            }
            else if (transform.localScale.x >= MaxScale * .67f && !NeighborsChecked[1])
            {
                NeighborsChecked[0] = true;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerControl>().NeighborCheck(this, NeighborCount);
            }
        }
        else if(ChoppedDown)
        {
            transform.Rotate(ChopVector * 60 * Time.deltaTime);
            counter += Time.deltaTime;
            if (counter >= 1.45f)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerControl>().Trees.Remove(this);
                Destroy(gameObject);
            }
        }
        else
        {
            counter += Time.deltaTime;
            if(counter>=1)
            {
                counter = 0;
                
            }
        }
        NewRate -= Time.deltaTime;
        if (NewRate <= 0)
        {
            NewRate = 4* (1/SeasonsGrowth[SeasonIndex]);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerControl>().SpawnTree(ID, transform.position, true);
        }
	}
}
