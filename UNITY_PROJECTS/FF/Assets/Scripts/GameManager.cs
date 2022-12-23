using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int Plat;
    public Text Plat_Amt;
    public Text Message;
    public GameObject progress;
    public GameObject CurrentUIBox;
    public int[] ShipCosts = new int[5];
    public int[] BuildingCosts = new int[6];
    public int[] ModuleCosts = new int[5];
    public float[] ShipbuildTime = new float[5];
    public float[] constructionTime = new float[6];
    public ushort GameState; //Pan, Select, Order
    Vector2 OriginPoint=Vector2.zero;
    Vector3 CamDifference;
    Vector3 CamRefPoint;
    bool Drag = false;
    public List<GameObject> CurrentSelection;
    public GameObject CurrentUIWindow; //TODO add this
    public GameObject Resource;
    public List<GameObject> Buildings = new List<GameObject> { };
    bool showSelectionBox;
    GameObject[] box = new GameObject[4];
    public GameObject SelectionBoxSprite;
    public List<GameObject> Ships = new List<GameObject> { };
    public List<GameObject> Turrets = new List<GameObject> { };
    public List<GameObject> CapitalMods = new List<GameObject> { };
    public GameObject CSDisplay;
    int EnemyBaseCount;
    public GameObject EnemyBase;

    // Use this for initialization
    void Start()
    {
        EnemyBaseCount = 1;
        UpdateGUI();
    }

    public void InvokeEnemyBase()
    {
        Invoke("BuildEnemyBase", 15);
        EnemyBaseCount++;
    }

    void BuildEnemyBase()
    {
        GameObject go = Instantiate(EnemyBase) as GameObject;
        HealthScript hs = (HealthScript)go.GetComponent(typeof(HealthScript));
        hs.hp = 1337 * EnemyBaseCount;
        hs.maxHp= 1337 * EnemyBaseCount;
    }

    public void ShowMessage(string s)
    {
        if(IsInvoking())
        {
            CancelInvoke();
        }
        Message.text = s;
        Invoke("ClearMessage", 3.5f);
    }

    void ClearMessage()
    {
        Message.text = "";
    }

    public bool checkPlat(int P)
    {
        if (P <= Plat)
            return true;
        else
            ShowMessage("Need " + P.ToString() + " Platinum");
        return false;
    }


    public void UpdateGUI()
    {
        Plat_Amt.text = Plat.ToString();
    }
    void moveSelection()
    {
        Vector2 movePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = 0;
        float y =0;
        foreach (GameObject g in CurrentSelection)
        {
            if (g != null)
            {
                x += g.transform.position.x;
                y += g.transform.position.y;
            }
        }
        Vector2 AveragePos = new Vector2(x / CurrentSelection.Count, y / CurrentSelection.Count);
        Vector2 DiffPos = movePos - AveragePos;
        foreach (GameObject g in CurrentSelection)
        {
            if (g != null)
            {
                UnitScript us = (UnitScript)g.GetComponent(typeof(UnitScript));
                us.move(DiffPos);
            }
        }

    }
    public void Select(GameObject other)
    {
        CurrentSelection.Add(other.gameObject);
        other.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void Deselect(GameObject other)
    {
        CurrentSelection.Remove(other.gameObject);
        if(other != null)
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameState==2)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (!hit || hit.collider.CompareTag("enemy"))
                moveSelection();
        }
        if(Input.GetMouseButtonUp(0) && GameState==1)
        {
            showSelectionBox = false;
            if (box[0] != null)
            {
                for (int i = 0; i < 4; i++) 
                    Destroy(box[i]);
            }
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (!hit || !hit.collider.CompareTag("UI") || !!hit.collider.CompareTag("buildings"))
            {
                GameState = 2;
                CurrentUIBox.transform.position = Camera.main.transform.GetChild(2).transform.position;
            }
        }
        if (Input.GetMouseButton(0) && GameState == 1)
        {
            if(!showSelectionBox)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (!hit || !hit.collider.CompareTag("UI") || !!hit.collider.CompareTag("buildings"))
                {
                    if (CurrentSelection.Count > 0)
                    {
                        int l = CurrentSelection.Count;
                        for (int i = 0; i < l; i++)
                            Deselect(CurrentSelection[0]);
                    }
                }
                OriginPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                showSelectionBox = true;
            }
            if(showSelectionBox)
            {

                Vector2 Pos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 Difference = Pos-OriginPoint;
                if(!Difference.Equals(Vector2.zero))
                {
                    if (box[0]==null)
                    {
                        
                        box[0] = Instantiate(SelectionBoxSprite, OriginPoint, Quaternion.identity) as GameObject;
                        box[1] = Instantiate(SelectionBoxSprite, OriginPoint, Quaternion.Euler(new Vector3(0,0,90))) as GameObject;
                        box[2] = Instantiate(SelectionBoxSprite, new Vector2(OriginPoint.x, Pos.y), Quaternion.Euler(new Vector3(0, 0, 90))) as GameObject;
                        box[3] = Instantiate(SelectionBoxSprite, new Vector2(Pos.x, OriginPoint.y), Quaternion.identity) as GameObject;
                        box[0].AddComponent<BoxCollider2D>();
                        box[0].GetComponent<BoxCollider2D>().isTrigger = true;
                        box[0].AddComponent<Rigidbody2D>();
                        box[0].GetComponent<Rigidbody2D>().isKinematic=true;
                        SelectionScript ss=(SelectionScript)box[0].AddComponent(typeof(SelectionScript));
                        ss.GM = this;
                    }
                    box[0].GetComponent<BoxCollider2D>().offset = new  Vector2(Difference.x/2,-.5f);
                    box[0].GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(Difference.x), 1);
                    box[0].transform.localScale = new Vector2(1, -1*Difference.y);
                    box[1].transform.localScale = new Vector2(1, Difference.x);
                    box[2].transform.localScale = new Vector2(1, Difference.x);
                    box[2].transform.position = new Vector2(OriginPoint.x, Pos.y);
                    box[3].transform.localScale = new Vector2(1, -1*Difference.y);
                    box[3].transform.position = new Vector2(Pos.x, OriginPoint.y);

                }
            }
        }


    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0) && GameState == 0)
        {
            CamDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
            if (!Drag)
            {
                Drag = true;
                CamRefPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else {
            Drag = false;
        }
        if (Drag)
        {
            Vector2 Pos= (Vector2)CamRefPoint - (Vector2)CamDifference;
            Camera.main.transform.position = new Vector3(Pos.x, Pos.y, -10);
        }
    }
}
