using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldScript : MonoBehaviour
{
    public GameObject Cell;
    public GameObject Border;
    public int height;
    public int width;
    public bool Torus;
    public bool isPaused;
    public GameObject PauseButton;
    public GameObject CommandWindow;
    public GameObject CellWindow;
    public int DropValue;

    public Button RandomButton;
    public Button ResetButton;
    List<Color> Colors = new List<Color> { Color.blue, Color.cyan, Color.green, Color.yellow, Color.red, Color.magenta, Color.white };
    // Use this for initialization
    void Start()
    {
        pause();
        PauseButton.GetComponent<Button>().onClick.AddListener(delegate { pause(); });
        RandomButton.onClick.AddListener(delegate { RandSetUp(); });
        ResetButton.onClick.AddListener(delegate { Application.LoadLevel(0); });
        CreateGrid();
    }

    void RandSetUp()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        int Count = RNG.Next(12, 103);
        for(int i=0;i<Count;i++)
        {
            Vector2 V = (Vector2)transform.position+new Vector2(RNG.Next(width),RNG.Next(height));
            if (Physics2D.Raycast(V, Vector2.zero))
                Destroy(Physics2D.Raycast(V, Vector2.zero).collider.gameObject);
            GameObject Go=Instantiate(Cell, V, Quaternion.identity) as GameObject;
            int c = RNG.Next(Colors.Count);
            Go.GetComponent<SpriteRenderer>().color = Colors[c];
            CellScript CS = (CellScript)Go.GetComponent(typeof(CellScript));
            CS.WS = this;
            CS.WSobj = gameObject;
            CS.ColorID = c;
            CS.PossibleActions.Add(CS.move);
            CS.PossibleActions.Add(CS.destroyTarget);
            CS.PossibleActions.Add(CS.replicate);
            CS.PossibleActions.Add(CS.DoAllTheThings);
            for (int t = 0; t < 8; t++)
            {
                int actionID = RNG.Next(4);
                int target = RNG.Next(8);
                int SupActionID = RNG.Next(4);
                int SupTarget = RNG.Next(8);
                CS.Actions[t] = CS.PossibleActions[actionID];
                CS.ActionParams[t] =target;
                CS.ActionsID[t] = actionID;
                CS.SupressedActions[t] = CS.PossibleActions[SupActionID];
                CS.SupressedActionParams[t] = SupTarget;
                CS.SupressedActionsID[t] = SupActionID;
                CS.PriorityList[t] = t; //change this if priority list ever matters
                CS.counter = 0;
                CS.index = 0;
            }
            CS.Initialized = true;
        }
    }

    void pause() { isPaused = !isPaused;
        if (isPaused)
            PauseButton.transform.GetChild(0).GetComponent<Text>().text = "Play";
        else
            PauseButton.transform.GetChild(0).GetComponent<Text>().text = "Pause";
    }

    public void CreateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(Border, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && CommandWindow==null && CellWindow ==null)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (!hit)
            {
                Vector2 v = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v = new Vector2(Mathf.Round(v.x - transform.position.x), Mathf.Round(v.y - transform.position.y));
                if (v.x >= 0 && v.y >= 0)
                {
                    GameObject Go = Instantiate(Cell, new Vector3(v.x, v.y, 0) + transform.position, Quaternion.identity) as GameObject;
                    CellScript cs = (CellScript)Go.GetComponent(typeof(CellScript));
                    cs.WSobj = gameObject;
                }
            }
        }
    }
}
