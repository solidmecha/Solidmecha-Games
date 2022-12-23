using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public int Direction;
    public int layer;
    public CubeControl CC;

    void OnMouseDown()
    {
        CC.CubeRotation(layer, Direction);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
