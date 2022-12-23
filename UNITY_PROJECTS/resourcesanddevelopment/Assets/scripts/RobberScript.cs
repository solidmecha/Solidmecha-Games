using UnityEngine;
using System.Collections;

public class RobberScript : MonoBehaviour {

    int[] Pattern;
    int CurrentIndex;
    public int GCIndex;
    public TileScript CurrentTile;

	// Use this for initialization
	void Start () {
        SetPattern();
        CurrentIndex = 0;
	}

    public void SetPattern()
    {
        int count = GameControl.singleton.RNG.Next(3)+2;
        Pattern = new int[count];
        int r = GameControl.singleton.RNG.Next(6);
        
        for(int i=0;i<count;i++)
        {
            Pattern[i] = r;
            int a= GameControl.singleton.RNG.Next(6);
            r = (r + a) % 6;
        }
    }

    public void Move()
    {
        Vector2 v= (Vector2)transform.position + GameControl.singleton.nOffsets[Pattern[CurrentIndex]];
        if (v.y > 4.5f || v.y < -3f || v.x < -7.7f || v.x > 7.7f)
        {
            Pattern[CurrentIndex] = (Pattern[CurrentIndex]+3)%6;
            v = (Vector2)transform.position + GameControl.singleton.nOffsets[Pattern[CurrentIndex]];
        }

        RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);
        if (CurrentTile != null)
        {
            CurrentTile.GamePiece = null;
        }
        if (hit.collider != null)
        {
            CurrentTile = hit.collider.GetComponent<TileScript>();
            if (CurrentTile.GamePiece != null)
            {
                if (CurrentTile.GamePiece.CompareTag("Resource"))
                    CurrentTile.StockPile[0] = 0;
                else if (CurrentTile.GamePiece.CompareTag("Dice"))
                    CurrentTile.GenValue = -1;
                else if (CurrentTile.GamePiece.CompareTag("Robber"))
                    GameControl.singleton.RemoveRobber(CurrentTile.GamePiece.GetComponent<RobberScript>().GCIndex);
                if(CurrentTile.GamePiece != null)
                    Destroy(CurrentTile.GamePiece);
            }
            CurrentTile.GamePiece = gameObject;
        }
        else
            CurrentTile = null;
        transform.position = v;
        CurrentIndex = (CurrentIndex + 1) % Pattern.Length;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
