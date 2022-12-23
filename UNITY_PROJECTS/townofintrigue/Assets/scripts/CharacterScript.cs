using UnityEngine;
using System.Collections.Generic;

public class CharacterScript : MonoBehaviour {

    public int CharacterID;
    public new string name;
    public List<string> Statements=new List<string> { };
    public List<int[]> StatementInfo=new List<int[]> { };
    public int[] StatementIndex = new int[5];
   public  List<int> LieIndex=new List<int> { };
    int LieCount;
    public int[] Locations;
    int LieValue;



    private void OnMouseDown()
    {
        WorldManager.Singleton.OutlineTransform.position = transform.position;
        WorldManager.Singleton.ActiveCharID = CharacterID;
        ShowStatements();
    }

    private void OnMouseOver()
    {
     if(Input.GetMouseButtonDown(1))
        {
            if(WorldManager.Singleton.solution[0]==CharacterID)
            {
                WorldManager.Singleton.StatementText[5].text = "It was "+ name + " at " +WorldManager.Singleton.Locations[WorldManager.Singleton.solution[1]]+"!";
                foreach (CharacterScript c in WorldManager.Singleton.Characters)
                {
                    c.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = c.name + " in " + WorldManager.Singleton.Clothes[c.CharacterID];
                    Destroy(c.GetComponent<BoxCollider2D>());
                }
                for (int i = 0; i < 5; i++)
                {
                    WorldManager.Singleton.StatementText[i].text = "";
                }
                WorldManager.Singleton.OutlineTransform.position= new Vector2(100, 100);
                WorldManager.Singleton.ActiveCharID = -1;
            }
            else
            {
                WorldManager.Singleton.VictimID = CharacterID;
                WorldManager.Singleton.OutlineTransform.position = new Vector2(100, 100);
                WorldManager.Singleton.ActiveCharID = -1;
                bool possible=false;
                for(int i=0;i<32;i++)
                {
                    if (Locations[i] > 0 && Locations[i] == WorldManager.Singleton.Characters[WorldManager.Singleton.solution[0]].Locations[i])
                        possible = true;
                }
                if(!possible)
                {
                    for(int i=0;i<32;i++)
                        Locations[i] = WorldManager.Singleton.Characters[WorldManager.Singleton.solution[0]].Locations[i];
                }
                foreach (CharacterScript c in WorldManager.Singleton.Characters)
                {
                    c.StatementInfo.Clear();
                }
                WorldManager.Singleton.Sim();
                foreach (CharacterScript c in WorldManager.Singleton.Characters)
                {
                    c.RerollStatements();
                }
                for (int i = 0; i < 5; i++)
                {
                    WorldManager.Singleton.StatementText[i].text = "";
                }
                transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";
                Destroy(GetComponent<BoxCollider2D>());

            }
        }

    }


    public void ShowStatements()
    {
        for(int i=0;i<5;i++)
        {
            WorldManager.Singleton.StatementText[i].text = Statements[i];
            if (LieIndex.Contains(i))
                WorldManager.Singleton.StatementText[i].color = Color.white;
            else
                WorldManager.Singleton.StatementText[i].color = WorldManager.Singleton.TextColor;
        }
    }

    public string SetStatement(int Cindex, int Lindex, int Tindex, string modifier)
    {
        if (Cindex != CharacterID)
        {
            if (Lindex < 0)
                Lindex *= -1;
            string cn = "";
            if (WorldManager.Singleton.RNG.Next(4) == 2)
                cn = "someone in " + WorldManager.Singleton.Clothes[Cindex];
            else
                cn=WorldManager.Singleton.Characters[Cindex].name;
            return "I saw " + cn + modifier + WorldManager.Singleton.Locations[Lindex] + " at " + WorldManager.Singleton.GetTimeStamp(Tindex);
        }
        else
        {
            string m = " left ";
            if (Tindex > 0 && Locations[Tindex - 1] < 0 && Locations[Tindex] > 0)
            {
                m=" arrived at ";
            }

            return "I"+m+ WorldManager.Singleton.Locations[Lindex]+" at " + WorldManager.Singleton.GetTimeStamp(Tindex);
        }
    }

    int Lie(int i, int max, int restricted)
    {
        int r = r = WorldManager.Singleton.RNG.Next(max);
        while (r == i || r == restricted)
            r = WorldManager.Singleton.RNG.Next(max);
        return r;
    }
        
    public void DetermineLocations()
    {
        List<int> Locs = new List<int> { 1, 2, 3, 4, 5 };
        int[] timeSpent = new int[5];
        int totalTime=0;
        for(int i=0; i<4;i++)
        {
            timeSpent[i] = WorldManager.Singleton.RNG.Next(2, ((20-totalTime)/2)-2);
            totalTime += timeSpent[i];
        }
        timeSpent[4] = 20 - totalTime;
        int Lindex = 0;
        int L = Locs[WorldManager.Singleton.RNG.Next(Locs.Count)];
        for (int i=0;i<5;i++)
        {
            for(int j=0;j<timeSpent[L-1];j++)
            {
                if(Lindex<32)
                Locations[Lindex] = L;
                Lindex++;
            }
            if (i != 4)
            {
                Locs.Remove(L);
                L = Locs[WorldManager.Singleton.RNG.Next(Locs.Count)];
                for (int j = 0; j < WorldManager.Singleton.TravelTimes[L-1]; j++)
                {
                    Locations[Lindex] = -1*L;
                    Lindex++;
                }
            }
            else
            {
                for (int j = Lindex; j < 32; j++)
                {
                    Locations[j] = L;
                }
            }
        }
    }

    public void RerollStatements()
    {
        Statements.Clear();
        LieIndex.Clear();
        DetermineStatements();
    }

    void DetermineStatements()
    {
        List<int> temp = new List<int> { };
        LieCount = WorldManager.Singleton.RNG.Next(1, 4);
        List<int> temp5 = new List<int> { 0, 1, 2, 3, 4 };
        for (int i = 0; i < LieCount; i++)
        {
            int r = WorldManager.Singleton.RNG.Next(temp5.Count);
            LieIndex.Add(temp5[r]);
            temp5.RemoveAt(r);
        }
        for(int i=0;i<StatementInfo.Count;i++)
        {
            temp.Add(i);
        }
        for(int i=0;i<5;i++)
        {
            int r = WorldManager.Singleton.RNG.Next(i*(temp.Count/5), ((i+1)*temp.Count)/5);
            StatementIndex[i] = temp[r];
            temp.RemoveAt(r);
            if (!LieIndex.Contains(i))
            {
                Statements.Add(SetStatement(StatementInfo[StatementIndex[i]][0], StatementInfo[StatementIndex[i]][1], StatementInfo[StatementIndex[i]][2], WorldManager.Singleton.ModifierLookup(WorldManager.Singleton.Characters[StatementInfo[StatementIndex[i]][0]], StatementInfo[StatementIndex[i]][2])));
            }
            else
            {
                int[] tempvals = new int[3];
                tempvals[2] = StatementInfo[StatementIndex[i]][2];
                if (LieValue == 0)
                {
                    tempvals[0] = Lie(StatementInfo[StatementIndex[i]][0], 5, CharacterID);
                    tempvals[1] = StatementInfo[StatementIndex[i]][1];

                    int L = WorldManager.Singleton.Characters[i].Locations[tempvals[2]];
                    if ((L > 0 && L - 1 == tempvals[1]) || (L < 0 && L + 1 == tempvals[1]))
                    {
                        tempvals[0] = StatementInfo[StatementIndex[i]][0];
                        LieIndex.Remove(i);
                    }
                }
                else
                {
                    tempvals[0] = StatementInfo[StatementIndex[i]][0];
                    tempvals[1] = Lie(StatementInfo[StatementIndex[i]][1], 5, StatementInfo[StatementIndex[i]][1]);
                }
                Statements.Add(SetStatement(tempvals[0], tempvals[1], tempvals[2], WorldManager.Singleton.ModifierLookup(WorldManager.Singleton.Characters[StatementInfo[StatementIndex[i]][0]], StatementInfo[StatementIndex[i]][2])));

            }
        }
    }

	// Use this for initialization
	void Start () {
        LieValue = WorldManager.Singleton.RNG.Next(2);
        DetermineStatements();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
