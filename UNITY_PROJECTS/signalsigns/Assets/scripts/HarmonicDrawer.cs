using UnityEngine;
using System.Collections;

public class HarmonicDrawer : MonoBehaviour {
    public HarmonicWalker Xwalker;
    public HarmonicWalker Ywalker;
    public float counter;
    public float CycleTime;
    int cycleCount;
    public GameControl GC;
    LineRenderer CurrentLine;
    // Use this for initialization
    void Start () {
    }

    public void Setup()
    {
        if (Xwalker.speed < Ywalker.speed)
        {
            cycleCount = (int)Ywalker.speed;
            CycleTime = (1f / Xwalker.speed);
        }
        else
        {
            cycleCount = (int)Xwalker.speed;
            CycleTime = (1f / Ywalker.speed);
        }
        StartNewLine();
    }

    public void StartNewLine()
    {
        transform.position = new Vector2(Xwalker.transform.position.x, Ywalker.transform.position.y);
        GameObject go = Instantiate(GC.Line) as GameObject;
        CurrentLine = go.GetComponent<LineRenderer>();
        CurrentLine.SetPosition(0, transform.position);
        CurrentLine.SetPosition(1, transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        if (GC.ShowSign)
        {
            //counter += Time.deltaTime;
            transform.position = new Vector2(Xwalker.transform.position.x, Ywalker.transform.position.y);
            CurrentLine.SetPosition(1, transform.position);
            /* if (VertCount < 64*cycleCount)
            /// {
            if (counter >= CycleTime)
            {
                counter = 0;
                
            }
            //}*/
        }

	}
}
