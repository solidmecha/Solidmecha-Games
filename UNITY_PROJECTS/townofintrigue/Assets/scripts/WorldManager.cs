using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour {
    public static WorldManager Singleton;
    public Text[] StatementText;
    public string[] Locations;
    public int[] TravelTimes;
    public List<CharacterScript> Characters;
    public System.Random RNG;
    public int[] solution = new int[3];
    public Color TextColor;
    public int VictimID;
    public int ActiveCharID;
    public Transform OutlineTransform;
    bool happened;
    public int[] currentHint;
    int hintIndex;
    public string[] ClothesOption = new string[6];
    public string[] Clothes = new string[6];

    private void Awake()
    {
        Singleton = this;
        RNG = new System.Random();
        Setup();
    }

    void Setup()
    {
        List<int> co = new List<int> {0, 1, 2, 3, 4, 5};
        for(int i=0;i<6;i++)
        {
            int r = RNG.Next(co.Count);
            Clothes[i] = ClothesOption[co[r]];
            co.RemoveAt(r);
        }
        List<int> times = new List<int> { 1, 2, 3, 4, 5 };
        for (int i = 0; i < 5; i++)
        {
            int r = RNG.Next(times.Count);
            int t = times[r];
            TravelTimes[i] = t;
            times.RemoveAt(r);
        }
        foreach (CharacterScript c in Characters)
        {
            c.DetermineLocations();
        }

        Sim();
    }

    public void Sim()
    {
        List<int[]> Possibilities = new List<int[]> { };

        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < Characters.Count; j++)
            {
                for (int k = j - 1; k > -1; k--)
                {
                    if (Characters[j].Locations[i] == Characters[k].Locations[i])
                    {
                        if ((Characters[j].Locations[i] - 1) >= 0)
                        {
                            if (j == VictimID && (!happened || solution[0] == k))
                                Possibilities.Add(new int[3] { k, Characters[j].Locations[i] - 1, i });
                            else if(k != VictimID)
                                Characters[j].StatementInfo.Add(new int[3] { k, Characters[j].Locations[i] - 1, i });
                        }
                        else if(j != VictimID && k != VictimID)
                            Characters[j].StatementInfo.Add(new int[3] { k, Characters[j].Locations[i] + 1, i });
                    }
                }


                if (i > 0 && Characters[j].Locations[i - 1] < 0 && Characters[j].Locations[i] > 0)
                {
                    Characters[j].StatementInfo.Add(new int[3] { j, Characters[j].Locations[i] - 1, i });
                }
                else if (i < 31 && Characters[j].Locations[i + 1] < 0 && Characters[j].Locations[i] > 0)
                {
                    Characters[j].StatementInfo.Add(new int[3] { j, Characters[j].Locations[i] - 1, i });
                }

                for (int k = j + 1; k < Characters.Count - 1; k++)
                {
                    if (Characters[j].Locations[i] == Characters[k].Locations[i])
                    {
                        if ((Characters[j].Locations[i] - 1) >= 0)
                        {
                            if (j == VictimID && (!happened || solution[0]==k))
                                Possibilities.Add(new int[3] { k, Characters[j].Locations[i] - 1, i });
                            else if(k !=VictimID)
                                Characters[j].StatementInfo.Add(new int[3] { k, Characters[j].Locations[i] - 1, i });
                        }
                        else if(j != VictimID && k != VictimID)
                            Characters[j].StatementInfo.Add(new int[3] { k, Characters[j].Locations[i] + 1, i });
                    }
                }
            }
        }

        if (!happened)
        {
            happened = true;
            int murderIndex = RNG.Next(Possibilities.Count);
            solution[0] = Possibilities[murderIndex][0];
            solution[1] = Possibilities[murderIndex][1];
            solution[2] = Possibilities[murderIndex][2];
            GenerateHint();
        }
        else
        {
            int murderIndex = RNG.Next(Possibilities.Count);
            solution[1] = Possibilities[murderIndex][1];
            solution[2] = Possibilities[murderIndex][2];
            showHint(0);
            currentHint[1] = RNG.Next(1, 7);
            showHint(currentHint[1]);
            currentHint[0] = 0;
        }
    }

    public void NewStateMent()
    {
        if(ActiveCharID>-1)
        {
            Characters[ActiveCharID].RerollStatements();
            Characters[ActiveCharID].ShowStatements();
        }
    }

   public void GenerateHint()
    {
        int a = RNG.Next(7);
        int b = RNG.Next(7);
        while(a==b || currentHint[0]==a || currentHint[1]==a || currentHint[0] == b || currentHint[1] == b)
        {
            a = RNG.Next(7);
            b = RNG.Next(7);
        }
        currentHint[0] = a;
        currentHint[1] = b;
        showHint(a);
        showHint(b);
    }

    void showHint(int Val)
    {
        
        string hint = "";
        switch (Val)
        {
            case 0:
                hint = Characters[VictimID].name + " last seen around " + GetTimeStamp(solution[2]);
                break;

          case 1:
        int[] Lorder = new int[5];
        Lorder[0] = Characters[solution[0]].Locations[0];
        int currentIndex = 0;
        for (int i = 1; i < 32; i++)
        {
            if (Characters[solution[0]].Locations[i] > 0 && Characters[solution[0]].Locations[i] != Lorder[currentIndex])
            {
                currentIndex++;
                Lorder[currentIndex] = Characters[solution[0]].Locations[i];
            }
        }
        int r = RNG.Next(4);
        int b = RNG.Next(1, 5 - r);
        hint = "The Culprit was at " + Locations[Lorder[r] - 1] + " and later visited " + Locations[Lorder[r+b] - 1];
        break;
            case 2:
                r = RNG.Next(5);
                while(solution[1]==r)
                    r = RNG.Next(5);
                hint = "It was not at " + Locations[r];
                break;
            case 3:
                r = RNG.Next(5);
                hint="It takes "+(TravelTimes[r] * 15).ToString() + " min to get to " + Locations[r];
                break;
            case 4:
                r=RNG.Next(6);
                int a = RNG.Next(6);
                while (r == a)
                    a = RNG.Next(6);
                List<int> Sightings = new List<int> { };
                for(int i=0;i<32;i++)
                {
                    if (Characters[r].Locations[i] > 0 && Characters[r].Locations[i] == Characters[a].Locations[i])
                    {
                        Sightings.Add(Characters[r].Locations[i] - 1);
                    }
                }
                
                if (Sightings.Count > 0)
                {
                    int SightingsIndex=RNG.Next(Sightings.Count);
                    hint = "The person in " + Clothes[Characters[r].CharacterID] + " saw the person in " + Clothes[Characters[a].CharacterID] + " at " + Locations[Sightings[SightingsIndex]];
                }
                else
                {
                    hint = "The person in " + Clothes[Characters[r].CharacterID] + " never saw the person in " + Clothes[Characters[a].CharacterID];
                }
                    break;
            case 5:
                r = RNG.Next(6);
                 a = RNG.Next(6);
                while (r == a)
                    a = RNG.Next(6);
                hint = Characters[r].name + " was not wearing " + Clothes[a];
                break;
            case 6:
                a = RNG.Next(6);
                while (solution[0] == a)
                    a = RNG.Next(6);
                hint = "The culprit was not wearing " + Clothes[a];
                break;      
        }
        
        StatementText[5+hintIndex].text = hint;
        hintIndex=(hintIndex+1)%2;
    }


    public string ModifierLookup(CharacterScript C, int index)
    {
        if (C.Locations[index] >= 0)
        {
            if (index > 0 && C.Locations[index - 1] < 0)
            {
                return " arrive at ";
            }
            else if (index < 31 && C.Locations[index + 1] < 0)
            {
                return " leave from ";
            }
            else
                return " at ";
        }
        else
            return " going to ";
    }


    public string GetTimeStamp(int I)
    {
        // return I.ToString();
        string h = ((I / 4)+1).ToString();
        string m = "";
        if (I % 4 != 0)
            m = (15 * (I % 4)).ToString();
        else
            m = ("00");
         
        return h+":"+m;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	}
}
