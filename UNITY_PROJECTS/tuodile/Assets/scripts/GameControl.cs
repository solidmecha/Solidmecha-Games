using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject Tile;
    public Color[] colors;
    public Sprite[] Sprites;
    System.Random RNG =new System.Random();
    Transform ActiveTile;
    public float GameSpeed;
    float counter;
    int RotationCounter;
    bool SonicDrop;
    public List<TileControl> DropList;
    bool Resolving;
    public Text Score;
    public Text MatchCount;
    int CurrentMatchCount;
    int CurrentScore;
    int MatchValue;
    public Vector3 HoldLocation;

	// Use this for initialization
	void Start () {
        for (int i = 2; i < 9; i++)
            BuildTile(new Vector2(i, 9));
        RotationCounter = 0;
        ActiveTile=BuildTile(new Vector2(2, 8)).transform;
    }

   GameObject BuildTile(Vector2 Pos)
    {
        GameObject go = Instantiate(Tile, Pos, Quaternion.identity) as GameObject;
        if (RotationCounter != 2)
        {
            int[] ia = new int[4];
            do
            {
                for (int i = 0; i < 4; i++)
                {
                    ia[i] = RNG.Next(4);
                }
            } while (CheckSameness(ia));
            go.GetComponent<TileControl>().ColorIDs = ia;
            for (int i = 0; i < 4; i++)
            {
                go.transform.GetChild(i).GetComponent<SpriteRenderer>().color = colors[ia[i]];
            }
            RotationCounter++;
        }
        else
        {
            go.tag = "R";
            RotationScript RS = go.AddComponent<RotationScript>();
            int r = RNG.Next(5);
            go.GetComponent<SpriteRenderer>().sprite=Sprites[r];
            go.GetComponent<SpriteRenderer>().color = Color.white;
            switch (r)
            {
                case 0:
                    RS.RotationValue = 90;
                    break;
                case 1:
                    RS.RotationValue = -90;
                    break;
                case 2:
                    RS.VertReflect = true;
                    break;
                case 3:
                    RS.HorizonReflect = true;
                    break;
                case 4:
                    RS.VertReflect = true;
                    RS.HorizonReflect = true;
                    RS.RotationValue = 180;
                    break;
            }
            RotationCounter = 0;
        }
        return go;
    }
	
    bool CheckSameness(int[] numbers)
    {
        int[] ia = new int[4] { 0, 0, 0, 0 };
        for(int i=0; i<4;i++)
        {
            for(int j=0;j<4;j++)
            {
                if (i == numbers[j])
                {
                    ia[i]++;
                    if (ia[i] == 4)
                        return true;
                }
            }
        }
        return false;
    }

    void MoveUpPreview()
    {
       ActiveTile = Physics2D.Raycast(new Vector2(2, 9), Vector2.zero).transform;
       ActiveTile.Translate(Vector2.down);
       for(int i=3;i<9;i++)
        {
            Physics2D.Raycast(new Vector2(i, 9), Vector2.zero).transform.Translate(Vector2.left);
        }
        BuildTile(new Vector2(8, 9));
    }

    void UpdateScore(int i)
    {
        CurrentScore += MatchValue*i;
        Score.text = "Score: "+CurrentScore.ToString();
        CurrentMatchCount++;
        MatchCount.text = "Matches: "+CurrentMatchCount.ToString();
    }

    void ResolveMatches()
    {
        bool Matches=false;
     List<int[]> Columns=new List<int[]> { };
     for(int x=0;x<6;x++)
        {
            Columns.Clear();
            for (int y=0;y<9; y++)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
                if(hit.collider !=null && hit.collider.CompareTag("T"))
                {
                    Columns.Add(hit.collider.GetComponent<TileControl>().ColorIDs);
                }
                if ((y == 8 || hit.collider == null || !hit.collider.CompareTag("T")) && Columns.Count>=4)
                 {
                    int[] ia=CheckColumn(Columns);
                    if(ia[1]!=0)
                    {
                        UpdateScore(ia[1]+1);
                        for (int i = 0; i <= ia[1]; i++)
                        {
                            Matches = true;
                            Physics2D.Raycast(new Vector2(x, ia[0] + i), Vector2.zero).collider.GetComponent<TileControl>().matched = true;
                        }
                    }
                    Columns.Clear();
                }
            }
        }

        int StartIndex = -1;
        for (int y = 0; y< 9; y++)
        {
            Columns.Clear();
            StartIndex = -1;
            for (int x = 0; x < 6; x++)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("T"))
                {
                    Columns.Add(hit.collider.GetComponent<TileControl>().ColorIDs);
                    if (StartIndex < 0)
                        StartIndex = x;
                }
                if ((x == 5 || hit.collider == null || !hit.collider.CompareTag("T")) && Columns.Count >= 4)
                {
                    int[] ia = CheckRow(Columns);
                    if (ia[1] != 0)
                    {
                        UpdateScore(ia[1]+1);
                        for (int i = 0; i <= ia[1]; i++)
                        {
                            Matches = true;
                            Physics2D.Raycast(new Vector2(StartIndex + ia[0] + i, y), Vector2.zero).collider.GetComponent<TileControl>().matched = true;
                        }
                    }

                        Columns.Clear();
                        StartIndex = -1;
                }
                else if (hit.collider == null || !hit.collider.CompareTag("T"))
                {
                    Columns.Clear();
                    StartIndex = -1;
                }
            }
        }
        if (!Matches)
        {
            MoveUpPreview();
            Resolving = false;
            MatchValue = 1;
        }
        else
        {
            MatchValue += 100;
            Resolving = true;
            Invoke("DropDown", .42f);
            Invoke("ResolveMatches", .84f);
        }
    }

    void DropDown()
    {
        foreach (TileControl t in DropList)
            t.fall = true;
        Invoke("StopDrop", .42f);
    }

    void StopDrop()
    {
        foreach (TileControl t in DropList)
        {
            t.transform.position = new Vector2(t.transform.position.x, Mathf.Round(t.transform.position.y));
            t.fall = false;
            t.fallDistance = 0;
        }
        DropList.Clear();
    }

    //start index and count
    int[] CheckColumn(List<int[]> IDs)
    {
        int MatchCount=0;
       for(int i=0; i<IDs.Count-1;i++)
        {
            if (IDs[i][1] == IDs[i + 1][3])
            {
                MatchCount++;
            }
            else
            {
                if (MatchCount < 3)
                {
                    MatchCount = 0;
                }
                else
                {
                    return new int[] { i - MatchCount, MatchCount };
                }
            }
            if(i==IDs.Count-2 && MatchCount>=3)
                return new int[] { i - MatchCount+1, MatchCount };
        }
        return new int[] { 0, 0};
    }

    //start index and count
    int[] CheckRow(List<int[]> IDs)
    {
        int MatchCount = 0;
        for (int i = 0; i < IDs.Count - 1; i++)
        {
            if (IDs[i][2] == IDs[i + 1][0])
            {
                MatchCount++;
            }
            else
            {
                if (MatchCount < 3)
                {
                    MatchCount = 0;
                }
                else
                {
                    return new int[] { i - MatchCount, MatchCount};
                }
            }
            if (i == IDs.Count - 2 && MatchCount >= 3)
            {
                return new int[] { i - MatchCount+1, MatchCount };
            }
        }
        return new int[] { 0, 0 };
    }


    int[] HandleRotationReflection(int[] ia, bool vertical, bool horizontal, int RotationValue)
    {
        if(vertical)
        {
            int t = ia[0];
            ia[0] = ia[2];
            ia[2] = t;
        }
        if(horizontal)
        {
            int t = ia[1];
            ia[1] = ia[3];
            ia[3] = t;
        }
            switch(RotationValue)
            {
                case 90:
                    {
                        int t = ia[0];
                        for (int i = 0; i < 3; i++)
                            ia[i] = ia[i + 1];
                        ia[3] = t;
                    }
                    break;
                case -90:
                    {
                        int t = ia[3];
                        for (int i = 3; i > 0; i--)
                            ia[i] = ia[i - 1];
                        ia[0] = t;
                    }
                    break;
            default: break;
            }
     
        return ia;
    }

    void HandleRotationCollision(Vector2 Dir, Transform RotationTile, Transform ColorTile)
    {
        ActiveTile.position = (Vector2)ActiveTile.position + Dir;
        Destroy(RotationTile.GetComponent<BoxCollider2D>());
        RotationTile.gameObject.AddComponent<FadeOut>();
        RotationScript rs = RotationTile.GetComponent<RotationScript>();
       ColorTile.GetComponent<TileControl>().ColorIDs = HandleRotationReflection(ColorTile.GetComponent<TileControl>().ColorIDs, rs.VertReflect, rs.HorizonReflect, rs.RotationValue);

        RotateIt ri = ColorTile.gameObject.AddComponent<RotateIt>();
        ri.vertical = rs.VertReflect;
        ri.horizontal = rs.HorizonReflect;
        if (rs.VertReflect && rs.RotationValue == 0)
        {
            ri.Rotation = new Vector3(0, 180, 0);
        }
        else if (rs.HorizonReflect && rs.RotationValue == 0)
        {
            ri.Rotation = new Vector3(180, 0, 0);
        }
        else
        {
            ri.Rotation = new Vector3(0, 0, rs.RotationValue);
        }

    }

	// Update is called once per frame
	void Update () {

        if (!Resolving)
        {
            counter += Time.deltaTime;
            if (counter >= GameSpeed)
            {
                if (!SonicDrop)
                    counter = 0;
                RaycastHit2D hit=new RaycastHit2D();
                hit = Physics2D.Raycast((Vector2)ActiveTile.position + Vector2.down, Vector2.zero);

                if (hit.collider != null)
                {
                    SonicDrop = false;
                    counter = 0;
                    if (ActiveTile.CompareTag("R") && hit.collider.CompareTag("T"))
                    {
                        HandleRotationCollision(Vector2.down, ActiveTile, hit.transform);
                        ResolveMatches();
                    }
                    else if (ActiveTile.CompareTag("T") && hit.collider.CompareTag("R"))
                    {
                        HandleRotationCollision(Vector2.down, hit.transform, ActiveTile);
                    }
                    else
                    {
                        ResolveMatches();
                    }
                }
                else
                {
                  ActiveTile.position = (Vector2)ActiveTile.position + Vector2.down;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)ActiveTile.position + Vector2.left, Vector2.zero);
                if (hit.collider != null)
                {
                    if (ActiveTile.CompareTag("R") && hit.collider.CompareTag("T"))
                    {
                        HandleRotationCollision(Vector2.left, ActiveTile, hit.transform);
                        ResolveMatches();
                    }
                    else if (ActiveTile.CompareTag("T") && hit.collider.CompareTag("R"))
                    {
                        HandleRotationCollision(Vector2.left, hit.transform, ActiveTile);
                    }
                }
                else
                    ActiveTile.position = (Vector2)ActiveTile.position + Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)ActiveTile.position + Vector2.right, Vector2.zero);
                if (hit.collider != null)
                {
                    if (ActiveTile.CompareTag("R") && hit.collider.CompareTag("T"))
                    {
                        HandleRotationCollision(Vector2.right, ActiveTile, hit.transform);
                        ResolveMatches();
                    }
                    else if (ActiveTile.CompareTag("T") && hit.collider.CompareTag("R"))
                    {
                        HandleRotationCollision(Vector2.right, hit.transform, ActiveTile);
                    }
                }
                else
                    ActiveTile.position = (Vector2)ActiveTile.position + Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                counter = GameSpeed;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                counter = GameSpeed;
                SonicDrop = true;
            }
            else if(Input.GetKeyDown(KeyCode.Delete))
            {
                Destroy(ActiveTile.gameObject);
                MoveUpPreview();
                counter = 0;
            }
            else if(Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                
                GameObject g = ActiveTile.gameObject;
                RaycastHit2D hit = Physics2D.Raycast(HoldLocation, Vector2.zero);
                if (hit.collider != null)
                {
                    ActiveTile = hit.transform;
                    ActiveTile.position = new Vector3(2, 8);
                    counter = 0;
                }
                else
                {
                    MoveUpPreview();
                    
                }
                g.transform.position = HoldLocation;
                counter = 0;

            }
        }
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (GameSpeed > 2)
                GameSpeed = 1;
            GameSpeed -= .25f;
            if (GameSpeed < .1f)
            {
                GameSpeed = .1f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
            GameSpeed = Mathf.Clamp(42,1,GameSpeed+1);

    }
}
