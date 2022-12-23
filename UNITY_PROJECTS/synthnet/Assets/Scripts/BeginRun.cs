using UnityEngine;
using System.Collections;

public class BeginRun : MonoBehaviour {

    public int ID;

    private void OnMouseDown()
    {
        ActionLookup();
        Destroy(gameObject);

    }

    void ActionLookup()
    {
        switch(ID)
        {
            case 0:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().BeginRun();
                break;
            case 1:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().RepairAll();
                break;
            case 2:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().ChargeAll();
                break;
            case 3:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().BuyModule();
                break;

            case 4:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().RepairWrenches();
                break;
            case 5:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().ExpandRig();
                break;
            case 6:
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().AddClick();
                break;
        }
    }

    // Use this for initialization
    void Start () {
        if (ID == 5 && GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().rigHeight == 9)
            Destroy(gameObject);
        if (ID == 6 && GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().ClickCount == 7)
            Destroy(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
