using UnityEngine;
using System.Collections;

public class Scanner : MonoBehaviour {

    public GameObject tile;
    GameObject[] Tiles=new GameObject[30];
    float counter=0;
    public float Frametime;
    int index=0;
    public int TargetIndex;
    public Color colorp;
    bool win;
    bool playing=true;

	// Use this for initialization
	void Start () {
        Frametime = 1f / FrameControl.singleton.FPS;
        TargetIndex = FrameControl.singleton.RNG.Next(30);
        for (int x=0;x<6;x++)
        {
            for(int y=0;y<5;y++)
            {
               Tiles[x*5+y]=Instantiate(tile, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity, transform) as GameObject;
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!win && playing)
        {
            bool pushFrame = false;
            counter += Time.deltaTime;
            if (counter >= Frametime)
            {
                counter = 0;
                pushFrame = true;
            }
            if (pushFrame)
            {
                int prev = index;
                index = (index + 1) % 30;
                Tiles[prev].GetComponent<SpriteRenderer>().color = Color.white;
                if (index != TargetIndex)
                    Tiles[index].GetComponent<SpriteRenderer>().color = colorp;
                else
                    Tiles[index].GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (index == TargetIndex && Input.anyKeyDown)
            {
                FrameControl.singleton.win();
                Destroy(gameObject);
            }
        }
        if (Input.anyKeyDown)
            playing = !playing;

	}
}
