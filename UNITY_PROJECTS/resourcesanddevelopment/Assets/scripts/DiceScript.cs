using UnityEngine;
using System.Collections;

public class DiceScript : MonoBehaviour {

    public bool TurnDice;
    public int Val;
	// Use this for initialization
	void Start () {
            Roll();
	}

    void Roll()
    {
        Val=GameControl.singleton.RNG.Next(6);
        GetComponent<SpriteRenderer>().sprite = GameControl.singleton.DiceFaces[Val];
        if (!TurnDice)
        {
            transform.parent.GetComponent<TileScript>().GenValue = Val;
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
