using UnityEngine;
using System.Collections;

public class PatternMovement : MonoBehaviour {

   public Vector2[] Path;
    int index;
    int dir=1;
	// Use this for initialization
	void Start () {
	
	}

    public Vector2 Next()
    {
        index +=dir;
        if(index==Path.Length)
        {
            index = Path.Length - 2;
            dir = -1;
        }
        else if(index==-1)
        {
            index = 1;
            dir = 1;
        }
        return Path[index];
    }

	// Update is called once per frame
	void Update () {
	
	}
}
