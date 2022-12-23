using UnityEngine;
using System.Collections;

public class SwapScript : MonoBehaviour {
    public int LaneOne;
    public bool backOne;
    public int LaneTwo;
    public bool backTwo;

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Swap(); });
    }

    void Swap()
    {

        BattleScript.singleton.Swap(LaneOne, backOne, LaneTwo, backTwo);
        if (BattleScript.singleton.inBattle)
        {
            BattleScript.singleton.NextTurn();
            Destroy(transform.root.gameObject);
        }
        else if (LaneOne < 0)//pass button
        {
            Destroy(transform.root.gameObject);
            WorldControl.singleton.ExitPreview();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
