using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public List<TileScript> PlayerTiles;
    public GameObject DefaultTile;
    public GameObject Block;
    public GameObject Peg;
    public int width;
    public int height;
    System.Random RNG;
    public int level;
    public List<Vector2> BlockLocs;
    public List<Vector2> PegLocs;

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        level = RNG.Next(12);
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Instantiate(DefaultTile, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity);
            }
        }
        SetBlocks();
        foreach (Vector2 v in BlockLocs)
            Instantiate(Block, v + (Vector2)transform.position, Quaternion.identity);
        foreach(Vector2 v in PegLocs)
            Instantiate(Peg, v + (Vector2)transform.position, Quaternion.identity);
        for (int i=0;i<width;i++)
        {
            Instantiate(Block, new Vector2(i, width) + (Vector2)transform.position, Quaternion.identity);
            Instantiate(Block, new Vector2(i, -1) + (Vector2)transform.position, Quaternion.identity);
        }
        for (int i = 0; i < height; i++)
        {
            Instantiate(Block, new Vector2(height, i) + (Vector2)transform.position, Quaternion.identity);
            Instantiate(Block, new Vector2(-1, i) + (Vector2)transform.position, Quaternion.identity);
        }
        for (int i=0;i<transform.childCount;i++)
        {
            if (transform.GetChild(i).GetComponent<TileScript>() != null)
                PlayerTiles.Add(transform.GetChild(i).GetComponent<TileScript>());
        }
	}

    void SetBlocks()
    {
        switch(level)
        {
            case 0:
                BlockLocs = new List<Vector2> { new Vector2(1,5), new Vector2(2,4), new Vector2(2,0), new Vector2(4,3), new Vector2(6,0), new Vector2(6,5), new Vector2(6,6)};
                PegLocs = new List<Vector2> { new Vector2(0,0), new Vector2(0,1), new Vector2(0,2), new Vector2(1,0), new Vector2(3,0), new Vector2(4,0), new Vector2(5,0) };
                break;
            case 1:
                BlockLocs = new List<Vector2> { new Vector2(1,0), new Vector2(1,4), new Vector2(2,0), new Vector2(2,2), new Vector2(3,3), new Vector2(3,6), new Vector2(4,1), new Vector2(4,3), new Vector2(4,4), new Vector2(5,3)};
                PegLocs = new List<Vector2> { new Vector2(0,5), new Vector2(0,6), new Vector2(1,5), new Vector2(1,6), new Vector2(4,6), new Vector2(5,6), new Vector2(6,6)};
                break;
            case 2:
                BlockLocs = new List<Vector2> { new Vector2(2,1), new Vector2(3,3), new Vector2(4,2), new Vector2(4,6), new Vector2(5,5), new Vector2(6,2)};
                PegLocs = new List<Vector2> { new Vector2(2,6), new Vector2(3,2), new Vector2(3,6), new Vector2(4,5), new Vector2(5,6), new Vector2(6,5), new Vector2(6,6)};
                break;
            case 3:
                BlockLocs = new List<Vector2> { new Vector2(1,2), new Vector2(1,3), new Vector2(2,1), new Vector2(4,1), new Vector2(4,2), new Vector2(4,3), new Vector2(5,4)};
                PegLocs = new List<Vector2> { new Vector2(1,0), new Vector2(2,0), new Vector2(3,0), new Vector2(3,1), new Vector2(4,0), new Vector2(5,0), new Vector2(6,0)};
                break;
            case 4:
                BlockLocs = new List<Vector2> { new Vector2(1, 4), new Vector2(2, 2), new Vector2(3,0), new Vector2(3,1), new Vector2(4,1), new Vector2(4,3), new Vector2(4,6), new Vector2(5,3), new Vector2(5,4)};
                PegLocs = new List<Vector2> { new Vector2(4,0), new Vector2(5,0), new Vector2(6,0), new Vector2(5,1), new Vector2(5,2), new Vector2(6,1), new Vector2(6,2)};
                break;
            case 5:
                BlockLocs = new List<Vector2> { new Vector2(1,0), new Vector2(1,3), new Vector2(2,0), new Vector2(3,2), new Vector2(5,1), new Vector2(5,5), new Vector2(6,6)};
                PegLocs = new List<Vector2> { new Vector2(5,6), new Vector2(6,0), new Vector2(6,1), new Vector2(6,2), new Vector2(6,3), new Vector2(6,4), new Vector2(6,5)};
                break;
            case 6:
                BlockLocs = new List<Vector2> { new Vector2(1,1), new Vector2(1,2), new Vector2(1,4), new Vector2(3,1), new Vector2(3,3), new Vector2(5,2), new Vector2(5,4), new Vector2(6,2), new Vector2(6,4)};
                PegLocs = new List<Vector2> { new Vector2(0,1), new Vector2(0,2), new Vector2(2,1), new Vector2(4,0), new Vector2(5,0), new Vector2(6,0), new Vector2(6,5)};
                break;
            case 7:
                BlockLocs = new List<Vector2> { new Vector2(2,1), new Vector2(2,2), new Vector2(3,5), new Vector2(6,0)};
                PegLocs = new List<Vector2> { new Vector2(0,0), new Vector2(0,1), new Vector2(0,2), new Vector2(1,0), new Vector2(3,0), new Vector2(3,1), new Vector2(4,0)};
                break;
            case 8:
                BlockLocs = new List<Vector2> { new Vector2(1,5), new Vector2(1,6), new Vector2(2,0), new Vector2(2,3), new Vector2(2,5), new Vector2(3,2), new Vector2(5,3), new Vector2(5,6), new Vector2(6,2)};
                PegLocs = new List<Vector2> { new Vector2(0,5), new Vector2(0,6), new Vector2(2,6), new Vector2(3,5), new Vector2(3,6), new Vector2(4,6), new Vector2(6,6)};
                break;
            case 9:
                BlockLocs = new List<Vector2> { new Vector2(1,2), new Vector2(2,3), new Vector2(4,1), new Vector2(4,6), new Vector2(6,6)};
                PegLocs = new List<Vector2> { new Vector2(0,6), new Vector2(1,6), new Vector2(2,6), new Vector2(3,6), new Vector2(6,3), new Vector2(6,4), new Vector2(6,5)};
                break;
            case 10:
                BlockLocs = new List<Vector2> { new Vector2(2,5), new Vector2(3,0), new Vector2(3,1), new Vector2(3,6), new Vector2(4,2), new Vector2(5,6), new Vector2(6,3)};
                PegLocs = new List<Vector2> { new Vector2(2,6), new Vector2(4,6), new Vector2(4,5), new Vector2(5,5), new Vector2(6,5), new Vector2(6,6), new Vector2(6,2)};
                break;
            case 11:
                BlockLocs = new List<Vector2> { new Vector2(1,1), new Vector2(1,3), new Vector2(1,5), new Vector2(3,4), new Vector2(4,2), new Vector2(4,4), new Vector2(5,0), new Vector2(6,3)};
                PegLocs = new List<Vector2> { new Vector2(0,6), new Vector2(1,6), new Vector2(2,6), new Vector2(2,1), new Vector2(2,3), new Vector2(2,5), new Vector2(5,2)};
                break;
        }
    }

    void SetDirection(Vector2 V)
    {
        bool canMove=true;
        foreach (TileScript T in PlayerTiles)
        {
            if (T.isMoving)
                canMove = false;
        }

        if (canMove)
        {
            foreach (TileScript T in PlayerTiles)
            {
                T.Direction = V;
                T.isMoving = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            SetDirection(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            SetDirection(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            SetDirection(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SetDirection(Vector2.down);
    }
}
