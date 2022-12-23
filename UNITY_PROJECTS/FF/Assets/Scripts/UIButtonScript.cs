using UnityEngine;
using System.Collections;

public class UIButtonScript : MonoBehaviour {

    public GameManager GM;
    public ushort id;

    void OnMouseDown()
    {
        GM.GameState = id;
        GM.CurrentUIBox.transform.position = transform.position;
    }


	// Use this for initialization
	void Start () {
        GM = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
