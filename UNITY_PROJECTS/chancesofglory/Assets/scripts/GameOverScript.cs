using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DiceControl.singleton.StopAllCoroutines();
        GameControl.singleton.CancelInvoke();
        if (DiceControl.singleton.DiceObj != null)
            Destroy(DiceControl.singleton.DiceObj);
        transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Game Over. Wins: " + GameControl.singleton.WinCount.ToString()+" Losses: 1";
        transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { GameControl.singleton.Restart(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
