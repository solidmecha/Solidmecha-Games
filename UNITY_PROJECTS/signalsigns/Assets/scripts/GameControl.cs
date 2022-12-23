using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject Line;
    bool isDrawing;
    public List<Vector3>[] Shapes = new List<Vector3>[2];
    int ShapeIndex;
    LineRenderer CurrentLine;
    public HarmonicWalker[] HarmonicWalkers;
    public HarmonicDrawer HD;
    public bool ShowSign;

	// Use this for initialization
	void Start () {
        Shapes[0] = new List<Vector3> { };
        Shapes[1] = new List<Vector3> { };
    }

    private void SignalSign()
    {
        if (Shapes[0].Count > 1 && Shapes[1].Count > 1)
        {
            HarmonicWalkers[0].Setup();
            HarmonicWalkers[1].Setup();
            HD.Setup();
            ShowSign = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (p.y > 2.249f)
            {
                ShapeIndex = 1;
            }
            else
                ShapeIndex = 0;

            GameObject go = Instantiate(Line) as GameObject;
            CurrentLine = go.GetComponent<LineRenderer>();
            CurrentLine.SetPosition(0, p);
            Shapes[ShapeIndex].Add(p);
        }
        if(isDrawing)
        {
            Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CurrentLine.SetPosition(1, p);
        }
        if(Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CurrentLine.SetPosition(1, p);
            Shapes[ShapeIndex].Add(p);
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isDrawing)
        {
            if (!ShowSign)
                SignalSign();
            else
                Application.LoadLevel(0);
        }
	
	}
}
