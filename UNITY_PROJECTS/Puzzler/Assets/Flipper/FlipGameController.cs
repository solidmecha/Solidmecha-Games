using UnityEngine;
using System.Collections.Generic;

public class FlipGameController : MonoBehaviour {

    public GameObject[][] World;

    public Sprite[] Sprites;

    public int Width;
    public int Height;
    public GameObject Tile;
    System.Random RNG;
    public float Delay;

    // Use this for initialization
    void Start () {
        World = new GameObject[Width][];
         RNG = new System.Random();
	    for(int i=0;i<Width;i++)
        {
            World[i] = new GameObject[Height];
            for(int j=0;j<Height;j++)
            {
                GameObject go=Instantiate(Tile, new Vector2(i, j), Quaternion.identity) as GameObject;
                int R1 = RNG.Next(3);
                int R2 = RNG.Next(3);
                go.GetComponent<FlipControl>().Flip_Behavior_ID = R1;
                go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Sprites[R1];
                go.transform.GetChild(1).GetComponent<FlipControl>().Flip_Behavior_ID = R2;
                go.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = Sprites[R2];
                World[i][j] = go;
            }
        }

        for (int i = 0; i < 16; i++)
            Flipem();

        InvokeRepeating("Flipem", 0, Delay);
	}

    void Flipem()
    {
        World[RNG.Next(Width)][RNG.Next(Height)].GetComponent<FlipControl>().Flip();
    }
	
	// Update is called once per frame
	void Update () {
	 
	}
}
