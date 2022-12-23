using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    public int ResourceID;
    public GameObject GamePiece;
    public bool isLocked;
    public int[] EdgeTypes;
    public int GenValue;
    public int[] StockPile;

	// Use this for initialization
	void Start () {
	}

    public void HandleRotation()
    {
        int temp = EdgeTypes[0];
        for(int i=0;i<5;i++)
            EdgeTypes[i] = EdgeTypes[i+1];
        EdgeTypes[5] = temp;
    }

    public void BuildVillage()
    {
        GamePiece=Instantiate(GameControl.singleton.Pieces[0], transform.position, Quaternion.identity) as GameObject;
        GamePiece.transform.SetParent(transform);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
