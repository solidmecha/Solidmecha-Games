using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveControl : MonoBehaviour {

    public System.Random RNG = new System.Random();
    public Color[] Colors;
    public int minionCount;
    public string WinMessage;
    public string LossMessage;
    public Text Message;
    public GameObject WinButton;
    public int LevelIndex;
    public bool lost;


    public void Win()
    {
        if (!lost)
        {
            Message.text = WinMessage;
            if (LevelIndex + 1 < Application.levelCount)
            {
                GameObject go = Instantiate(WinButton) as GameObject;
                go.transform.GetChild(0).GetComponent<WinButtonScript>().NextLevelIndex = LevelIndex + 1;
                if (LevelIndex == 4)
                    go.transform.GetChild(0).GetComponentInChildren<Text>().text="Endless Randomizer Mode";
            }
        }
    }
    public void Loss()
    {
        Message.text = LossMessage;
        Message.color = Color.red;
        lost = true;
    }


    // Use this for initialization
    void Start () {
       RNG = new System.Random();
        //Colors = new Color[3] { new Color(0f, 160f / 255f, 255f / 255f), new Color(251f / 255f, 13f / 255f, 47f / 255f), new Color(49f / 255f, 143f / 255f, 13f / 255f) };
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
