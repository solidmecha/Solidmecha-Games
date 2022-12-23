using UnityEngine;
using System.Collections;

public class TileSelectScript : MonoBehaviour {

    public GameObject GC_OBJ;
    public GameControl GC;
    public int ID;
    public GameObject Outline;

    void OnMouseDown()
    {
        GC.SelectedTile = GC.GameTiles[ID];
        GC.SelectedTileID = ID;
        if(GC.current_Outline !=null)
        {
            Destroy(GC.current_Outline);
        }
        GC.current_Outline = Instantiate(Outline, transform.position, Quaternion.identity) as GameObject;
    }

	// Use this for initialization
	void Start () {
        GC = (GameControl)GC_OBJ.GetComponent(typeof(GameControl));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
