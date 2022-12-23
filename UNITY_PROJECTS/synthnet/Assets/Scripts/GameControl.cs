using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {


    public GameObject PlayerRig;
    public SubroutineSelect TargetSubRoutine;
    public int ClickCount;
    public int Clicks;
    public IceControl IceEncountered;
    public GameObject ClickIcon;
    public GameObject SubRoutinePrefab;
    public GameObject IcePrefab;
    System.Random RNG = new System.Random();
    public GameObject BrokenStrikeout;
    public GameObject RigTile;
    public int rigWidth;
    public int rigHeight;
    public Transform InventoryLocations;
    public int ChargeCount;
    public GameObject BeginRunButton;
    public int IconCount;
    public int CreditCount;
    public Text CreditText;
    public GameObject Reticle;
    public int IceCount;
    int IceTurns;
    public Text IceCountText;
    public Text IceTurnText;
    public List<Transform> Modules=new List<Transform> { };


    public void UseClick()
    {
        ClickCount--;
        ClickIcon.transform.GetChild(ClickCount).GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, .5f);
        if(ClickCount==0)
        {
            ClickCount = Clicks;
            foreach (SpriteRenderer s in ClickIcon.GetComponentsInChildren<SpriteRenderer>())
                s.color = Color.white;
            UnleashIce();
        }
    }

    public void PassTurn()
    {
        ChargeCount++;
        if(ChargeCount==2)
        {
            for (int i = 0; i < PlayerRig.transform.childCount; i++)
            {
                foreach (NodeID n in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
                {
                    if (n.ID==4 && !n.Charged && !n.Destroyed)
                    {
                        n.Charged = true;
                        n.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
            ChargeCount = 0;
        }
        UseClick();
    }

    public void UnleashIce()
    {
        foreach(SubroutineSelect s in IceEncountered.GetComponentsInChildren<SubroutineSelect>())
        {

            if (!s.Broken)
            {
                for (int i = 0; i < s.ActionCount; i++)
                    IceEncountered.GetSubID(s.ActionID, s.ActionValue);
            }
            else
            {
                s.Broken = false;
                Destroy(s.transform.GetChild(s.transform.childCount - 1).gameObject);
            }
        }
        IceEncountered.Turns--;
        UpdateIceTurns();
        IceEncountered.transform.GetChild(IceEncountered.transform.childCount - 1).GetComponent<SubroutineSelect>().SetToolTip();
        if (!CheckForHeart())
            ByeByeRig();
        if(IceEncountered.Turns<0)
        {
            ResetModuleTransforms();
            MoveBackToRig();
            IceEncountered.RemoteAcess();
            Destroy(IceEncountered.gameObject);
            TargetSubRoutine = null;
            GetComponent<PlayerControl>().Breaking = false;
            Instantiate(BeginRunButton);
            Reticle.transform.position = new Vector3(10, 10, 10);
            ClickCount = Clicks;
            foreach (SpriteRenderer s in ClickIcon.GetComponentsInChildren<SpriteRenderer>())
                s.color = Color.white;
        }
    }


    void MoveAwayfromRig()
    {
        foreach(Transform T in Modules)
        {
            foreach(NodeID n in T.GetComponentsInChildren<NodeID>())
            {
                if (Mathf.Round(n.transform.position.x) < Mathf.Round(transform.position.x) || Mathf.Round(n.transform.position.x) >= Mathf.Round(transform.position.x + rigWidth)
                    || Mathf.Round(n.transform.position.y) < Mathf.Round(transform.position.y) || Mathf.Round(n.transform.position.y) >= Mathf.Round(transform.position.y + rigHeight))
                {
                    if (T.parent != null && T.parent.CompareTag("Player"))
                        T.SetParent(null);
                    T.position = (Vector2)T.position+Vector2.up * 20;
                    break;
                }

            }
        }
    }


    void MoveBackToRig()
    {
        foreach (Transform T in Modules)
        {
            foreach (NodeID n in T.GetComponentsInChildren<NodeID>())
            {
                if (Mathf.Round(n.transform.position.y) >= Mathf.Round(transform.position.y + rigHeight))
                {
                    T.position = (Vector2)T.position - Vector2.up * 20;
                    break;
                }

            }
        }
    }

    bool CheckForHeart()
    {
        for (int i = 0; i < PlayerRig.transform.childCount; i++)
        {
            foreach (NodeID n in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                if (n.ID == 6 && n.Charged && !n.Destroyed)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void ByeByeRig()
    {
        for (int i = PlayerRig.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(PlayerRig.transform.GetChild(i).gameObject);
        }
    }
    void UpdateIceTurns()
    {
        IceTurns++;
        IceTurnText.text = "Ice Turns: " + IceTurns.ToString();
    }

    void UpdateIceCount()
    {
        IceCount++;
        IceCountText.text = "Ice Encountered: " + IceCount.ToString();
    }

    public void ResetModuleTransforms()
    {
        for (int i = PlayerRig.transform.childCount-1; i >=0 ; i--)
        {
            PlayerRig.transform.GetChild(i).SetParent(null);
            /*
            foreach (NodeID n in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                n.transform.SetParent(null);
            }
            */
        }
    }

    public void RepairAll()
    {
        if (UpdateCredit(1500))
        {

            SetRig();
            CheckModuleCollision();
            MoveAwayfromRig();
            for (int i = 0; i < PlayerRig.transform.childCount; i++)
            {
                foreach (NodeID n in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
                {
                    if (n.Destroyed)
                    {
                        n.Destroyed = false;
                        Destroy(n.DestroyedIcon);
                    }
                }
            }
            MoveBackToRig();
            ResetModuleTransforms();
        }
    }

    public void ChargeAll()
    {
        if(UpdateCredit(300))
        {
            SetRig();
            CheckModuleCollision();
            MoveAwayfromRig();
            for (int i = 0; i < PlayerRig.transform.childCount; i++)
            {
                foreach (NodeID n in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
                {
                    if (!n.Charged)
                    {
                        n.Charged = true;
                        n.GetComponent<SpriteRenderer>().color=Color.white;
                    }
                }
            }
            MoveBackToRig();
            ResetModuleTransforms();
        }
    }

    public void BuyModule()
    {
        if (UpdateCredit(300))
        {
            int x = RNG.Next(3, 6);
            int y = RNG.Next(3, 6);
            GameObject go = GetComponent<ModuleControl>().CreateModule(x, y, RNG.Next(3, x + y));
            go.transform.position = new Vector2(7, 2);
        }
    }

    public void RepairWrenches()
    {
        if(UpdateCredit(500))
        {
            SetRig();
            CheckModuleCollision();
            MoveAwayfromRig();
            for (int i = 0; i < PlayerRig.transform.childCount; i++)
            {
                foreach (NodeID n in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
                {
                    if (n.Destroyed && n.ID==3)
                    {
                        n.Destroyed = false;
                        Destroy(n.DestroyedIcon);
                    }
                }
            }
            MoveBackToRig();
            ResetModuleTransforms();
        }
    }

    public void ExpandRig()
    {
        if (UpdateCredit(2000))
        {
            rigHeight++;
            rigWidth++;

            for (int i = 0; i < rigWidth; i++)
            {
                for (int j = 0; j < rigHeight; j++)
                {
                    if (i == rigWidth - 1 || j == rigHeight - 1)
                        Instantiate(RigTile, new Vector2(i, j) + (Vector2)transform.position, Quaternion.identity, transform);
                }

            }
        }

    }

    public void AddClick()
    {
        if (UpdateCredit(2500))
        {
            Instantiate(ClickIcon.transform.GetChild(ClickCount - 1), (Vector2)ClickIcon.transform.GetChild(ClickCount - 1).position + Vector2.down, Quaternion.identity, ClickIcon.transform);
            ClickCount++;
            Clicks++;
        }
    }

    public bool UpdateCredit(int amt)
    {
        if (CreditCount >= amt)
        {
            CreditCount -= amt;
            CreditText.text = "$"+CreditCount.ToString();
            return true;
        }
        else
            return false;
    }

    public void CreateNetwork()
    { }

    public void CreateRigGrid()
    {
        for(int i=0;i<rigWidth;i++)
        {
            for (int j = 0; j < rigHeight; j++)
                Instantiate(RigTile, new Vector2(i, j) + (Vector2)transform.position, Quaternion.identity, transform);
        }
    }

    public void BeginRun()
    {
        GetComponent<PlayerControl>().Breaking = true;
        SetRig();
        CheckModuleCollision();
        MoveAwayfromRig();
        CreateIce();
    }

    public void SetRig()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(i).position, Vector2.zero);
            if (hit.collider != null)
                hit.collider.transform.parent.SetParent(PlayerRig.transform);
        }
    }


    public void CheckModuleCollision()
    {
        for(int i= PlayerRig.transform.childCount-1; i>=0;i --)
        {
            foreach(NodeID N in PlayerRig.transform.GetChild(i).GetComponentsInChildren<NodeID>())
            {
                N.gameObject.layer = 2; //ignore raycast
                RaycastHit2D hit=Physics2D.Raycast(N.transform.position, Vector2.zero);
                if(hit.collider != null)
                {
                    PlayerRig.transform.GetChild(i).position = new Vector2(7, 0);
                    PlayerRig.transform.GetChild(i).SetParent(null);
                    N.gameObject.layer = 0;
                    break;
                }
                N.gameObject.layer = 0;
            }
        }
    }

    public void CreateIce()
    {
        GameObject go=Instantiate(IcePrefab);
        IceEncountered = go.GetComponent<IceControl>();
        IceEncountered.GC = this;
        int maxIce = IceCount + 3;
        int minIce = IceCount / 2 + 2;
        if (maxIce > 9)
            maxIce = 9;
        if (minIce > 4)
            minIce = 4;
        int subcount = RNG.Next(minIce, maxIce);
        for(int i=0;i<subcount;i++)
        {
           GameObject sub=Instantiate(SubRoutinePrefab, (Vector2)SubRoutinePrefab.transform.position + i * Vector2.down*1.5f, Quaternion.identity, go.transform) as GameObject;
            SubroutineSelect s = sub.GetComponent<SubroutineSelect>();
            s.BreakID = 2;
            if(RNG.Next(18)==4) //not sword break
            {
                int[] ids = new int[4] { 0, 1, 5, 6 };
                s.BreakID = ids[RNG.Next(4)];
                s.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = GetComponent<ModuleControl>().Icons[s.BreakID];
            }

            s.BreakCount = RNG.Next(1, 5);
            s.ActionID = RNG.Next(6);
            //s.ActionID = 5;
            s.ActionValue = RNG.Next(IconCount);
            if (s.ActionID != 2 && s.ActionID !=5)
                s.ActionCount = RNG.Next(1, 4);
            else
                s.ActionCount = 1;
        }

        //roll click loss
        if(RNG.Next(4)<5)
        {
            SubroutineSelect s=IceEncountered.transform.GetChild(RNG.Next(subcount)).GetComponent<SubroutineSelect>();
            s.ActionID = 12;
            s.ActionCount = RNG.Next(1, ClickCount-1);
        }

        //create last subRoutine as turn count
        GameObject sub2 = Instantiate(SubRoutinePrefab, (Vector2)SubRoutinePrefab.transform.position + subcount * Vector2.down * 1.5f, Quaternion.identity, go.transform) as GameObject;
        SubroutineSelect s2 = sub2.GetComponent<SubroutineSelect>();
        s2.isReadable = true;
        s2.BreakID = 5; //key to break
        s2.BreakCount = RNG.Next(2, 6);
        s2.ActionID = 100;  //turn count
        s2.ActionValue = RNG.Next(3, 8);
        s2.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = GetComponent<ModuleControl>().Icons[5];
        IceEncountered.Turns = s2.ActionValue;
        UpdateIceCount();
    }

	// Use this for initialization
	void Start () {
       // CreateIce();
        CreateRigGrid();

        for (int i = 0; i < InventoryLocations.childCount; i++)
        {
            int x = RNG.Next(3, 6);
            int y = RNG.Next(3, 6);
            GameObject go=GetComponent<ModuleControl>().CreateModule(x, y, RNG.Next(3, x + y));
            go.transform.position = InventoryLocations.GetChild(i).position;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
