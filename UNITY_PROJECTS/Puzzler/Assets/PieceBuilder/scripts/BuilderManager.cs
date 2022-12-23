using UnityEngine;
using System.Collections.Generic;

public class BuilderManager : MonoBehaviour {

    public GameObject BaseTile;
    List<GameObject> newPiece=new List<GameObject> { };
    System.Random RNG = new System.Random(ThreadSafeRandom.Next());
   public Vector2 V = Vector2.zero;

    // Use this for initialization
    void Start () {
        createPiece();
	}

    public void createPiece()
    {
        foreach(GameObject g in newPiece)
        {
            //Destroy(g);
        }
        newPiece.Clear();
        int r=RNG.Next(3, 9);
        List<Vector2> PosList = new List<Vector2> { };
        for (int i = 0; i < r; i++)
        {
            GameObject go=Instantiate(BaseTile, V, Quaternion.identity) as GameObject;
            PosList.Add(V);
            while(PosList.Contains(V))
            {
                switch(RNG.Next(6))
                {
                    case 0:
                        V += Vector2.right;
                        break;
                    case 1:
                        V += Vector2.left;
                        break;
                    case 2:
                        V += new Vector2(.5f, .75f);
                        break;
                    case 3:
                        V += new Vector2(-.5f, .75f);
                        break;
                    case 4:
                        V += new Vector2(.5f, -.75f);
                        break;
                    case 5:
                        V += new Vector2(-.5f, -.75f);
                        break;
                }
                newPiece.Add(go);
                if(i==0)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                }
                if (i == r - 1)
                {
                    go.AddComponent<BoxCollider2D>();
                    go.AddComponent<TileLeader>();
                    go.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            } 
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
