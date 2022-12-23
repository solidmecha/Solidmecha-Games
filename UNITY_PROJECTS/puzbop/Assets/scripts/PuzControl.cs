using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzControl : MonoBehaviour {

    public GameObject[] tiles;
    public static PuzControl singleton;
    public System.Random RNG;
    public Text ScoreText;
    public GameObject pointpopUp;
    int Score;
    public GameObject FreezeObj;
    int TargetScore=1000;
    bool FreezeMode;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
    }

    // Use this for initialization
    void Start () {
        SpawnDrop(Vector2.zero + Vector2.up * 4f);
        SpawnDrop(Vector2.right*1.5f+Vector2.up*4f);
        SpawnDrop(Vector2.right * 3f + Vector2.up * 4f);
        SpawnDrop(Vector2.left * 1.5f + Vector2.up * 4f);
        SpawnDrop(Vector2.left * 3f + Vector2.up * 4f);
        Fireworks(Vector2.zero);
        Fireworks(Vector2.zero);
        Fireworks(Vector2.zero);
        Fireworks(Vector2.zero);
    }

    void StartPieces()
    {
    }



    public void SpawnDrop(Vector2 v)
    {
        GameObject go = (Instantiate(tiles[RNG.Next(7)], v, Quaternion.identity) as GameObject);
        go.GetComponent<TileScript>().dropReady = true;
        go.GetComponent<TileScript>().isHandling = true;
        go.GetComponent<Rigidbody2D>().gravityScale = -1f;
    }

   public void HandleCollision(TileScript A, TileScript B)
    {
        if(FreezeMode)
        {
            Fireworks(A.transform.position);
            FreezeMode = RNG.Next(5) == 4;
        }
        else if (A.ID == B.ID - 7)
        {
            Fireworks(A.transform.position);
            Destroy(A.gameObject);
            Destroy(B.gameObject);
        }
        else if (A.ID <7 &&  B.ID < 7)
        {
            Destroy(A.gameObject);
            Destroy(B.gameObject);
        }
        else
        {
            int val = (A.ID + B.ID +1) * 5;
            ScorePoints(val);
            (Instantiate(pointpopUp, A.transform.position, Quaternion.identity, ScoreText.transform.parent) as GameObject).GetComponent<Text>().text = "+" + val.ToString();
            Destroy(B.gameObject);
        }

    }

    void CollisionRules()
    {
        //fireworks
        //bumper
        //static spawn
        //nothing
        //gravity
        //explode
        //stasis
        //points
        //drop
        //clear
        //scale up

    }

    void Fireworks(Vector2 v)
    {
        int c = RNG.Next(4, 9);
        for(int i=0;i<c;i++)
        {
            (Instantiate(tiles[RNG.Next(7, tiles.Length)], v, Quaternion.identity) as GameObject).GetComponent<Rigidbody2D>().AddForce(new Vector2(RNG.Next(-30,31)/60f, RNG.Next(10, 41) / 45f)*340f);
        }
    }

    void UpdateScore()
    {
        ScoreText.text = Score.ToString();
    }

    void ScorePoints(int val)
    {
        Score += val;
        if(Score>=TargetScore)
        {
            Instantiate(FreezeObj);
            (Instantiate(pointpopUp, Vector2.zero, Quaternion.identity, ScoreText.transform.parent) as GameObject).GetComponent<Text>().text = "Freeze!";
            FreezeMode = true;
            TargetScore += (1000*TargetScore/1000);
        }
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
    }
}
