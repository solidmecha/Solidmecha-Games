using UnityEngine;
using System.Collections.Generic;

public class Creation : MonoBehaviour {
    System.Random RNG;
    public GameObject Vert;
    public GameObject Edge;
    public int[] VerMinMax;
    public List<VertScript> Verts;
    public GameObject Selection;
	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        CreateGraph();
        Scramble();
	}

    void CreateGraph()
    {
        int count = RNG.Next(VerMinMax[0], VerMinMax[1]);
        for(int i=0;i<count;i++)
        {
            Vector2 loc = new Vector2(RNG.Next(-7, 8), RNG.Next(-5, 6));
            if(Physics2D.Raycast(loc, Vector2.zero).collider == null)
            {
                GameObject go = Instantiate(Vert, loc, Quaternion.identity) as GameObject;
                Verts.Add(go.GetComponent<VertScript>());
                go.GetComponent<VertScript>().Pos = loc;
                go.GetComponent<VertScript>().ID = Verts.Count;
            }
        }

        for(int i=0; i<Verts.Count;i++)
        {
            int c = RNG.Next(1, 4);
            for (int j = 0; j < c; j++)
            {
                List<int> temp = new List<int> { };
                for(int t=0;t<Verts.Count;t++)
                {
                    if(t!=i && !Verts[t].connections.Contains(Verts[i].ID) && Verts[t].connections.Count < (Verts.Count / 2))
                    {
                        temp.Add(t);
                    }
                }
                if (temp.Count > 0)
                {
                    int r = RNG.Next(temp.Count);
                    if (Verts[i].connections.Count<= Verts[temp[r]].connections.Count)
                        MakeEdge(Verts[i], Verts[temp[r]]);
                    else
                        MakeEdge(Verts[temp[r]], Verts[i]);
                }
            }
        }

    }

    void MakeEdge(VertScript A, VertScript B)
    {
        A.connections.Add(B.ID);
        B.connections.Add(A.ID);
        Vector2 loc = (A.transform.position + B.transform.position) * .5f;
        Quaternion q = Quaternion.LookRotation(Vector3.forward, A.transform.position - B.transform.position);
        GameObject E = Instantiate(Edge, loc, q) as GameObject;
        E.transform.localEulerAngles = new Vector3(0, 0, E.transform.localEulerAngles.z + 90);
        E.transform.localScale = new Vector2(Vector2.Distance(A.transform.position, B.transform.position), .1f);
        E.transform.GetChild(0).localScale = new Vector2(.2f / E.transform.localScale.x, 2);
        E.transform.GetChild(1).localScale = new Vector2(.2f / E.transform.localScale.x, 2);
        E.transform.SetParent(A.transform);
    }

    void Scramble()
    {
        foreach(VertScript v in Verts)
        {
            v.transform.position= new Vector2(RNG.Next(-7, 8), RNG.Next(-5, 6));
            v.transform.localEulerAngles = new Vector3(0, 0, 90 * RNG.Next(4));
        }
    }

    void Solve()
    {
        foreach (VertScript v in Verts)
        {
            v.transform.position = v.Pos;
            v.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
                Selection = hit.collider.gameObject;
        }
        if(Input.GetMouseButton(0) && Selection!= null)
        {
            Selection.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E)) && Selection != null)
        {
            Selection.transform.Rotate(new Vector3(0, 0, -90));
        }
        if ((Input.GetMouseButtonDown(3) || Input.GetKeyDown(KeyCode.Q)) && Selection != null)
        {
            Selection.transform.Rotate(new Vector3(0, 0, 90));
        }
        if (Input.GetMouseButtonUp(0))
        {
            Selection = null;
        }

        if (Input.GetKeyDown(KeyCode.S))
            Solve();
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
    }
}
