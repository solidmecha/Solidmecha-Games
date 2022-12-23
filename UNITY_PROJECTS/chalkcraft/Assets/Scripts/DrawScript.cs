using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrawScript : MonoBehaviour {

    public SpriteRenderer CanvasBase;
    public int radius;
    public Vector2 Center;
    public GameObject plane;
    public GameObject VigorRunner;
    bool drawingVigor;
    Vector2 startPos;
    public GameObject Forbid;
    public int BindCount;
    public int ChalklingCount;
    public GameObject[] Chalkling;
    public int ChalkIndex;
    public GameObject CoverTile;
    public bool drawingChalking;
    public GameObject currentlyDrawing;
    public bool OrderGiven;
    public int ChalkAmt;
    public int[] WildIndicies;
    public float WildDelay;
    public float WildCount;
    System.Random rng;
    public Text ChalkText;
    float chalkCounter=1;
    public GameObject EndObj;
    int Score;
    public Transform SelectBox;
    // Use this for initialization
    void Start () {
       rng = new System.Random();
        for (int i = 0; i < rng.Next(3, 6); i++)
        {
            Instantiate(Forbid, new Vector2(rng.Next(-6, -1), rng.Next(2, 5)), Quaternion.Euler(new Vector3(0, 0, rng.Next(181))));
            Instantiate(Forbid, new Vector2(rng.Next(-6, -1), rng.Next(-3, -1)), Quaternion.Euler(new Vector3(0, 0, rng.Next(181))));
            Instantiate(Forbid, new Vector2(rng.Next(2, 7), rng.Next(-3, -1)), Quaternion.Euler(new Vector3(0, 0, rng.Next(181))));
            Instantiate(Forbid, new Vector2(rng.Next(2, 7), rng.Next(2, 5)), Quaternion.Euler(new Vector3(0, 0, rng.Next(181))));
        }
	}

    public void EndGame()
    {
        GameObject go=Instantiate(EndObj) as GameObject;
        go.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Application.LoadLevel(0); });
        go.transform.GetChild(1).GetComponent<Text>().text = "Total Score: "+Score.ToString();
    }

    void DrawWild()
    {
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, rng.Next(4) * 90));
        GameObject Go=Instantiate(Chalkling[WildIndicies[rng.Next(WildIndicies.Length)]], new Vector2(rng.Next(-6, 7), rng.Next(-3, 4)), q) as GameObject;
        Go.transform.localScale *= ((float)BindCount / 3f);
    }

    public void UpdateChalk(int amt)
    {
        ChalkAmt += amt;
        if (amt < 0)
            amt *= -1;
        Score += amt * BindCount;
        if (ChalkAmt > 100)
            ChalkAmt = 100;
        if (ChalkAmt < 0)
            ChalkAmt = 0;
        ChalkText.text = ChalkAmt.ToString();
    }

    /*
    void drawWarding(Vector2 Center)
    {
        float cX, cY;
        cX = Center.x + 5.1f;
        cX = cX / 10.2f;
        cX *= 1024;
        cY = Center.y + 5f;
        cY = cY / 10f;
        cY *= 1024;
        Center = new Vector2(cX, cY);
        int minX, minY, maxX, maxY;
        minX = (int)(Center.x - radius);
        maxX = (int)(Center.x + radius);
        minY = (int)(Center.y - radius);
        maxY = (int)(Center.y + radius);
        for (int i = 0; i < 360; i++)
        {
           float sin= Mathf.Sin(i);
           float cos= Mathf.Cos(i);
           Vector2 v = new Vector2(sin, cos);
           v = v * radius;
           v= Center + v;
            for(int j=0;j<5;j++)
               CanvasBase.sprite.texture.SetPixel((int)v.x+j, (int)v.y+j, Color.white);
        }
        CanvasBase.sprite.texture.Apply();
    } */

    void drawVigor(Vector2 V)
    {
        Vector2 result=  (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)-startPos;
        float a = Mathf.Atan2(result.y, result.x)*Mathf.Rad2Deg;
        VigorRunner =Instantiate(plane, startPos, Quaternion.Euler(0, 180, -1*a)) as GameObject;
        drawingVigor = true;
    }
	
	// Update is called once per frame
	void Update () {
        WildCount += Time.deltaTime;
         if(WildCount>=WildDelay)
        {
            WildCount = 0;
            WildDelay = 10 - BindCount;
            DrawWild();
        }
        chalkCounter -= Time.deltaTime;
        if(chalkCounter<=0)
        {
            UpdateChalk(1);
            chalkCounter = 1;
        }
        /* Vigor*/
        if (Input.GetMouseButtonDown(0))
        {

            //print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //drawWarding(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            RaycastHit2D hit;
            hit=Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null && !OrderGiven)
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (drawingChalking && ChalklingCount<BindCount*3 && ChalkAmt>=Chalkling[ChalkIndex].GetComponent<ChalklingControl>().ChalkCost)
                {
                    if (currentlyDrawing != null)
                    {
                        Destroy(currentlyDrawing.GetComponent<DrawHelper>().Chalkling);
                        Destroy(currentlyDrawing);
                    }
                    GameObject go = Instantiate(CoverTile, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
                    GameObject go1 = Instantiate(Chalkling[ChalkIndex], (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
                    
                    go.transform.GetChild(0).localScale = new Vector3(1 ,go1.GetComponent<ChalklingControl>().CoverOffset,1);
                    go.transform.localPosition = (Vector2)go.transform.localPosition+.5f*Vector2.up* go.transform.GetChild(0).localScale.y;
                    //go.transform.GetChild(0).localPosition = new Vector2(0, -.5f * go.transform.GetChild(0).localScale.y);
                    go.GetComponent<DrawHelper>().DrawTime = Chalkling[ChalkIndex].GetComponent<ChalklingControl>().DrawTime;
                    go.GetComponent<DrawHelper>().Chalkling = go1;
                    currentlyDrawing = go;


                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (!OrderGiven)
            {
                if (!drawingChalking)
                {
                    if (ChalkAmt >= 5)
                    {
                        UpdateChalk(-5);
                        drawVigor(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
            else
                OrderGiven = false;

        }

        if(drawingVigor)
        {
            /* moved to vigorHelper
            Vector2 v = (Vector2)VigorRunner.transform.localScale + Vector2.right * Time.deltaTime*33;
            VigorRunner.transform.localScale = v;
            VigorRunner.GetComponent<Renderer>().material.mainTextureScale=new Vector3(v.x*2/33,.99f);
            */
        }
        
    }
}
