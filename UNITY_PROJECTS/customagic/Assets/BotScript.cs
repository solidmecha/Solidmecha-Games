using UnityEngine;
using System.Collections;

public class BotScript : MonoBehaviour {

    public int PathIndex;
    int index;

	// Use this for initialization
	void Start () {
	
	}

    public Vector2 Next()
    {
        if(PathIndex>=WorldControl.singleton.PossiblePaths.Count)
        {
            GetComponent<WeaponScript>().SetHolding();
            return transform.position;
        }
        index = (index + 1) % WorldControl.singleton.PossiblePaths[PathIndex].Length;
        if (index == 0)
            return WorldControl.singleton.PossiblePaths[PathIndex][WorldControl.singleton.PossiblePaths[PathIndex].Length - 1];
        else
            return WorldControl.singleton.PossiblePaths[PathIndex][index - 1];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
