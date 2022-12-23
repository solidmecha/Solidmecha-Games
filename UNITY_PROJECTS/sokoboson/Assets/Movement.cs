using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

    public GameObject tile;
    Transform[][] world = new Transform[4][];
    public int[] PlayerXY;
    int[] UpDownScalars;
    int[] LeftRightScalars;
    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;
    public UnityEngine.UI.Text WinBox;
    bool[][] ClearFlags=new bool[4][];

	// Use this for initialization
	void Start () {
        System.Random R = new System.Random();
        List<Color> Colors = new List<Color> { Color.blue, Color.blue, Color.blue, Color.blue,
            Color.red, Color.red, Color.red, Color.red,
            Color.green, Color.green, Color.green, Color.green,
            Color.white, Color.white, Color.white, Color.white
            };

        for(int x=0;x<4;x++)
        {
            world[x] = new Transform[4];
            ClearFlags[x] = new bool[4] { false, false, false, false };
            for(int y=0;y<4;y++)
            {
                GameObject go = Instantiate(tile, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity) as GameObject;
                int i = R.Next(Colors.Count);
                go.GetComponent<SpriteRenderer>().color = Colors[i];
                go.tag = TagByColor(Colors[i]);
                Colors.RemoveAt(i);
                go.transform.SetParent(transform);
                world[x][y] = go.transform;
            }
        }

        PlayerXY = new int[2] {R.Next(4), R.Next(4)};
        //world[PlayerXY[0]][PlayerXY[1]].GetComponent<SpriteRenderer>().color *= new Vector4(1, 1, 1, .5f);
        UpDownScalars = new int[2] { R.Next(-1,2), R.Next(-1,2) };
        LeftRightScalars = new int[2] { R.Next(-1,2), R.Next(-1,2)};
        Xmax = transform.position.x + 3.5f;
        Ymax = transform.position.y + 3.5f;
        Xmin = transform.position.x - .5f;
        Ymin = transform.position.y - .5f;
    }

    void MoveColumn(Vector2 Dir, int i)
    {
        foreach (Transform t in world[i])
        {
            t.position = (Vector2)t.position + Dir;
            if (t.position.y > Ymax)
                t.position = new Vector2(t.position.x, Ymin + .5f);
            else if (t.position.y < Ymin)
                t.position = new Vector2(t.position.x, Ymax - .5f);
        }
    }

    void MoveRow(Vector2 Dir, int c)
    {
        for (int i = 0; i < 4; i++)
        {
            world[i][c].position = (Vector2)world[i][c].position + Dir;
            if (world[i][c].position.x > Xmax)
                world[i][c].position = new Vector2( Xmin + .5f, world[i][c].position.y);
            else if (world[i][c].position.x < Xmin)
                world[i][c].position = new Vector2(Xmax - .5f, world[i][c].position.y);
        }
    }

    string TagByColor(Color C)
    {
        if (C.Equals(Color.blue))
            return "Blue";
        else if (C.Equals(Color.red))
            return "Red";
        else if (C.Equals(Color.green))
            return "Green";
        else
            return "White";
    }
	
    void RecalcWorld()
    {
        for(int i=0;i<16;i++)
        {
            Vector2 v = transform.GetChild(i).position;
            world[(int)v.x][(int)v.y] = transform.GetChild(i);
        }
        UpdateMessage("Blue");
        UpdateMessage("Red");
        UpdateMessage("Green");
        UpdateMessage("White");
    }

    int ClearIndexByColorTag(string C)
    {
        switch (C)
        {
            case "Blue": return 0;
            case "Red": return 1;
            case "Green": return 2;
            default: return 3;
        }
    }

    void UpdateMessage(string C)
    {
        string s = CheckForClears(C);
        if (s != null)
            WinBox.text += s;
    }

    string CheckForClears(string ColorTag)
    {
        int[] XY = GetFirstPosionByTag(ColorTag);
        int index = ClearIndexByColorTag(ColorTag);

        //vertical check
        if (!ClearFlags[index][0] && XY[1] == 0)
        {
            for (int i = 1; i < 4; i++)
            {
                if (!world[XY[0]][XY[1] + i].CompareTag(ColorTag))
                {
                    break;
                }
                else if (i == 3)
                {
                    ClearFlags[index][0] = true;
                    return ColorTag + " cleared in Vertical Line!" + '\n';
                }
            }
        }

        //horizontal check
        if (!ClearFlags[index][1] && XY[0] == 0)
        {
            for (int i = 1; i < 4; i++)
            {
                if (!world[XY[0] + i][XY[1]].CompareTag(ColorTag))
                {
                    break;
                }
                else if (i == 3)
                {
                    ClearFlags[index][1] = true;
                    return ColorTag + " cleared in Horizontal Line!" + '\n';
                }
            }
        }

        //box check
        if(!ClearFlags[index][2] && XY[0]+1<4 && XY[1]+1<4)
        {
            if(world[XY[0]+1][XY[1]+1].CompareTag(ColorTag) && world[XY[0]][XY[1] + 1].CompareTag(ColorTag) && world[XY[0] + 1][XY[1]].CompareTag(ColorTag))
            {
                ClearFlags[index][2] = true;
                return ColorTag + " cleared in a Box!" + '\n';
            }
        }

        //diagonal check
        if(!ClearFlags[index][3])
        {
            if(XY[0]==0 && XY[1]==0)
            {
                if (world[XY[0] + 1][XY[1] + 1].CompareTag(ColorTag) && world[XY[0]+2][XY[1] + 2].CompareTag(ColorTag) && world[XY[0] + 3][XY[1]+3].CompareTag(ColorTag))
                {
                    ClearFlags[index][3] = true;
                    return ColorTag + " cleared in a Diagonal Line!" + '\n';
                }
            }
            else if(XY[0]==3 && XY[1]==0)
            {
                if (world[XY[0] - 1][XY[1] + 1].CompareTag(ColorTag) && world[XY[0] - 2][XY[1] + 2].CompareTag(ColorTag) && world[XY[0] - 3][XY[1] + 3].CompareTag(ColorTag))
                {
                    ClearFlags[index][3] = true;
                    return ColorTag + " cleared in a Diagonal Line!" + '\n';
                }
            }
        }

        return null;
    }

    int[] GetFirstPosionByTag(string ColorTag)
    {
        for(int i=0;i<world.Length;i++)
        {
            for(int j=0;j<world[i].Length;j++)
            {
                if(world[i][j].CompareTag(ColorTag))
                {
                    return new int[2] { i, j };
                }
            }
        }
        return new int[2] { -1, -1 };
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveColumn(Vector2.up, (int)world[PlayerXY[0]][PlayerXY[1]].position.x);
            int leftIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.x - 1;
            if (leftIndex < 0)
                leftIndex = 3;
            int rightIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.x + 1;
            if (rightIndex > 3)
                rightIndex = 0;
            MoveColumn(Vector2.up * LeftRightScalars[0], leftIndex);
            MoveColumn(Vector2.up * LeftRightScalars[1], rightIndex);
            RecalcWorld();
            PlayerXY[1] += 1;
            if (PlayerXY[1] == 4)
                PlayerXY[1] = 0;

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveRow(Vector2.left, (int)world[PlayerXY[0]][PlayerXY[1]].position.y);
            int downIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.y - 1;
            if (downIndex < 0)
                downIndex = 3;
            int upIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.y + 1;
            if (upIndex > 3)
                upIndex = 0;
            MoveRow(Vector2.left * UpDownScalars[0], downIndex);
            MoveRow(Vector2.left * UpDownScalars[1], upIndex);
            RecalcWorld();
            PlayerXY[0] += -1;
            if (PlayerXY[0] < 0)
                PlayerXY[0] = 3;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveColumn(Vector2.down, (int)world[PlayerXY[0]][PlayerXY[1]].position.x);
            int leftIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.x - 1;
            if (leftIndex < 0)
                leftIndex = 3;
            int rightIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.x + 1;
            if (rightIndex > 3)
                rightIndex = 0;
            MoveColumn(Vector2.down * LeftRightScalars[0], leftIndex);
            MoveColumn(Vector2.down * LeftRightScalars[1], rightIndex);
            RecalcWorld();
            PlayerXY[1] += -1;
            if (PlayerXY[1] < 0)
                PlayerXY[1] = 3;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveRow(Vector2.right, (int)world[PlayerXY[0]][PlayerXY[1]].position.y);
            int downIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.y - 1;
            if (downIndex < 0)
                downIndex = 3;
            int upIndex = (int)world[PlayerXY[0]][PlayerXY[1]].position.y + 1;
            if (upIndex > 3)
                upIndex = 0;
            MoveRow(Vector2.right * UpDownScalars[0], downIndex);
            MoveRow(Vector2.right * UpDownScalars[1], upIndex);
            RecalcWorld();
            PlayerXY[0] += 1;
            if (PlayerXY[0] == 4)
                PlayerXY[0] = 0;
        }
        else if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
    }
}
