using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    float moveCounter;
    public float moveDelay;
    Vector2 MoveVec;
    Vector2 InitialVec;
    bool isMoving;
    GameControl GC;
    System.Random RNG;

    // Use this for initialization
    void Start () {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
        RNG = new System.Random(ThreadSafeRandom.Next());
	}

    bool CheckMoveVec(Vector2 v)
    {
        Vector2 target = v + (Vector2)transform.position - (Vector2)GC.transform.position;
        target = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));
        if (target.x < 0 || target.y < 0 || target.x >= GC.width || target.y >= GC.height)
            return false;
        else
            return true;
    }

    void ChangeLand()
    {
        Vector2 Pos = transform.position - GC.gameObject.transform.position;
        Pos = new Vector2(Mathf.Round(Pos.x), Mathf.Round(Pos.y));
        TileScript ts = (TileScript)GC.World[(int)Pos.x][(int)Pos.y].GetComponent(typeof(TileScript));
        if (ts.id < 6)
        {
            GameObject go = Instantiate(GC.LandTiles[7], Pos + (Vector2)GC.transform.position, Quaternion.identity) as GameObject;
            TileScript tsnew = (TileScript)go.GetComponent(typeof(TileScript));
            tsnew.id = 7;
            GC.World[(int)Pos.x][(int)Pos.y] = tsnew;
            Destroy(ts.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {

        moveCounter += Time.deltaTime;

        if (moveCounter >= moveDelay && !isMoving)
        {
            MoveVec = new Vector2(RNG.Next(-1, 2), RNG.Next(-1, 2));
            InitialVec = transform.position;
            if (!CheckMoveVec(MoveVec))
            { MoveVec = Vector2.zero; }
            isMoving = true;
            moveCounter = 0;
        }

        if (isMoving && moveCounter <= 1)
        {
            transform.position = InitialVec + MoveVec * moveCounter;
        }
        else if (isMoving && moveCounter >= 1)
        {
            isMoving = false;
            ChangeLand();
            moveCounter = 0;
        }

    }
}
