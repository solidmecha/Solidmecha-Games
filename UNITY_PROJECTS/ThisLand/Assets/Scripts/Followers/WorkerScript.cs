using UnityEngine;
using System.Collections;

public class WorkerScript : MonoBehaviour {

    System.Random RNG;
    public Sprite Alt;
    float lifeTime;
    public float lunchTime;
    float counter;
    float moveCounter;
    public float moveDelay;
    Vector2 MoveVec;
    Vector2 InitialVec;
    bool isMoving;
    GameControl GC;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Enemy(Clone)"))
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
        RNG = new System.Random(ThreadSafeRandom.Next());
        lifeTime = RNG.Next(12, 20);
        if(RNG.Next(2)==1)
        {
            GetComponent<SpriteRenderer>().sprite = Alt;
        }
	}

    void Collect()
    {
        Vector2 Pos = transform.position - GC.gameObject.transform.position;
        Pos = new Vector2(Mathf.Round(Pos.x), Mathf.Round(Pos.y));
        if(GC.World[(int)Pos.x][(int)Pos.y].NaturalRes.Count >0)
        {
            GameObject go = GC.World[(int)Pos.x][(int)Pos.y].NaturalRes[0];
            GC.World[(int)Pos.x][(int)Pos.y].NaturalRes.Remove(GC.World[(int)Pos.x][(int)Pos.y].NaturalRes[0]);
            NatResScript nrs = (NatResScript)go.GetComponent(typeof(NatResScript));
            GC.ResourceAmount[nrs.id] += RNG.Next(1, 16);
            GC.UpdateGUI();
            Destroy(go);
        }
        else if(GC.World[(int)Pos.x][(int)Pos.y].Building != null)
        {
            switch(GC.World[(int)Pos.x][(int)Pos.y].Building.name)
            {
                case "Field(Clone)":
                    GrowScript GS = (GrowScript)GC.World[(int)Pos.x][(int)Pos.y].Building.GetComponent(typeof(GrowScript));
                    GS.Harvest();
                    break;
                case "Rax(Clone)":
                    Instantiate(GC.Soldier, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    break;
                case "Church(Clone)":
                    lifeTime += 12;
                    lunchTime++;
                    break;
            }
        }
    }

    void ChangeLand()
    {
        Vector2 Pos = transform.position - GC.gameObject.transform.position;
        Pos = new Vector2(Mathf.Round(Pos.x), Mathf.Round(Pos.y));
         TileScript ts = (TileScript)GC.World[(int)Pos.x][(int)Pos.y].GetComponent(typeof(TileScript));
        if(ts.id>5)
        {
            int r = GC.RNG.Next(6);
            GameObject go = Instantiate(GC.LandTiles[r], Pos+(Vector2)GC.transform.position, Quaternion.identity) as GameObject;
            TileScript tsnew= (TileScript)go.GetComponent(typeof(TileScript)); 
            tsnew.id = r;
            GC.World[(int)Pos.x][(int)Pos.y] = tsnew;
            Destroy(ts.gameObject);
        }
    }

    bool CheckMoveVec(Vector2 v)
    {
        Vector2 target = v + (Vector2)transform.position - (Vector2)GC.transform.position;
        target = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));
        if (target.x < 0 || target.y < 0 || target.x  >= GC.width || target.y >= GC.height)
            return false;
        else
            return true;
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        counter += Time.deltaTime;
        moveCounter += Time.deltaTime;
        if(lifeTime<=0)
        { Destroy(gameObject);}

        if(counter>=lunchTime)
        {
            counter = 0;
            if (GC.ResourceAmount[1] > 0)
            {
                GC.ResourceAmount[1]--;
                GC.UpdateGUI();
            }
            else
                Destroy(gameObject);
        }
        if(moveCounter>=moveDelay && !isMoving)
        {
            MoveVec=new Vector2(RNG.Next(-1, 2), RNG.Next(-1,2));
            InitialVec = transform.position;
            if(!CheckMoveVec(MoveVec))
            { MoveVec = Vector2.zero; }
            isMoving = true;
            moveCounter = 0;
        }

        if (isMoving && moveCounter <= 1)
        {
            transform.position=InitialVec+MoveVec*moveCounter;
        }
        else if(isMoving && moveCounter>=1)
        {
            isMoving = false;
            Collect();
            ChangeLand();
            moveCounter = 0;
        }

	}
}
