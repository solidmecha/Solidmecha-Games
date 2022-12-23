using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CellUI : MonoBehaviour {

    public CellScript CS;
    public GameObject ActiveButton;
    public GameObject CloseButton;
    public GameObject ThreeXThree;
    public GameObject CommandSprite;
    public Toggle ScanToggle;
    public List<Sprite> Sprites=new List<Sprite> { };
    List<Color> Colors = new List<Color> { Color.blue, Color.cyan, Color.green, Color.yellow, Color.red, Color.magenta, Color.white };
    public bool isShowingActived;
    public Slider ColorSlide;
    public Slider DelaySlide;
    public Text DelayText;

    public void SwitchActive()
    {
        if (CS.WS.CommandWindow == null)
        {
            isShowingActived = !isShowingActived;
            if (isShowingActived)
            {
                ActiveButton.transform.GetChild(0).GetComponent<Text>().text = "Activated";
            }
            else
            {
                ActiveButton.transform.GetChild(0).GetComponent<Text>().text = "Supressed";
            }
            SetCommandSprites();
        }
    }

    public void InvokeResetActive()
    {
        Invoke("ResetActive", .2f);
    }

    void ResetActive()
    {
        ActiveButton.GetComponent<Button>().interactable = true;
        ScanToggle.interactable = true;
    }

    public void SetCommandSprites()//show the 8 Commands
    {
        for(int i=0;i<8;i++)
        {
            if (ThreeXThree.transform.GetChild(i).childCount > 0)
                Destroy(ThreeXThree.transform.GetChild(i).GetChild(0).gameObject);
            GameObject go = Instantiate(CommandSprite) as GameObject;
            NeighborHelper nh = (NeighborHelper)ThreeXThree.transform.GetChild(i).gameObject.GetComponent(typeof(NeighborHelper));
            if (isShowingActived)
            {
              
                go.GetComponent<SpriteRenderer>().sprite = Sprites[CS.ActionsID[i]];
                go.transform.SetParent(ThreeXThree.transform.GetChild(i));
                go.transform.localPosition = go.transform.localScale.x * CS.gameObject.transform.GetChild(CS.ActionParams[i]).localPosition;
                nh.sprite = Sprites[CS.ActionsID[i]];
            }
            else
            {
                if(CS.SupressedActionsID[i] >= 0)
                {
                    go.GetComponent<SpriteRenderer>().sprite = Sprites[CS.SupressedActionsID[i]];
                    nh.sprite = Sprites[CS.SupressedActionsID[i]];
                }


                go.transform.SetParent(ThreeXThree.transform.GetChild(i));
                go.transform.localPosition = go.transform.localScale.x * CS.gameObject.transform.GetChild(CS.SupressedActionParams[i]).localPosition;
            }
            nh.Loc = go.transform.localPosition;
        }
    }

	// Use this for initialization
	void Start () {
        transform.position = transform.position + Vector3.back;
        DelaySlide.value = CS.delay*10f;
        float f = DelaySlide.value / 10f;
        DelayText.text = f.ToString() + " Delay";
        ColorSlide.value = CS.ColorID;
        ScanToggle.isOn = (int)CS.transform.GetChild(9).GetComponent<SpriteRenderer>().color.a==1;
        DelaySlide.onValueChanged.AddListener(delegate { SetDelaySlide(); });
        ColorSlide.onValueChanged.AddListener(delegate { SetColorSlide(); });
        CloseButton.GetComponent<Button>().onClick.AddListener(delegate { exitWindow(); });
        ActiveButton.GetComponent<Button>().onClick.AddListener(delegate { SwitchActive(); });
        ScanToggle.onValueChanged.AddListener(delegate { ShowScanner(); });
        isShowingActived = true;
        SetCommandSprites();
	}

    void ShowScanner()
    {
        if(ScanToggle.isOn)
            CS.transform.GetChild(9).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        else
            CS.transform.GetChild(9).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    void SetColorSlide()
    {
        if(CS.WS.CommandWindow !=null)
        { ColorSlide.value = CS.ColorID; }
        CS.ColorID = (int)ColorSlide.value;
        CS.GetComponent<SpriteRenderer>().color = Colors[CS.ColorID];
    }

    void SetDelaySlide()
    {
        if (CS.WS.CommandWindow != null)
            DelaySlide.value = CS.delay * 10f;
        float f = DelaySlide.value / 10f;
        DelayText.text = f.ToString() + " Delay";
        CS.delay=f;
    }
	
    void exitWindow()
    {
        if (CS.WS.CommandWindow != null)
            Destroy(CS.WS.CommandWindow);
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
