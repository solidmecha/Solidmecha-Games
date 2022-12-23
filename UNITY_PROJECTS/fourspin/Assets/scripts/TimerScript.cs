using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerScript : MonoBehaviour {
    public Text TimeText;
    public float GameTime;
    public bool StartedTimer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(StartedTimer)
        {
            GameTime -= Time.deltaTime;
            int t=Mathf.RoundToInt(GameTime * 10f);
            string s= (t / 10f).ToString();
            if(GameTime>=100)
            {
                if (s.Length == 3)
                    s = s + ".0";
            }
            else if (GameTime >= 10)
            {
                if (s.Length == 2)
                    s = s + ".0";
            }
            else if (GameTime >= 10)
            {
                if (s.Length == 1)
                    s = s + ".0";
            }
            TimeText.text = s;
            if(GameTime<=0)
            {
                TimeText.text = "0.00";
                Destroy(GetComponent<PlayerControl>());
            }
        }
	}
}
