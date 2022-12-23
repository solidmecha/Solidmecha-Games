using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {
    public static GameScript singleton;
    public List<BlockScript> Blocks;
    public float Timer;
    public float TickDelay;
    public float NewPieceDelay;
    Vector2[] Directions = new Vector2[8] { Vector2.up, Vector2.left, Vector2.right, Vector2.down, new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };
    public GameObject[] BlockPrefabs;
    public System.Random RNG;
    public GameObject SpawnIndicator;
    public bool inRotation;
    public int TargetRotation;
    public int NextBlockID;
    int Score;
    public Text ScoreText;
    public bool GameModeTA;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
    }

    // Use this for initialization
    void Start() {
    }

    public void KOSetUp()
    {
        int KOindex = RNG.Next(-6, 7);
        for (int x = -6; x < 7; x++)
        {
            int y = 12;
            while (y > -1)
            {
                if (y != 12 || x != KOindex)
                    MakeBlock(new Vector2(x, y - 6), RNG.Next(4));
                y--;
            }
        }
        PreMatchClears();
        Destroy(ScoreText.transform.parent.GetChild(7).gameObject);
        Destroy(ScoreText.transform.parent.GetChild(6).gameObject);
    }

    public void TASetup()
    {
        GameModeTA = true;
        SetNextBlock();
        for (int x = -6; x < 7; x++)
        {
            int y = RNG.Next(1, 10);
            while (y > -1)
            {
                MakeBlock(new Vector2(x, y - 6), RNG.Next(4));
                y--;
            }
        }
        PreMatchClears();
        GetComponent<TimerScript>().StartedTimer=true;
        Destroy(ScoreText.transform.parent.GetChild(7).gameObject);
        Destroy(ScoreText.transform.parent.GetChild(6).gameObject);

    }

    void UpdateScore(int change)
    {
        Score += change;
        ScoreText.text = "Score:\n" + Score.ToString();
    }

    void PreMatchClears()
    {
        bool hasMatches = true;
        while (hasMatches)
        {
            HandleMatches();
            List<BlockScript> MatchedBlocks = new List<BlockScript> { };
                for (int i = Blocks.Count - 1; i > -1; i--)
            {
                if (Blocks[i].Matched)
                {
                    MatchedBlocks.Add(Blocks[i]);
                    Blocks.RemoveAt(i);
                }
            }
            if (MatchedBlocks.Count == 0)
                hasMatches = false;
            else
            {
                foreach(BlockScript b in MatchedBlocks)
                {
                    Vector2 loc = b.transform.position;
                    Destroy(b.gameObject);
                    MakeBlock(loc, RNG.Next(4));
                }
            }
        }
    }

    void SetNextBlock()
    {
        NextBlockID = RNG.Next(4);
        SpawnIndicator.GetComponent<SpriteRenderer>().sprite=BlockPrefabs[NextBlockID].GetComponent<SpriteRenderer>().sprite;
    }

    public void Rotate(int Angle)
    {
        if(!inRotation)
        {
            Invoke("ResetInRotation", .5f);
            inRotation = true;
            Timer += .5f;
            TargetRotation += Angle;
            if (TargetRotation >= 360)
                TargetRotation -= 360;
            else if (TargetRotation <= -360)
                TargetRotation += 360;
            gameObject.AddComponent<RotationScript>().RotationSpeed=Angle*2;
            foreach (BlockScript B in Blocks)
                B.gameObject.AddComponent<RotationScript>().RotationSpeed = Angle * -2;
        }
    }

    public void ResetInRotation()
    {
        inRotation = false;
        Destroy(GetComponent<RotationScript>());
        transform.rotation=Quaternion.Euler(new Vector3(0, 0, TargetRotation));
        foreach (BlockScript B in Blocks)
        {
            Destroy(B.gameObject.GetComponent<RotationScript>());
            B.transform.localRotation=Quaternion.Euler(new Vector3(0, 0, -1 * TargetRotation));
            B.transform.localPosition = new Vector2(Mathf.RoundToInt(B.transform.localPosition.x), Mathf.RoundToInt(B.transform.localPosition.y));
        }
    }

    public void MakeBlock(Vector2 location, int BlockIndex)
    {
            GameObject g = Instantiate(BlockPrefabs[BlockIndex], location, Quaternion.identity) as GameObject;
            g.transform.parent = transform;
            Blocks.Add(g.GetComponent<BlockScript>());
    }

    public void SpawnBlock()
    {
        if (GameModeTA && NewPieceDelay <= 0 && !inRotation && Physics2D.Raycast((Vector2)SpawnIndicator.transform.position+Vector2.down, Vector2.zero).collider == null)
        {
            NewPieceDelay = .6f;
            SpawnIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            MakeBlock((Vector2)SpawnIndicator.transform.position + Vector2.down, NextBlockID);
            SetNextBlock();
        }
    }

    void GameTick()
    {
        MoveTilesDown();
        HandleMatches();
        FadeOutMatches();
    }
	
    void MoveTilesDown()
    {
        foreach(BlockScript B in Blocks)
        {
            if (isFalling(B.transform.position))
                B.transform.position = (Vector2)B.transform.position + Vector2.down;
        }
    }

    bool isFalling(Vector2 BlockStart)
    {
        int Row = 1;
        bool checking = true;
        while (checking)
        {
            RaycastHit2D hit = Physics2D.Raycast(BlockStart + Vector2.down * Row, Vector2.zero);
            if (hit.collider == null)
                return true;
            else if (hit.collider.CompareTag("Ground"))
                return false;
            else
                Row++;
        }
        return false;
    }

    void HandleMatches()
    {
        foreach(BlockScript B in Blocks)
        {
            if(!B.Matched)
            {
                for (int i = 0; i < 8; i++)
                {
                    bool checkingDirection = true;
                    int Magnitude = 1;
                    List<BlockScript> PossibleMatches = new List<BlockScript> { B };
                    while (checkingDirection)
                    {
                        RaycastHit2D hit = Physics2D.Raycast((Vector2)B.transform.position + Directions[i] * Magnitude, Vector2.zero);
                        if (hit.collider != null && !hit.collider.CompareTag("Ground"))
                        {
                            if (hit.collider.GetComponent<BlockScript>().ID == B.ID)
                            {
                                PossibleMatches.Add(hit.collider.GetComponent<BlockScript>());
                                if (PossibleMatches.Count == 4)
                                {
                                    if (GameModeTA)
                                    {
                                        UpdateScore(GetComponent<BonusScript>().BonusTotal() + 45);
                                        GetComponent<BonusScript>().ShowBonus(3);
                                        GetComponent<BonusScript>().ColorClears[B.ID] = true;
                                        GetComponent<BonusScript>().PrismaticCheck();
                                    }
                                    foreach (BlockScript m in PossibleMatches)
                                        m.Matched = true;
                                }
                                else if (PossibleMatches.Count > 4)
                                {
                                    if (GameModeTA)
                                    {
                                        UpdateScore(15);
                                    }
                                        hit.collider.GetComponent<BlockScript>().Matched = true;
                                }
                                Magnitude++;
                                if(Magnitude==5 && GameModeTA)
                                {
                                    GetComponent<BonusScript>().ShowBonus(2);
                                }
                            }
                            else
                                checkingDirection = false;
                        }
                        else
                        {
                            checkingDirection = false;
                        }
                    }
                }
            }
        }
    }

    void FadeOutMatches()
    {
        for (int i = Blocks.Count - 1; i > -1; i--)
        {
            if (Blocks[i].Matched)
            {
                Blocks[i].gameObject.AddComponent<FadeOut>();
                Blocks.RemoveAt(i);
                if(GameModeTA && Blocks.Count<=25)
                {
                    GetComponent<BonusScript>().ShowBonus(0);
                }
                if(Blocks.Count==0)
                {
                    UpdateScore(25000);
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        Timer -= Time.deltaTime;
        if (NewPieceDelay >= 0)
        {
            NewPieceDelay -= Time.deltaTime;
            if(NewPieceDelay <=0)
            {
                SpawnIndicator.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
            }
        }

        if(Timer<=0)
        {
            GameTick();
            Timer = TickDelay;
        }
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	}
}
