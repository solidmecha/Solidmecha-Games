using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {
	public GameObject tile;

	public static GameObject SelectedPiece;
	// Use this for initialization
	void Start () {
		for (int r=0; r<18; r+=2) {
			for(int c=0;c<18;c+=2)
			{
				Instantiate(tile,new Vector2(r,c), Quaternion.identity);
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
