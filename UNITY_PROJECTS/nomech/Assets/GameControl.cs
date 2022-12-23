using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static GameControl singleton;
    public System.Random RNG;
    public GameObject Lines;
    public GameObject Target;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
    }
    // Use this for initialization
    void Start () {
        int tCount = RNG.Next(4, 8);
        for(int i=0;i<tCount;i++)
        {
            Instantiate(Target, new Vector2(RNG.Next(-8, 8), RNG.Next(-5, 6)), Quaternion.identity);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
