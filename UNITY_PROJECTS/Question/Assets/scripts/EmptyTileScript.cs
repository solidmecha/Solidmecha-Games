using UnityEngine;
using System.Collections;

public class EmptyTileScript : MonoBehaviour {

    public GameControl GC;
    public GameObject CurrentTile;
    public int[] Loc;

    void OnMouseOver()
    {

        if (Input.GetMouseButton(0))
        {
            if (CurrentTile != null)
                Destroy(CurrentTile);

            CurrentTile= GC.PlaceTile(new Vector3(transform.position.x, transform.position.y, 0));
            GC.GameWorld[Loc[0]][Loc[1]] = GC.SelectedTileID;
        }

       
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!GC.InBuildMode)
            Destroy(transform.parent.gameObject);
	
	}
}
