using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
    public static GameControl singleton;
    public GameObject Character;
    public CharacterScript SelectedChar;
    public int BoostCount;
    public GameObject[] SkillFX;
    public enum AoE_Type { Cross, Diamond, Line, Cone}
    public List<CharacterScript> Characters;
    public System.Random RNG;
    public Sprite[] CharSprites;
    List<int[]> StartPos=new List<int[]> { new int[2] {0,7}, new int[2] { 0, 5 }, new int[2] { 0, 4 }, new int[2] { 0, 2 }, new int[2] { 0, 0},
                                            new int[2] {0,3},  new int[2] { 0, 6 }, new int[2] { 0, 1 }};
    int PassCount;
    int CurrentCharIndex;
    public Text[] CharStat_Text;
    public string[] ClassNames;
    public GameObject Board;
    public GameObject TileOutline;
    public List<GameObject> OutlineTiles =new List<GameObject> { };
    public int TeamSize;
    public GameObject CommandMenu;
    Vector2 CmdMenuOffset = new Vector2(1,0);
    public GameObject BackButton;
    public GameObject BoostButton;
    public GameObject[] StartMenus;
    public GameObject InfoCanvas;
    public enum GameState { Selecting, Playing, Moving, Attacking, Waiting};
    public GameState CurrentState;
    public List<int[]> AttackedSquares=new List<int[]> { };
    public List<int> PCIndices;
    public Sprite[] CharIndicators;
    

    private void Awake()
    {
        CurrentState = GameState.Selecting;
        singleton = this;
        RNG = new System.Random();
    }


    public void CreateCharacter()
    {
        GameObject C=Instantiate(Character) as GameObject;
        int ClassID = RNG.Next(CharSprites.Length);
        CharacterScript CS = C.GetComponent<CharacterScript>();
        CS.ClassID = ClassID;
        CS.Movement = RNG.Next(2, 5);
        CS.Speed = RNG.Next(1, 21);
        C.GetComponent<SpriteRenderer>().sprite = CharSprites[ClassID];
        CS.HP[1] = RNG.Next(400, 901);
        CS.HP[0] = CS.HP[1];
        CS.BP[1] = RNG.Next(3, 6);
        CS.BP[0] = CS.BP[1];
        if (ClassID < 7)
            CS.SkillIDs[0] = 0; //add skills here later maybe
        else
            CS.GetComponent<SkillScript>().ID = 1;//healers
        CS.Evasion = RNG.Next(4, 21);
        CS.Armor = RNG.Next(51);
        CS.CharName = RandomName();
        SelectedChar = CS;
        SkillGenerator.singleton.GenericSkills(C.GetComponent<SkillScript>());
        C.GetComponent<PassivesScript>().PassiveID = RNG.Next(10);
        C.GetComponent<PassivesScript>().SetPassive();
        ShowStats(SelectedChar);
    }

    public void PickChar()
    {
        SelectedChar.PlayerTeam = true;
        Characters.Insert(InitiativeOrderIndex(SelectedChar.Speed), SelectedChar);
        SelectedChar.transform.position = Offset_By_TileXY(StartPos[CurrentCharIndex]);
        SelectedChar.Position[0] = StartPos[CurrentCharIndex][0];
        SelectedChar.Position[1] = StartPos[CurrentCharIndex][1];
        SelectedChar.transform.localScale = Vector3.one * 2;
        CurrentCharIndex++;
        if (CurrentCharIndex < TeamSize)
            CreateCharacter();
        else
        {
            Instantiate(Board);
            BeginBattle();
        }
    }

    public void PassChar()
    {
        Characters.Insert(InitiativeOrderIndex(SelectedChar.Speed), SelectedChar);
        SelectedChar.gameObject.AddComponent<BotScript>();
        SelectedChar.transform.position = Offset_By_TileXY(new int[2] { 7, StartPos[PassCount][1] });
        SelectedChar.Position[0] = 7;
        SelectedChar.Position[1] = StartPos[PassCount][1];
        SelectedChar.transform.localScale = Vector3.one * 2;
        SelectedChar.GetComponent<SpriteRenderer>().flipX = true;
        if(CurrentCharIndex < TeamSize)
           CreateCharacter();
        PassCount++;
        if (PassCount == 8)
        {
            Destroy(StartMenus[1]);
        }
    }

    int InitiativeOrderIndex(int Speed)
    {
        for(int i=0;i<Characters.Count;i++)
        {
            if (Characters[i].Speed < Speed)
            {
                return i;
            }
        }
        return Characters.Count;
    }

    public string RandomName()
    {
        const string constanants = "bcdfghjklmnpqrstvwxyz";
        const string vowels = "aeiou";
        string[] strings = new string[2] { constanants, vowels };
        string N = "";
        int L = RNG.Next(2, 7);
        int L2 = RNG.Next(2, 7);
        if (L < L2)
            L = L2;
        int off = 0;
        if (RNG.Next(10) == 4)
            off = 1;
        for (int i = 0; i < L; i++)
        {
            N += OneOrTwoChar(strings[(i + off) % 2]);
        }
        N = N[0].ToString().ToUpper() + N.Remove(0, 1);
        return N;
    }

    string OneOrTwoChar(string s)
    {
        string n = s[RNG.Next(s.Length)].ToString();
        if (RNG.Next(3) == 2)
        {
            n += s[RNG.Next(s.Length)].ToString();
        }
        return n;
    }

    public void ShowStats(CharacterScript C)
    {
        CharStat_Text[0].text = C.CharName+"\n"+ClassNames[C.ClassID];
        CharStat_Text[1].text = "HP:"+C.HP[0].ToString()+"/"+ C.HP[1].ToString();
        CharStat_Text[2].text = "BP:" + C.BP[0].ToString() + "/" + C.BP[1].ToString();
        CharStat_Text[3].text = "Movement:" + C.Movement.ToString();
        CharStat_Text[4].text = "Initiative:" + C.Speed.ToString();
        CharStat_Text[5].text = "Armor:" + C.Armor.ToString();
        CharStat_Text[6].text = "Evasion:" + C.Evasion.ToString();
        CharStat_Text[7].text=C.GetComponent<SkillScript>().Skill_Name;
        CharStat_Text[8].text="Damage:"+C.GetComponent<SkillScript>().Damage[0].ToString()+ "-"+ C.GetComponent<SkillScript>().Damage[1].ToString();
        CharStat_Text[9].text = "Range:" + C.GetComponent<SkillScript>().Range.ToString();
        CharStat_Text[10].text = C.GetComponent<SkillScript>().Skill_AoE.ToString()+" Boost";
        CharStat_Text[11].text = C.GetComponent<PassivesScript>().Name;
        CharStat_Text[12].text = C.GetComponent<PassivesScript>().Description;
    }

    // Use this for initialization
    void Start () {
        CreateCharacter();
    }

    public void CancelMove()
    {
        CurrentState = GameState.Playing;
        OutlineTileCleanUp();
        StartPos.Clear();
        CommandMenu.transform.position = (Vector2)SelectedChar.transform.position + CmdMenuOffset;
        Destroy(StartMenus[1]);
    }

    public void CancelBoost()
    {
        SelectedChar.BP[0] += BoostCount;
        ShowStats(SelectedChar);
        BoostCount = 0;
        OutlineTileCleanUp();
        StartPos.Clear();
        AttackedSquares.Clear();
        ShowAttack();
        Destroy(StartMenus[0]);
    }

    public void BeginBattle()
    {
        CurrentState = GameState.Waiting;
        if(Characters.Count<13)
        {
            for(int i=Characters.Count; i<13;i++)
            {
                CreateCharacter();
                PassChar();
            }
        }
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            if (Characters[i].PlayerTeam)
            {
                PCIndices.Add(i);
                Characters[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
                Characters[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
        CurrentCharIndex = 0;
        Destroy(StartMenus[0]);
        Destroy(StartMenus[1]);
        SelectedChar = Characters[CurrentCharIndex];
        StartPos.Clear();
        NextTurn();
    }

    public void NextTurn()
    {
        SelectedChar = Characters[CurrentCharIndex];
        SelectedChar.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = CharIndicators[1];
        ShowStats(SelectedChar);
        if (SelectedChar.PlayerTeam)
        {
            if (SelectedChar.HP[0] >= 0)
            {
                CurrentState = GameState.Playing;
                CommandMenu.transform.position = (Vector2)SelectedChar.transform.position + CmdMenuOffset;
                CommandMenu.transform.GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
                CommandMenu.transform.GetChild(0).GetChild(1).GetComponent<Button>().interactable = true;
            }
            else
                EndTurn();
        }
        else
        {
            SimulateTurn();
        }
    }

    public void EndTurn()
    {
        if(SelectedChar.GetComponent<PassivesScript>().PassiveID==9)
        {
            for(int i=-1;i<2;i++)
            {
                for(int j=-1;j<2;j++)
                {
                    if(i != 0 || j!=0)
                    {
                        CharacterScript C = FindCharacterAtPosition(new int[2] { SelectedChar.Position[0] + i, SelectedChar.Position[1] + j });
                        if (C != null)
                        {
                            int min = SelectedChar.GetComponent<SkillScript>().Damage[0];
                            int max = SelectedChar.GetComponent<SkillScript>().Damage[1];
                            SelectedChar.GetComponent<SkillScript>().Damage[0] = 10;
                            SelectedChar.GetComponent<SkillScript>().Damage[1] = 26;
                            SelectedChar.DealDamage(C);
                            SelectedChar.GetComponent<SkillScript>().Damage[0] = min;
                            SelectedChar.GetComponent<SkillScript>().Damage[1] = max;
                        }
                    }
                }
            }
        }
        SelectedChar.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = CharIndicators[0];
        BoostCount = 0;
        AttackedSquares.Clear();
        CommandMenu.transform.position = new Vector2(100, 100);
        CurrentCharIndex++;
        if (CurrentCharIndex == Characters.Count)
            CurrentCharIndex = 0;
        NextTurn();
    }

    public void SimulateTurn()
    {
        // Auto play with random move
        CurrentState = GameState.Waiting;
        if (SelectedChar.HP[0] > 0)
        {
            SelectedChar.GetComponent<BotScript>().TakeTurn();
        }
        else
            EndTurn();//Invoke("EndTurn", .05f);

    }

    public void ShowMoves()
    {
        for(int x=1; x<=SelectedChar.Movement; x++)
        {
                if(SelectedChar.Position[1]+x<8)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0,.9f) + Offset_By_TileXY(new int[2] { 0, x }), Quaternion.identity) as GameObject);
                if(SelectedChar.Position[1]-x>=0)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { 0, -1*x }), Quaternion.identity) as GameObject);
            if (SelectedChar.Position[0] + x < 8)
                OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] {  x,0 }), Quaternion.identity) as GameObject);
            if(SelectedChar.Position[0]-x >=0)
                OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { -1 * x, 0 }), Quaternion.identity) as GameObject);
            if (x < SelectedChar.Movement)
            {
                if(SelectedChar.Position[0]-x>=0 && SelectedChar.Position[1]+x<8)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { -1 * x, x }), Quaternion.identity) as GameObject);
                if (SelectedChar.Position[0] + x < 8 && SelectedChar.Position[1] - x >= 0)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] {  x,-1* x }), Quaternion.identity) as GameObject);
                if (SelectedChar.Position[0] - x >= 0 && SelectedChar.Position[1] - x >= 0)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { -1 * x, -1*x }), Quaternion.identity) as GameObject);
                if (SelectedChar.Position[0] + x < 8 && SelectedChar.Position[1] + x < 8)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { x, x }), Quaternion.identity) as GameObject);
            }
        }
        if (SelectedChar.PlayerTeam)
        {
            StartPos.Clear();
            CurrentState = GameState.Moving;
            StartMenus[1] = Instantiate(BackButton) as GameObject;
            StartMenus[1].transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { CancelMove(); });
            CommandMenu.transform.position = new Vector2(100, 100);
            foreach(GameObject g in OutlineTiles)
            {
                int[] ia = TileXY_By_World_Position(g.transform.position);
                if (FindCharacterAtPosition(ia) == null)
                    StartPos.Add(ia);
            }
        }
    }

    public void ShowAttack()
    {

        for (int x = 1; x <= SelectedChar.GetComponent<SkillScript>().Range; x++)
        {
            if (SelectedChar.Position[1] + x < 8)
                OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { 0, x }), Quaternion.identity) as GameObject);
            if (SelectedChar.Position[1] - x >= 0)
                OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { 0, -1 * x }), Quaternion.identity) as GameObject);
            if (SelectedChar.Position[0] + x < 8)
                OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { x, 0 }), Quaternion.identity) as GameObject);
            if (SelectedChar.Position[0] - x >= 0)
                OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { -1 * x, 0 }), Quaternion.identity) as GameObject);
            if (x < SelectedChar.GetComponent<SkillScript>().Range)
            {
                if (SelectedChar.Position[0] - x >= 0 && SelectedChar.Position[1] + x < 8)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { -1 * x, x }), Quaternion.identity) as GameObject);
                if (SelectedChar.Position[0] + x < 8 && SelectedChar.Position[1] - x >= 0)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { x, -1 * x }), Quaternion.identity) as GameObject);
                if (SelectedChar.Position[0] - x >= 0 && SelectedChar.Position[1] - x >= 0)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { -1 * x, -1 * x }), Quaternion.identity) as GameObject);
                if (SelectedChar.Position[0] + x < 8 && SelectedChar.Position[1] + x < 8)
                    OutlineTiles.Add(Instantiate(TileOutline, (Vector2)SelectedChar.transform.position + new Vector2(0, .9f) + Offset_By_TileXY(new int[2] { x, x }), Quaternion.identity) as GameObject);
            }
        }
        foreach (GameObject g in OutlineTiles)
            g.GetComponent<SpriteRenderer>().color = Color.red;
        if (SelectedChar.PlayerTeam)
        {
            StartPos.Clear();
            CurrentState = GameState.Attacking;
            StartMenus[1] = Instantiate(BackButton) as GameObject;
            StartMenus[1].transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { CancelMove(); });
            CommandMenu.transform.position = new Vector2(100, 100);
            foreach (GameObject g in OutlineTiles)
            {
                int[] ia = TileXY_By_World_Position(g.transform.position);
                StartPos.Add(ia);
            }
        }
    }

    public void ConfirmAttack()
    {
        Destroy(StartMenus[0]);
        OutlineTileCleanUp();
        foreach (int[] ia in AttackedSquares)
        {
            if(inBounds(ia))
                Instantiate(SkillFX[SelectedChar.GetComponent<SkillScript>().FX_ID], Offset_By_TileXY(ia), Quaternion.identity);
            CharacterScript C = FindCharacterAtPosition(ia);
            if (C != null)
            {
                SelectedChar.DealDamage(C);
            }
        }
        CharacterScript Cs = FindCharacterAtPosition(AttackedSquares[0]);
        if(Cs!=null)
        {
            int[] ia = new int[2] { Cs.Position[0] - SelectedChar.Position[0], Cs.Position[1] - SelectedChar.Position[1] };
            if (ia[0] > 0)
                ia[0] = 1;
            else if (ia[0] < 0)
                ia[0] = -1;
            if (ia[1] > 0)
                ia[1] = 1;
            else if (ia[1] < 0)
                ia[1] = -1;
            ia[0] = Cs.Position[0] + ia[0] * SelectedChar.GetComponent<SkillScript>().KnockBack;
            ia[1] = Cs.Position[1] + ia[1] * SelectedChar.GetComponent<SkillScript>().KnockBack;
            if (!(ia[0] < 0 || ia[0] > 7 || ia[0] < 0 || ia[1] > 7))
            {
                CharacterScript block=FindCharacterAtPosition(ia);
                if(block==null)
                {
                    Cs.Position = ia;
                    Cs.transform.position = Offset_By_TileXY(ia);
                }
            }
        }
        AttackedSquares.Clear();
        if(SelectedChar.PlayerTeam)
            CommandMenu.transform.position = (Vector2)SelectedChar.transform.position + CmdMenuOffset;
        CommandMenu.transform.GetChild(0).GetChild(1).GetComponent<Button>().interactable = false;
    }

    public void ShowBoost()
    {
        foreach(int[] ia in AttackedSquares)
            OutlineTiles.Add(Instantiate(TileOutline, Offset_By_TileXY(ia), Quaternion.identity) as GameObject);
        foreach (GameObject g in OutlineTiles)
            g.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void ChangeBoost(int Change)
    {
        BoostCount += Change;
        for (int i = AttackedSquares.Count - 1; i > 0; i--)
        {
            AttackedSquares.RemoveAt(i);
        }
        OutlineTileCleanUp();
        if (BoostCount<=0)
        {
            ShowBoost();
            BoostCount = 0;
            return;
        }
        else if(BoostCount>3)
        {
            BoostCount = 3;
        }

        BoostHelper booster = new BoostHelper();

        switch (SelectedChar.GetComponent<SkillScript>().Skill_AoE)
        {
            case AoE_Type.Cone:
                {
                    int[] AtkOffset = new int[2] {AttackedSquares[0][0]-SelectedChar.Position[0], AttackedSquares[0][1]-SelectedChar.Position[1]};
                    if (AtkOffset[0] > 0 && AtkOffset[1] == 0)
                        booster.ConeForward();
                    else if (AtkOffset[0] < 0 && AtkOffset[1] == 0)
                        booster.ConeBack();
                    else if (AtkOffset[0] == 0 && AtkOffset[1] > 0)
                        booster.ConeUp();
                    else if (AtkOffset[0] == 0 && AtkOffset[1] < 0)
                        booster.ConeDown();
                    else if (AtkOffset[0] > 0 && AtkOffset[1] > 0)
                        booster.ConeUpDiag();
                    else if (AtkOffset[0] > 0 && AtkOffset[1] < 0)
                        booster.ConeDownDiag();
                    else if (AtkOffset[0] < 0 && AtkOffset[1] > 0)
                        booster.ConeBackUPDiag();
                    else
                        booster.ConeBackDownDiag();

                        for (int i = 0; i < BoostCount; i++)
                    {
                        for (int j = 0; j < booster.Cone[i].Count; j++)
                            AttackedSquares.Add(new int[2] { AttackedSquares[0][0] + booster.Cone[i][j][0], AttackedSquares[0][1] + booster.Cone[i][j][1] });
                    }
                }
                break;
            case AoE_Type.Cross:
                {
                    for (int i = 1; i < BoostCount + 1; i++)
                    {
                        for (int j = 0; j < 4; j++)
                            AttackedSquares.Add(new int[2] { AttackedSquares[0][0] + (int)booster.Cross[j].x * i, AttackedSquares[0][1] + (int)booster.Cross[j].y * i });
                    }
                }
                break;
            case AoE_Type.Diamond:
                {
                    for (int i = 1; i < BoostCount + 1; i++)
                    {
                        for (int j = 0; j < 4; j++)
                            AttackedSquares.Add(new int[2] { AttackedSquares[0][0] + (int)booster.Diamond[j].x * i, AttackedSquares[0][1] + (int)booster.Diamond[j].y * i });
                    }
                }     
                break;
            case AoE_Type.Line:
                {
                    int[] LineDir;
                    int[] AtkOffset = new int[2] { AttackedSquares[0][0] - SelectedChar.Position[0], AttackedSquares[0][1] - SelectedChar.Position[1] };
                    if (AtkOffset[0] > 0 && AtkOffset[1] == 0)
                        LineDir = new int[2] { 1, 0 };
                    else if (AtkOffset[0] < 0 && AtkOffset[1] == 0)
                        LineDir = new int[2] { -1, 0 };
                    else if (AtkOffset[0] == 0 && AtkOffset[1] > 0)
                        LineDir = new int[2] { 0, 1};
                    else if (AtkOffset[0] == 0 && AtkOffset[1] < 0)
                        LineDir = new int[2] { 0, -1 };
                    else if (AtkOffset[0] > 0 && AtkOffset[1] > 0)
                        LineDir = new int[2] { 1, 1 };
                    else if (AtkOffset[0] > 0 && AtkOffset[1] < 0)
                        LineDir = new int[2] { 1, -1 };
                    else if (AtkOffset[0] < 0 && AtkOffset[1] > 0)
                        LineDir = new int[2] { -1, 1};
                    else
                        LineDir = new int[2] { -1, -1 };
                    for (int i = 1; i < BoostCount + 1; i++)
                    {
                       AttackedSquares.Add(new int[2] { AttackedSquares[0][0] + LineDir[0] * i, AttackedSquares[0][1] + LineDir[1] * i });
                    }
                }
                break;
            default:
                break;
        }
        ShowBoost();
    }

    public void UpBoost()
    {
        if (BoostCount<3 && SelectedChar.BP[0] > 0)
        {
            SelectedChar.BP[0]--;
            ChangeBoost(1);
            ShowStats(SelectedChar);
        }

    }

    public void DownBoost()
    {
        if (BoostCount > 0)
        {
            ChangeBoost(-1);
            SelectedChar.BP[0]++;
            ShowStats(SelectedChar);
        }
    }

    public void OutlineTileCleanUp()
    {
        foreach (GameObject g in OutlineTiles)
            Destroy(g);
        OutlineTiles.Clear();
    }

    public Vector2 Offset_By_TileXY(int[] Tile_XY)
    {
        return new Vector2(-.75f, .45f) * Tile_XY[1] + new Vector2(.755f, .45f) * Tile_XY[0]+new Vector2(0,-.9f);
    }

    public int[] TileXY_By_World_Position (Vector2 Pos)
    {
        Vector2 p = Pos-new Vector2(0, -1);//subtract origin offset
        int X= Mathf.RoundToInt((p.x / .755f + p.y / .45f) / 2);
        int Y= Mathf.RoundToInt(p.y/.45f - (p.x / .755f)) / 2;
        return new int[2] { X, Y };
    }

    public CharacterScript FindCharacterAtPosition(int[] Pos)
    {
        foreach (CharacterScript C in Characters)
            if (C.Position[0] == Pos[0] && C.Position[1] == Pos[1])
                return C;
        return null;
    }

    public int TileDistance(int[] pos1, int[]pos2)
    {
        return Mathf.Abs(pos2[0] - pos1[0])
                + Mathf.Abs(pos2[1] - pos1[1]);
    }

    public bool inBounds(int[] pos)
    {
        return (pos[0] >= 0 && pos[0] < 8 && pos[1] >= 0 && pos[1] < 8);
    }

    public bool canMove(int[] pos)
    {
        return (IAcontains(pos)  && inBounds(pos));
    }

    public bool IAcontains(int[] ia)
    {
        foreach(int[] ar in StartPos)
        {
            if (ar[0] == ia[0] && ar[1] == ia[1])
                return true;
        }
        return false;
    }

    public bool canAttack(int[] pos)
    {
        return (IAcontains(pos) && inBounds(pos));
    }
	
	// Update is called once per frame
	void Update () {
	if(Input.GetMouseButtonDown(0))
        {
            if (SelectedChar != null)
            {
                if (GameState.Selecting==CurrentState)
                {
                    int[] MouseTile = TileXY_By_World_Position(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    SelectedChar.transform.position = Offset_By_TileXY(new int[2] { MouseTile[0], MouseTile[1] });
                }
                else if (CurrentState==GameState.Moving)
                {
                    int[] MouseTile = TileXY_By_World_Position(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (canMove(MouseTile))
                    {
                        CurrentState = GameState.Playing;
                        OutlineTileCleanUp();
                        SelectedChar.Position[0] = MouseTile[0];
                        SelectedChar.Position[1] = MouseTile[1];
                        SelectedChar.transform.position = Offset_By_TileXY(new int[2] { MouseTile[0], MouseTile[1] });
                        CommandMenu.transform.position = (Vector2)SelectedChar.transform.position + CmdMenuOffset;
                        CommandMenu.transform.GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
                        Destroy(StartMenus[1]);
                    }
                }
                else if(CurrentState==GameState.Attacking)
                {
                    int[] MouseTile = TileXY_By_World_Position(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (canAttack(MouseTile))
                    {
                        CurrentState = GameState.Playing;
                        Destroy(StartMenus[1]);
                        StartMenus[0] = Instantiate(BoostButton);
                        OutlineTileCleanUp();
                        OutlineTiles.Add(Instantiate(TileOutline, Offset_By_TileXY(MouseTile), Quaternion.identity) as GameObject);
                        OutlineTiles[0].GetComponent<SpriteRenderer>().color = Color.red;
                        AttackedSquares.Add(MouseTile);
                    }
                }
            }
        }
	}
}
