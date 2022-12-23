using UnityEngine;
using System.Collections.Generic;

public class ShipControl : MonoBehaviour {


    public void OnMouseDown()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        if (gc.PlayerTurn)
        {
            if (gc.SelectedShip != null)
                gc.ClearMovement();
            gc.DisplayMovement((int)transform.position.x, (int)transform.position.y);
            gc.SelectedShip = transform;
        }

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
