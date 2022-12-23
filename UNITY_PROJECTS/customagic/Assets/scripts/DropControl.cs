using UnityEngine;
using System.Collections;

public class DropControl : MonoBehaviour {

    public static DropControl singleton;
    public GameObject[] Drops;


    public void RandomDrop(Vector2 pos)
    {
        Instantiate(Drops[WorldControl.singleton.RNG.Next(Drops.Length)], pos, Quaternion.identity);
    }

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
