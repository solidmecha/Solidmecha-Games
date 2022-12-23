using UnityEngine;
using System.Collections.Generic;

public class Levels : MonoBehaviour {
    // 0 blank,1 red,2 green,3 blue blocks, 4 walls, 5 boxes,6 playerstart
    //0 push, 1 vac, 2 hook, 3 bolt,4 CCW rotation, 5 CW rotation, 6 red,7 green,8 blue pegs
    System.Random RNG;
    public GameControl GC;

    public string[] LevelLookUp(int I)
    {
        string[] Level = new string[7];
        switch(I)
        {
            case 0:
                Level[6] = "6100000";
                Level[5] = "0200000";
                Level[4] = "0300000";
                Level[3] = "0400000";
                Level[2] = "0400000";
                Level[1] = "0400000";
                Level[0] = "0400000";
                break;
            default:
                Level[6] = "0000000";
                Level[5] = "0000000";
                Level[4] = "0000000";
                Level[3] = "0000000";
                Level[2] = "0000000";
                Level[1] = "0000000";
                Level[0] = "0000000";
                break;
        }
        return Level;
    }

    public List<Vector4> ModLocations(int I) //posX, posY, rot, ID
    {
        List<Vector4> V4s = new List<Vector4> { };

        switch(I)
        {
            case 0:
                Vector4 V = new Vector4(0, 5, 0, 1);
                Vector4 V1 = new Vector4(0, 4, 180, 2);
                Vector4 V2 = new Vector4(0, 4, 90, 3);
                Vector4 V3 = new Vector4(0, 3, 270, 0);
                Vector4 V4 = new Vector4(5, 6, 0, 6);
                Vector4 V5 = new Vector4(5, 5, 0, 7);
                Vector4 V6 = new Vector4(5, 4, 0, 8);
                Vector4 V7 = new Vector4(0, 1, 0, 4);
                Vector4 V8 = new Vector4(0, 0, 0, 5);
                V4s.Add(V);
                V4s.Add(V1);
                V4s.Add(V2);
                V4s.Add(V3);
                V4s.Add(V4);
                V4s.Add(V5);
                V4s.Add(V6);
                V4s.Add(V7);
                V4s.Add(V8);
                break;
            case 1:
                break;
        }
        return V4s;
    }

    public void GenerateRandomLevel()
    {
        RNG = new System.Random(ThreadSafeRandom.Next());
        string[] RandLevel=new string[7];
        int[] PegCounter = new int[3] { 0,0,0};
        List<Vector2> PegsPos = new List<Vector2> { };
        List<Vector2> ModsPos= new List<Vector2> { };
        List<Vector2> StartPos = new List<Vector2> { };
        for (int i=0;i<7;i++)
        {
            RandLevel[i] = "";
            for(int j=0;j<7;j++)
            {
                int r = RNG.Next(25);
                if (r == 11 || r == 12)
                    r = 5;
                else if (r > 5)
                    r = 0;
                RandLevel[i] = RandLevel[i] + r.ToString();
                if (r == 1 || r == 2 || r == 3)
                {
                    PegCounter[r - 1]++;
                }
                else if (r == 0)
                    StartPos.Add(new Vector2(j, i));
                if(r!=4)
                    PegsPos.Add(new Vector2(j, i));
                if(r==0||r==5)
                    ModsPos.Add(new Vector2(j, i));
            }
        }
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<PegCounter[i];j++)
            {
                int r = RNG.Next(PegsPos.Count);
                Vector4 v = new Vector4(PegsPos[r].x, PegsPos[r].y, 0, i + 6);
                GC.CurMods.Add(v);
                PegsPos.RemoveAt(r);
            }
        }
        PegsPos.Clear();
        
        for(int i=0;i<4;i++)
        {
            //place attachments
            int r = RNG.Next(ModsPos.Count);
            GC.CurMods.Add(new Vector4(ModsPos[r].x, ModsPos[r].y, RNG.Next(4)*90, i));
            ModsPos.RemoveAt(r);
        }
        int roCount = RNG.Next(7, 13);
        for(int i=0;i<roCount;i++)
        {
            int r = RNG.Next(ModsPos.Count);
            GC.CurMods.Add(new Vector4(ModsPos[r].x, ModsPos[r].y, 0, RNG.Next(4,6)));
            ModsPos.RemoveAt(r);
        }
        int R = RNG.Next(StartPos.Count);
        string temp = RandLevel[(int)StartPos[R].x];
        RandLevel[(int)StartPos[R].x] = "";
        for (int i=0;i<7;i++)
        {
            if (i != (int)StartPos[R].y)
            {
                RandLevel[(int)StartPos[R].x] = RandLevel[(int)StartPos[R].x] + temp[i].ToString();
            }
            else
                RandLevel[(int)StartPos[R].x] = RandLevel[(int)StartPos[R].x] + "6";
        }
        GC.CurLevel = RandLevel;
        GC.CreateLevel();
    }

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
