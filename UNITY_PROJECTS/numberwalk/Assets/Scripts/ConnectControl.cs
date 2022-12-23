using UnityEngine;
using System.Collections;

public class ConnectControl : MonoBehaviour {

    bool drawingLine;
    public GameObject tile;
    public GameObject line;
    GameObject CurrentLine;
    System.Random RNG;
	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        int pointCount = RNG.Next(4, 11);
        for (int i = 0; i < pointCount; i++)
        {
            Vector2 v= new Vector2(RNG.Next(-5, 6), RNG.Next(-4, 5));
            RaycastHit2D hit= Physics2D.Raycast(v, Vector2.zero, 0);
            if (hit)
                Destroy(hit.collider.gameObject);
            GameObject go=Instantiate(tile, v, Quaternion.identity) as GameObject;
            go.GetComponentInChildren<UnityEngine.UI.Text>().text = RNG.Next(1, 11).ToString();
            go.name = go.name + i.ToString();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetMouseButtonDown(0) && !drawingLine)
        {
            Vector3 v = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            CurrentLine = Instantiate(line, v, Quaternion.identity) as GameObject;
            CurrentLine.GetComponent<LineRenderer>().SetColors(new Color(RNG.Next(11) / 10f, RNG.Next(10) / 10f, RNG.Next(11) / 10f), new Color(RNG.Next(11) / 10f, RNG.Next(11) / 10f, RNG.Next(11) / 10f));
            CurrentLine.GetComponent<LineRenderer>().SetPosition(0, v);
            CurrentLine.GetComponent<LineRenderer>().SetPosition(1, v);
            drawingLine = true;
        }
        else if(drawingLine)
        {
            Vector3 v = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            CurrentLine.GetComponent<LineRenderer>().SetPosition(1, v);
        }
        if(Input.GetMouseButtonUp(0))
        {
            drawingLine = false;
        }


	}
}
