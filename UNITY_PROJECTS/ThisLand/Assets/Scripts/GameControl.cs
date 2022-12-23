using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameControl : MonoBehaviour {

    public List<GameObject> LandTiles = new List<GameObject> { };
    public List<GameObject> ResourceList = new List<GameObject> { };
    public int[] ResourceAmount = new int[4]; //coin, food,  stone, wood
    public Text ResourceText;
    public int width;
    public int height;
    public TileScript[][] World;
    public System.Random RNG;
    public GameObject Soldier;
    public float GrainTime;

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        BuildWorld();
        UpdateGUI();
	}

    public void UpdateGUI()
    {
        if(ResourceText != null)
            ResourceText.text = ResourceAmount[0].ToString() + " COIN, " + ResourceAmount[1].ToString() + " FOOD, " + ResourceAmount[2].ToString() + " STONE, " + ResourceAmount[3].ToString() + " WOOD";
    }

    void PlaceResource(int R, Vector2 V, TileScript TS)
    {
            int count=RNG.Next(6);
            for(int i=0;i<count;i++)
            {
             float Xoff = RNG.Next(-24, 25);
             float Yoff = RNG.Next(-18, 17);
            Xoff = Xoff / 100f;
            Yoff = Yoff / 100f;
            GameObject Res = Instantiate(ResourceList[R], V + new Vector2(Xoff, Yoff), Quaternion.identity) as GameObject;
            Res.transform.SetParent(TS.transform);
            TS.NaturalRes.Add(Res);
        }

    }

    void BuildWorld()
    {
        World = new TileScript[width][];
        for (int i=0;i<width;i++)
        {
            World[i] = new TileScript[height];
            for(int j=0;j<height;j++)
            {              
                int R = RNG.Next(LandTiles.Count);
                if (i == 13 && j == 13)
                { R = 0; }
                Vector2 Pos = transform.position + new Vector3(i, j);
                GameObject go=Instantiate(LandTiles[R], Pos, Quaternion.identity) as GameObject;
                World[i][j] = (TileScript)go.GetComponent(typeof(TileScript));
                World[i][j].id = R;
                
                if(R<6) //grass
                {
                    int r = RNG.Next(12);
                    if (i == 13 && j == 13)
                    { r = 9; }
                    if (r<8)
                        PlaceResource(r, Pos, World[i][j]);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
