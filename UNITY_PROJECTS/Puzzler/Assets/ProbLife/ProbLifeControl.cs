using UnityEngine;
using System.Collections.Generic;

public class ProbLifeControl : MonoBehaviour {

    public GameObject cell;
    ProbCell[][] probCells;
    System.Random RNG = new System.Random();
    float LastUpdate;
    public float delta;
    public int width;
    public int height;
    public int LiveNeighborChange;
    public int DeadNeighborChange;
    public int BaseProb;
    public bool UseValue;
    public bool Torus;
    public int Xstart;
    public int Ystart;

    [Tooltip("0:R, 1:UR, 2:DR, 3:L, 4:UL, 5:DL, 6:U, 7:D")]
    public int[] NeighborWeights;

    // Use this for initialization
    void Start()
    {
        probCells = new ProbCell[width][];
        for (int i = 0; i < width; i++)
        {
            probCells[i] = new ProbCell[height];
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(cell, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                probCells[i][j] = go.GetComponent<ProbCell>();
                    probCells[i][j].SetLife();
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                probCells[i][j].Neighbors = new ProbCell[8];
                if (i < width-1)
                {
                    probCells[i][j].Neighbors[0] = probCells[i + 1][j];
                    if (j < height-1)
                        probCells[i][j].Neighbors[1] = probCells[i + 1][j + 1];
                    if (j > 0)
                        probCells[i][j].Neighbors[2] = probCells[i + 1][j - 1];
                }
                else if(Torus)
                {
                    probCells[i][j].Neighbors[0] = probCells[0][j];
                    if (j < height - 1)
                        probCells[i][j].Neighbors[1] = probCells[0][j + 1];
                    else
                        probCells[i][j].Neighbors[1] = probCells[0][0];
                    if (j > 0)
                        probCells[i][j].Neighbors[2] = probCells[0][j - 1];
                    else
                        probCells[i][j].Neighbors[2] = probCells[0][height-1];
                }
                if (i > 0)
                {
                        probCells[i][j].Neighbors[3] = probCells[i - 1][j];
                    if (j < height - 1)
                        probCells[i][j].Neighbors[4] = probCells[i - 1][j + 1];
                    if (j > 0)
                        probCells[i][j].Neighbors[5] = probCells[i - 1][j - 1];
                }
                else if (Torus)
                {
                    probCells[i][j].Neighbors[3] = probCells[width-1][j];
                    if (j < height - 1)
                        probCells[i][j].Neighbors[4] = probCells[width - 1][j + 1];
                    else
                        probCells[i][j].Neighbors[4] = probCells[width - 1][0];
                    if (j > 0)
                        probCells[i][j].Neighbors[5] = probCells[width - 1][j - 1];
                    else
                        probCells[i][j].Neighbors[5] = probCells[width - 1][height - 1];
                }
                if (j< height - 1)
                    probCells[i][j].Neighbors[6] = probCells[i][j+1];
                else if(Torus)
                {
                    probCells[i][j].Neighbors[6] = probCells[i][0];
                }
                if(j>0)
                    probCells[i][j].Neighbors[7] = probCells[i][j-1];
                else if(Torus)
                    probCells[i][j].Neighbors[7] = probCells[i][height - 1];

            }
        }
        // probCells[5][5].SetLife();
        LiveNeighborChange = RNG.Next(0, 16);
        DeadNeighborChange = RNG.Next(-10, 5);
        BaseProb = RNG.Next(20);
        for (int i = 0; i < 8; i++)
            NeighborWeights[i] = RNG.Next(-10, 11);
    }

    void SetProb()
    {
        for (int i = Xstart; i < width; i++)
        {
            for (int j = Ystart; j < height; j++)
            {
                int p = BaseProb;
                for(int n=0;n<8; n++)
                {
                    if (probCells[i][j].Neighbors[n] != null && probCells[i][j].Neighbors[n].live)
                        p += LiveNeighborChange*NeighborWeights[n];
                    else if (probCells[i][j].Neighbors[n] != null && !probCells[i][j].Neighbors[n].live)
                        p += DeadNeighborChange*NeighborWeights[n];
                }

                probCells[i][j].probability = p;
            }
        }
    }

    void RollProb()
    {
        for (int i = Xstart; i < width; i++)
        {
            for (int j = Ystart; j < height; j++)
            {
                if (probCells[i][j].probability > RNG.Next(100))
                    probCells[i][j].SetLife();
                else
                    probCells[i][j].SetDeath();
            }
        }
    }

    void RollValue()
    {
        for (int i = Xstart; i < width; i++)
        {
            for (int j = Ystart; j < height; j++)
            {
                int LifeCount=0;
                for (int t = 0; t < 10; t++)
                {
                    if (probCells[i][j].probability > RNG.Next(100))
                        LifeCount++;
                }
                probCells[i][j].SetValue(LifeCount);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	if(Time.time>=LastUpdate+delta)
        {
            LastUpdate = Time.time;
            SetProb();
            if (!UseValue)
                RollProb();
            else
                RollValue();
        }
        if (Input.GetKey(KeyCode.R))
            Application.LoadLevel(0);
    }

}
