using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameSetup : MonoBehaviour {

    public GameObject PlayerUp;
    public GameObject PlayerDown;
    public GameObject LifeUp;
    public GameObject LifeDown;
    public GameObject GemUp;
    public GameObject GemDown;
    public GameObject StartButton;

    public GameObject UI_stuff;

    public GameObject PlayerDisplay;
    public GameObject LifeDisplay;
    public GameObject GemDisplay;
    public int PlayerCount;
    public int LifeCount;
    public int GemCount;
    public GameObject Manager;

    // Use this for initialization
    void Start () {
        PlayerCount=3;
        GemCount = 10;
        LifeCount = 4;
        UpdateUI();
        PlayerUp.GetComponent<Button>().onClick.AddListener(delegate { playerCounter(1); });
        PlayerDown.GetComponent<Button>().onClick.AddListener(delegate { playerCounter(-1); });
        LifeUp.GetComponent<Button>().onClick.AddListener(delegate { lifeCounter(1); });
        LifeDown.GetComponent<Button>().onClick.AddListener(delegate {lifeCounter(-1); });
        GemUp.GetComponent<Button>().onClick.AddListener(delegate { gemCounter(1); });
       GemDown.GetComponent<Button>().onClick.AddListener(delegate {gemCounter(-1); });
        StartButton.GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });

    }

    public void StartGame()
    {
        GameManager GM = (GameManager)Manager.GetComponent(typeof(GameManager));
        GM.PlayerCount = PlayerCount;
        GM.Start_Life = LifeCount;
        GM.Gem_Goal = GemCount;
        GM.SetUpGame();
        UI_stuff.transform.position = Vector3.zero;
        Destroy(gameObject);
    }

    public void playerCounter(int i)
    {
        if(PlayerCount+i >1 && PlayerCount+i<=6)
        PlayerCount += i;
        UpdateUI();
    }

    public void lifeCounter(int i)
    {
        if(LifeCount+i>0)
        LifeCount += i;
        UpdateUI();
    }

    public void gemCounter(int i)
    {
        if (GemCount + i > 0)
            GemCount +=i;
        UpdateUI();
    }

    void UpdateUI()
    {
        PlayerDisplay.GetComponent<Text>().text = PlayerCount.ToString();
        LifeDisplay.GetComponent<Text>().text = LifeCount.ToString();
        GemDisplay.GetComponent<Text>().text = GemCount.ToString();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
