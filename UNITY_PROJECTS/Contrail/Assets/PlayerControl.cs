using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameObject tile;
    public GameObject MoveButton;
    public GameObject orb;
    public List<Color> Colors;
    public int lastMove;
    System.Random RNG;

    public bool moveUp, moveDown, moveLeft, moveRight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetChild(lastMove).GetComponent<SpriteRenderer>().color = collision.GetComponent<SpriteRenderer>().color;
        Destroy(collision.gameObject);
    }


    // Use this for initialization
    void Start () {
        Colors = new List<Color> { Color.black, Color.blue, Color.white, Color.yellow, Color.magenta, Color.cyan, Color.green, Color.red };
        Color[] AvailableColors = new Color[4];
        RNG = new System.Random();
        for (int i = 0; i < 4; i++)
        {
            int R = RNG.Next(Colors.Count);
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = Colors[R];
            AvailableColors[i]= Colors[R];
            Colors.RemoveAt(R);
        }

        

        
     //random 5x5
        for (int i=0; i<5; i++)
        {
            for(int j=0;j<5;j++)
            {
                GameObject go = Instantiate(orb, new Vector2(i, j)-new Vector2(2,2), Quaternion.identity) as GameObject;
                int r = RNG.Next(4);
                go.GetComponent<SpriteRenderer>().color = AvailableColors[r];
                
            }
        }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Instantiate(MoveButton, new Vector2(i, j) - new Vector2(3, 3), Quaternion.identity);
            }
        }

                /* Impossible board
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        GameObject go = Instantiate(orb, new Vector2(i, j) - new Vector2(2, 2), Quaternion.identity) as GameObject;

                        if(j%2==0 && i%2==0)
                            go.GetComponent<SpriteRenderer>().color = AvailableColors[0];
                        if (j % 2 != 0 && i % 2 == 0)
                            go.GetComponent<SpriteRenderer>().color = AvailableColors[1];
                        if (j % 2 == 0 && i % 2 != 0)
                            go.GetComponent<SpriteRenderer>().color = AvailableColors[2];
                        if (j % 2 != 0 && i % 2 != 0)
                            go.GetComponent<SpriteRenderer>().color = AvailableColors[3];

                    }
                } 

                //old orb recolor generator
                int R = RNG.Next(5, 11);
                for(int i=0;i<R;i++)
                {
                    GameObject go = Instantiate(orb, new Vector2(RNG.Next(-8, 9), RNG.Next(-6, 7)), Quaternion.identity) as GameObject;
                    go.GetComponent<SpriteRenderer>().color = Colors[RNG.Next(Colors.Count)];
                }
                //RandomHelper(RNG.Next(4));
                */

            }


    void ShowContrail(int ci)
    {
        GameObject go=Instantiate(tile, transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<SpriteRenderer>().color = transform.GetChild(ci).GetComponent<SpriteRenderer>().color;
    }

    IEnumerator RandomWalk()
    {
        yield return new WaitForSeconds(1);
        RandomHelper(RNG.Next(4));
        
    }

    void RandomHelper(int i)
    {
        switch (i)
        {
            case 0:
                ShowContrail(2);
                lastMove = 0;
                transform.position += Vector3.up;
                break;
            case 1:
                ShowContrail(3);
                lastMove = 1;
                transform.position += Vector3.left;
                break;
            case 2:
                ShowContrail(0);
                lastMove = 2;
                transform.position += Vector3.down;
                break;
            case 3:
                ShowContrail(1);
                lastMove = 3;
                transform.position += Vector3.right;
                break;
        }
        StartCoroutine(RandomWalk());
    }
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || moveUp)
        {
            ShowContrail(2);
            lastMove = 0;
            transform.position += Vector3.up;
            moveUp = false;

        }
    else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || moveLeft)
        {
            ShowContrail(3);
            lastMove = 1;
            transform.position += Vector3.left;
            moveLeft = false;
        }
    else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || moveDown)
        {
            ShowContrail(0);
            lastMove = 2;
            transform.position += Vector3.down;
            moveDown = false;
        }
    else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || moveRight)
        {
            ShowContrail(1);
            lastMove = 3;
            transform.position += Vector3.right;
            moveRight = false;
        }
	}
}
