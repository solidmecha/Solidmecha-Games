using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerControl : MonoBehaviour {

    public GameObject[] Seeds;
    int SeedIndex;
    System.Random RNG = new System.Random();
    public int MaxTrees;
    public List<TreeControl> Trees;
    int Sunbeams;
    public float[] MaxNeighborCount;
    public float[] MinNeighborDistance;
    public float[] MaxNeighborDistance;
    Renderer Grass;
    Renderer Leaves;
    public Color[] Grounds;
    public Color[] LeafColors;
    int SeasonCounter=0;
    float counter=0;
    bool isChanging;
    public bool NewForest;

    // Use this for initialization
    void Start () {
        for(int i=0;i<4; i++)
        {
            MaxNeighborCount[i] = RNG.Next(2, 6);
            float r = RNG.Next(100, 200) / 100f;
            MinNeighborDistance[i] = r*r;
            Seeds[i].GetComponent<TreeControl>().SeasonsGrowth[i] = GrowFactor(RNG.Next(3));
        }
        SeasonCounter = RNG.Next(4);
        //ChangeSeasons();
        Grass = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>();
        Leaves = Seeds[0].transform.GetChild(1).GetComponent<Renderer>();
        Grass.sharedMaterial.color = Grounds[SeasonCounter];
        Leaves.sharedMaterial.color = LeafColors[SeasonCounter];
    }


    float GrowFactor(int i)
    {
        switch(i)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return .5f;
            default: return 0;
        }
    }

    Vector3 ChopVec(int i)
    {
        switch(i)
        {
            case 0: return Vector3.forward;
            case 1: return Vector3.back;
            case 2: return Vector3.left;
            case 3: return Vector3.right;
            default: return Vector3.zero;
        }
    }

    Vector3[] Offsets(int i)
    {
        switch(i)
        {
            case 0: return new Vector3[] {new Vector3(1,0,1), new Vector3(-1,0,1) , new Vector3(-1,0,-1) , new Vector3(1,0,-1) };
            case 1: return new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            default: return new Vector3[4];
        }
    }

    void CalcPenalties(Vector3 Pos, int ID)
    {
        for(int i=0; i<Trees.Count; i++)
        {
            if (NewForest && (Trees[i].transform.position - Pos).sqrMagnitude <= MinNeighborDistance[Trees[i].ID])
                Trees[i].ChopDown(ChopVec(RNG.Next(4)));
             if((Trees[i].transform.position - Pos).sqrMagnitude <= MaxNeighborDistance[Trees[i].ID])
            {
                Trees[i].NeighborCount++;
                if (Trees[i].NeighborCount > MaxNeighborCount[Trees[i].ID])
                    Trees[i].ChopDown(ChopVec(RNG.Next(4)));
            }
        }
    }

    public void NeighborLoss(Vector3 Pos, int ID)
    {
        for (int i = 0; i < Trees.Count; i++)
        {
            if ((Trees[i].transform.position - Pos).sqrMagnitude <= MaxNeighborDistance[Trees[i].ID])
            {
                Trees[i].NeighborCount--;
            }
        }
    }

    void SpawnFour()
    {
        Vector3[] Pos = Offsets(RNG.Next(2));
        int R = RNG.Next(1,3);
        for(int i=0;i<4; i++)
        {
            SpawnTree(i, transform.position+Pos[i]*2.5f*R);
        }
    }

    public void NeighborCheck(TreeControl tc, int count)
    {
        /*
        if(count>MaxNeighborCount[tc.ID])
        {
            tc.ChopDown(ChopVec(RNG.Next(4)));
        } */
        if (!NewForest)
        {
            for (int i = 0; i < Trees.Count; i++)
            {
                float dist = (Trees[i].transform.position - tc.transform.position).sqrMagnitude;
                if (dist <= MinNeighborDistance[tc.ID] && dist != 0)
                    tc.ChopDown(ChopVec(RNG.Next(4)));
            }
        }
    }


        public void SpawnTree(int i, Vector3 Pos)
    {
            GameObject go=Instantiate(Seeds[i], Pos, Quaternion.identity) as GameObject;
            CalcPenalties(Pos, i);
        TreeControl t = go.GetComponent<TreeControl>();
        Trees.Add(t);
        t.SeasonIndex = SeasonCounter;
    }

    public void SpawnTree(int i, Vector3 Pos, bool Rand)
    {
       
        Vector3 Offset= new Vector3(RNG.Next(-300, 301)/100f, 0, RNG.Next(-300, 301) / 100f);
        while(Offset.sqrMagnitude <= MinNeighborDistance[i])
            Offset= new Vector3(RNG.Next(-300, 301) / 100f, 0, RNG.Next(-300, 301) / 100f);
        Pos += Offset;
        if (!(Pos.x < -5 || Pos.x > 5 || Pos.z < -5 || Pos.z > 5))
        {
            if (Trees.Count < MaxTrees)
            {
                GameObject go = Instantiate(Seeds[i], Pos, Quaternion.identity) as GameObject;
                CalcPenalties(Pos, i);
                TreeControl t = go.GetComponent<TreeControl>();
                Trees.Add(t);
                t.SeasonIndex = SeasonCounter;
            }
        }
    }

    void ChangeSeasons()
    {
        foreach (TreeControl t in Trees)
            t.SeasonIndex = SeasonCounter;
    }

    // Update is called once per frame
    void Update () {
	 if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if(hit.collider != null && hit.collider.CompareTag("Player"))
            {
                SpawnTree(SeedIndex, hit.point);
                SeedIndex++;
                if (SeedIndex == 4)
                    SeedIndex = 0;
            }
        }
     else if(Input.GetMouseButtonDown(1))
        {

            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if (hit.collider != null && hit.collider.transform.root.CompareTag("Finish"))
            {
                hit.collider.transform.root.GetComponent<TreeControl>().ChopDown(ChopVec(RNG.Next(4)));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SeedIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SeedIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SeedIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SeedIndex = 3;
        if(Input.GetKeyDown(KeyCode.N))
        {
            NewForest = !NewForest;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnFour();

        if(isChanging)
        {
            counter += Time.deltaTime;
            Grass.sharedMaterial.color = Color.Lerp(Grounds[SeasonCounter], Grounds[(SeasonCounter + 1) % 4], counter);
            Leaves.sharedMaterial.color = Color.Lerp(LeafColors[SeasonCounter], LeafColors[(SeasonCounter + 1) % 4], counter);
            if (counter >= 1)
            {
                SeasonCounter = (SeasonCounter + 1) % 4;
                counter = 0;
                isChanging = false;
                ChangeSeasons();
            }
        }
        else
        {
            counter += Time.deltaTime;
            if(counter>=11f)
            {
                counter = 0;
                isChanging = true;
            }
        }

    }
}
