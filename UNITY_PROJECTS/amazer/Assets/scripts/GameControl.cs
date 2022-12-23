using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject SelectedPiece;
    public GameObject Selection;
    public GameObject tile;
    public GameObject Dopple;
    public GameObject piece;
    public TileControl[][] Tiles=new TileControl[9][];
    public List<GameObject> Mirrors=new List<GameObject> { };
    public List<PieceControl> Pieces = new List<PieceControl> { };
    public GameObject Victory;
    public bool Won;
    public void CheckVictory()
    {
        if (!Won)
        {
            int c = 0;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            {
                if ((g.transform.position + transform.position).y > -.5f)
                    c++;
            }
            if (c == 5)
            {
                Instantiate(Victory);
                Won = true;
            }
        }
    }

    public void SetSelection(GameObject g)
    {
        SelectedPiece = g;
        Selection.transform.parent = g.transform;
        Selection.transform.localPosition = Vector2.zero;
    }

	// Use this for initialization
	void Start () {
        System.Random rng = new System.Random();
	for (int i=0;i<9;i++)
        {
            Tiles[i] = new TileControl[9];
            for (int j = 0; j < 9; j++)
            {
                GameObject t=Instantiate(tile, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                Tiles[i][j]=t.GetComponent<TileControl>();
                if (j==0)
                {
                    GameObject go=Instantiate(piece, transform.position + new Vector3(i, j, -1f), Quaternion.identity) as GameObject;
                    GameObject m=Instantiate(Dopple, transform.position + new Vector3(i, j + 6, -1f), Quaternion.identity) as GameObject;
                    Pieces.Add(go.GetComponent<PieceControl>());
                    Mirrors.Add(m);
                }
                if(j==1 && i!=4)
                {
                    //GameObject go = Instantiate(piece, transform.position + new Vector3(i, j, -1f), Quaternion.identity) as GameObject;
                    //GameObject m = Instantiate(piece, transform.position + new Vector3(i, j + 6, -1f), Quaternion.identity) as GameObject;
                    //Pieces.Add(go.GetComponent<PieceControl>());
                    //Mirrors.Add(m);
                }
                else if(j==2)
                {
                    GameObject go = Instantiate(piece, transform.position + new Vector3(i, j, -1f), Quaternion.identity) as GameObject;
                    GameObject m = Instantiate(Dopple, transform.position + new Vector3(i, j + 6, -1f), Quaternion.identity) as GameObject;
                    Pieces.Add(go.GetComponent<PieceControl>());
                    Mirrors.Add(m);
                }
            }
        }
        for(int i=0;i<Pieces.Count;i++)
        {
            int r = rng.Next(Mirrors.Count);
            SetMirror(Pieces[i], Mirrors[r], new Color(rng.Next(20, 246) / 255f, rng.Next(20, 246) / 255f, rng.Next(20, 246) / 255f));
            Mirrors.RemoveAt(r);
        }
	}

    void SetMirror(PieceControl p, GameObject m, Color c)
    {
        p.Mirror = m;
        p.GetComponent<SpriteRenderer>().color = c;
        m.GetComponent<SpriteRenderer>().color = c;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
