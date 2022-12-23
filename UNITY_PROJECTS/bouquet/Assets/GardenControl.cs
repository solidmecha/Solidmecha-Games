using UnityEngine;
using System.Collections.Generic;

public class GardenControl : MonoBehaviour {

    public static GardenControl singleton;
    public Sprite[] LargeBulbs;
    public Sprite[] SmallBulbs;
    public Sprite[] MidBulbs;
    public Sprite Tulip;
    public Color[] Colors;
    public GameObject Flower;
    public System.Random RNG;
    public Sprite[] Leafs;
    List<int> ColorOrder;
    List<int> BulbOrder;
    List<int>[] LeafOrders=new List<int>[4];
    bool BackLeafDominant;
    List<GameObject> Flowers=new List<GameObject> { };
    public GameObject ActiveFlower;
    public GameObject Triangle;
    public GameObject tile;
    public bool isMoreRandom;
    List<int[]> StarterFlowers= new List<int[]> { };

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        RNG = new System.Random();
        BackLeafDominant = RNG.Next(2) == 1;
        ColorOrder = new List<int> { };
        RandomList(ColorOrder, 9);
        BulbOrder = new List<int> { };
        RandomList(BulbOrder, 4);

        for(int i=0;i<4;i++)
        {
            LeafOrders[i] = new List<int> { };
            RandomList(LeafOrders[i],3);
        }

        for(int x=0;x<4;x++)
        {
            for (int y = 0; y < 4; y++)
                GenerateFlower(new Vector2(x + 98, y + 98));
        }
        RandomPairing();
        CleanUp();
      
	}




    void NextGeneration()
    {
        foreach (TileScript t in GetComponentsInChildren<TileScript>())
            t.HandleCross();
        CleanUp();
    }

    void CleanUp()
    {
        for(int i=Flowers.Count-1;i>-1;i--)
        {
            if(Flowers[i].GetComponent<FlowerScript>().PickFlower)
            {
                Destroy(Flowers[i]);
                Flowers.RemoveAt(i);
            }
            else
            {
                if (Flowers[i].GetComponent<FlowerScript>().isNew)
                {
                    Flowers[i].GetComponent<FlowerScript>().isNew = false;
                    Flowers[i].transform.position = (Vector2)Flowers[i].transform.position - new Vector2(100, 100);
                }
            }
        }
    }

    void RandomList(List<int> ListR, int max)
    {
        List<int> temp = new List<int> { };
        for(int x=0;x<max;x++)
        {
            temp.Add(x);
        }
        for(int i=0; i<max; i++)
        {
            int r = RNG.Next(temp.Count);
            ListR.Add(temp[r]);
            temp.RemoveAt(r);
        }
    }

    void RandomPairing()
    {
        List<int> pairIndex=new List<int> { };
        RandomList(pairIndex, 16);
        for(int i=0;i<8;i++)
        {
            CrossFlowers(Flowers[pairIndex[i]].GetComponent<FlowerScript>(), Flowers[pairIndex[i + 8]].GetComponent<FlowerScript>(), (Vector2)transform.position + new Vector2(200, 200));
        }

        pairIndex.Clear();
        RandomList(pairIndex, 8);
        for(int i=0;i<4;i++)
        {
          CrossFlowers(Flowers[16+pairIndex[i]].GetComponent<FlowerScript>(), Flowers[16+pairIndex[i + 4]].GetComponent<FlowerScript>(), new Vector2(3, -.75f+.75f*i));
          Destroy(Flowers[Flowers.Count - 1].GetComponent<BoxCollider2D>());
          Flowers.RemoveAt(Flowers.Count - 1);
        }
       
    }

    public void CrossFlowers(FlowerScript FM, FlowerScript FF, Vector2 Loc)
    {
        GameObject F = Instantiate(Flower, Loc, Quaternion.identity) as GameObject;
        FlowerScript FC = F.GetComponent<FlowerScript>();
        int MBulbIndex=FindIndex(BulbOrder, FM.FlowerID[0]);
        int MColorIndex = FindIndex(ColorOrder, (FM.FlowerID[1] % 9));
        int FBulbIndex= FindIndex(BulbOrder, FF.FlowerID[0]);
        int FColorIndex = FindIndex(ColorOrder, (FF.FlowerID[1] % 9));
        if (isMoreRandom)
        {
            for (int i = 0; i < 6; i++)
            {
                if(RNG.Next(3)==0)
                {
                    if (i == 0)
                        FC.FlowerID[i] = RNG.Next(BulbOrder.Count);
                    else if (i == 1)
                    {
                        FC.FlowerID[i] = RNG.Next(ColorOrder.Count);
                        if (RNG.Next(2) == 0)
                            FC.FlowerID[i] += 9;
                    }
                    else if (i > 1)
                        FC.FlowerID[i] = RNG.Next(3);
                }
                else if (RNG.Next(2) == 0)
                    FC.FlowerID[i] = FF.FlowerID[i];
                else
                    FC.FlowerID[i] = FM.FlowerID[i];
            }
            FlowerSetup(FC);
            Flowers.Add(F);
            return;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (FindIndex(LeafOrders[i], FM.FlowerID[2 + i]) >= FindIndex(LeafOrders[i], FF.FlowerID[2 + i]))
                {
                    FC.FlowerID[2 + i] = FM.FlowerID[2 + i];
                }
                else
                {
                    FC.FlowerID[2 + i] = FF.FlowerID[2 + i];
                }
            }

            if ((MBulbIndex >= FBulbIndex && (FBulbIndex != 0 || MBulbIndex != BulbOrder.Count - 1)) || (MBulbIndex == 0 && FBulbIndex == BulbOrder.Count - 1))
            {
                FC.FlowerID[0] = BulbOrder[MBulbIndex];
            }
            else
            {
                FC.FlowerID[0] = BulbOrder[FBulbIndex];
            }

            if ((MColorIndex >= FColorIndex && (FColorIndex != 0 || MColorIndex != ColorOrder.Count - 1)) || (MColorIndex == 0 && FColorIndex == ColorOrder.Count - 1))
            {
                if (((BackLeafDominant && (FM.FlowerID[1] > 8 || FF.FlowerID[1] > 8)) || (FM.FlowerID[1] > 8 && FF.FlowerID[1] > 8)) && FC.FlowerID[0] < 3)
                {
                    FC.FlowerID[1] = ColorOrder[MColorIndex] + 9;
                }
                else
                    FC.FlowerID[1] = ColorOrder[MColorIndex];
            }
            else
            {
                if (((BackLeafDominant && (FM.FlowerID[1] > 8 || FF.FlowerID[1] > 8)) || ((FM.FlowerID[1] > 8 && FF.FlowerID[1] > 8)) && FC.FlowerID[0] < 3))
                {
                    FC.FlowerID[1] = ColorOrder[FColorIndex] + 9;
                }
                else
                    FC.FlowerID[1] = ColorOrder[FColorIndex];
            }

            FlowerSetup(FC);
            Flowers.Add(F);
        }
    }

    int FindIndex(List<int> L, int I)
    {
    for (int i = 0; i < L.Count; i++)
        if (L[i] == I)
            return i;
    return -1;
    }

    private void Reset()
    {
        foreach (GameObject G in Flowers)
            Destroy(G);
        Flowers.Clear();
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                GameObject F=Instantiate(Flower, new Vector2(x + 98, y + 98), Quaternion.identity) as GameObject;
                F.GetComponent<FlowerScript>().FlowerID = StarterFlowers[x * 4 + y];
                FlowerSetup(F.GetComponent<FlowerScript>());
                Flowers.Add(F);
            }
        }
        CleanUp();

    }

    void FlowerSetup(FlowerScript F)
    {
        switch (F.FlowerID[0])
        {
            case 0:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = LargeBulbs[F.FlowerID[1]];
                break;
            case 1:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = SmallBulbs[F.FlowerID[1]];
                break;
            case 2:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = MidBulbs[F.FlowerID[1]];
                break;
            case 3:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = Tulip;
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Colors[F.FlowerID[1]%9];
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            if (F.FlowerID[2 + i]>0)
            {
                F.transform.GetChild(0).GetChild(1 + i).GetComponent<SpriteRenderer>().sprite = Leafs[F.FlowerID[2 + i]-1];
            }
        }

    }

    void GenerateFlower(Vector2 loc)
    {
        GameObject F = Instantiate(Flower, loc, Quaternion.identity) as GameObject;
        Flowers.Add(F);
        FlowerScript FS = F.GetComponent<FlowerScript>();
        int r = RNG.Next(LargeBulbs.Length);
        FS.FlowerID[1] = r;
        switch (RNG.Next(4))
        {
            case 0:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = LargeBulbs[r];
                FS.FlowerID[0] = 0;
                break;
            case 1:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = SmallBulbs[r];
                FS.FlowerID[0] = 1;
                break;
            case 2:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = MidBulbs[r];
                FS.FlowerID[0] = 2;
                break;
            case 3:
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = Tulip;
                if (r >= 9)
                    r -= 9;
                FS.FlowerID[1] = r;
                F.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Colors[r];
                FS.FlowerID[0] = 3;
                break;
        }

        for(int i=0;i<4;i++)
        {
            if (RNG.Next(3) == 0)
            {
                int l = RNG.Next(Leafs.Length);
                F.transform.GetChild(0).GetChild(1 + i).GetComponent<SpriteRenderer>().sprite = Leafs[l];
                FS.FlowerID[2 + i] = l+1;
            }
            else
            {
                FS.FlowerID[2 + i] = 0;
            }
        }
        StarterFlowers.Add(FS.FlowerID);
    }

    void Fill()
    {
        foreach (GameObject G in Flowers)
            Destroy(G);
        Flowers.Clear();
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
                GenerateFlower(new Vector2(x + 98, y + 98));
        }
        CleanUp();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            NextGeneration();
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
                ActiveFlower = hit.collider.gameObject;
            else if (ActiveFlower != null)
            {
                Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ActiveFlower.transform.position = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
            Reset();
        else if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Fill();
            isMoreRandom = !isMoreRandom;
        }

	}
}
