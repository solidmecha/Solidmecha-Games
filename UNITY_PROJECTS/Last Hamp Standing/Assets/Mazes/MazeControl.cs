using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeControl : MonoBehaviour {
    public static MazeControl singleton;
    public bool[][] Grid;
    public GameObject Wall;
    int width=13;
    int height=13;

    // Use this for initialization
    void Start () {
        singleton = this;
        GenerateMaze();
    }

    void BuildMaze()
    {
        GenerateMaze();
    }

    public bool[] OneD()
    {
        bool[] temp = new bool[169];
        for (int i = 0; i < 169; i++)
        {
            int x = i % 13;
            int y = i / 13;
            temp[i] = Grid[x][y];
        }
        return temp;
    }

    public void GenerateMaze()
    {
       Grid = new bool[width][];

        for (int i = 0; i < width; i++)

            Grid[i] = new bool[height];

        List<int[]> Frontier = new List<int[]> { };
        System.Random RNG = new System.Random();
        int a = 1+10*RNG.Next(2);
        int b = 1 + 10 * RNG.Next(2);
        Grid[a][b] = true;

        foreach (int[] ia in DetermineFrontier(new int[2] { a, b}))

        {

            Frontier.Add(ia);

        }


        while (Frontier.Count > 0)
        {

            int[] IA = new int[2];

            int R = RNG.Next(Frontier.Count);

            IA = Frontier[R];

            if (!Grid[IA[0]][IA[1]])
            {

                Grid[IA[0]][IA[1]] = true;

                int[] NIndex = CheckNeighbors(new int[2] { IA[0], IA[1] })[RNG.Next(CheckNeighbors(new int[2] { IA[0], IA[1] }).Count)];

                Grid[NIndex[0]][NIndex[1]] = true;

                foreach (int[] ia in DetermineFrontier(IA))
                {

                    if (!Frontier.Contains(ia))

                        Frontier.Add(ia);

                }

            }

            Frontier.RemoveAt(R);

        }

    }



List<int[]> DetermineFrontier(int[] Indices)
{
    List<int[]> TempList = new List<int[]> { };

    if (Indices[0] - 2 >= 1 && !Grid[Indices[0] - 2][Indices[1]])

        TempList.Add(new int[2] { Indices[0] - 2, Indices[1] });

    if (Indices[0] + 2 < width-1 && !Grid[Indices[0] + 2][Indices[1]])

        TempList.Add(new int[2] { Indices[0] + 2, Indices[1] });

    if (Indices[1] - 2 >= 1 && !Grid[Indices[0]][Indices[1] - 2])

        TempList.Add(new int[2] { Indices[0], Indices[1] - 2 });

    if (Indices[1] + 2 < height-1 && !Grid[Indices[0]][Indices[1] + 2])

        TempList.Add(new int[2] { Indices[0], Indices[1] + 2 });

    return TempList;

}

List<int[]> CheckNeighbors(int[] Indices)

{

    List<int[]> TempList = new List<int[]> { };

    if (Indices[0] - 2 >= 0 && Grid[Indices[0] - 2][Indices[1]])

        TempList.Add(new int[2] { Indices[0] - 1, Indices[1] });

    if (Indices[0] + 2 < width && Grid[Indices[0] + 2][Indices[1]])

        TempList.Add(new int[2] { Indices[0] + 1, Indices[1] });

    if (Indices[1] - 2 >= 0 && Grid[Indices[0]][Indices[1] - 2])

        TempList.Add(new int[2] { Indices[0], Indices[1] - 1 });

    if (Indices[1] + 2 < height && Grid[Indices[0]][Indices[1] + 2])

        TempList.Add(new int[2] { Indices[0], Indices[1] + 1 });

    return TempList;

}

// Update is called once per frame
void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("S"))
                Destroy(g);
            BuildMaze();
        }
	}
}
