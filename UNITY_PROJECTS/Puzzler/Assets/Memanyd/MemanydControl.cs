using UnityEngine;
using System.Collections;

public class MemanydControl : MonoBehaviour {

    public GameObject tile;

	// Use this for initialization
	void Start () {
	 for(int i=0;i<6;i++)
        {
            for(int j=0;j<6;j++)
            {
                GameObject g=Instantiate(tile, (Vector2)transform.position+new Vector2(i, j), Quaternion.identity)as GameObject;
                switch(i)
                {
                    case 0:
                        g.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    case 1:
                        g.GetComponent<SpriteRenderer>().color = Color.cyan;
                        break;
                    case 2:
                        g.GetComponent<SpriteRenderer>().color = Color.magenta;
                        break;
                    case 3:
                        g.GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                    case 4:
                        g.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                    case 5:
                        g.GetComponent<SpriteRenderer>().color = Color.yellow;
                        break;
                }
            }
        }
	}

    public void RowSwap(float y)
    {
        y -= transform.position.y;
        GameObject[]row = new GameObject[6];
        for (int x = 0; x < 6; x++)
        {
            row[x] = Physics2D.Raycast((Vector2)transform.position + new Vector2(x, y), Vector2.zero).collider.gameObject;
        }
        for (int i = 0; i < 3; i++)
        {
            float t=row[i].transform.position.x;
            row[i].transform.position = new Vector2(t + 5 - 2*i, y+transform.position.y);
            t = row[i + 3].transform.position.x;
            row[i + 3].transform.position = new Vector2(t - 1 - 2*i, y + transform.position.y);
        }
    }

    public void ColumnSwap(float x)
    {
        x -= transform.position.x;
        GameObject[] Column = new GameObject[6];
        for(int y=0; y<6;y++)
        {
            Column[y]=Physics2D.Raycast((Vector2)transform.position+new Vector2(x, y), Vector2.zero).collider.gameObject;
        }
        for(int i=0; i<3;i++)
        {
            int t = i;
            Column[i].transform.position = new Vector2(x+transform.position.x, Column[i].transform.position.y + 5 - 2*t);
            Column[i + 3].transform.position = new Vector2(x + transform.position.x, Column[i + 3].transform.position.y - 1 - 2*t);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
