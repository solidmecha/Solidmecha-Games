using UnityEngine;
using System.Collections.Generic;

public class FrameControl : MonoBehaviour {

    public static FrameControl singleton;
    public int FPS;
    public GameObject[] Games;
    public System.Random RNG;
    List<int> Order=new List<int> { };
    int index =0;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        List<int> temp = new List<int> { 0, 1, 2, 3, 4, 5 };
        for(int i=0;i<6;i++)
        {
            int r = RNG.Next(temp.Count);
            Order.Add(temp[r]);
            temp.RemoveAt(r);
        }
    }
    // Use this for initialization
    void Start () {
        Instantiate(Games[Order[index]]);
	}

    public void win()
    {
        if (FPS < 60)
            FPS += 5;
        if (index < 5)
        {
            index++;
        }
        else
            index = RNG.Next(6);
        Instantiate(Games[Order[index]]);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);	
	}
}
