using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour {

    public GameObject peg;
    int height=7;
    int width=5;

	// Use this for initialization
	void Start () {

        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Vector2 v = new Vector2(i, j)+(Vector2)transform.position;
                if(j%2 !=0)
                {
                    v=v + new Vector2(.5f, 0f);
                }
                Instantiate(peg, v, Quaternion.identity);
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
