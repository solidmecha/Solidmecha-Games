using UnityEngine;
using System.Collections;

public class NextButton : MonoBehaviour {

    public GameManager GM;

    void OnMouseDown()
    {
        GM.NextTurn();
    }

	// Use this for initialization
	void Start () {
       GM= GameObject.Find("GameControl").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
