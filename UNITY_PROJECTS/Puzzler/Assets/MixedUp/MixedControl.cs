using UnityEngine;
using System.Collections.Generic;

public class MixedControl : MonoBehaviour {

    int[] OrderIndex=new int[] { 0, 1, 2, 3 };
    Vector2[] Directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	// Use this for initialization
	void Start () {
	
	}

    void MixUp()
    {
        System.Random rng = new System.Random();
        List<int> list = new List<int> { 0, 1, 2, 3};
        for(int i=0;i<4;i++)
        {
            int r = rng.Next(list.Count);
            OrderIndex[i] = list[r];
            list.RemoveAt(r);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
            transform.position = (Vector2)transform.position + Directions[OrderIndex[0]];
        else if (Input.GetKeyDown(KeyCode.S))
            transform.position = (Vector2)transform.position + Directions[OrderIndex[1]];
        else if (Input.GetKeyDown(KeyCode.A))
            transform.position = (Vector2)transform.position + Directions[OrderIndex[2]];
        else if (Input.GetKeyDown(KeyCode.D))
            transform.position = (Vector2)transform.position + Directions[OrderIndex[3]];
        if (Input.GetKeyDown(KeyCode.Space))
            MixUp();
    }
}
