using UnityEngine;
using System.Collections.Generic;

public class ProbLifeControl : MonoBehaviour {

    public GameObject cell;
    ProbCell[][] probCells;
    System.Random RNG = new System.Random();
    float LastUpdate;
    public int width;
    public int height;
    public float delta;
    public bool Torus;
    public List<RegionControl> Regions;
    bool RegionEdit;
    public Transform[] RegionEditors;
    Vector2 MouseStart;
    int RegionIndex;
    int MoveIndex;
    public UnityEngine.UI.Text PlayPauseLabel;
    bool Paused;

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
                probCells[i][j].Neighbors = new ProbCell[9];
                probCells[i][j].Neighbors[8] = probCells[i][j];
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
        foreach (RegionControl r in GetComponents<RegionControl>())
            Regions.Add(r);
        RandomRegions();
        CenterRegions();
    }

    public void SetLife()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                probCells[i][j].SetLife();
                probCells[i][j].Value = 9;
            }
        }

    }

    public void SetDeath()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                probCells[i][j].SetDeath();
                probCells[i][j].Value = 0;
            }
        }
    }

    public void SetCellRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int r = RNG.Next(10);
                probCells[i][j].Value = r;
                if(r>=5)
                    probCells[i][j].SetLife();
                else
                    probCells[i][j].SetDeath();

            }
        }
    }

    public void ShowPositionColorCanvas()
    {
        RegionEditors[0].position = new Vector3(10, 10, -10);
        RegionEditors[1].position = Vector3.zero;
        RegionEditors[2].position = new Vector3(10, 10, -10);
    }

    public void ShowProbCanvas()
    {
        RegionEditors[0].position = new Vector3(10, 10, -10);
        RegionEditors[1].position = new Vector3(10, 10, -10);
        RegionEditors[2].position = Vector3.zero;
    }
    public void ShowButtons()
    {
        RegionEditors[0].position = Vector3.zero;
        RegionEditors[1].position = new Vector3(10, 10, -10);
        RegionEditors[2].position = new Vector3(10, 10, -10);
    }

    public void MaxRegions()
    {
        foreach(RegionControl r in Regions)
        {
            r.Xstart = 0;
            r.Ystart = 0;
            r.width = width;
            r.height = height;
        }
    }

    public void SetRegionX(int val)
    {
        Regions[RegionIndex].Xstart = val;
    }

    public void SetRegionY(int val)
    {
        Regions[RegionIndex].Ystart = val;
    }

    public void SetRegionHeight(int val)
    {
        Regions[RegionIndex].height = val;
    }

    public void SetRegionWidth(int val)
    {
        Regions[RegionIndex].width = val;
    }

    public void SetRegionOneRed(int val)
    {
        Regions[RegionIndex].One.r = val/255f;
    }

    public void SetRegionOneGreen(int val)
    {
        Regions[RegionIndex].One.g = val / 255f;
    }
    public void SetRegionOneBlue(int val)
    {
        Regions[RegionIndex].One.b = val / 255f;
    }
    public void SetRegionTwoRed(int val)
    {
        Regions[RegionIndex].Two.r = val / 255f;
    }

    public void SetRegionTwoGreen(int val)
    {
        Regions[RegionIndex].Two.g = val / 255f;
    }

    public void SetRegionTwoBlue(int val)
    {
        Regions[RegionIndex].Two.b = val / 255f;
    }

    public void SetNeighborWeight(int val, int index)
    {
        Regions[RegionIndex].NeighborWeights[index] = val;
    }

    public void SetBaseProb(int val)
    {
        Regions[RegionIndex].BaseProb = val;
    }

    public void SetLiveNeighborChange(int val)
    {
        Regions[RegionIndex].LiveNeighborChange = val;
    }

    public void SetDeadNeighborChange(int val)
    {
        Regions[RegionIndex].DeadNeighborChange = val;
    }

    public void HandleSliderChange(int Value, int ID)
    {
        switch(ID)
        {
            case -1:
                delta = Value/10f;
                break;
            case 0:
                SetRegionX(Value);
                break;
            case 1:
                SetRegionWidth(Value);
                break;
            case 2:
                SetRegionY(Value);
                break;
            case 3:
                SetRegionHeight(Value);
                break;
            case 4:
                SetRegionOneRed(Value);
                break;
            case 5:
                SetRegionOneGreen(Value);
                break;
            case 6:
                SetRegionOneBlue(Value);
                break;
            case 7:
                SetRegionTwoRed(Value);
                break;
            case 8:
                SetRegionTwoGreen(Value);
                break;
            case 9:
                SetRegionTwoBlue(Value);
                break;
            case 10:
                SetBaseProb(Value);
                break;
            case 11:
                SetLiveNeighborChange(Value);
                break;
            case 12:
                SetDeadNeighborChange(Value);
                break;
            default:
                SetNeighborWeight(Value, ID - 13);
                break;
        }
        if (ID < 10 && ID > -1)
            HandleRegions();
    }

    public void HandleButtonClick(int ID)
    {
        RegionIndex = ID;
        SetSliderValues();
        ShowPositionColorCanvas();
    }

    void SetSliderValues()
    {
        foreach(SliderControl s in RegionEditors[1].GetComponentsInChildren<SliderControl>())
        {
            switch (s.ID)
            {
                case 0:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue= Regions[RegionIndex].Xstart/ s.GetComponent<UnityEngine.UI.Slider>().maxValue;
                    break;
                case 1:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].width / s.GetComponent<UnityEngine.UI.Slider>().maxValue;
                    break;
                case 2:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].Ystart / s.GetComponent<UnityEngine.UI.Slider>().maxValue;
                    break;
                case 3:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].height / s.GetComponent<UnityEngine.UI.Slider>().maxValue;
                    break;
                case 4:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].One.r;
                    break;
                case 5:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].One.g;
                    break;
                case 6:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].One.b;
                    break;
                case 7:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].Two.r;
                    break;
                case 8:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].Two.g;
                    break;
                case 9:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = Regions[RegionIndex].Two.b;
                    break;
            }
        }

        foreach(SliderControl s in RegionEditors[2].GetComponentsInChildren<SliderControl>())
        {
            switch(s.ID)
            {
                case 10:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = (Regions[RegionIndex].BaseProb + 50) / (s.GetComponent<UnityEngine.UI.Slider>().maxValue + 50);
                    break;
                case 11:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = (Regions[RegionIndex].LiveNeighborChange + 50) / (s.GetComponent<UnityEngine.UI.Slider>().maxValue + 50);
                    break;
                case 12:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = (Regions[RegionIndex].DeadNeighborChange + 50) / (s.GetComponent<UnityEngine.UI.Slider>().maxValue + 50);
                    break;
                default:
                    s.GetComponent<UnityEngine.UI.Slider>().normalizedValue = (Regions[RegionIndex].NeighborWeights[s.ID - 13] + 50) / (s.GetComponent<UnityEngine.UI.Slider>().maxValue + 50);
                    break;
            }
            s.SetValueText();
        }
    }

    public void PlayPauseSwitch()
    {
        Paused = !Paused;
        if (Paused)
            PlayPauseLabel.text = "Play";
        else
            PlayPauseLabel.text = "Pause";
    }

    public void CenterRegions()
    {
        for(int i=2;i>-1;i--)
        {
            for (int j = 2; j > -1; j--)
            {
                Regions[j * 3 + i].Xstart = 33*(2- i);
                Regions[j * 3 + i].width = 33* (2 - i)+33;
                Regions[j * 3 + i].Ystart = 33 * j;
                Regions[j * 3 + i].height = 33 * j + 33;
            }
        }
    }
    public void RandomRegions()
    {
        foreach (RegionControl r in Regions)
        {
            r.Xstart = RNG.Next(82);
            r.Ystart = RNG.Next(82);
            r.width = 1 + r.Xstart + RNG.Next(width - r.Xstart);
            r.height = 1 + r.Ystart + RNG.Next(height - r.Ystart);
            if (RNG.Next(2) == 0)
            {
                r.LiveNeighborChange = RNG.Next(-1, 15);
                r.DeadNeighborChange = RNG.Next(-5, 8);
                r.BaseProb = RNG.Next(20);
                for (int i = 0; i < 9; i++)
                    r.NeighborWeights[i] = RNG.Next(-4, 5);
            }
            else
            {
                r.LiveNeighborChange = RNG.Next(-50, 51);
                r.DeadNeighborChange = RNG.Next(-50, 51);
                r.BaseProb = RNG.Next(-100, 101);
                for (int i = 0; i < 9; i++)
                    r.NeighborWeights[i] = RNG.Next(-50, 51);
            }
        }

    }

    void SetProb(RegionControl R)
    {
        for (int i = R.Xstart; i < R.width; i++)
        {
            for (int j = R.Ystart; j < R.height; j++)
            {
                int p = R.BaseProb;
                for(int n=0;n<9; n++)
                {
                    if (probCells[i][j].Neighbors[n] != null && probCells[i][j].Neighbors[n].live)
                        p += R.LiveNeighborChange*R.NeighborWeights[n];
                    else if (probCells[i][j].Neighbors[n] != null && !probCells[i][j].Neighbors[n].live)
                        p += R.DeadNeighborChange*R.NeighborWeights[n];
                }

                probCells[i][j].probability = p;
            }
        }
    }

    void RollProb(int Xstart, int Ystart, int width, int height)
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

    void RollValue(int Xstart, int Ystart, int width, int height, Color one, Color two)
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
                probCells[i][j].SetValue(LifeCount, one, two);
            }
        }
    }

    public void HandleRegions()
    {
        foreach(RegionControl R in Regions)
        {
            SetProb(R);
            RollValue(R.Xstart, R.Ystart, R.width, R.height, R.One, R.Two);
        }
    }

    bool MouseMoveBeyondThreshold()
    {
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 Start = Camera.main.ScreenToWorldPoint(MouseStart) - transform.position;
        return Mathf.Abs(Pos.x - Start.x) >= 1 || Mathf.Abs(Pos.y - Start.y) >= 1;
    }

    int FindRegionIndex(Vector2 Pos)
    {
        for(int i= Regions.Count-1; i>-1;i--)
        {
            if (Regions[i].Xstart <= Pos.x && Regions[i].Ystart <= Pos.y && Regions[i].width >= Pos.x && Regions[i].height >= Pos.y)
                return i;
        }
        return -1;
    }
	
	// Update is called once per frame
	void Update () {

       
    if(Input.GetMouseButtonDown(0) && !RegionEdit)
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (Pos.x < 100)
            {
                MoveIndex = FindRegionIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                if (MoveIndex >= 0)
                {
                    RegionEdit = true;
                    MouseStart = Input.mousePosition;
                }
            }

        }
    else if(RegionEdit && Input.GetMouseButtonUp(0))
        {
            RegionEdit = false;
        }
    else if(RegionEdit && MouseMoveBeyondThreshold())
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Vector2 Start= Camera.main.ScreenToWorldPoint(MouseStart) - transform.position;
            if (Regions[MoveIndex].width - Pos.x <= Pos.x - Regions[MoveIndex].Xstart)
                Regions[MoveIndex].width = (int)Mathf.Clamp(Regions[MoveIndex].width + Mathf.Round(Pos.x-Start.x), Regions[MoveIndex].Xstart, width);
            else
                Regions[MoveIndex].Xstart = (int)Mathf.Clamp(Regions[MoveIndex].Xstart + Mathf.Round(Pos.x - Start.x), 0, Regions[MoveIndex].width);
            if (Regions[MoveIndex].height - Pos.y <= Pos.y - Regions[MoveIndex].Ystart)
                Regions[MoveIndex].height = (int)Mathf.Clamp(Regions[MoveIndex].height + Mathf.Round(Pos.y-Start.y), Regions[MoveIndex].Ystart, height);
            else
                Regions[MoveIndex].Ystart = (int)Mathf.Clamp(Regions[MoveIndex].Ystart + Mathf.Round(Pos.y-Start.y), 0, Regions[MoveIndex].height);
            MouseStart = Input.mousePosition;
        }

	if(Time.time>=LastUpdate+delta && !Paused)
        {
            LastUpdate = Time.time;
            HandleRegions();
        }
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
    }

}
