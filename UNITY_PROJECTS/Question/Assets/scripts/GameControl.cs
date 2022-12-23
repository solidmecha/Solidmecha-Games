using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameControl : MonoBehaviour {

    public GameObject EmptyTile;
    public GameObject SelectedTile;
    public int SelectedTileID;
    public GameObject current_Outline;

    public GameObject Checkpoint;
    public int width;
    public int height;

    public GameObject Player;

    public GameObject InputText;

    public List<GameObject> GameTiles;

    public int[][] GameWorld;

    public bool InBuildMode;

    // Use this for initialization
    void Start () {
        GameWorld = new int[width][];
        for (int i = 0; i < width; i++)
        {
            GameWorld[i] = new int[height];
            for(int j=0;j<height;j++)
            {
                GameWorld[i][j] = 0;
            }
        }

        InBuildMode = true;
        setBuildCamera();
        createGrid();
	
	}

    void setBuildCamera()
    {
        CameraScript CS = (CameraScript)Camera.main.gameObject.GetComponent(typeof(CameraScript));
        CS.GC = this;
        Camera.main.orthographicSize=33;
    }

    void setPlayCamera()
    {

    }

    void createGrid()
    {
        for(int i=0;i< width;i++)
        {
            for(int j=0;j<height;j++)
            {             
                GameObject go= Instantiate(EmptyTile, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity) as GameObject;
                EmptyTileScript ets = (EmptyTileScript)go.transform.GetChild(0).GetComponent(typeof(EmptyTileScript));
                ets.GC = this;
                ets.Loc = new int[2];
                ets.Loc[0] = i;
                ets.Loc[1] = j;
               
            }
        }
        for(int i=0;i<width;i++)
        {
            Instantiate(GameTiles[1], (Vector2)transform.position + new Vector2(-1, i), Quaternion.identity);
            Instantiate(GameTiles[1], (Vector2)transform.position + new Vector2(width, i), Quaternion.identity);
            Instantiate(GameTiles[1], (Vector2)transform.position + new Vector2(i, -1), Quaternion.identity);
            Instantiate(GameTiles[1], (Vector2)transform.position + new Vector2(i, height), Quaternion.identity);
            Instantiate(GameTiles[11], (Vector2)transform.position + new Vector2(-1, i), Quaternion.identity);
            Instantiate(GameTiles[11], (Vector2)transform.position + new Vector2(width, i), Quaternion.identity);
            Instantiate(GameTiles[11], (Vector2)transform.position + new Vector2(i, -1), Quaternion.identity);
            Instantiate(GameTiles[11], (Vector2)transform.position + new Vector2(i, height), Quaternion.identity);
        }

    }

    void createGridFromHash(string Hash)
    {
        List<GameObject> HashedGameObjects=new List<GameObject> { };
        foreach(char C in Hash)
        {
            HashedGameObjects.Add(TileLookup(C));
        }

        int HashPointer=0;

        for(int i=0; i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if(HashedGameObjects[HashPointer] !=null)
                {
                    Instantiate(HashedGameObjects[HashPointer], (Vector2)transform.position + new Vector2(i, j), Quaternion.identity);
                }
                HashPointer++;
            }
        }

    }

    public GameObject TileLookup(char C)
    {
        int i = -1;
        switch (C)
        {
            case 'A':
                i=10;
                break;

            case 'B':
                i=11;
                break;

            case 'C':
                i=12;
                break;
        }
        if (i<0)
            i = Convert.ToInt32(new string(C, 1));
        GameObject GO = GameTiles[i];
        return GO;
    }

    public GameObject PlaceTile(Vector2 v)
    {
        GameObject go = null;
        if (SelectedTile != null)
             go = Instantiate(SelectedTile, v, Quaternion.identity) as GameObject;
        switch (SelectedTileID)
        {

            case 2:
                CheckpointScript CS = (CheckpointScript)go.GetComponent(typeof(CheckpointScript));
                CS.GC = this;
                break;
            //rotate spikes
            case 4:
                go.transform.eulerAngles = new Vector3(0, 0, 180);
                break;

            case 5:
                go.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case 6:
                go.transform.eulerAngles = new Vector3(0, 0, -90);
                break;

            case 13: //player
                if (Player != null)
                    Destroy(Player);
                Player = go;                
                break;
      

            default:
                break;
                
        }

        return go;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P) && Player != null)
        {
           Checkpoint= Instantiate(GameTiles[2], Player.transform.position, Quaternion.identity) as GameObject;
           CheckpointScript cs= (CheckpointScript)Checkpoint.GetComponent(typeof(CheckpointScript));
            cs.GC = this;
            Checkpoint.GetComponent<SpriteRenderer>().color = Color.green;
            InBuildMode = !InBuildMode;
            Camera.main.orthographicSize = 10;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            LevelHashGenerator LHG = new LevelHashGenerator();
            print(LHG.GenerateHash(GameWorld, width, height));
            TextEditor te = new TextEditor();
            te.text = LHG.GenerateHash(GameWorld, width, height);
            te.SelectAll();
            te.Copy();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {

            TextEditor te = new TextEditor();
            te.text = "";
            te.SelectTextStart();
            te.Paste();
            createGridFromHash(te.text);
        }

        }
}
