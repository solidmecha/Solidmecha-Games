using UnityEngine;
using System.Collections;

public class CastleScript : MonoBehaviour {

    public int id;
    public CastleControl CC;
    void OnMouseDown()
    {
        CC.CurrentCastle = this;
        CC.Outline.transform.position = transform.position;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
